using UnityEngine;

public class Player : MonoBehaviour
{
    #region 변수
    [Header("=====> 애니메이션 <=====")]
    [SerializeField] private Animator PlayerAnimator;
    [SerializeField] private Animator SwordAnimator;
    [SerializeField] private Animator WeaponAnimator;

    [Header("=====> 기본공격 설정 <=====")]
    [Space]
    [SerializeField] private Vector2 MeleeSize; // 공격 범위
    [SerializeField] private Transform MeleePos; // 공격이 나가는 위치

    [Header("=====> 플레이어 정보 <=====")]
    [Space]
    [SerializeField] private float PlayerMaxHp = 0f;
    [SerializeField] private float PlayerCurrentHp = 0f;
    [SerializeField] private float PlayerMaxMana = 0f;
    [SerializeField] private float PlayerCurrentMana = 0f;
    [SerializeField] private float PlayerAtk =0f;
    [SerializeField] private float PlayerBasicAtkCoolTime =0f;
    [SerializeField] private int PlayerLevel = 0;

    private SpriteRenderer PlayerSprite;
    #endregion // 변수

    #region 프로퍼티 
    public int oPlayerCurrentGold { get; set; }
    public NPC oNPC { get; set; }
    #endregion // 프로퍼티

    #region 함수
    /** 확인용 */
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(MeleePos.position, MeleeSize);
    }

    /** 초기화 */
    private void Awake()
    {
        PlayerSprite = GetComponent<SpriteRenderer>();
        SettingPlayerData();
    }

    /** 초기화 */
    private void Start()
    {
        PlayerCurrentHp = PlayerMaxHp;
        PlayerCurrentMana = PlayerMaxMana;
    }

    /** 초기화 => 상태를 갱신한다 */
    private void Update()
    {
        // 상태바 업데이트
        StateBar.Instance.UpdateStateBar(PlayerMaxHp, PlayerCurrentHp, PlayerMaxMana,
            PlayerCurrentMana, oPlayerCurrentGold, PlayerLevel, PlayerAtk,PlayerBasicAtkCoolTime,
            PlayerSprite);

        // 기본공격
        BasicAtk();

        // 상호작용 키
        SettingInteractionKey();
    }

    /** 초기화 => 접촉중인 상태일때 */
    private void OnTriggerStay2D(Collider2D collision)
    {
        // 태그가 Item 이면
        if (collision.gameObject.CompareTag("Item"))
        {
            ItemAdd GetItem = collision.gameObject.GetComponent<ItemAdd>();

            // Z 키를 눌렀을 때, 아이템이 존재 할때
            if (Input.GetKey(KeySetting.Keys[UserKeyAction.Pickup]) && GetItem != null)
            {
                AudioManager.Inst.PlaySFX(SFXEnum.PickItem);
                ItemInfoTable Item = GetItem.ItemAdd();
                var AddItem = Inventory.Instance.AddItem(Item);

                // 아이템을 인벤토리에 넣었으면 
                if (AddItem == true)
                {
                    Destroy(collision.gameObject);
                }
            }
        }
    }

    /** 플레이어 저장된 데이터 세팅 */
    private void SettingPlayerData()
    {
        PlayerMaxHp = GameManager.Inst.oPlayerMaxHp;
        PlayerMaxMana = GameManager.Inst.oPlayerMaxMana;
        PlayerLevel = GameManager.Inst.oPlayerLevel;
        oPlayerCurrentGold = GameManager.Inst.oPlayerGold;
        PlayerAtk = GameManager.Inst.oPlayerAtk;
        PlayerBasicAtkCoolTime = GameManager.Inst.oPlayerBasicAtkCoolTime;
    }

    /** 상호작용 키를 설정한다 */
    private void SettingInteractionKey()
    {
        // F키를 눌를 경우
        if (Input.GetKeyDown(KeySetting.Keys[UserKeyAction.Interaction]) && CSceneManager.Instance.OptionObj.activeSelf == false)
        {
            Interaction();
        }
    }

    /** 접촉한 대상과 상호작용을 한다 */
    private void Interaction()
    {
        // 대상 정보가 없을 경우
        if (oNPC == null)
        {
            return;
        }
        
        // NPC 상호작용
        oNPC.NPCInteraction();
    }

    /** 플레이어 기본 공격 */
    private void BasicAtk()
    {
        // 공격 준비 상태가 되었을 경우
        if (GameManager.Inst.IsBasicAttack == true)
        {
            // Q 키를 눌렀을 때
            if (Input.GetKey(KeySetting.Keys[UserKeyAction.Skill_Q]))
            {
                GameManager.Inst.IsBasicAttack = false;
                AudioManager.Inst.PlaySFX(SFXEnum.BasicAttack);
                // 공격
                PlayerAnimator.SetTrigger("Attack");
                WeaponAnimator.SetTrigger("Attack");
                SwordAnimator.SetTrigger("Attack");

                Collider2D[] Collider2DList = Physics2D.OverlapBoxAll(MeleePos.position, MeleeSize, 0);
                foreach(Collider2D Collider in Collider2DList)
                {
                    if(Collider.gameObject.CompareTag("Enemy"))
                    {
                        var Enemy = Collider.GetComponent<EnemySetting>();
                        Enemy.TakeDamageOnHit(PlayerAtk);
                    }
                }

                // 쿨타임 실행
                StartCoroutine(StateBar.Instance.CheckCoolTime(0, PlayerBasicAtkCoolTime));
            }
        }
    }

    /** 타격을 받는다 */
    public void TakeDamage(float Damage)
    {
        PlayerCurrentHp -= Damage;
    }
    #endregion // 함수
}
