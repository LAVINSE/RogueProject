using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class StageSceneManager : CSceneManager
{
    #region 변수
    [SerializeField] private GameObject Player = null;
    [SerializeField] private GameObject PlayerSpawnPoint = null;
    [SerializeField] private TilemapRenderer OutLineTileMap = null;

    private Inventory PlayerInven;
    private GameObject InventoryObj;
    private GameObject StateBarObj;
    private bool IsShowInven = false;
    #endregion // 변수

    #region 프로퍼티
    public static StageSceneManager Instance { get; private set; }
    #endregion // 프로퍼티

    /** 초기화 */
    public override void Awake()
    {
        base.Awake();
        Instance = this;

        StageSetting();
    }

    /** 초기화 */
    private void Start()
    {
        PlayerInven.oItemList = GameManager.Inst.PlayerItemList;
    }

    /** 초기화 => 상태를 갱신한다 */
    private void Update()
    {
        ShowInven();
    }

    /** 스테이지 설정 */
    private void StageSetting()
    {
        // 플레이어 위치 설정
        Player.transform.position = PlayerSpawnPoint.transform.position;

        // 아웃라인타일 렌더러 비활성화
        OutLineTileMap.enabled = false;

        // 인벤토리 생성
        this.InventoryObj = CreateInventroy().gameObject;

        // 생성된 인벤토리 컴포넌트 가져오기, 비활성화
        PlayerInven = InventoryObj.GetComponent<Inventory>();
        this.InventoryObj.SetActive(false);

        StateBarObj = CreateStateBar().gameObject;
       
    }

    /** 인벤토리를 활성화/비활성화 한다 */
    private void ShowInven()
    {
        // I 키를 눌렀을 경우
        if (Input.GetKeyDown(KeyCode.I))
        {
            // 인벤토리 슬롯 업데이트
            PlayerInven.UpdateSlot();

            // 활성화, 비활성화
            IsShowInven = !IsShowInven;
            this.InventoryObj.SetActive(IsShowInven);
        }
    }
}
