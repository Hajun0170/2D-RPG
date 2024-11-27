using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class mon1 : MonoBehaviour
{
    public float movePower = 1f;

    Vector3 movement;
    Animator animator;
    int movementFlag = 0;//����, 0 idle, 1 left, 2right

    public float chaseSpeed = 2f;
    Transform player; // �÷��̾��� Transform
    public float detectRange = 5f; // �÷��̾ �߰��ϴ� ����
    bool isHit = false; // �ǰ� ���� ����
    bool isChasing = false; // �߰� ���� ����


    void Start()
    {
        animator = gameObject.GetComponentInChildren<Animator>(); //���̴�?
      
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
                    movementFlag = 2; // ���������� �̵�
                }
                else
                {
                    movementFlag = 1; // �������� �̵�
                }

                animator.SetBool("isMoving", true); // �����̴� �ִϸ��̼� Ȱ��ȭ

                yield return new WaitForSeconds(1f); // ���� �ð� ���
            }
            */

            /*
            StopCoroutine("changemovement");
            movementFlag = Random.Range(0, 3);

            if (movementFlag == 0)
            {
                animator.SetBool("isMoving", false); //??? �� �ڻ쳲
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
            return; // �ǰ� �ÿ��� �̵� ����

        if (isChasing)
        {
            ChasePlayer();
        }
        else
        {
            Move();
        }

        DetectPlayer();





       // Move(); //�Ʒ����� ������ ����
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
        transform.localScale = new Vector3(direction.x < 0 ? 1 : -1, 1, 1); // ���� ��ȯ
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

    public void OnHit() // �÷��̾��� ������ �¾��� �� ȣ��
    {
        StartCoroutine(HitReaction());
    }

    IEnumerator HitReaction()
    {
        isHit = true;
        animator.SetTrigger("isHit"); // ����Ÿ��� �ִϸ��̼� Ʈ����
        yield return new WaitForSeconds(0.5f); // ����Ÿ��� �ð�
        isHit = false;
        isChasing = true; // �߰� ����
    }

}
