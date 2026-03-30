using UnityEngine;

public class FloatScript : MonoBehaviour
{
    [SerializeField, Range(0f, 1f)]
    float bobbingScaleFactor = 1f;

    Vector3 startingPosition;

    void Start() => startingPosition = transform.position;

    void LateUpdate() => transform.position = startingPosition + (WaterWaveManager.Instance.GetWaterHeight(startingPosition) * bobbingScaleFactor) * Vector3.up;
}
