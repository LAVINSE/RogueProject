using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : CSingleton<GameManager>
{
    #region 변수
    #endregion // 변수

    #region 프로퍼티
    public ObjectPoolManager PoolManager { get; private set; }
    public List<ItemInfo> PlayerItemList = new List<ItemInfo>();
    #endregion // 프로퍼티

    #region 함수
    /** 초기화 */
    public override void Awake()
    {
        base.Awake();
        PoolManager = CFactory.CreateObject<ObjectPoolManager>("ObjectPoolManager", this.gameObject,
            Vector3.zero, Vector3.one, Vector3.zero);
    }

    /** 초기화 => 상태를 갱신한다 */
    private void Update()
    {
        PlayerItemList = Inventory.Instance.oItemList;
    }
    #endregion // 함수
}
