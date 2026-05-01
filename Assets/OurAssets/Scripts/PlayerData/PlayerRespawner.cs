using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerRespawner : MonoBehaviour
{
    [SerializeField]
    Transform cameraTarget;

    CharacterController cc;

	void Awake() => cc = GetComponent<CharacterController>();

	public void Respawn(Transform respawnPoint)
    {
        cc.enabled = false;
        transform.SetPositionAndRotation(respawnPoint.position, respawnPoint.rotation);
        cameraTarget.forward = transform.forward;
        cc.enabled = true;
    }
}
