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
        // I 키를 눌렀을 경우
        if(Input.GetKeyDown(KeyCode.I))
        {
            PlayerInven.UpdateSlot();
            IsShowInven = !IsShowInven;
            this.InventoryObj.SetActive(IsShowInven);
        }
    }

    /** 스테이지 설정 */
    private void StageSetting()
    {
        Player.transform.position = PlayerSpawnPoint.transform.position;
        OutLineTileMap.enabled = false;

        this.InventoryObj = CreateInventroy().gameObject;
        PlayerInven = InventoryObj.GetComponent<Inventory>();
        this.InventoryObj.SetActive(false);
    }
}
