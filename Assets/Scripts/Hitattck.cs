using UnityEngine;
using System.Collections;

public class Hitattck : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    public float pushForce = 5.0f; // 밀침 힘

    void PushEnemy(Collider2D enemy)
    {
        // 적의 Rigidbody2D 가져오기
        Rigidbody2D enemyRb = enemy.GetComponent<Rigidbody2D>();
        if (enemyRb != null)
        {
            // 밀침 방향 계산 (플레이어 → 적 방향)
            Vector2 pushDirection = (enemy.transform.position - transform.position).normalized;

            // 적에게 힘 가하기
            enemyRb.AddForce(pushDirection * pushForce, ForceMode2D.Impulse);
        }
    }

    // 공격 애니메이션 이벤트에서 호출
    public void OnAttackHit()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, 1.5f); // 공격 범위
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy")) // 적 태그 확인
            {
                PushEnemy(enemy);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        // 공격 범위 시각화
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1.5f);
    }

}
