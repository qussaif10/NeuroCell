using UnityEngine;

public class Nucleus : MonoBehaviour
{
    public GameObject nucleusSegment;
    public GameObject dna;
    public float radius;
    public int numberOfSegments;
    public float springiness;

    private GameObject[] _segments;

    private void Awake()
    {
        _segments = new GameObject[numberOfSegments];
    }

    private void Start()
    {
        CreateNucleus();
    }

    private void CreateNucleus()
    {
        var angleStep = 360f / numberOfSegments;
        var centerRigidBody = new GameObject().AddComponent<Rigidbody2D>();

        centerRigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
        centerRigidBody.transform.position = new Vector3(-2, 1, 0);
        
        for (var i = 0; i < numberOfSegments; i++)
        {
            var angle = angleStep * i;
            var position = new Vector3(
                Mathf.Cos(angle * Mathf.Deg2Rad) - 2,
                Mathf.Sin(angle * Mathf.Deg2Rad) + 1,
                0f
            ) * radius;

            _segments[i] = Instantiate(nucleusSegment, position, Quaternion.identity, transform);
        }

        foreach (var segment in _segments)
        {
            segment.AddComponent<Rigidbody2D>();
            segment.AddComponent<CircleCollider2D>();
            segment.AddComponent<SpringJoint2D>();
            segment.AddComponent<SpringJoint2D>();
        }

        for (var i = 0; i < _segments.Length; i++)
        {
            var springs = _segments[i].GetComponents<SpringJoint2D>();
            _segments[i].GetComponent<Rigidbody2D>().gravityScale = 0;
            _segments[i].GetComponent<Rigidbody2D>().drag = 1.2f;
            _segments[i].GetComponent<Rigidbody2D>().angularDrag = 0.2f;
            springs[0].connectedBody = i == _segments.Length - 1 ? _segments[0].GetComponent<Rigidbody2D>() : _segments[i + 1].GetComponent<Rigidbody2D>();
            springs[1].connectedBody = centerRigidBody;
            springs[0].frequency = springiness;
            springs[1].frequency = springiness;
        }
    }
}