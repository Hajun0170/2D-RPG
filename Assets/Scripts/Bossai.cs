using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
//using static UnityEditor.PlayerSettings;
using UnityEngine.UI;
public class Bossai : MonoBehaviour
{
    public float moveSpeed = 2.0f;
    public float detectionRange = 5.0f;
    public float attackRange = 1.5f;
    public float attackCooldown = 2.0f;
    public float patrolPauseTime = 2.0f;

    private Transform player;
    private Animator animator;
    private Rigidbody2D rb;
    private float lastAttackTime = 0.0f;
    private bool isPatrollingPaused = false;
    private SpriteRenderer spriteRenderer;
    private enum State { Patrol, Chase, Attack, Dash }
    private State currentState = State.Patrol;
    Vector2 targetPos;
    public int Hp = 15;
    public Transform pos;
    public Vector2 boxSize;

    public int maxHealth = 15;  // �ִ� ü��
    private int currentHealth;     // ���� ü��
    public Slider healthBar;         // ü�¹� �����̴�
    public AudioSource audioSource;

    public void PlaySound()
    {
        // AudioSource�� Play() �޼��带 ȣ���Ͽ� ȿ������ ����մϴ�.
        audioSource.Play();
    }
    public void TakeDamege(int damage) //int damege
    {
        if (Hp == 0)
        {

            Destroy(gameObject);
            SceneManager.LoadScene("end");
        }

        Hp = Hp - damage;
        currentHealth = currentHealth - damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // ü���� 0���� maxHealth�� ����
        UpdateHealthBar();

        PlaySound();
        animator.SetTrigger("Hit1");
        int dirc = spriteRenderer.flipX ? -1 : 1;
        rb.AddForce(new Vector2(dirc * 30, 2) * 20, ForceMode2D.Force);

      
     
    }




    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        healthBar.value = currentHealth;  // ü�¹� ���� ���� ü�¿� �°� ����
    }

    void Update()
    {
        // ���� �ӽ� ����
        switch (currentState)
        {
            case State.Patrol:
                HandlePatrol();
                break;
            case State.Chase:
                HandleChase();
                break;
            case State.Attack:
                HandleAttack();
                break;
            case State.Dash:
                HandleDash();
                break;
        }

        // ���� ��ȯ
        CheckStateTransitions();
    }

    void HandleDash() //���۽� ���� ���ϰ� �ȱ� �������µ� �ȱ� ��Ȱ��ȭ �صθ� �ذ�ȴ�. ��������
    {
       
        animator.SetBool("Dash", true);

        Vector2 direction = (player.position - transform.position).normalized;
        // ���⿡ ���� ��������Ʈ ����

        // �÷��̾� ���⿡ ���� �̹��� ����
        if (player.position.x < transform.position.x)
            spriteRenderer.flipX = false; // ������ �ٶ�
        else
            spriteRenderer.flipX = true; // �������� �ٶ�


        rb.linearVelocity = new Vector2(direction.x * moveSpeed * 3, rb.linearVelocity.y);


        // SetAnimation("Idle", false); >> �� �κ��� ��������..�ȱ� ��½� �ߺ����� ��µǾ ��������
        // rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocity.y);


    }

    void HandlePatrol() //���۽� ���� ���ϰ� �ȱ� �������µ� �ȱ� ��Ȱ��ȭ �صθ� �ذ�ȴ�. ��������
    {
        
       
        // �÷��̾� ���⿡ ���� �̹��� ����
        if (player.position.x < transform.position.x)
            spriteRenderer.flipX = false; // ������ �ٶ�
        else
            spriteRenderer.flipX = true; // �������� �ٶ�

    }

    void HandleChase() //�ɾ ����. ��������
    {

        animator.SetBool("Walk", true);
       
       


        Vector2 direction = (player.position - transform.position).normalized;
        // �÷��̾� ���⿡ ���� �̹��� ����
        if (player.position.x < transform.position.x)
            spriteRenderer.flipX = false; // ������ �ٶ�
        else
            spriteRenderer.flipX = true; // �������� �ٶ�

        rb.linearVelocity = new Vector2(direction.x * moveSpeed, rb.linearVelocity.y);
    }

    IEnumerator AttakD(float delay)
    {
        // �Ҽ��� ������ ��Ȯ�� ���
        yield return new WaitForSeconds(delay);
        // ���� ���� �� �÷��̾�� ������ ó��
        Collider2D[] colliders = Physics2D.OverlapBoxAll(pos.position, boxSize, 0);
        foreach (Collider2D collider in colliders)
        {
            if (collider.tag == "Player") // �÷��̾� Ȯ��
            {
                collider.GetComponent<HeroKnight>().TakeDamage(1); // ������ �ֱ�
            }
        }

        Debug.Log("�ߵ�");
    }

    void HandleAttack()
    {
        rb.linearVelocity = Vector2.zero;
        if (Time.time - lastAttackTime > attackCooldown)
        {

            lastAttackTime = Time.time;
            animator.SetTrigger("Attack1");
            StartCoroutine(AttakD(0.6f)); //������ ��ã������ �׳� ��ü �������ٷ�
            PlaySound();
            // �÷��̾� ���⿡ ���� �̹��� ����
            if (player.position.x < transform.position.x)
                spriteRenderer.flipX = false; // ������ �ٶ�
            else
                spriteRenderer.flipX = true; // �������� �ٶ�
  
        }
    }

    void CheckStateTransitions()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            currentState = State.Attack;
        }
        else if (distanceToPlayer <= detectionRange)
        {
            currentState = State.Chase; //�����Ǹ� �ɾ
        }
      
        else
        {
            currentState = State.Dash; //�Ÿ��� �־����� ������
        }

    }

    IEnumerator PatrolPause() //��������
    {
        isPatrollingPaused = true;
        rb.linearVelocity = Vector2.zero;
        //SetAnimation("Idle", true);
        yield return new WaitForSeconds(patrolPauseTime);
        isPatrollingPaused = false;
    }

    private void SetAnimation(string parameter, bool value)
    {
        if (!animator.GetBool(parameter))
        {
            animator.SetBool(parameter, value);
        }
    }

}
