using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : CSingleton<GameManager>
{
    #region 변수
    private bool IsInventory = false;

    private GameObject Inventory = null;
    #endregion // 변수

    #region 함수
    /** 초기화 */
    public override void Awake()
    {
        base.Awake();
        Inventory = CreateInventroy();
    }

    /** 초기화 => 상태를 갱신한다 */
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            IsInventory = !IsInventory;
            Inventory.SetActive(IsInventory);
        }
    }

    /** 인벤토리를 생성한다 */
    private GameObject CreateInventroy()
    {
        var Inventory = CFactory.CreateCloneObj("Inventory",
            Resources.Load<GameObject>("Prefabs/UI/InventoryUI"),
            GameObject.Find("Canvas"), Vector3.zero, Vector3.one, Vector3.zero);

        Inventory.SetActive(false);
        return Inventory;
    }
    #endregion // 함수
}
