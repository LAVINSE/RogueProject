using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : CSingleton<GameManager>
{
    #region 변수
    [SerializeField] private float PlayerMaxHp = 20f;
    [SerializeField] private float PlayerMaxMana = 20f;
    [SerializeField] private float PlayerAtk = 5f;
    [SerializeField] private int PlayerGold = 10;
    [SerializeField] private int PlayerLevel = 0;
    [SerializeField] private float PlayerBasicAtkCoolTime = 2f;

    #endregion // 변수

    #region 프로퍼티
    public ObjectPoolManager oPoolManager { get; private set; }
    public List<ItemInfoTable> oPlayerItemList = new List<ItemInfoTable>();

    public int oPlayerGold
    {
        get => PlayerGold;
        set => PlayerGold = Mathf.Max(0, value);

    }
    public int oPlayerLevel
    {
        get => PlayerLevel;
        set => PlayerLevel = Mathf.Max(0, value);
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
    public bool IsBasicAttack { get; set; } = true;
    #endregion // 프로퍼티

    #region 함수
    /** 초기화 */
    public override void Awake()
    {
        base.Awake();
        oPoolManager = CFactory.CreateObject<ObjectPoolManager>("ObjectPoolManager", this.gameObject,
            Vector3.zero, Vector3.one, Vector3.zero);

    }

    /** 씬을 변경한다 */
    public void ChangeScene(string SceneName)
    {
        DataManager.Inst.JsonSave();
        LoadingScene.LoadScene(SceneName);
    }
    #endregion // 함수
}
