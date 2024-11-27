using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonOutlineColorOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Button button;  // 버튼 컴포넌트
    public Color hoverColor = Color.green;  // 마우스가 올려졌을 때 외곽선 색상
    public Color normalColor = Color.white;  // 기본 외곽선 색상

    private Outline outline;  // 외곽선 컴포넌트

    void Start()
    {
        outline = button.GetComponent<Outline>();  // 버튼의 Outline 컴포넌트 참조
        if (outline == null)
        {
            outline = button.gameObject.AddComponent<Outline>();  // 없으면 추가
        }
        outline.effectColor = normalColor;  // 기본 외곽선 색상 설정
    }

    // 포인터가 버튼 위로 들어왔을 때 호출되는 함수
    public void OnPointerEnter(PointerEventData eventData)
    {
        outline.effectColor = hoverColor;  // 외곽선 색상 변경
    }

    // 포인터가 버튼을 벗어났을 때 호출되는 함수
    public void OnPointerExit(PointerEventData eventData)
    {
        outline.effectColor = normalColor;  // 원래 색상으로 복원
    }
}
