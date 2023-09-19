using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region 변수
    [Header("=====> 인스펙터 확인용 <=====")]
    [SerializeField] private float PlayerMaxSpeed;
    [SerializeField] private float PlayerMinSpeed;
    [SerializeField] private float PlayerJumpPower;

    private Rigidbody2D Rigid2D;
    #endregion // 변수

    #region 함수
    /** 초기화 */
    private void Awake()
    {
        Rigid2D = GetComponent<Rigidbody2D>();
    }

    /** 초기화 => 상태를 갱신한다 */
    private void Update()
    {
        PlayerMoveStop();
        Jump();
    }

    /** 초기화 => 상태를 갱신한다 */
    private void FixedUpdate()
    {
        PlayerMove();
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
        if (Input.GetButtonUp("Horizontal"))
        {
            Rigid2D.velocity = new Vector2(0.3f * Rigid2D.velocity.normalized.x, Rigid2D.velocity.y);
        }

    }

    /** 점프를 한다 */
    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            Rigid2D.AddForce(Vector2.up * PlayerJumpPower, ForceMode2D.Impulse);
        }
    }
    #endregion // 함수
}
