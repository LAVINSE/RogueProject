using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyStateFSM
{
    public abstract class StateFSM
    {
        public Player PlayerData
        {
            get
            {
                var Player = CSceneManager.Instance.PlayerObj.GetComponent<Player>();
                return Player;
            }
        }

        public abstract void EnemyStateEnter(EnemyState Enemy);
        public abstract void EnemyStateUpdate(EnemyState Enemy, float Time);
        public abstract void EnemyStateExit(EnemyState Enemy);
    }
}

