using UnityEngine;

[RequireComponent(typeof(Collider))]
public class HealthCollectable : MonoBehaviour
{
	void OnValidate() => GetComponent<Collider>().isTrigger = true;

	void OnTriggerEnter(Collider other)
	{
		if (!other.CompareTag("Player")) return;
		CheckpointManager.Instance.GainLife();
		SFXManager.Instance.PlayAudio("Pickup", SFXManager.Instance.DefaultAudioSource);
		Destroy(gameObject);
	}
}
