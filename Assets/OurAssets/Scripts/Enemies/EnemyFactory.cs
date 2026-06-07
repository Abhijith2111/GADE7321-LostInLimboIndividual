using System;
using UnityEngine;

// EnemyFactory is just a MonoBehavior wrapper for the internal real factory
public class EnemyFactory : MonoBehaviour
{
    static EnemyFactory s_Instance;
    public static EnemyFactory Instance
    {
        get
        {
            if (!s_Instance)
            {
                GameObject go = new GameObject("EnemyFactory");
                s_Instance = go.AddComponent<EnemyFactory>();
                s_Instance.m_EnemyFactory = new EnemyFactoryInternal();
            }
            return s_Instance;
        }
    }

    EnemyFactoryInternal m_EnemyFactory;

    void Awake()
    {
        if (s_Instance && s_Instance != this) Destroy(gameObject);
        else
        {
            s_Instance = this;
            m_EnemyFactory = new EnemyFactoryInternal();
        }
    }

    /// <summary>
    /// <para>To create a <see cref="StationaryEnemy"/>, args = (<see cref="StationaryEnemy"/> prefab, <see cref="Transform"/> parent)</para>
    /// <para>To create a <see cref="PatrollingEnemy"/>, args = (<seealso cref="PatrollingEnemy"/> prefab, <see cref="EnemyPatrolPoints"/> points)</para>
    /// <para>To create a <see cref="BossAI"/>, args = (<seealso cref="BossAI"/> prefab, <see cref="BossPatrolPoints"/> points)</para>
    /// </summary>
    /// <typeparam name="T">The type of <see cref="BaseEnemy"/> to create</typeparam>
    /// <param name="args">The arguments needed to create the enemy</param>
    /// <returns>A <see cref="BaseEnemy"/> of the type <typeparamref name="T"/></returns>
    public T Create<T>(params object[] args) where T : BaseEnemy
    {
        object[] argsAndType = new object[args.Length + 1];
        args.CopyTo(argsAndType, 1);
        argsAndType[0] = typeof(T);
        return m_EnemyFactory.Create(argsAndType) as T;
    }

    class EnemyFactoryInternal : AbstractFactory<BaseEnemy>
    {
        public override BaseEnemy Create(params object[] args)
        {
            if (args == null || args.Length == 0 || args[0] is not Type t)
            {
                Debug.LogWarning("To spawn an enemy args[0] should be the type of enemy to spawn");
                return null;
            }
            if (t == typeof(StationaryEnemy))
            {
                if (args.Length != 3 || args[1] is not StationaryEnemy prefab || args[2] is not Transform parent)
                {
                    Debug.LogWarning("To spawn a stationary enemy args[1] needs to be the prefab and args[2] needs to be the parent transform");
                    return null;
                }
                return CreateStationaryEnemy(prefab, parent);
            }
            else if (t == typeof(PatrollingEnemy))
            {
                if (args.Length != 3 || args[1] is not PatrollingEnemy prefab || args[2] is not EnemyPatrolPoints points)
                {
                    Debug.LogWarning("To spawn a patrolling enemy args[1] needs to be the prefab and args[2] needs to be the patrol points");
                    return null;
                }
                return CreatePatrollingEnemy(prefab, points);
            }
            else if (t == typeof(BossAI))
            {
                if (args.Length != 3 || args[1] is not BossAI prefab || args[2] is not BossPatrolPoints points)
                {
                    Debug.LogWarning("To spawn a boss args[1] needs to be the prefab and args[2] needs to be the patrol points");
                    return null;
                }
                return CreateBossAI(prefab, points);
            }
            throw new NotImplementedException($"There is no definition to create an enemy of type {t}");
        }

        StationaryEnemy CreateStationaryEnemy(StationaryEnemy prefab, Transform parent) => Instantiate(prefab.gameObject, parent).GetComponent<StationaryEnemy>();

        PatrollingEnemy CreatePatrollingEnemy(PatrollingEnemy prefab, EnemyPatrolPoints points)
        {
            PatrollingEnemy enemy = Instantiate(prefab.gameObject, points.PatrolPoints[0]).GetComponent<PatrollingEnemy>();
            LinkedListADT<Transform> patrolPoints = new LinkedListADT<Transform>(points.PatrolPoints);
            enemy.SetPatrolPoints(patrolPoints);
            return enemy;
        }

        BossAI CreateBossAI(BossAI prefab, BossPatrolPoints points)
        {
            GraphADT<Transform> graph = new GraphADT<Transform>();
            foreach (BossPatrolNode node in points.PatrolNodes)
            {
                GraphADTNode<Transform> _node = graph.Find(node.Node) ?? graph.CreateNode(node.Node); // Find node if it exists already or create if it doesn't
                foreach (Transform connection in node.ConnectedNodes)
                {
                    GraphADTNode<Transform> _connection = graph.Find(connection) ?? graph.CreateNode(connection); // Find node if it exists already or create if it doesn't
                    graph.CreateConnection(_node, _connection); // Create connection between the nodes
                }
            }
            BossAI enemy = Instantiate(prefab.gameObject, graph.Nodes[0].Value).GetComponent<BossAI>();
            enemy.SetPatrolPoints(graph);
            return enemy;
        }
    }
}
