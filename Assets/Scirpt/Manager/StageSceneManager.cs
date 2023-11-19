using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class StageSceneManager : CSceneManager
{
    #region 변수
    [SerializeField] private GameObject PlayerSpawnPoint = null;
    [SerializeField] private TilemapRenderer OutLineTileMap = null;

    private bool IsShowInven = false;

    private Inventory PlayerInven;

    private GameObject InventoryObj;
    private GameObject StateBarObj;
    private GameObject PlayerObj;
    #endregion // 변수

    #region 프로퍼티
    #endregion // 프로퍼티

    /** 초기화 */
    private void Start()
    {
        // 스테이지 설정
        StageSetting();
    }

    /** 초기화 => 상태를 갱신한다 */
    private void Update()
    {
        // 인벤토리를 보여준다
        ShowInven();

        // TODO : 함수로 만들기
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            // 옵션창이 활성화 되어있을 경우
            if(OptionObj.activeSelf == true)
            {
                OptionObj.SetActive(false);
            }
            else
            {
                OptionObj.SetActive(true);
            }   
        }
    }

    /** 스테이지 설정 */
    private void StageSetting()
    {
        // 아웃라인타일맵이 있을 경우
        if(OutLineTileMap != null)
        {
            // 아웃라인타일 렌더러 비활성화
            OutLineTileMap.enabled = false;
        }

        // 상태바 생성
        StateBarObj = CreateStateBar().gameObject;

        // 설정창 생성
        OptionObj = CreateOptionSetting().gameObject;

        // 옵션버튼 생성
        CreateOptionButton();

        // 플레이어 생성
        PlayerObj = CreatePlayer();

        // 플레이어 위치 설정
        PlayerObj.transform.position = PlayerSpawnPoint.transform.position;

        // 카메라 설정
        CameraManager.Instance.oFollowingTarget = PlayerObj.transform;

        // 인벤토리 생성
        this.InventoryObj = CreateInventroy().gameObject;

        // 생성된 인벤토리 컴포넌트 가져오기, 비활성화
        PlayerInven = InventoryObj.GetComponent<Inventory>();
        this.InventoryObj.SetActive(false);
    }

    /** 인벤토리를 활성화/비활성화 한다 */
    private void ShowInven()
    {
        // I 키를 눌렀을 경우
        if (Input.GetKeyDown(KeySetting.Keys[UserKeyAction.Inventory]))
        {
            // 인벤토리 슬롯 업데이트
            PlayerInven.UpdateSlot();

            // 활성화, 비활성화
            IsShowInven = !IsShowInven;
            this.InventoryObj.SetActive(IsShowInven);
        }
    }
}
