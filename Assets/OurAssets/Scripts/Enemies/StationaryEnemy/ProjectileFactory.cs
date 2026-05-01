using System.Collections.Generic;
using UnityEngine;

public class ProjectileFactory : MonoBehaviour
{
    static ProjectileFactory s_Instance;
    public static ProjectileFactory Instance
    {
        get
        {
            if (!s_Instance)
            {
                GameObject go = new GameObject("ProjectileFactory");
                s_Instance = go.AddComponent<ProjectileFactory>();
                s_Instance.InitHolders();
            }
            return s_Instance;
        }
    }

    GameObject m_ActiveProjectilesHolder;
    GameObject m_InactiveProjectilesHolder;
    readonly Dictionary<ProjectileData, Dictionary<Projectile, bool>> m_Projectiles = new Dictionary<ProjectileData, Dictionary<Projectile, bool>>();
    readonly Dictionary<ProjectileData, Transform> m_ActiveHolders = new Dictionary<ProjectileData, Transform>();
    readonly Dictionary<ProjectileData, Transform> m_InactiveHolders = new Dictionary<ProjectileData, Transform>();

    void Awake()
    {
        if (s_Instance && s_Instance != this) Destroy(gameObject);
        else
        {
            s_Instance = this;
            InitHolders();
        }
    }

    void InitHolders()
    {
        m_ActiveProjectilesHolder = new GameObject("Active Projectiles");
        m_ActiveProjectilesHolder.transform.parent = transform;
        m_InactiveProjectilesHolder = new GameObject("Inactive Projectiles");
        m_InactiveProjectilesHolder.transform.parent = transform;
    }

    public Projectile SpawnProjectile(ProjectileData projectileData, Transform spawnTransform)
    {
        if (!projectileData || !spawnTransform) return null;
        if (!m_Projectiles.ContainsKey(projectileData)) CreateNewPool(projectileData);
		Projectile projectile = FirstAvailableOrNewProjectile(projectileData);
        projectile.transform.parent = m_ActiveHolders[projectileData];
        projectile.transform.SetPositionAndRotation(spawnTransform.position, spawnTransform.rotation);
        m_Projectiles[projectileData][projectile] = false;
        return projectile;
    }

    public void DeactivateProjectile(Projectile projectile)
    {
        if (!m_Projectiles.ContainsKey(projectile.ProjectileData) || !m_Projectiles[projectile.ProjectileData].ContainsKey(projectile)) return;
        Transform holder = m_InactiveHolders[projectile.ProjectileData];
        projectile.transform.parent = holder;
        projectile.transform.SetPositionAndRotation(holder.position, holder.rotation);
        m_Projectiles[projectile.ProjectileData][projectile] = true;
    }

    void CreateNewPool(ProjectileData projectileData)
    {
        m_Projectiles.Add(projectileData, new Dictionary<Projectile, bool>());
        GameObject activeProjectiles = new GameObject($"{projectileData.name}s");
        activeProjectiles.transform.parent = m_ActiveProjectilesHolder.transform;
        m_ActiveHolders.Add(projectileData, activeProjectiles.transform);
        GameObject inactiveProjectiles = new GameObject($"{projectileData.name}s");
        inactiveProjectiles.transform.parent = m_InactiveProjectilesHolder.transform;
        m_InactiveHolders.Add(projectileData, inactiveProjectiles.transform);
    }

    Projectile CreateNewProjectile(ProjectileData projectileData)
    {
        GameObject go = new GameObject(projectileData.name);
        Projectile projectile = go.AddComponent<Projectile>();
		projectile.ProjectileData = projectileData;
		projectile.gameObject.SetActive(false);
        m_Projectiles[projectileData].Add(projectile, true);
        return projectile;
    }

    Projectile FirstAvailableOrNewProjectile(ProjectileData projectileData)
    {
        Projectile projectile = null;
        foreach (Projectile p in m_Projectiles[projectileData].Keys)
        {
            if (!m_Projectiles[projectileData][p]) continue;
            projectile = p;
            break;
        }
        projectile ??= CreateNewProjectile(projectileData);
        return projectile;
    }
}
