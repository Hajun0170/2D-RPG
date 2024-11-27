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

    public int maxHealth = 15;  // 최대 체력
    private int currentHealth;     // 현재 체력
    public Slider healthBar;         // 체력바 슬라이더
    public AudioSource audioSource;

    public void PlaySound()
    {
        // AudioSource의 Play() 메서드를 호출하여 효과음을 재생합니다.
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
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // 체력은 0에서 maxHealth로 제한
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
        healthBar.value = currentHealth;  // 체력바 값을 현재 체력에 맞게 설정
    }

    void Update()
    {
        // 상태 머신 실행
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

        // 상태 전환
        CheckStateTransitions();
    }

    void HandleDash() //시작시 원래 얘하고 걷기 합쳐졌는데 걷기 비활성화 해두면 해결된다. 문제없음
    {
       
        animator.SetBool("Dash", true);

        Vector2 direction = (player.position - transform.position).normalized;
        // 방향에 따라 스프라이트 반전

        // 플레이어 방향에 따라 이미지 반전
        if (player.position.x < transform.position.x)
            spriteRenderer.flipX = false; // 왼쪽을 바라봄
        else
            spriteRenderer.flipX = true; // 오른쪽을 바라봄


        rb.linearVelocity = new Vector2(direction.x * moveSpeed * 3, rb.linearVelocity.y);


        // SetAnimation("Idle", false); >> 이 부분이 문제였음..걷기 출력시 중복으로 출력되어서 문제였다
        // rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocity.y);


    }

    void HandlePatrol() //시작시 원래 얘하고 걷기 합쳐졌는데 걷기 비활성화 해두면 해결된다. 문제없음
    {
        
       
        // 플레이어 방향에 따라 이미지 반전
        if (player.position.x < transform.position.x)
            spriteRenderer.flipX = false; // 왼쪽을 바라봄
        else
            spriteRenderer.flipX = true; // 오른쪽을 바라봄

    }

    void HandleChase() //걸어서 따라감. 문제없음
    {

        animator.SetBool("Walk", true);
       
       


        Vector2 direction = (player.position - transform.position).normalized;
        // 플레이어 방향에 따라 이미지 반전
        if (player.position.x < transform.position.x)
            spriteRenderer.flipX = false; // 왼쪽을 바라봄
        else
            spriteRenderer.flipX = true; // 오른쪽을 바라봄

        rb.linearVelocity = new Vector2(direction.x * moveSpeed, rb.linearVelocity.y);
    }

    IEnumerator AttakD(float delay)
    {
        // 소수점 단위로 정확히 대기
        yield return new WaitForSeconds(delay);
        // 공격 범위 내 플레이어에게 데미지 처리
        Collider2D[] colliders = Physics2D.OverlapBoxAll(pos.position, boxSize, 0);
        foreach (Collider2D collider in colliders)
        {
            if (collider.tag == "Player") // 플레이어 확인
            {
                collider.GetComponent<HeroKnight>().TakeDamage(1); // 데미지 주기
            }
        }

        Debug.Log("발동");
    }

    void HandleAttack()
    {
        rb.linearVelocity = Vector2.zero;
        if (Time.time - lastAttackTime > attackCooldown)
        {

            lastAttackTime = Time.time;
            animator.SetTrigger("Attack1");
            StartCoroutine(AttakD(0.6f)); //이유를 못찾것으니 그냥 자체 딜레이줄래
            PlaySound();
            // 플레이어 방향에 따라 이미지 반전
            if (player.position.x < transform.position.x)
                spriteRenderer.flipX = false; // 왼쪽을 바라봄
            else
                spriteRenderer.flipX = true; // 오른쪽을 바라봄
  
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
            currentState = State.Chase; //감지되면 걸어감
        }
      
        else
        {
            currentState = State.Dash; //거리가 멀어지면 돌진함
        }

    }

    IEnumerator PatrolPause() //문제없음
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
