using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : CSingleton<GameManager>
{
    #region 변수
    [SerializeField] private float PlayerMaxHp = 0f;
    [SerializeField] private float PlayerMaxMana = 0f;
    [SerializeField] private int PlayerLevel = 0;
    [SerializeField] private float PlayerAtk = 0f;
    [SerializeField] private float PlayerBasicAtkCoolTime = 0f;
    #endregion // 변수

    #region 프로퍼티
    public ObjectPoolManager oPoolManager { get; private set; }
    public List<ItemInfoTable> oPlayerItemList = new List<ItemInfoTable>();

    public bool IsBasicAttack { get; set; } = true;

    public int oPlayerLevel
    {
        get => PlayerLevel;
        set => PlayerLevel = Mathf.Max(0,value);
    }

    public float oPlayerMaxMana
    {
        get => PlayerMaxMana;
        set => PlayerMaxMana = Mathf.Max(0, value);
    }

    public float oPlayerMaxHp
    { 
        get => PlayerMaxHp;
        set => PlayerMaxHp = Mathf.Max(0, value);
    }

    public float oPlayerAtk
    {
        get => PlayerAtk;
        set => PlayerAtk = Mathf.Max(0, value);
    }
    public float oPlayerBasicAtkCoolTime
    {
        get => PlayerBasicAtkCoolTime;
        set => PlayerBasicAtkCoolTime = Mathf.Max(0, value);
    }
    #endregion // 프로퍼티

    #region 함수
    /** 초기화 */
    public override void Awake()
    {
        base.Awake();
        oPoolManager = CFactory.CreateObject<ObjectPoolManager>("ObjectPoolManager", this.gameObject,
            Vector3.zero, Vector3.one, Vector3.zero);
    }

    /** 플레이어 데이터를 저장한다 */
    public void PlayerDataSave(float PlayerMaxHp, float PlayerAtk, float PlayerBasicAtkCoolTime)
    {
        oPlayerAtk = PlayerAtk;
        oPlayerBasicAtkCoolTime = PlayerBasicAtkCoolTime;
        oPlayerMaxHp = PlayerMaxHp;
    }
    #endregion // 함수
}
