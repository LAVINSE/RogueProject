using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyTable : ScriptableObject
{
    public enum EnemyType
    {
        None,
        Normal,
        Elite,
        Boss,
    }

    public GameObject EnemyPrefab = null;
    public EnemyType oEnemyType = EnemyType.None;
    public float EnemyHp = 0.0f;
    public float EnemyAtk = 0.0f;
    public int EnemyGold = 0;
    public float EnemyMoveSpeed = 0.0f;
    public float EnemyTrackingRange = 0.0f;
    public float EnemyAttackRange = 0.0f;
}