using UnityEngine;

[System.Serializable]
public enum AxisOfMotion
{
	Forward,
	Back,
	Right,
	Left,
	Up,
	Down,
}

public static class ProjectileHelper
{
	public static Vector3 GetMotionDirection(Transform projectileTransform, AxisOfMotion axisOfMotion)
	{
		if (!projectileTransform) throw new System.ArgumentNullException("projectileTransform is null");
		return axisOfMotion switch
		{
			AxisOfMotion.Forward => projectileTransform.forward,
			AxisOfMotion.Back => -projectileTransform.forward,
			AxisOfMotion.Right => projectileTransform.right,
			AxisOfMotion.Left => -projectileTransform.right,
			AxisOfMotion.Up => projectileTransform.up,
			AxisOfMotion.Down => -projectileTransform.up,
			_ => throw new System.ArgumentException("Somehow you input an AxisOfMotion that doesn't exist")
		};
	}
}

[CreateAssetMenu(fileName = "ProjectileData", menuName = "Scriptable Objects/Projectile Data")]
public class ProjectileData : ScriptableObject
{
	[field: SerializeField]
	public LayerMask HittableLayers { get; private set; }
	[SerializeField]
	bool m_CollideWithTriggers = false;
	[field: SerializeField, Min(0.1f)]
	public float Speed { get; private set; } = 5f;
	[field: SerializeField, Min(0.1f)]
	public float MaxTravelTime { get; private set; } = 2.5f;
	[field: SerializeField]
	public AxisOfMotion AxisOfMotion { get; private set; } = AxisOfMotion.Forward;
	[field: SerializeField]
	public GameObject Mesh { get; private set; }

	public QueryTriggerInteraction QueryTriggerInteraction => m_CollideWithTriggers ? QueryTriggerInteraction.Collide : QueryTriggerInteraction.Ignore;
}
