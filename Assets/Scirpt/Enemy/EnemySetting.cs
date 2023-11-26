using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static WeightDropTable;

public class EnemySetting : MonoBehaviour
{
    #region 변수
    [Header("=====> 적 정보 <=====")]
    [SerializeField] private EnemyTable.EnemyType oEnemyType;
    [SerializeField] private float EnemyMaxHp = 0.0f;
    [SerializeField] private float EnemyHp = 0.0f;
    [SerializeField] private float EnemyAtk = 0.0f;
    [SerializeField] private int EnemyGold = 0;
    [SerializeField] private float EnemyMoveSpeed = 0.0f;
    [SerializeField] private float EnemyTrackingRange = 0.0f;
    [SerializeField] private float EnemyAttackRange = 0.0f;
    
    [Header("=====> 적 기본공격 설정 <=====")]
    [Space]
    [SerializeField] private Transform MeleePos;
    [SerializeField] private Vector2 MeleeSize;

    [Header("=====> 적 드랍정보 <=====")]
    [Space]
    [SerializeField] private WeightDropTable EnemyDrop;

    [Header("=====> 적 이미지 <=====")]
    [Space]
    [SerializeField] private SpriteRenderer Mount = null;
    [SerializeField] private SpriteRenderer Body = null;
    [SerializeField] private SpriteRenderer WingRight = null;
    [SerializeField] private SpriteRenderer WingLeft = null;

    [Header("=====> 적 UI <=====")]
    [Space]
    [SerializeField] private Image HpBarImg = null;

    [Header("=====> 인스펙터 <=====")]
    [Space]
    [SerializeField] private EnemyTable oEnemyTable;

    private Rigidbody2D Rigid2D;
    private int NextMove;
    #endregion // 변수

    #region 프로퍼티
    public EnemyState oEnemyState { get; set; }
    public Transform oMeleePos
    {
        get => MeleePos;
        set => MeleePos = value;
    }
    public Animator oEnemyAnimator { get; set; }

    public float oEnemyTrackingRange
    {
        get => EnemyTrackingRange;
        set => EnemyTrackingRange = value;
    }
    public float oEnemyAttackRange
    {
        get => EnemyAttackRange;
        set => EnemyAttackRange = value;
    }
    public float oEnemyMoveSpeed
    {
        get => EnemyMoveSpeed;
        set => EnemyMoveSpeed = value;
    }

    public int oEnemyGold
    {
        get => EnemyGold;
        set => EnemyGold = value;
    }

    public bool oIsEnemyMove { get; set; } = true;
    #endregion // 프로퍼티

    #region 함수
    /** 확인용 */
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(MeleePos.position, MeleeSize);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(this.transform.position, this.oEnemyTrackingRange);

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(this.transform.position, this.oEnemyAttackRange);
    }

    /** 초기화 */
    private void Awake()
    {
        oEnemyState = GetComponent<EnemyState>();
        oEnemyAnimator = GetComponent<Animator>();
        Rigid2D = GetComponent<Rigidbody2D>();

        // 적 좌우 자유 이동
        Invoke("MoveEnemy", 3);
    }

    /** 초기화 => 상태를 갱신한다 */
    private void Update() 
    {
        HpBarImg.fillAmount = EnemyHp / EnemyMaxHp;
        if (EnemySpawnerManager.Instance.oIsEnemyAllKill == true)
        {
            EnemyDie();
        }  
    }

    /** 초기화 => 상태를 갱신한다 */
    private void FixedUpdate()
    {
        Rigid2D.velocity = new Vector2(NextMove * EnemyMoveSpeed, Rigid2D.velocity.y);
    }

    /** 적 비활성화가 완료 되었을 경우 */
    private void CompleteDespawn(object Obj)
    {
        (Obj as GameObject).SetActive(false);
    }

    /** 적 정보를 세팅한다 */
    public void EnemyInfoSetting(EnemyTable EnemyScriptTable)
    {
        this.oEnemyTable = EnemyScriptTable;
        this.oEnemyType = this.oEnemyTable.oEnemyType;

        this.EnemyMaxHp = this.oEnemyTable.EnemyHp;
        this.EnemyAtk = this.oEnemyTable.EnemyAtk;
        this.EnemyGold = this.oEnemyTable.EnemyGold;
        this.EnemyTrackingRange = this.oEnemyTable.EnemyTrackingRange;
        this.EnemyAttackRange = this.oEnemyTable.EnemyAttackRange;
        this.EnemyMoveSpeed = this.oEnemyTable.EnemyMoveSpeed;

        EnemyHp = this.EnemyMaxHp;
    }

    /** 적 filpX를 설정한다 */
    public void EnemyfilpX(bool IsfilpX)
    {
        Mount.flipX = IsfilpX;
        Body.flipX = IsfilpX;
        WingRight.flipX = IsfilpX;
        WingLeft.flipX = !IsfilpX;
    }

    /** 타격을 받는다 */
    public void TakeDamageOnHit(float Damage)
    {
        // 적이 죽었는지 확인하고
        // 안 죽었으면, 데미지를 받고, 적이 죽었는지 확인하고, 아닐경우 HIt State실행
        // 피격 모션 추가예정

        // 적이 죽지 않았을 경우
        if (EnemyIsDie() == false)
        {
            EnemyHp -= Damage;

            // 적이 죽었을 경우
            if(EnemyIsDie() == true)
            {
                oEnemyState.ChangeState(EnemyState.EnemyStateType.Dead);
            }
            else
            {
                oEnemyState.ChangeState(EnemyState.EnemyStateType.Hit);
            }
        }
    }

    /** 적 기본공격 */
    public void EnemyBasicAttack()
    {
        // 공격 애니메이션
        oEnemyAnimator.SetTrigger("Attack");

        Collider2D[] Collider2DList = Physics2D.OverlapBoxAll(MeleePos.position, MeleeSize, 0);
        foreach (Collider2D Collider in Collider2DList)
        {
            if (Collider.gameObject.CompareTag("Player"))
            {
                var Player = Collider.GetComponent<Player>();
                Player.TakeDamage(EnemyAtk);
            }
        }  
    }

    /** 적이 좌우로 돌아다닌다 */
    public void MoveEnemy()
    {
        if (oIsEnemyMove == false) return;

        NextMove = Random.Range(-1, 2);

        if (NextMove == -1)
        {
            EnemyfilpX(false);
        }
        else if (NextMove == 1)
        {
            EnemyfilpX(true);
        }

        Invoke("MoveEnemy", 3);
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
   
        EnemySpawnerManager.Instance.oEnemyList.Remove(this);
        GameManager.Inst.oPoolManager.DeSpawnObj<EnemySetting>(this.gameObject, CompleteDespawn);
    }

    /** 적이 죽었는지 검사한다 */
    public bool EnemyIsDie()
    {
        return this.EnemyHp <= 0.0f;
    }
    #endregion // 함수
}
