using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    #region 변수
    [Header("=====> 카메라 <=====")]
    [SerializeField] private Transform FollowingTarget = null;
    [SerializeField] private Vector2 CanMoveAreaCenter = Vector2.zero; // 카메라 이동 가능영역 중심
    [SerializeField] private Vector2 CanMoveAreaSize = Vector2.zero; // 카메라 이동 가능영역 크기
    [SerializeField] private float CameraMoveSpeed = 0.0f; // 카메라 이동 속도

    private float CameraWidth; // 카메라 월드 공간에서의 가로 길이
    private float CameraHeight; // 카메라 월드 공간에서의 세로 길이
    #endregion // 변수

    #region 함수
    /** 초기화 */
    private void Awake()
    {
        CameraHeight = Camera.main.orthographicSize;
        CameraWidth = Screen.width * CameraHeight / Screen.height;
    }

    /** 초기화 */
    private void Start()
    {
        FollowingTarget = GameObject.Find("Player").transform;
    }

    /** 초기화 => 상태를 갱신한다 */
    private void LateUpdate()
    {
        FollowTarget();
    }

    /** 카메라 타겟에 따라 이동이 가능한 영역을 표시한다*/
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(CanMoveAreaCenter, CanMoveAreaSize);
    }

    /** 타겟을 추적한다 */
    private void FollowTarget()
    {
        Vector3 TargetPosition = new Vector3(FollowingTarget.position.x,
                                            FollowingTarget.position.y,
                                            -10.0f);
        this.transform.position = Vector3.Lerp(this.transform.position, TargetPosition,
                                                CameraMoveSpeed * Time.deltaTime);

        float RestrictionAreaX = CanMoveAreaSize.x * 0.5f - CameraWidth;
        float RestirctionAreaY = CanMoveAreaSize.y * 0.5f - CameraHeight;

        float ClampX = Mathf.Clamp(this.transform.position.x,
                                    -RestrictionAreaX + CanMoveAreaCenter.x,
                                    RestrictionAreaX + CanMoveAreaCenter.x);

        float ClampY = Mathf.Clamp(this.transform.position.y,
                                    -RestirctionAreaY + CanMoveAreaCenter.y,
                                    RestirctionAreaY + CanMoveAreaCenter.y);

        this.transform.position = new Vector3(ClampX, ClampY, -10.0f);
    }
    #endregion // 함수
}
