using UnityEngine;
using System.Collections.Generic;

public class MembraneCreator : MonoBehaviour
{
    public GameObject phospholipidPrefab;
    public int numberOfPhospholipidsPerLayer = 100;
    public float radius = 5f;
    public float layerDistance = 0.5f; // The distance between the two layers

    private List<GameObject> phospholipids = new List<GameObject>();

    void Start()
    {
        CreateDoubleLayerMembrane();
    }

    void CreateDoubleLayerMembrane()
    {
        float angleStep = 360f / numberOfPhospholipidsPerLayer;
        for (int i = 0; i < numberOfPhospholipidsPerLayer; i++)
        {
            // Calculate angle for the current phospholipid
            float angle = i * angleStep;

            // Create outer layer
            Vector3 outerPosition = PositionForAngle(angle, radius);
            InstantiateAndRotatePhospholipid(outerPosition, outerPosition.normalized);

            // Create inner layer
            Vector3 innerPosition = PositionForAngle(angle, radius - layerDistance);
            InstantiateAndRotatePhospholipid(innerPosition, -innerPosition.normalized);
        }
    }

    Vector3 PositionForAngle(float angle, float layerRadius)
    {
        return new Vector3(
            Mathf.Cos(angle * Mathf.Deg2Rad),
            Mathf.Sin(angle * Mathf.Deg2Rad),
            0f
        ) * layerRadius;
    }

    void InstantiateAndRotatePhospholipid(Vector3 position, Vector3 upDirection)
    {
        GameObject phospholipid = Instantiate(phospholipidPrefab, position, Quaternion.identity, transform);
        phospholipid.transform.up = upDirection;
        phospholipids.Add(phospholipid);
    }
}