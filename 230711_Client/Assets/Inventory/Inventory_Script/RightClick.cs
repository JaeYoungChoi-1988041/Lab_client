using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RightClick : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            // ��Ŭ�� �� ����� �Լ� ȣ��
            Debug.Log(gameObject.name+" Right Clicked!");

            // ���ϴ� �Լ� ȣ��
            // YourFunction();
        }
    }
}
