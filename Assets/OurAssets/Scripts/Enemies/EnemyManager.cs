using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct EnemyPatrolPoints
{
    public List<Transform> PatrolPoints;
}

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    List<Transform> m_StationaryEnemySpawns;
    [SerializeField]
    StationaryEnemy m_StationaryEnemyPrefab;
    [SerializeField]
    List<EnemyPatrolPoints> m_EnemyPatrolPoints;
    //[SerializeField]
    //PatrollingEnemy m_PatrollingEnemyPrefab;

    List<StationaryEnemy> m_StationaryEnemies = new List<StationaryEnemy>(); // Is this even needed?
    //List<PatrollingEnemy> m_PatrollingEnemies = new List<PatrollingEnemy>();

    void Start()
    {
        SpawnStationaryEnemies();
        SpawnPatrollingEnemies();
    }

	void SpawnStationaryEnemies()
	{
        foreach (Transform spawn in m_StationaryEnemySpawns)
        {
			StationaryEnemy enemy = EnemyFactory.Instance.Create<StationaryEnemy>(m_StationaryEnemyPrefab, spawn);
            m_StationaryEnemies.Add(enemy);
		}
	}

    void SpawnPatrollingEnemies() // Just need patrolling enemy code from abhi
    {
        foreach (EnemyPatrolPoints points in m_EnemyPatrolPoints)
        {
            //PatrollingEnemy enemy = EnemyFactory.Instance.Create<PatrollingEnemy>(m_PatrollingEnemyPrefab, points);
            //m_PatrollingEnemies.Add(enemy);
        }
    }
}
