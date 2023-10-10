using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryMove : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    #region 변수
    private Vector2 BeginPoint; // 시작 위치
    private Vector2 MovePoint; // 움직일 위치
    #endregion // 변수

    /** 드래그 시작 위치 지정 */
    public void OnPointerDown(PointerEventData eventData)
    {
        BeginPoint = this.transform.position;
        MovePoint = eventData.position;
    }

    /** 마우스 커서 위치로 이동 */
    public void OnDrag(PointerEventData eventData)
    {
        this.transform.position = BeginPoint + (eventData.position - MovePoint);
    }
}
