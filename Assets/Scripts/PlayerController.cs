using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public Transform targetPosition; // 이동할 위치를 할당할 변수

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("MovePoint"))
        {
            Debug.Log("MovePoint에 도달했습니다!"); // 로그 출력
            transform.position = targetPosition.position; // targetPosition의 위치로 이동
        }
        if (other.CompareTag("MovePoint3"))
        {
            Debug.Log("MovePoint3에 도달했습니다!"); // 로그 출력
            SceneManager.LoadScene("Scene2");
        }
    }
}








