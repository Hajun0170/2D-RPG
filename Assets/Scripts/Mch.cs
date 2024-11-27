using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Mch : MonoBehaviour
{
    public float movespeed = 1f; 
    void Start(){

    }
    void Update(){

    }

    void FixedUpdate()
    {
        Move();
    }


    void Move()
    {
        Vector3 movePosition = Vector3.zero;
        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            movePosition = Vector3.left;
        }
        else if (Input.GetAxisRaw("Horizontal") > 0)
        {
            movePosition = Vector3.right;
        }
       
        transform.position += movePosition * movespeed * Time.deltaTime;

    }

}


public class PlayerAttack : MonoBehaviour
{
    public float attackRange = 1f; // 공격 범위
    public int damage = 1; // 공격 데미지
    public LayerMask enemyLayer; // 공격 대상 레이어

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // 공격 키 입력 (Space 키)
        {
            Attack();
        }
    }

    void Attack()
    {
        animator.SetTrigger("Attack"); // 공격 애니메이션 실행

        // 공격 범위 내에 있는 적을 감지
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayer);

        // 적에게 데미지 전달
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<mon1>()?.OnHit(); // mon1 스크립트의 OnHit() 메서드 호출
        }

    }

    // 공격 범위를 시각화 (디버그용)
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }



}




/*
public class Mch : MonoBehaviour
{


    void Start()
    {
        transform.position = new Vector3(0, 0, 0); //시작위치 고정


    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal"); //방향키 적용
        if (h > 0) transform.localScale = new Vector3(-1, 1, 1); //벡터는 크기임, 가장 왼쪽 값에 따라 방향이 결정됨: 좌우
        // 좌표값 기준으로 오른쪽으로 ...
        else if (h < 0) transform.localScale = new Vector3(1, 1, 1);
        transform.Translate(new Vector3 (h, 0, 0)* Time.deltaTime);
        //new Vector3 (h, 0, 0)에 프레임당 시간을 곱하기 때문에 값이 있으면 자동으로 가게 된다.

        //transform.Translate(Vector3.right * Time.deltaTime);
        //Time.deltaTime은 1프레임당 걸리는 시간
    }

}
*/
