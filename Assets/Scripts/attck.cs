using UnityEngine;
using System.Collections;

public class attck : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private bool isPushed = false; // 밀림 상태 확인
    private float pushRecoveryTime = 0.5f; // 밀림 상태에서 복구 시간
    private float pushEndTime = 0.0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isPushed && Time.time > pushEndTime)
        {
            isPushed = false; // 밀림 상태 해제
            rb.linearVelocity = Vector2.zero; // 속도 초기화
        }

        if (!isPushed)
        {
            // 일반적인 AI 행동
            HandleAI();
        }
    }

    void HandleAI()
    {
        // 기존 AI 로직 (Idle, Patrol, Chase 등)
    }

    public void ApplyPush(Vector2 force)
    {
        isPushed = true; // 밀림 상태 활성화
        pushEndTime = Time.time + pushRecoveryTime; // 밀림 지속 시간 설정

        rb.AddForce(force, ForceMode2D.Impulse); // 물리적 힘 가하기
    }
}
