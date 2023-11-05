using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemySpawnerManager;
using static EnemyTable;

public class EnemySpawnerManager : MonoBehaviour
{
    #region 변수
    [Header("=====> 적 스크립트 오브젝트 <=====")]
    [SerializeField] private List<EnemyTable> EnemyTableList = new List<EnemyTable>();

    [Header("=====> 적 스폰 위치 <=====")]
    [SerializeField] private List<Transform> EnemySpawnNormalPoint = new List<Transform>();
    [SerializeField] private List<Transform> EnemySpawnElitePoint = new List<Transform>();
    [SerializeField] private List<Transform> EnemySpawnBossPoint = new List<Transform>();
    [SerializeField] private GameObject EnemyRoot = null;

    [Header("=====> 인스펙터 제어 변수 <=====")]
    [SerializeField] private bool IsStart = false; // 스테이지 입장 했는지 확인하는 변수
    #endregion // 변수

    #region 프로퍼티
    #endregion // 프로퍼티

    #region 함수
    /** 초기화 => 상태를 갱신한다 */
    private void Update()
    {
        EnterplayerSpawnEnemy();
    }

    /** 스테이지에 플레이어가 입장했을 경우 */
    private void EnterplayerSpawnEnemy()
    {
        // 스테이지에 입장 했을 경우
        if (IsStart == true)
        {
            for (int i = 0; i < EnemySpawnNormalPoint.Count; i++)
            {
                SpawnEnemy(EnemyTableList, EnemySpawnNormalPoint, EnemyRoot, EnemyType.Normal);
            }
            IsStart = false;
        }
    }

    /** 적을 소환한다 */
    public void SpawnEnemy(List<EnemyTable> EnemyTableList, List<Transform> EnemySpawnPoint,
        GameObject EnemyRoot, EnemyTable.EnemyType EnemyType)
    {
        var EnemyObject = EnemyObjectPool(EnemySelect(EnemyTableList, EnemyType), EnemyRoot);
        var Enemy = EnemyObject.GetComponent<EnemySetting>();
        Enemy.EnemyInfoSetting(EnemySelect(EnemyTableList, EnemyType));

        for(int i = 0; i< EnemySpawnPoint.Count; i++)
        {
            // EnemySpawnPonint가 활성화 상태일때
            if (EnemySpawnPoint[i].gameObject.activeSelf)
            {
                EnemyObject.transform.position = EnemySpawnPoint[i].transform.position;
                EnemySpawnPoint[i].gameObject.SetActive(false);
                break;
            }
        }
    }

    /** 적을 선택한다 */
    private EnemyTable EnemySelect(List<EnemyTable> EnemyTableList, EnemyTable.EnemyType EnemyType)
    {
        for (int i = 0; i < EnemyTableList.Count; i++)
        {
            if (EnemyTableList[i].oEnemyType == EnemyType)
            {
                return EnemyTableList[i];
            }
        }

        return null;
    }

    // TODO : 테이블에서 특정 몬스터만 서치에서 사용하게 변경 예정
    /** 적 오브텍트 풀링 */
    private EnemySetting EnemyObjectPool(EnemyTable EnemyTableScript, GameObject EnemyRoot)
    {
        var Enemy = GameManager.Inst.PoolManager.SpawnObj<EnemySetting>(() =>
        {
            return CFactory.CreateCloneObj(EnemyTableScript.oEnemyType.ToString(), EnemyTableScript.EnemyPrefab,
                EnemyRoot, Vector3.zero, Vector3.one, Vector3.zero);
        }) as GameObject;

        Enemy.SetActive(true);
        return Enemy.GetComponent<EnemySetting>();
    }
    #endregion // 함수
}
