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
    public float attackRange = 1f; // ���� ����
    public int damage = 1; // ���� ������
    public LayerMask enemyLayer; // ���� ��� ���̾�

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // ���� Ű �Է� (Space Ű)
        {
            Attack();
        }
    }

    void Attack()
    {
        animator.SetTrigger("Attack"); // ���� �ִϸ��̼� ����

        // ���� ���� ���� �ִ� ���� ����
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayer);

        // ������ ������ ����
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<mon1>()?.OnHit(); // mon1 ��ũ��Ʈ�� OnHit() �޼��� ȣ��
        }

    }

    // ���� ������ �ð�ȭ (����׿�)
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
        transform.position = new Vector3(0, 0, 0); //������ġ ����


    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal"); //����Ű ����
        if (h > 0) transform.localScale = new Vector3(-1, 1, 1); //���ʹ� ũ����, ���� ���� ���� ���� ������ ������: �¿�
        // ��ǥ�� �������� ���������� ...
        else if (h < 0) transform.localScale = new Vector3(1, 1, 1);
        transform.Translate(new Vector3 (h, 0, 0)* Time.deltaTime);
        //new Vector3 (h, 0, 0)�� �����Ӵ� �ð��� ���ϱ� ������ ���� ������ �ڵ����� ���� �ȴ�.

        //transform.Translate(Vector3.right * Time.deltaTime);
        //Time.deltaTime�� 1�����Ӵ� �ɸ��� �ð�
    }

}
*/
