using UnityEngine;
using System.Collections;

public class Hitattck : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    public float pushForce = 5.0f; // ��ħ ��

    void PushEnemy(Collider2D enemy)
    {
        // ���� Rigidbody2D ��������
        Rigidbody2D enemyRb = enemy.GetComponent<Rigidbody2D>();
        if (enemyRb != null)
        {
            // ��ħ ���� ��� (�÷��̾� �� �� ����)
            Vector2 pushDirection = (enemy.transform.position - transform.position).normalized;

            // ������ �� ���ϱ�
            enemyRb.AddForce(pushDirection * pushForce, ForceMode2D.Impulse);
        }
    }

    // ���� �ִϸ��̼� �̺�Ʈ���� ȣ��
    public void OnAttackHit()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, 1.5f); // ���� ����
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy")) // �� �±� Ȯ��
            {
                PushEnemy(enemy);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        // ���� ���� �ð�ȭ
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1.5f);
    }

}
