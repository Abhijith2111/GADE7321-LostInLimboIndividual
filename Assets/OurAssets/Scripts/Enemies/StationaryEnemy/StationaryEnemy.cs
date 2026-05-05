using UnityEngine;

public class StationaryEnemy : BaseEnemy
{
    [SerializeField]
    ProjectileData m_Projectile;
    [SerializeField]
    Transform m_ProjectileSpawnTransform;

    public void Shoot() => ProjectileFactory.Instance.Create(m_Projectile, m_ProjectileSpawnTransform).Activate();
}
