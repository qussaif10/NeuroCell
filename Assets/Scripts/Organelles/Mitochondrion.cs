using UnityEngine;

namespace Organelles
{
    public class Mitochondrion : MonoBehaviour
    {
        public GameObject MitochondrionSegment;
        public GameObject DNA;
        public int numberOfSegments;
        public float scale;
        public float xOffset;
        public float yOffset;

        private GameObject[] segments;
        private Rigidbody2D _centerRigidBody;
        public float springiness;

        private void Awake()
        {
            segments = new GameObject[numberOfSegments];
            _centerRigidBody = new GameObject().AddComponent<Rigidbody2D>();
        }

        private void Start()
        {
            CreateMitochondrion();
        }

        private void CreateMitochondrion()
        {
            var stepAngle = Mathf.PI * 2 / numberOfSegments;
            var centerSprings = new SpringJoint2D[segments.Length];
            var adjacentSprings = new SpringJoint2D[segments.Length];
    
            _centerRigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
            _centerRigidBody.transform.position = new Vector3(xOffset, yOffset, 0);
    
            for (var i = 0; i < segments.Length; i++)
            {
                var theta = stepAngle * i;

                const float rotationAngle = -35f;
                var rotation = Quaternion.Euler(0, 0, rotationAngle);

                // Define the semi-major and semi-minor axes for the ellipse
                float a = scale; // Semi-major axis
                float b = scale / 1.8f; // Semi-minor axis

                // Calculate position using the parametric equation of an ellipse
                var position = new Vector3(
                    a * Mathf.Cos(theta), // x = a * cos(t)
                    b * Mathf.Sin(theta), // y = b * sin(t)
                    0);

                var rotatedPosition = rotation * position;

                rotatedPosition.x += xOffset;
                rotatedPosition.y += yOffset;

                segments[i] = Instantiate(MitochondrionSegment, rotatedPosition, Quaternion.identity, transform);
            }


            for (var i = 0; i < segments.Length; i++)
            {
                var segment = segments[i];
                segment.AddComponent<CircleCollider2D>();
                segment.AddComponent<Rigidbody2D>();
                var joint1 = segment.AddComponent<SpringJoint2D>();
                var joint2 = segment.AddComponent<SpringJoint2D>();
            
                centerSprings[i] = joint1;
                adjacentSprings[i] = joint2;
            
                joint1.connectedBody = _centerRigidBody;
                joint1.autoConfigureConnectedAnchor = true;
                joint1.frequency = springiness;
                joint2.connectedBody = i == 0 ? segments[^1].GetComponent<Rigidbody2D>() : segments[i - 1].GetComponent<Rigidbody2D>();
                joint2.autoConfigureConnectedAnchor = true;
                joint2.frequency = springiness;
            }
        }
    }
}