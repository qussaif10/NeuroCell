using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class MembraneCreator : MonoBehaviour
{
    public GameObject phospholipidPrefab;
    public int numberOfPhospholipidsPerLayer = 100;
    public float radius = 5f;
    public float layerDistance = 0.5f;
    private readonly List<GameObject> _innerPhospholipids = new();
    private readonly List<GameObject> _outerPhospholipids = new();

    private void Start()
    {
        CreateDoubleLayerMembrane();
        AddSpringJointToPhospholipid();
    }

    private void CreateDoubleLayerMembrane()
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
        var lipidCount = 0;
        
        if (_innerPhospholipids.Count == _outerPhospholipids.Count)
        {
            lipidCount = _innerPhospholipids.Count;
        }
        
        foreach (var lipid in _outerPhospholipids)
        {
            lipid.AddComponent<SpringJoint2D>();
            lipid.AddComponent<SpringJoint2D>();
            
            lipid.GetComponent<SpringJoint2D>().enableCollision = true;
            lipid.GetComponent<SpringJoint2D>().autoConfigureConnectedAnchor = true;
            lipid.GetComponent<SpringJoint2D>().distance = 0f;
        }
        
        foreach (var lipid in _innerPhospholipids)
        {
            lipid.AddComponent<SpringJoint2D>();
            lipid.AddComponent<SpringJoint2D>();
            lipid.GetComponent<SpringJoint2D>().enableCollision = true;
            lipid.GetComponent<SpringJoint2D>().autoConfigureConnectedAnchor = true;
            lipid.GetComponent<SpringJoint2D>().distance = 0f;
        }
        
        for (var i = 0; i < lipidCount; i++)
        {
            if (i != lipidCount - 1)
            {
                _outerPhospholipids[i].GetComponent<SpringJoint2D>().connectedBody = _outerPhospholipids[i + 1].GetComponent<Rigidbody2D>();
                _innerPhospholipids[i].GetComponent<SpringJoint2D>().connectedBody = _innerPhospholipids[i + 1].GetComponent<Rigidbody2D>();
            }
            else
            {
                _outerPhospholipids[i].GetComponent<SpringJoint2D>().connectedBody = _outerPhospholipids[0].GetComponent<Rigidbody2D>();
                _innerPhospholipids[i].GetComponent<SpringJoint2D>().connectedBody = _innerPhospholipids[0].GetComponent<Rigidbody2D>();
            }
        }
    }
}