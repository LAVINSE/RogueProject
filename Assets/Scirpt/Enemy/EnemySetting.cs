using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WeightDropTable;

public class EnemySetting : MonoBehaviour
{
    #region 변수
    [Header("=====> 적 정보 <=====")]
    [SerializeField] private EnemyTable.EnemyType oEnemyType;
    [SerializeField] private float EnemyHp = 0.0f;
    [SerializeField] private float EnemyAtk = 0.0f;

    [Space]
    [SerializeField] private WeightDropTable EnemyDrop;

    [Header("=====> 인스펙터 <=====")]
    [SerializeField] private EnemyTable oEnemyTable;
    #endregion // 변수

    #region 함수
    /** 초기화 => 상태를 갱신한다 */
    private void Update()
    {
        if(EnemySpawnerManager.Instance.IsEnemyAllKill == true)
        {
            EnemyDie();
        }
    }

    /** 적 정보를 세팅한다 */
    public void EnemyInfoSetting(EnemyTable EnemyScriptTable)
    {
        this.oEnemyTable = EnemyScriptTable;
        this.oEnemyType = this.oEnemyTable.oEnemyType;
        this.EnemyHp = this.oEnemyTable.EnemyHp;
        this.EnemyAtk = this.oEnemyTable.EnemyAtk;
    }

    /** 데미지를 받는다 */
    public void TakeDamage(float Damage)
    {
        // 피격 모션 추가예정
        EnemyHp -= Damage;
    }

    /** 적이 죽었을 때 */
    public void EnemyDie()
    {
        // 아이템 드랍
        var Drop = EnemyDrop.ItemDrop();

        // 아이템이 선택됐을 경우
        if(Drop != null)
        {
            // 아이템 생성
            Instantiate(Drop.ItemPrefab, this.transform.position, Quaternion.identity);   
        }
        
        GameManager.Inst.oPoolManager.DeSpawnObj<EnemySetting>(this.gameObject, CompleteDespawn);
    }

    /** 적 비활성화가 완료 되었을 경우 */
    private void CompleteDespawn(object Obj)
    {
        (Obj as GameObject).SetActive(false);
    }
    #endregion // 함수
}
