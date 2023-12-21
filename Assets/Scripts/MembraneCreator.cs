using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class MembraneCreator : MonoBehaviour
{
    public GameObject phospholipidPrefab;
    public int numberOfPhospholipidsPerLayer = 100;
    public float radius = 5f;
    public float layerDistance = 0.5f;
    private List<GameObject> _innerPhospholipids = new();
    private List<GameObject> _outerPhospholipids = new();

    private void Start()
    {
        CreateDoubleLayerMembrane();
    }

    void CreateDoubleLayerMembrane()
    {
        var angleStep = 360f / numberOfPhospholipidsPerLayer;
        for (var i = 0; i < numberOfPhospholipidsPerLayer; i++)
        {
            var angle = i * angleStep;

            var outerPosition = PositionForAngle(angle, radius);
            InstantiateAndRotatePhospholipid(outerPosition, outerPosition.normalized, true);

            var innerPosition = PositionForAngle(angle, radius - layerDistance);
            InstantiateAndRotatePhospholipid(innerPosition, -innerPosition.normalized, false);
        }
    }

    private static Vector3 PositionForAngle(float angle, float layerRadius)
    {
        return new Vector3(
            Mathf.Cos(angle * Mathf.Deg2Rad),
            Mathf.Sin(angle * Mathf.Deg2Rad),
            0f
        ) * layerRadius;
    }

    private void InstantiateAndRotatePhospholipid(Vector3 position, Vector3 upDirection, bool outerlayer)
    {
        var phospholipid = Instantiate(phospholipidPrefab, position, Quaternion.identity, transform);
        phospholipid.transform.up = upDirection;
        
        if (outerlayer)
        {
            _outerPhospholipids.Add(phospholipid);
        }
        else
        {
            _innerPhospholipids.Add(phospholipid);
        }
    }

    private void AddSpringJointToPhospholipid()
    {
        
        var lipidCount = _innerPhospholipids.Count;


        foreach (var lipid in _outerPhospholipids.Concat(_innerPhospholipids))
        {
            lipid.AddComponent<SpringJoint>();
        }
        
        for (var i = 0; i < lipidCount; i++)
        {

        }
    }
}