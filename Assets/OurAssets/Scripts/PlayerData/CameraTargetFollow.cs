using UnityEngine;

public class CameraTargetFollow : MonoBehaviour
{
    [SerializeField]
    Transform player;
    [SerializeField]
    Vector3 offset = new Vector3(0f, 0.7f, 0f);

    void Update() => transform.position = player.position + offset;
}
