using UnityEngine;
using System.Collections;
using UnityEngine.UI;
//using static UnityEditor.PlayerSettings;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
public class Monai : MonoBehaviour
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
    SpriteRenderer spriteRenderer;
    private enum State { Patrol, Chase, Attack }
    private State currentState = State.Patrol;
    Vector2 targetPos;
    public int Hp;
    public float damage = 10f;  // ���ݷ�
    private HeroKnight monsterHealth;

    public Transform pos;
    public Vector2 boxSize;
    public AudioSource audioSource;


    public void TakeDamege(int damege) //int damege
    {
        if (Hp == 0)
        {
           
            Destroy(gameObject);
           
        }
        animator.SetTrigger("Hit1");
        Hp = Hp - damege;
        PlaySound();


        int dirc = spriteRenderer.flipX ? -1 : 1;
        rb.AddForce(new Vector2(dirc * 30, 2) * 20, ForceMode2D.Force); //ForceMode2D.Impulse

    }

    public void PlaySound()
    {
        // AudioSource�� Play() �޼��带 ȣ���Ͽ� ȿ������ ����մϴ�.
        audioSource.Play();
    }


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // SpriteRenderer �ʱ�ȭ
        audioSource = GetComponent<AudioSource>();

        //  monsterHealth = GetComponent<HeroKnight>();
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
        }

        // ���� ��ȯ
        CheckStateTransitions();
    }



    void HandlePatrol() //���۽� ���� ���ϰ� �ȱ� �������µ� �ȱ� ��Ȱ��ȭ �صθ� �ذ�ȴ�. ��������
    {
        animator.SetBool("Idle", true);
        animator.SetBool("Walk", false);
       
      
        // SetAnimation("Idle", false); >> �� �κ��� ��������..�ȱ� ��½� �ߺ����� ��µǾ ��������
        // rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocity.y);


    }

    void HandleChase() //�ɾ ����. ��������
    {
        animator.SetTrigger("Dash");
        animator.SetBool("Walk", true);
        animator.SetBool("Attack", false);

        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = new Vector2(direction.x * moveSpeed, rb.linearVelocity.y);

        // �÷��̾� ���⿡ ���� �̹��� ����
        if (player.position.x < transform.position.x)
            spriteRenderer.flipX = false; // ������ �ٶ�
        else
            spriteRenderer.flipX = true; // �������� �ٶ�


       // Vector2 direction = (player.position - transform.position).normalized;
       // rb.linearVelocity = new Vector2(direction.x * moveSpeed, rb.linearVelocity.y);
    }

    void HandleAttack()
    {
        rb.linearVelocity = Vector2.zero;
        if (Time.time - lastAttackTime > attackCooldown)
        {
            
            lastAttackTime = Time.time;
         
            animator.SetTrigger("Hit2");

            // ���� ���� �� �÷��̾�� ������ ó��
            Collider2D[] colliders = Physics2D.OverlapBoxAll(pos.position, boxSize, 0);
            foreach (Collider2D collider in colliders)
            {
                if (collider.tag == "Player") // �÷��̾� Ȯ��
                {
                    collider.GetComponent<HeroKnight>().TakeDamage(1); // ������ �ֱ�
                }
            }
        }


        //  animator.SetBool("walk", true);
        // �÷��̾� ���⿡ ���� �̹��� ����
        if (player.position.x < transform.position.x)
            spriteRenderer.flipX = false; // ������ �ٶ�
        else
            spriteRenderer.flipX = true; // �������� �ٶ�
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
            currentState = State.Patrol;
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

