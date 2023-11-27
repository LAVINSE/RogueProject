using EnemyStateFSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState : MonoBehaviour
{
    public enum EnemyStateType
    {
        Wait,
        Tracking,
        Attack,
        Hit,
        Dead,
    }

    #region 변수
    private StateFSM[] StateArray;
    private StateFSM CurrentState;
    #endregion // 변수

    #region 프로퍼티
    public EnemySetting oEnemy { get; set; }
    #endregion // 프로퍼티

    #region 함수
    /** 초기화 */
    private void Awake()
    {
        oEnemy = GetComponent<EnemySetting>();

        StateArray = new StateFSM[6];
        StateArray[(int)EnemyStateType.Wait] = new EnemyStateWait();
        StateArray[(int)EnemyStateType.Tracking] = new EnemyStateTracking();
        StateArray[(int)EnemyStateType.Attack] = new EnemyStateAttack();
        StateArray[(int)EnemyStateType.Hit] = new EnemyStateHit();
        StateArray[(int)EnemyStateType.Dead] = new EnemyStateDead();

        CurrentState = StateArray[(int)EnemyStateType.Wait];
    }

    /** 초기화 => 상태를 갱신한다 */
    private void Update()
    {
        // 상태가 있을 경우
        if(CurrentState != null )
        {
            CurrentState.EnemyStateUpdate(this, Time.deltaTime);
        }
    }

    /** 상태를 변경한다 */
    public void ChangeState(EnemyStateType NewState)
    {
        // 새로 바꾸려는 상태가 없으면, 바꾸지 않는다
        if (StateArray[(int)NewState] == null)
        {
            return;
        }

        // 현재 재생중인 상태가 있으면 Exit() 호출
        if(CurrentState != null )
        {
            CurrentState.EnemyStateExit(this);
        }

        // 새로운 상태로 변경, 새로 바뀐 상태의 Enter() 호출
        CurrentState = StateArray[(int)NewState];
        CurrentState.EnemyStateEnter(this);
    }
    #endregion // 함수

    #region 상태 클래스
    /** 적 대기 */
    public class EnemyStateWait : StateFSM
    {
        #region 변수
        private float SkipTime = 0.0f;
        #endregion // 변수

        /** 상태 진입 */
        public override void EnemyStateEnter(EnemyState Enemy)
        {
            Debug.Log("적 대기중");
            SkipTime = 0.0f;
        }

        /** 상태 갱신 */
        public override void EnemyStateUpdate(EnemyState Enemy, float UpdateTime)
        {
            SkipTime += UpdateTime;

            // 만약 플레이어가 공격 범위안에 있는지 검사하고, 공격범위라면 공격 상태로
            // 추적 범위안에 플레이어가 있을 경우 추적 상태로 
            var Distance = Vector2.Distance(Enemy.transform.position, PlayerData.transform.position);

            // 추적가능한 거리일 경우
            if(Distance <= Enemy.oEnemy.oEnemyTrackingRange)
            {
                Enemy.ChangeState(EnemyStateType.Tracking);
            }
            // 공격 가능한 거리일 경우
            else if(Distance <= Enemy.oEnemy.oEnemyAttackRange)
            {
                Enemy.ChangeState(EnemyStateType.Attack);
            }
        }

        /** 상태 종료 */
        public override void EnemyStateExit(EnemyState Enemy)
        {
            Debug.Log("적 대기종료");
        }
    }

    /** 적 추적 */
    public class EnemyStateTracking : StateFSM
    {
        /** 상태 진입 */
        public override void EnemyStateEnter(EnemyState Enemy)
        {
            Debug.Log(" 추적 시작 ");
        }

        /** 상태 갱신 */
        public override void EnemyStateUpdate(EnemyState Enemy, float Time)
        {
            var Distance = Vector2.Distance(Enemy.transform.position, PlayerData.transform.position);

            // 추적 범위가 아닐 경우
            if(Distance > Enemy.oEnemy.oEnemyTrackingRange)
            {
                Enemy.ChangeState(EnemyStateType.Wait);
            }
            // 추적 가능한 범위일 경우
            else
            {
                // 공격 범위가 아닐 경우
                if (Distance > Enemy.oEnemy.oEnemyAttackRange)
                {
                    // 공격 위치와, 스프라이트방향을 바꾼다
                    if(Enemy.transform.position.x > PlayerData.transform.position.x)
                    {
                        Enemy.oEnemy.oMeleePos.localPosition = new Vector3(-1.34f, 0f, 0f);
                        Enemy.oEnemy.EnemyfilpX(false);
                    }
                    else
                    {
                        Enemy.oEnemy.oMeleePos.localPosition = new Vector3(1.34f, 0f, 0f);
                        Enemy.oEnemy.EnemyfilpX(true);
                    }

                    // 플레이어 추적
                    Enemy.transform.position = Vector2.MoveTowards(Enemy.transform.position,
                        PlayerData.transform.position, Enemy.oEnemy.oEnemyMoveSpeed * Time);

                    // 추적 애니메이션
                    Enemy.oEnemy.oEnemyAnimator.SetBool("IsMoving", true);
                }
                // 공격 가능한 범위일 경우
                else if (Distance <= Enemy.oEnemy.oEnemyAttackRange)
                {
                    Enemy.ChangeState(EnemyStateType.Attack);
                }
            }
        }

        /** 상태 종료 */
        public override void EnemyStateExit(EnemyState Enemy)
        {
            // 추적 애니메이션
            Enemy.oEnemy.oEnemyAnimator.SetBool("IsMoving", false);
        }
    }

    /** 적 공격 */
    public class EnemyStateAttack : StateFSM
    {
        #region 변수
        private bool IsEnableAttack = false;
        private float SkipTime = 0.0f;
        #endregion // 변수

        /** 상태 진입 */
        public override void EnemyStateEnter(EnemyState Enemy)
        {
            Debug.Log(" 공격 시작");
            IsEnableAttack = true;
            SkipTime = 0.0f;

            // 공격 위치와, 스프라이트방향을 바꾼다
            if (Enemy.transform.position.x > PlayerData.transform.position.x)
            {
                Enemy.oEnemy.oMeleePos.localPosition = new Vector3(-1.34f, 0f, 0f);
                Enemy.oEnemy.EnemyfilpX(false);
            }
            else
            {
                Enemy.oEnemy.oMeleePos.localPosition = new Vector3(1.34f, 0f, 0f);
                Enemy.oEnemy.EnemyfilpX(true);
            }
        }

        /** 상태 갱신 */
        public override void EnemyStateUpdate(EnemyState Enemy, float Time)
        {
            SkipTime += Time;

            // TODO : 1.5f >> 쿨타임 변수로 관리하는게 좋아보임
            if (IsEnableAttack == true && SkipTime >= 1.5f)
            {
                IsEnableAttack = false;
                Enemy.oEnemy.EnemyBasicAttack();
                Enemy.ChangeState(EnemyStateType.Wait);
            }
        }

        /** 상태 종료 */
        public override void EnemyStateExit(EnemyState Enemy)
        {
            Debug.Log(" 공격 종료 ");
        }
    }

    /** 적 타격 */
    public class EnemyStateHit : StateFSM
    {
        /** 상태 진입 */
        public override void EnemyStateEnter(EnemyState Enemy)
        {
            Debug.Log(" 피격 시작 ");
        }

        /** 상태 갱신 */
        public override void EnemyStateUpdate(EnemyState Enemy, float Time)
        {
            Debug.Log("적 피격중 ");
            Enemy.ChangeState(EnemyStateType.Wait);
        }

        /** 상태 종료 */
        public override void EnemyStateExit(EnemyState Enemy)
        {
            Debug.Log(" 피격 종료 ");
        }
    }

    /** 적 죽음 */
    public class EnemyStateDead : StateFSM
    {
        /** 상태 진입 */
        public override void EnemyStateEnter(EnemyState Enemy)
        {
            Debug.Log(" 죽음 ");
            // 죽음 처리
            PlayerData.oPlayerCurrentGold += Enemy.oEnemy.oEnemyGold;
            Enemy.oEnemy.EnemyDie();
        }

        /** 상태 갱신 */
        public override void EnemyStateUpdate(EnemyState Enemy, float Time)
        {
            // DO Somthing
        }

        /** 상태 종료 */
        public override void EnemyStateExit(EnemyState Enemy)
        {
            // DO Somthing
        }
    }
    #endregion // 상태 클래스
}
