using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public Transform targetPosition; // �̵��� ��ġ�� �Ҵ��� ����

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("MovePoint"))
        {
            Debug.Log("MovePoint�� �����߽��ϴ�!"); // �α� ���
            transform.position = targetPosition.position; // targetPosition�� ��ġ�� �̵�
        }
        if (other.CompareTag("MovePoint3"))
        {
            Debug.Log("MovePoint3�� �����߽��ϴ�!"); // �α� ���
            SceneManager.LoadScene("Scene2");
        }
    }
}








