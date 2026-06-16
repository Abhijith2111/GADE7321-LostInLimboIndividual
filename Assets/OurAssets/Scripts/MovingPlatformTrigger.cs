using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class MovingPlatformTrigger : MonoBehaviour
{
    [SerializeField]
    MovingPlatform m_MovingPlatform;

    void OnValidate() => GetComponent<BoxCollider>().isTrigger = true;

    void OnEnable() => GetComponent<BoxCollider>().isTrigger = true;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.layer == LayerMask.NameToLayer("Player")) m_MovingPlatform.Activate();
    }
}
