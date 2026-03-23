using UnityEngine;

public class PlayerRespawner : MonoBehaviour
{
    [SerializeField]
    Transform cameraTarget;

    public void Respawn(Transform respawnPoint)
    {
        transform.SetPositionAndRotation(respawnPoint.position, respawnPoint.rotation);
        cameraTarget.forward = transform.forward;
    }
}
