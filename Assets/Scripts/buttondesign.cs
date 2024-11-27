using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonOutlineColorOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Button button;  // ��ư ������Ʈ
    public Color hoverColor = Color.green;  // ���콺�� �÷����� �� �ܰ��� ����
    public Color normalColor = Color.white;  // �⺻ �ܰ��� ����

    private Outline outline;  // �ܰ��� ������Ʈ

    void Start()
    {
        outline = button.GetComponent<Outline>();  // ��ư�� Outline ������Ʈ ����
        if (outline == null)
        {
            outline = button.gameObject.AddComponent<Outline>();  // ������ �߰�
        }
        outline.effectColor = normalColor;  // �⺻ �ܰ��� ���� ����
    }

    // �����Ͱ� ��ư ���� ������ �� ȣ��Ǵ� �Լ�
    public void OnPointerEnter(PointerEventData eventData)
    {
        outline.effectColor = hoverColor;  // �ܰ��� ���� ����
    }

    // �����Ͱ� ��ư�� ����� �� ȣ��Ǵ� �Լ�
    public void OnPointerExit(PointerEventData eventData)
    {
        outline.effectColor = normalColor;  // ���� �������� ����
    }
}
