using UnityEngine;

public class PlayerControll1 : MonoBehaviour
{
    public Transform targetPosition; // �̵��� ��ġ�� �Ҵ��� ����

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("MovePoint1"))
        {
            Debug.Log("MovePoint1�� �����߽��ϴ�!"); // �α� ���
            transform.position = targetPosition.position; // targetPosition�� ��ġ�� �̵�
        }
    }
}








