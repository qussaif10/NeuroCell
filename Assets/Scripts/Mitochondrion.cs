using UnityEngine;
using UnityEngine.UIElements;

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
        
        _centerRigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
        _centerRigidBody.transform.position = new Vector3(-1.7f, -2.2f, 0);
        
        for (var i = 0; i < segments.Length; i++)
        {
            var theta = stepAngle * i;

            const float rotationAngle = -35f;
            var rotation = Quaternion.Euler(0, 0, rotationAngle);
            
            var position = new Vector3(
                scale * Mathf.Cos(theta) * (Mathf.Sqrt(2) * (Mathf.Cos(2 * theta) + 2) * Mathf.Sin(theta) / 2),
                scale * Mathf.Sin(theta) * (Mathf.Sqrt(2) * (Mathf.Cos(2 * theta) + 2) * Mathf.Sin(theta) / 2),
                0);

            var rotatedPosition = rotation * position;

            rotatedPosition.x += xOffset;
            rotatedPosition.y += yOffset;

            segments[i] = Instantiate(MitochondrionSegment, rotatedPosition, Quaternion.identity, transform);
        }
    }
}