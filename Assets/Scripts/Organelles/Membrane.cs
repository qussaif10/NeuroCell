using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Organelles
{
    public class Membrane : MonoBehaviour
    {
        [Header("Phospholipid initialization")]
        public GameObject phospholipidPrefab;
        public GameObject centerRigidBody;
        public int numberOfPhospholipidsPerLayer = 100;
        public float radius = 5f;
        public float layerDistance = 0.5f;

        [Header("Springs between adjacent phospholipids")]
        [Range(0,1)] public float dampeningRatio0;
        public float frequency0 = 1;
        [Header("Springs between opposite phospholipids")]
        [Range(0,1)] public float dampeningRatio1;
        public float frequency1 = 1;
        [Header("Springs between center and phospholipids")]
        [Range(0,1)] public float dampeningRatio2;
        public float frequency2 = 1;

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

                springJoints[0].frequency = frequency0;
                springJoints[0].dampingRatio = dampeningRatio0;
                springJoints[1].frequency = frequency1;
                springJoints[1].dampingRatio = dampeningRatio1;
                springJoints[2].frequency = frequency2;
                springJoints[2].dampingRatio = dampeningRatio2;
                springJoints[2].connectedBody = centerRigidBody.GetComponent<Rigidbody2D>();
            
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

                springJoints[0].frequency = frequency0;
                springJoints[0].dampingRatio = dampeningRatio0;
                springJoints[1].frequency = frequency1;
                springJoints[1].dampingRatio = dampeningRatio1;
                springJoints[2].frequency = frequency2;
                springJoints[2].dampingRatio = dampeningRatio2;
                springJoints[2].connectedBody = centerRigidBody.GetComponent<Rigidbody2D>();

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

        public void KillCell()
        {
            foreach (var lipid in _outerPhospholipids.Concat(_innerPhospholipids))
            {
                var springs = lipid.GetComponents<SpringJoint2D>();
                for (int i = 0; i < 3; i++)
                {
                    springs[i].enabled = !springs[i].enabled;
                }
                lipid.GetComponent<Rigidbody2D>().AddForce(Random.insideUnitCircle.normalized * Random.Range(100f, 500f));
            }
        }
    }
}