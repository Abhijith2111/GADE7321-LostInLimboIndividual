using UnityEngine;

[RequireComponent(typeof(Animator))]
public class StationaryEnemyAnimationLinker : MonoBehaviour
{
    [SerializeField]
    StationaryEnemy m_StationaryEnemy;

    public void Shoot() => m_StationaryEnemy?.Shoot();
}
