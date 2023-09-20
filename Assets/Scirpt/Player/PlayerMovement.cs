using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region 변수
    [Header("=====> 플레이어 속도 정보 기본값 : 3 <=====")]
    [SerializeField] private float PlayerMaxSpeed = 3;
    [SerializeField] private float PlayerMinSpeed = 3;
    [SerializeField] private float PlayerJumpPower = 3;

    private Rigidbody2D Rigid2D;
    private SpriteRenderer SpriteRender;
    private Animator PlayerAnimator;

    private int JumpCount = 0; // 더블 점프 변수
    #endregion // 변수

    #region 함수
    /** 초기화 */
    private void Awake()
    {
        Rigid2D = GetComponent<Rigidbody2D>();
        SpriteRender = GetComponent<SpriteRenderer>();
        PlayerAnimator = GetComponent<Animator>();
    }

    /** 초기화 => 상태를 갱신한다 */
    private void Update()
    {
        // 좌우 이동 모션 변경
        PlayerflipX();

        // 좌우 이동 애니메이션
        PlayerXYAnimation();

        // 이동 멈춤
        PlayerMoveStop();

        // 점프
        Jump();
    }

    /** 초기화 => 상태를 갱신한다 */
    private void FixedUpdate()
    {
        // 좌우 이동
        PlayerMove();

        // 점프 제어자
        PlayerJumpController();
    }

    /** 플레이어가 움직인다 */
    private void PlayerMove()
    {
        float X = Input.GetAxisRaw("Horizontal");
        Rigid2D.AddForce(Vector2.right * PlayerMinSpeed * X, ForceMode2D.Impulse);

        // 최대 속도를 제한한다
        if (Rigid2D.velocity.x > PlayerMaxSpeed)
        {
            Rigid2D.velocity = new Vector2(PlayerMaxSpeed, Rigid2D.velocity.y);
        }
        else if (Rigid2D.velocity.x < PlayerMaxSpeed * -1)
        {
            Rigid2D.velocity = new Vector2(PlayerMaxSpeed * -1, Rigid2D.velocity.y);
        }
    }

    /** 플레이어 움직임을 멈춘다 */
    private void PlayerMoveStop()
    {
        // 버튼에서 손을 땠을경우
        if (Input.GetButtonUp("Horizontal"))
        {
            Rigid2D.velocity = new Vector2(0.5f * Rigid2D.velocity.normalized.x, Rigid2D.velocity.y);
        }

    }

    /** 플레이어가 점프를 한다 */
    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.C) && JumpCount < 2)
        {
            Rigid2D.AddForce(Vector2.up * PlayerJumpPower, ForceMode2D.Impulse);
            JumpCount++;
            PlayerAnimator.SetBool("IsJumping", true);
        }
    }

    /** 플레이어 점프를 제어한다 */
    private void PlayerJumpController()
    {
        RaycastHit2D RayHit = Physics2D.Raycast(Rigid2D.position, Vector3.down, 1f, LayerMask.GetMask("Platform"));

        if (Rigid2D.velocity.y < 0)
        {
            if (RayHit.collider != null)
            {
                if (RayHit.distance < 0.7f)
                {
                    JumpCount = 0;
                    PlayerAnimator.SetBool("IsJumping", false);
                }
            }
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
