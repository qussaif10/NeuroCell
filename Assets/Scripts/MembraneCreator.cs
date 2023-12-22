using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class MembraneCreator : MonoBehaviour
{
    [Header("Phospholipid initialization")]
    public GameObject phospholipidPrefab;
    public int numberOfPhospholipidsPerLayer = 100;
    public float radius = 5f;
    public float layerDistance = 0.5f;

    [Header("Springs between adjacent phospholipids")]
    public float dampeningRatio_0;
    public float frequency_0 = 1;
    [Header("Springs between opposite phospholipids")]
    public float dampeningRatio_1;
    public float frequency_1 = 1;
    [Header("Springs between center and phospholipids")]
    public float dampeningRatio_2;
    public float frequency_2 = 1;

    private readonly List<GameObject> _innerPhospholipids = new List<GameObject>();
    private readonly List<GameObject> _outerPhospholipids = new List<GameObject>();

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

    private void InstantiateAndRotatePhospholipid(Vector3 position, Vector3 upDirection, bool outerLayer)
    {
        var phospholipid = Instantiate(phospholipidPrefab, position, Quaternion.identity, transform);
        phospholipid.transform.up = upDirection;

        if (outerLayer)
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
        var outerSprings = new SpringJoint2D[numberOfPhospholipidsPerLayer];
        var innerSprings = new SpringJoint2D[numberOfPhospholipidsPerLayer];

        for (var i = 0; i < numberOfPhospholipidsPerLayer; i++)
        {
            _outerPhospholipids[i].AddComponent<SpringJoint2D>();
            _outerPhospholipids[i].AddComponent<SpringJoint2D>();
            _outerPhospholipids[i].AddComponent<SpringJoint2D>();
            
            var springJoints = _outerPhospholipids[i].GetComponents<SpringJoint2D>();
            outerSprings[i] = springJoints[1];

            springJoints[0].frequency = frequency_0;
            springJoints[0].dampingRatio = dampeningRatio_0;
            springJoints[1].frequency = frequency_1;
            springJoints[1].dampingRatio = dampeningRatio_1;
            springJoints[2].frequency = frequency_2;
            springJoints[2].dampingRatio = dampeningRatio_2;
            
            _outerPhospholipids[i].GetComponent<SpringJoint2D>().enableCollision = true;
            _outerPhospholipids[i].GetComponent<SpringJoint2D>().autoConfigureConnectedAnchor = true;
            _outerPhospholipids[i].GetComponent<SpringJoint2D>().distance = 0f;
        }

        for (var i = 0; i < _innerPhospholipids.Count; i++)
        {
            _innerPhospholipids[i].AddComponent<SpringJoint2D>();
            _innerPhospholipids[i].AddComponent<SpringJoint2D>();
            _innerPhospholipids[i].AddComponent<SpringJoint2D>();
            
            var springJoints = _innerPhospholipids[i].GetComponents<SpringJoint2D>();
            innerSprings[i] = springJoints[1];
            
            springJoints[0].frequency = frequency_0;
            springJoints[0].dampingRatio = dampeningRatio_0;
            springJoints[1].frequency = frequency_1;
            springJoints[1].dampingRatio = dampeningRatio_1;
            springJoints[2].frequency = frequency_2;
            springJoints[2].dampingRatio = dampeningRatio_2;
            
            _innerPhospholipids[i].GetComponent<SpringJoint2D>().enableCollision = true;
            _innerPhospholipids[i].GetComponent<SpringJoint2D>().autoConfigureConnectedAnchor = true;
            _innerPhospholipids[i].GetComponent<SpringJoint2D>().distance = 0f;
        }
        
        for (var i = 0; i < numberOfPhospholipidsPerLayer; i++)
        {
            if (i != numberOfPhospholipidsPerLayer - 1)
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

        for (var i = 0; i < numberOfPhospholipidsPerLayer; i++)
        {
            outerSprings[i].connectedBody = _innerPhospholipids[i].GetComponent<SpringJoint2D>().connectedBody;
            innerSprings[i].connectedBody = _outerPhospholipids[i].GetComponent<SpringJoint2D>().connectedBody;
        }
    }
}