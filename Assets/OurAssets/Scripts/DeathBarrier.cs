using UnityEngine;

[RequireComponent(typeof(Collider))] // Ensure attached to an object with a collider
public class DeathBarrier : MonoBehaviour
{
    void OnValidate() => GetComponent<Collider>().isTrigger = true; // Ensure the collider is a trigger collider

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) CheckpointManager.Instance.LoseLife();
    }
}
