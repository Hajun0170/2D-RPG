using UnityEngine;
using System.Collections;

public class attck : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private bool isPushed = false; // �и� ���� Ȯ��
    private float pushRecoveryTime = 0.5f; // �и� ���¿��� ���� �ð�
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
            isPushed = false; // �и� ���� ����
            rb.linearVelocity = Vector2.zero; // �ӵ� �ʱ�ȭ
        }

        if (!isPushed)
        {
            // �Ϲ����� AI �ൿ
            HandleAI();
        }
    }

    void HandleAI()
    {
        // ���� AI ���� (Idle, Patrol, Chase ��)
    }

    public void ApplyPush(Vector2 force)
    {
        isPushed = true; // �и� ���� Ȱ��ȭ
        pushEndTime = Time.time + pushRecoveryTime; // �и� ���� �ð� ����

        rb.AddForce(force, ForceMode2D.Impulse); // ������ �� ���ϱ�
    }
}
