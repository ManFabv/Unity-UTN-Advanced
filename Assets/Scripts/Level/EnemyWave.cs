using System;
using UnityEngine;

[Serializable]
public class EnemyWave
{
    public int MaxNumberOfSpawnedEnemies = 10;
    public GameObject[] EnemiesPrefab;
    public float TimeBetweenSpawns = 5;
    public float TimeForFirstSpawnRate = 2;
}
