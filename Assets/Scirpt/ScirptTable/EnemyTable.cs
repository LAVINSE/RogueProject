using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyTable : ScriptableObject
{
    public GameObject EnemyPrefab = null;
    public float EnemyHp = 0.0f;
    public float EnemyAtk = 0.0f;
}