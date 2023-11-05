using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSceneManager : MonoBehaviour
{
    #region 변수
    #endregion // 변수

    #region 프로퍼티
    public GameObject InventoryRoot { get; private set; }
    #endregion // 프로퍼티

    #region 함수 
    /** 초기화 */
    public virtual void Awake()
    {
        var RootObjs = this.gameObject.scene.GetRootGameObjects();

        for(int i =0; i < RootObjs.Length; i++)
        {
            this.InventoryRoot = this.InventoryRoot ??
                RootObjs[i].transform.Find("Canvas/InventoryRoot")?.gameObject;
        }
    }

    /** 인벤토리를 생성한다 */
    public Inventory CreateInventroy()
    {
        var Inventory = CFactory.CreateCloneObj<Inventory>("Inventory",
            Resources.Load<GameObject>("Prefabs/UI/Inventory"),
            InventoryRoot, Vector3.zero, Vector3.one, Vector3.zero);

        return Inventory;
    }
    #endregion // 함수
}
