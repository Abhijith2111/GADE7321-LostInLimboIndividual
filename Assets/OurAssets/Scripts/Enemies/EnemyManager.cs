using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct EnemyPatrolPoints
{
    public List<Transform> PatrolPoints;
}

[System.Serializable]
public struct BossPatrolNode
{
    public Transform Node;
    public List<Transform> ConnectedNodes;
}

[System.Serializable]
public struct BossPatrolPoints
{
    public List<BossPatrolNode> PatrolNodes;
}

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    List<Transform> m_StationaryEnemySpawns;
    [SerializeField]
    StationaryEnemy m_StationaryEnemyPrefab;
    [SerializeField, Min(1)]
    int m_StationaryEnemyCount = 3; // Probably going to need more but 3 is fine for now
    [SerializeField]
    List<EnemyPatrolPoints> m_EnemyPatrolPoints;
    [SerializeField]
    PatrollingEnemy m_PatrollingEnemyPrefab;
    [SerializeField, Min(1)]
    int m_PatrollingEnemyCount = 3; // Same thing as before
    [SerializeField]
    List<BossPatrolPoints> m_BossPatrolPoints;
    [SerializeField]
    BossAI m_BossAIPrefab;
    [SerializeField, Min(1)]
    int m_BossAICount = 3; // Same thing as before

    // Are these lists even needed? Idk if we need to access these enemies at
    // all so I'm storing them in lists in case. Idk how much ram it uses
    // because c# intellisense doesn't tell you like c++ but I'm sure it's
    // not that much. Probably will delete the later, who knows
    List<StationaryEnemy> m_StationaryEnemies = new List<StationaryEnemy>();
    List<PatrollingEnemy> m_PatrollingEnemies = new List<PatrollingEnemy>();
    List<BossAI> m_BossAIs = new List<BossAI>();
    // What do you know they did have a use in the end :)

    void Start()
    {
        SpawnStationaryEnemies();
        SpawnPatrollingEnemies();
        SpawnBossAIs();
        // Enable all the enemies once all of them are spawned in
        foreach (StationaryEnemy stationaryEnemy in m_StationaryEnemies)
        {
            stationaryEnemy.Enabled = true;
        }
        foreach (PatrollingEnemy patrollingEnemy in m_PatrollingEnemies)
        {
            patrollingEnemy.Enabled = true;
        }
        foreach (BossAI bossAI in m_BossAIs)
        {
            bossAI.Enabled = true;
        }
    }

    void SpawnStationaryEnemies()
    {
        if (m_StationaryEnemySpawns.Count == 0) return;
        // If too many that there is no free spawns then use 75% of the spawns
        int stationariesToSpawn = m_StationaryEnemyCount >= m_StationaryEnemySpawns.Count ? Mathf.CeilToInt(m_StationaryEnemySpawns.Count * 0.75f) : m_StationaryEnemyCount;
        List<Transform> _spawns = new List<Transform>(m_StationaryEnemySpawns);
        Shuffle(_spawns);
        for (int i = 0; i < stationariesToSpawn; ++i)
        {
            StationaryEnemy enemy = EnemyFactory.Instance.Create<StationaryEnemy>(m_StationaryEnemyPrefab, _spawns[i]);
            m_StationaryEnemies.Add(enemy);
        }
    }

    void SpawnPatrollingEnemies()
    {
        if (m_EnemyPatrolPoints.Count == 0) return;
        // If too many that there is no free spawns then use 75% of the spawns
        int patrollersToSpawn = m_PatrollingEnemyCount >= m_EnemyPatrolPoints.Count ? Mathf.CeilToInt(m_EnemyPatrolPoints.Count * 0.75f) : m_PatrollingEnemyCount;
        List<EnemyPatrolPoints> _patrols = new List<EnemyPatrolPoints>(m_EnemyPatrolPoints);
        Shuffle(_patrols);
        for (int i = 0; i < patrollersToSpawn; ++i)
        {
            PatrollingEnemy enemy = EnemyFactory.Instance.Create<PatrollingEnemy>(m_PatrollingEnemyPrefab, _patrols[i]);
            m_PatrollingEnemies.Add(enemy);
        }
    }

    void SpawnBossAIs()
    {
        if (m_BossPatrolPoints.Count == 0) return;
        // If too many that there is no free spawns then use 75% of the spawns
        int bossesToSpawn = m_BossAICount >= m_BossPatrolPoints.Count ? Mathf.CeilToInt(m_BossPatrolPoints.Count * 0.75f) : m_BossAICount;
        List<BossPatrolPoints> _patrols = new List<BossPatrolPoints>(m_BossPatrolPoints);
        Shuffle(_patrols);
        for (int i = 0; i < bossesToSpawn; ++i)
        {
            BossAI enemy = EnemyFactory.Instance.Create<BossAI>(m_PatrollingEnemyPrefab, _patrols[i]);
            m_BossAIs.Add(enemy);
        }
    }

    // Normally I'd write shuffle and swap as extensions but we only need them once so
    // just simple methods are fine

    // Fisher-Yates shuffle: https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle
    void Shuffle<T>(List<T> list)
    {
        // I genuinely don't know why all the examples use a while-loop. I think the
        // original was written in a language that didn't have for-loops, but come on
        // people, the for-loop does the exact same thing and in less lines smh
        System.Random rng = new System.Random();
        for (int i = list.Count - 1; i > 0; --i) // Go until 1 because no need to swap index 0 with itself
        {
            int j = rng.Next(i + 1); // Max is exclusive, dw, no index out of bounds exceptions here
            Swap(list, i, j);
        }
    }

    void Swap<T>(List<T> list, int i, int j)
    {
        T _temp = list[i];
        list[i] = list[j];
        list[j] = _temp;
    }
}
