using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RightClick : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            // 우클릭 시 실행될 함수 호출
            Debug.Log(gameObject.name+" Right Clicked!");

            // 원하는 함수 호출
            // YourFunction();
        }
    }
}
