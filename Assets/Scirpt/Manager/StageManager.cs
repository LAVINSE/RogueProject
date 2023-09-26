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

    #region 함수
    /** 초기화 */
    private void Awake()
    {
        Player.transform.position = PlayerSpawnPoint.transform.position;
        OutLineTileMap.enabled = false;
    }
    #endregion // 함수
}
