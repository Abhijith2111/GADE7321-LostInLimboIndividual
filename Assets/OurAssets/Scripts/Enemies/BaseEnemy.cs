using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour
{
	bool m_bHasTriggeredThisFrame = false; // Allows for multiple colliders

	protected virtual void OnTriggerEnter(Collider other) => CheckForPlayerCollision(other.gameObject);

	protected virtual void LateUpdate() => m_bHasTriggeredThisFrame = false;

	/// <summary>
	/// I don't recommend overriding this unless you really have to
	/// </summary>
	protected virtual void CheckForPlayerCollision(GameObject go)
	{
		if ((!go.CompareTag("Player") && gameObject.layer != LayerMask.NameToLayer("Player")) || m_bHasTriggeredThisFrame) return;
		m_bHasTriggeredThisFrame = true;
		CheckpointManager.Instance.LoseLife();
	}
}
