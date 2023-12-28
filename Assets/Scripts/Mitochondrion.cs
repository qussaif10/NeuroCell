using UnityEngine;
using UnityEngine.UIElements;

public class Mitochondrion : MonoBehaviour
{
    public GameObject MitochondrionSegment;
    public GameObject DNA;
    public int numberOfSegments;
    public float scale;

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
            
            var position = new Vector3(
                scale * Mathf.Cos(theta) * (Mathf.Sqrt(2) * (Mathf.Cos(2 * theta) + 2) * Mathf.Sin(theta) / 2) - 1.6f,
                scale * Mathf.Sin(theta) * (Mathf.Sqrt(2) * (Mathf.Cos(2 * theta) + 2) * Mathf.Sin(theta) / 2) - 2.3f,
                0);

            segments[i] = Instantiate(MitochondrionSegment, position, Quaternion.identity, transform);
        }

        // foreach (var segment in segments)
        // {
        //     segment.AddComponent<Rigidbody2D>();
        //     segment.AddComponent<SpringJoint2D>();
        //     segment.AddComponent<SpringJoint2D>();
        //     segment.AddComponent<SpringJoint2D>();
        //     segment.AddComponent<CircleCollider2D>();
        //     segment.GetComponent<Rigidbody2D>().gravityScale = 0;
        //     segment.GetComponent<Rigidbody2D>().drag = 1.2f;
        //     segment.GetComponent<Rigidbody2D>().angularDrag = 0.2f;
        //     segment.GetComponent<SpringJoint2D>().connectedBody = _centerRigidBody;
        //     segment.GetComponent<SpringJoint2D>().frequency = springiness;
        // }
        //
        // for (var i = 0; i < segments.Length; i++)
        // {
        //     var springs = segments[i].GetComponents<SpringJoint2D>();
        //     
        //     if (i == segments.Length - 1)
        //     {
        //         springs[1].connectedBody = segments[0].GetComponent<Rigidbody2D>();
        //         springs[1].frequency = springiness;
        //     }
        //     else
        //     {
        //         springs[1].connectedBody = segments[i + 1].GetComponent<Rigidbody2D>();
        //         springs[1].frequency = springiness;
        //     }
        //
        //     springs[2].frequency = springiness;
        // }
    }
}