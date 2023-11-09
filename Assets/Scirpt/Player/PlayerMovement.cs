using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region 변수
    [Header("=====> 플레이어 Move 값 <=====")]
    [SerializeField] private float PlayerSpeed = 3;
    [SerializeField] private float PlayerJumpPower = 10;
    [SerializeField] private int JumpCount = 0; // 더블 점프 변수

    [Space]
    [SerializeField] private Transform Pos;
    [SerializeField] private float Radius;
    [SerializeField] private LayerMask Layer;
    

    private Rigidbody2D Rigid2D;
    private SpriteRenderer SpriteRender;
    private Animator PlayerAnimator;

    private int JumtCnt;
    private bool IsGround;
    #endregion // 변수


    #region 함수
    /** 확인용 */
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(Pos.position, Radius);
    }

    /** 초기화 */
    private void Awake()
    {
        Rigid2D = GetComponentInParent<Rigidbody2D>();
        SpriteRender = GetComponentInParent<SpriteRenderer>();
        PlayerAnimator = GetComponentInParent<Animator>();

        JumtCnt = JumpCount;
    }

    /** 초기화 => 상태를 갱신한다 */
    private void Update()
    {
        // 좌우 이동 모션 변경
        PlayerflipX();

        // 좌우 이동 애니메이션
        PlayerXYAnimation();

        // 점프
        Jump();
    }

    /** 초기화 => 상태를 갱신한다 */
    private void FixedUpdate()
    {
        // 좌우 이동
        PlayerMove();
    }

    /** 플레이어가 움직인다 */
    private void PlayerMove()
    {
        float DirectX = Input.GetAxisRaw("Horizontal");
        Rigid2D.velocity = new Vector2(DirectX * PlayerSpeed, Rigid2D.velocity.y);
    }

    /** 플레이어가 점프를 한다 */
    private void Jump()
    {
        IsGround = Physics2D.OverlapCircle(Pos.position, Radius, Layer);

        if (IsGround == true && Input.GetKeyDown(KeyCode.C) && JumtCnt > 0)
        {
            Rigid2D.velocity = new Vector2(0, PlayerJumpPower);
            PlayerAnimator.SetBool("IsJumping", true);
        }

        if (IsGround == false && Input.GetKeyDown(KeyCode.C) && JumtCnt > 0)
        {
            Rigid2D.velocity = new Vector2(0, PlayerJumpPower);
            PlayerAnimator.SetBool("IsJumping", true);
        }

        if(Input.GetKeyUp(KeyCode.C))
        {
            JumtCnt--;
        }

        if(IsGround == true)
        {
            JumtCnt = JumpCount;
            PlayerAnimator.SetBool("IsJumping", false);
        }
    }

    /** 플레이어 좌우 방향 스프라이트를 설정한다 */
    private void PlayerflipX()
    {
        // FIXME : 중복 입력시 플립 안바뀌는 현상 고쳐야함
        if (Input.GetButton("Horizontal"))
        {
            // 왼쪽 방향일 경우 flipX 체크해제
            SpriteRender.flipX = Input.GetAxisRaw("Horizontal") == 1;
        }
    }

    /** 플레이어 좌우 방향 애니메이션을 설정한다 */
    private void PlayerXYAnimation()
    {
        if (Mathf.Abs(Rigid2D.velocity.x) < 0.3)
        {
            PlayerAnimator.SetBool("IsWalking", false);
        }
        else
        {
            PlayerAnimator.SetBool("IsWalking", true);
        }
    }
    #endregion // 함수
}
