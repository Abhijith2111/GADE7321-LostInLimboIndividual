using UnityEngine;

[RequireComponent(typeof(Animator))]
public class StationaryEnemyAnimationLinker : MonoBehaviour
{
    [SerializeField]
    StationaryEnemy m_StationaryEnemy;

	public bool DoAttack { get; set; } = false;

	Animator m_Animator;

	void Awake() => m_Animator = GetComponent<Animator>();


	void Update() => m_Animator.SetBool("Attack", DoAttack);

	public void Shoot() => m_StationaryEnemy?.Shoot();
}
