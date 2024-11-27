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
    public float damage = 10f;  // 공격력
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
        // AudioSource의 Play() 메서드를 호출하여 효과음을 재생합니다.
        audioSource.Play();
    }


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // SpriteRenderer 초기화
        audioSource = GetComponent<AudioSource>();

        //  monsterHealth = GetComponent<HeroKnight>();
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
        }

        // 상태 전환
        CheckStateTransitions();
    }



    void HandlePatrol() //시작시 원래 얘하고 걷기 합쳐졌는데 걷기 비활성화 해두면 해결된다. 문제없음
    {
        animator.SetBool("Idle", true);
        animator.SetBool("Walk", false);
       
      
        // SetAnimation("Idle", false); >> 이 부분이 문제였음..걷기 출력시 중복으로 출력되어서 문제였다
        // rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocity.y);


    }

    void HandleChase() //걸어서 따라감. 문제없음
    {
        animator.SetTrigger("Dash");
        animator.SetBool("Walk", true);
        animator.SetBool("Attack", false);

        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = new Vector2(direction.x * moveSpeed, rb.linearVelocity.y);

        // 플레이어 방향에 따라 이미지 반전
        if (player.position.x < transform.position.x)
            spriteRenderer.flipX = false; // 왼쪽을 바라봄
        else
            spriteRenderer.flipX = true; // 오른쪽을 바라봄


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

            // 공격 범위 내 플레이어에게 데미지 처리
            Collider2D[] colliders = Physics2D.OverlapBoxAll(pos.position, boxSize, 0);
            foreach (Collider2D collider in colliders)
            {
                if (collider.tag == "Player") // 플레이어 확인
                {
                    collider.GetComponent<HeroKnight>().TakeDamage(1); // 데미지 주기
                }
            }
        }


        //  animator.SetBool("walk", true);
        // 플레이어 방향에 따라 이미지 반전
        if (player.position.x < transform.position.x)
            spriteRenderer.flipX = false; // 왼쪽을 바라봄
        else
            spriteRenderer.flipX = true; // 오른쪽을 바라봄
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
            currentState = State.Patrol;
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

