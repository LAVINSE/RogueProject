using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class StageManager : MonoBehaviour
{
    #region 변수
    [SerializeField] private GameObject Player = null;
    [SerializeField] private GameObject PlayerSpawnPoint = null;
    [SerializeField] private TilemapRenderer OutLineTileMap = null;
    #endregion // 변수

    #region 프로퍼티
    public ObjectPoolManager PoolManager { get; private set; }
    public static StageManager Instance { get; private set; }
    #endregion // 프로퍼티

    #region 함수
    /** 초기화 */
    private void Awake()
    {
        Instance = this;
        Player.transform.position = PlayerSpawnPoint.transform.position;
        OutLineTileMap.enabled = false;
        PoolManager = CFactory.CreateObject<ObjectPoolManager>("ObjectPoolManager", this.gameObject,
            Vector3.zero, Vector3.one, Vector3.zero);
    }
    #endregion // 함수
}
