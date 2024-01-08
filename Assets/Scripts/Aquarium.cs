using UnityEngine;

public class Aquarium : MonoBehaviour
{
    public Collider topAquariumTransform;

    public float xMin = -8f;
    public float xMax = 8f;

    public float yMin = -4f;
    public float yMax = 4f;

    public float zMin = 8f;
    public float zMax = 0f;

    public Vector3 RandomPointInBounds()
    {
        return new Vector3(
            Random.Range(topAquariumTransform.bounds.min.x, topAquariumTransform.bounds.max.x),
            Random.Range(topAquariumTransform.bounds.min.y, topAquariumTransform.bounds.max.y),
            Random.Range(topAquariumTransform.bounds.min.z, topAquariumTransform.bounds.max.z)
        );
    }
}
