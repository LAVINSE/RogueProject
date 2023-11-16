using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSceneManager : MonoBehaviour
{
    #region 변수
    #endregion // 변수

    #region 프로퍼티
    public GameObject InventoryRoot { get; private set; }
    public GameObject PublicRoot { get; private set; }
    public GameObject Object { get; private set; }
    #endregion // 프로퍼티

    #region 함수 
    /** 초기화 */
    public virtual void Awake()
    {
        var RootObjs = this.gameObject.scene.GetRootGameObjects();

        for(int i =0; i < RootObjs.Length; i++)
        {
            this.Object = this.Object ??
                RootObjs[i].transform.Find("Object")?.gameObject;
            this.InventoryRoot = this.InventoryRoot ??
                RootObjs[i].transform.Find("Canvas/InventoryRoot")?.gameObject;
            this.PublicRoot = this.PublicRoot ??
                RootObjs[i].transform.Find("Canvas/PublicRoot")?.gameObject;
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

    /** 상태바를 생성한다 */
    public StateBar CreateStateBar()
    {
        var StateBar = CFactory.CreateCloneObj<StateBar>("StateBar",
            Resources.Load<GameObject>("Prefabs/UI/StateBar"), PublicRoot,
            Vector3.zero, Vector3.one, Vector3.zero);

        var StateRect = StateBar.GetComponent<RectTransform>();
        StateRect.anchorMin = new Vector2(0, 1);
        StateRect.anchorMax = new Vector2(0, 1);
        StateRect.pivot = new Vector2(0, 1);

        

        return StateBar;
    }

    /** 플레이어를 생성한다 */
    public GameObject CreatePlayer()
    {
        var Player = CFactory.CreateCloneObj("Player", Resources.Load<GameObject>("Prefabs/Player/Player"),
            Object, Vector3.zero, Vector3.one, Vector3.zero);

        return Player;
    }
    #endregion // 함수
}
