using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EnemyCollision : MonoBehaviour
{
	void OnCollisionEnter(Collision collision) => CheckForPlayerCollision(collision.gameObject);

	void OnTriggerEnter(Collider other) => CheckForPlayerCollision(other.gameObject);

	void CheckForPlayerCollision(GameObject go)
	{
		if (go.CompareTag("Player") || gameObject.layer == LayerMask.NameToLayer("Player"))
			CheckpointManager.Instance.LoseLife();
	}
}
