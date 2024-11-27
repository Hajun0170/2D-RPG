using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class mon1 : MonoBehaviour
{
    public float movePower = 1f;

    Vector3 movement;
    Animator animator;
    int movementFlag = 0;//선언, 0 idle, 1 left, 2right

    public float chaseSpeed = 2f;
    Transform player; // 플레이어의 Transform
    public float detectRange = 5f; // 플레이어를 추격하는 범위
    bool isHit = false; // 피격 상태 여부
    bool isChasing = false; // 추격 상태 여부


    void Start()
    {
        animator = gameObject.GetComponentInChildren<Animator>(); //차이는?
      
        StartCoroutine("Changemovement");
    }

    IEnumerator Changemovement()
    {
        while (!isChasing && !isHit)
        {
            movementFlag = Random.Range(0, 3);

            if (movementFlag == 0)
                animator.SetBool("isMoving", false);
            else
                animator.SetBool("isMoving", true);

            yield return new WaitForSeconds(3f);






            /*while (true)
            {
                if (movementFlag == 1)
                {
                    movementFlag = 2; // 오른쪽으로 이동
                }
                else
                {
                    movementFlag = 1; // 왼쪽으로 이동
                }

                animator.SetBool("isMoving", true); // 움직이는 애니메이션 활성화

                yield return new WaitForSeconds(1f); // 일정 시간 대기
            }
            */

            /*
            StopCoroutine("changemovement");
            movementFlag = Random.Range(0, 3);

            if (movementFlag == 0)
            {
                animator.SetBool("isMoving", false); //??? 왜 박살남
            }
            else
                animator.SetBool("isMoving", true);

            yield return new WaitForSeconds(2f);

            StartCoroutine("Changemovement");
            */
        }
    }

    void FixedUpdate()
    {
        if (isHit)
            return; // 피격 시에는 이동 정지

        if (isChasing)
        {
            ChasePlayer();
        }
        else
        {
            Move();
        }

        DetectPlayer();





       // Move(); //아래에서 정의할 예정
    }

    void Move()
    { 
    Vector3 moveVelocity = Vector3.zero; //moveVelocity ??

        if (movementFlag == 1)
        {
            moveVelocity = Vector3.left;
            //transform.localScale = new Vector3(1, 1, 1);
        }

        if (movementFlag == 2)
        {
            moveVelocity = Vector3.right;
           // transform.localScale = new Vector3(-1, 1, 1);
        }

        transform.position += moveVelocity * movePower * Time.deltaTime;

    }

    void ChasePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * chaseSpeed * Time.deltaTime;
        transform.localScale = new Vector3(direction.x < 0 ? 1 : -1, 1, 1); // 방향 전환
        animator.SetBool("isMoving", true);
    }

    void DetectPlayer()
    {
        if (Vector3.Distance(transform.position, player.position) < detectRange)
        {
            isChasing = true;
            StopCoroutine("Changemovement");
        }
    }

    public void OnHit() // 플레이어의 공격을 맞았을 때 호출
    {
        StartCoroutine(HitReaction());
    }

    IEnumerator HitReaction()
    {
        isHit = true;
        animator.SetTrigger("isHit"); // 움찔거리는 애니메이션 트리거
        yield return new WaitForSeconds(0.5f); // 움찔거리는 시간
        isHit = false;
        isChasing = true; // 추격 시작
    }

}
