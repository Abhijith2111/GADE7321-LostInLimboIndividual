using UnityEngine;

public class WaterCollision : MonoBehaviour
{
    [SerializeField]
    Transform playerTransform;
    [SerializeField, Min(2)]
    int resolution = 2;
    [SerializeField, Min(0f)]
    float size = 10f;
    [SerializeField]
    ComputeShader waterCompShader;
    [SerializeField, Range(1, 10)]
    int doEveryNthFrame = 1;

    ComputeBuffer vectorBuffer;
    Vector3[] vectorBufferData;
    int kernelIndex;
    Vector3[] baseVectors;
    Vector3[] modifiedVectors;
    SphereCollider[] collisionPoints;

    void Awake()
    {
        baseVectors = GeneratePoints();
        modifiedVectors = new Vector3[baseVectors.Length];
        collisionPoints = new SphereCollider[baseVectors.Length];
        GenerateCollisionPoints(ref collisionPoints, ref baseVectors);
        kernelIndex = waterCompShader.FindKernel("CSMain");
        if (vectorBuffer == null) InitBuffer();
    }

    void OnDisable()
    {
        vectorBuffer?.Release();
        vectorBuffer = null;
    }

    void OnEnable()
    {
        if (baseVectors != null && vectorBuffer == null) InitBuffer();
    }

    void OnDestroy()
    {
        vectorBuffer?.Release();
        vectorBuffer = null;
    }

    void LateUpdate()
    {
        if (Time.frameCount % doEveryNthFrame != 0) return;
        transform.position = new Vector3(playerTransform.position.x, 0f, playerTransform.position.z);
        baseVectors.CopyTo(vectorBufferData, 0);
        vectorBuffer.SetData(vectorBufferData);
        waterCompShader.SetVector("WorldOffset", transform.position);
        waterCompShader.SetFloat("WaveAmplitude", WaterWaveManager.Instance.WaveAmplitude);
        waterCompShader.SetFloat("WaveSpeed", WaterWaveManager.Instance.WaveSpeed);
        waterCompShader.SetFloat("WaveFrequency", WaterWaveManager.Instance.WaveFrequency);
        waterCompShader.SetFloat("WaterTime", WaterWaveManager.Instance.WaterTime);
        int threadGroups = Mathf.CeilToInt(baseVectors.Length / 64f);
        waterCompShader.Dispatch(kernelIndex, threadGroups, 1, 1);
        vectorBuffer.GetData(vectorBufferData);
        vectorBufferData.CopyTo(modifiedVectors, 0);
        UpdateCollisionPointsPositions(ref collisionPoints, ref modifiedVectors);
    }

    Vector3[] GeneratePoints()
    {
        Vector3[] vertices = new Vector3[resolution * resolution];
        for (int z = 0; z < resolution; ++z)
        {
            for (int x = 0; x < resolution; ++x)
            {
                float xPos = x * size / (resolution - 1) - size / 2f;
                float yPos = 0f;
                float zPos = z * size / (resolution - 1) - size / 2f;
                vertices[z * resolution + x] = new Vector3(xPos, yPos, zPos);
            }
        }
        return vertices;
    }

    void InitBuffer()
    {
        vectorBuffer?.Release();
        vectorBufferData = new Vector3[baseVectors.Length];
        baseVectors.CopyTo(vectorBufferData, 0);
        vectorBuffer = new ComputeBuffer(baseVectors.Length, 3 * sizeof(float));
        vectorBuffer.SetData(vectorBufferData);
        waterCompShader.SetBuffer(kernelIndex, "VectorBuffer", vectorBuffer);
        waterCompShader.SetInt("VectorCount", baseVectors.Length);
    }

    void GenerateCollisionPoints(ref SphereCollider[] colliders, ref Vector3[] points)
    {
        for (int i = 0; i < colliders.Length; ++i)
        {
            colliders[i] = gameObject.AddComponent<SphereCollider>();
            colliders[i].center = points[i];
            colliders[i].radius = size / resolution / 2;
            colliders[i].isTrigger = true;
        }
    }

    void UpdateCollisionPointsPositions(ref SphereCollider[] colliders, ref Vector3[] newPositions)
    {
        for (int i = 0; i < colliders.Length; ++i)
        {
            colliders[i].center = newPositions[i];
        }
    }
}
