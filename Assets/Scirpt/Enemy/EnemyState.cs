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
        /** 상태 진입 */
        public override void EnemyStateEnter(EnemyState Enemy)
        {
            Debug.Log("적 대기중");
        }

        /** 상태 갱신 */
        public override void EnemyStateUpdate(EnemyState Enemy, float Time)
        {
            // 만약 플레이어가 공격 범위안에 있는지 검사하고, 공격범위라면 공격 상태로
            // 추적 범위안에 플레이어가 있을 경우 추적 상태로 
        }

        /** 상태 종료 */
        public override void EnemyStateExit(EnemyState Enemy)
        {
            
        }
    }

    /** 적 추적 */
    public class EnemyStateTracking : StateFSM
    {
        /** 상태 진입 */
        public override void EnemyStateEnter(EnemyState Enemy)
        {

        }

        /** 상태 갱신 */
        public override void EnemyStateUpdate(EnemyState Enemy, float Time)
        {
            // 플레이어 추적
            // 플레이어가 공격 범위안에 들어왔을 경우, 공격 상태로
        }

        /** 상태 종료 */
        public override void EnemyStateExit(EnemyState Enemy)
        {

        }
    }

    /** 적 공격 */
    public class EnemyStateAttack : StateFSM
    {
        /** 상태 진입 */
        public override void EnemyStateEnter(EnemyState Enemy)
        {

        }

        /** 상태 갱신 */
        public override void EnemyStateUpdate(EnemyState Enemy, float Time)
        {

        }

        /** 상태 종료 */
        public override void EnemyStateExit(EnemyState Enemy)
        {

        }
    }

    /** 적 타격 */
    public class EnemyStateHit : StateFSM
    {
        /** 상태 진입 */
        public override void EnemyStateEnter(EnemyState Enemy)
        {

        }

        /** 상태 갱신 */
        public override void EnemyStateUpdate(EnemyState Enemy, float Time)
        {
            Debug.Log("적 피격");
            Enemy.ChangeState(EnemyStateType.Wait);


        }

        /** 상태 종료 */
        public override void EnemyStateExit(EnemyState Enemy)
        {

        }
    }

    /** 적 죽음 */
    public class EnemyStateDead : StateFSM
    {
        /** 상태 진입 */
        public override void EnemyStateEnter(EnemyState Enemy)
        {

        }

        /** 상태 갱신 */
        public override void EnemyStateUpdate(EnemyState Enemy, float Time)
        {
            Debug.Log("적 죽음");
        }

        /** 상태 종료 */
        public override void EnemyStateExit(EnemyState Enemy)
        {

        }
    }
    #endregion // 상태 클래스
}
