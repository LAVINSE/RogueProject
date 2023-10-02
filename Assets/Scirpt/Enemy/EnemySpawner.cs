using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    #region 변수
    [SerializeField] private List<EnemyTable> EnemyTableList = new List<EnemyTable>();
    [SerializeField] private List<Transform> EnemySpawnPointList = new List<Transform>();
    [SerializeField] private GameObject EnemyRoot = null;
    #endregion // 변수

    #region 함수
    /** 적을 소환한다 */
    private void SpawnEnemy(string EnemyName)
    {
        var EnemyObject = EnemyObjectPool(EnemySelect(EnemyName));
        var Enemy = EnemyObject.GetComponent<EnemySetting>();
        Enemy.EnemyInfoSetting(EnemySelect(EnemyName));
    }

    /** 적을 선택한다 */
    private EnemyTable EnemySelect(string EnemyName)
    {
        for (int i = 0; i < EnemyTableList.Count; i++)
        {
            if (EnemyTableList[i].EnemyName == EnemyName)
            {
                return EnemyTableList[i];
            }
        }

        return null;
    }

    // TODO : 테이블에서 특정 몬스터만 서치에서 사용하게 변경 예정
    /** 적 오브텍트 풀링 */
    private EnemySetting EnemyObjectPool(EnemyTable sd)
    {
        var Enemy = StageManager.Instance.PoolManager.SpawnObj<EnemySetting>(() =>
        {
            return CFactory.CreateCloneObj(sd.EnemyName, sd.EnemyPrefab, EnemyRoot,
                Vector3.zero, Vector3.one, Vector3.zero);
        }) as GameObject;

        Enemy.SetActive(true);
        return Enemy.GetComponent<EnemySetting>();
    }

    #endregion // 함수
}
