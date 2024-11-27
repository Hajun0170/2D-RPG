using UnityEngine;

public class PlayerControll1 : MonoBehaviour
{
    public Transform targetPosition; // 이동할 위치를 할당할 변수

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("MovePoint1"))
        {
            Debug.Log("MovePoint1에 도달했습니다!"); // 로그 출력
            transform.position = targetPosition.position; // targetPosition의 위치로 이동
        }
    }
}








