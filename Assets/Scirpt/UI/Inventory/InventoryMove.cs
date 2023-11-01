using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryMove : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    #region 변수
    [SerializeField] private Transform TargetTransform;

    private Vector2 BeginPoint; // 시작 위치
    private Vector2 MovePoint; // 움직일 위치
    #endregion // 변수
    /** 초기화 */
    private void Awake()
    {
        // 타겟 위치가 없을 경우
        if(TargetTransform == null)
        {
            TargetTransform = transform.parent;
        }
    }

    /** 드래그 시작 위치 지정 */
    public void OnPointerDown(PointerEventData eventData)
    {
        BeginPoint = TargetTransform.position;
        MovePoint = eventData.position;
    }

    /** 마우스 커서 위치로 이동 */
    public void OnDrag(PointerEventData eventData)
    {
        TargetTransform.position = BeginPoint + (eventData.position - MovePoint);
    }
}
