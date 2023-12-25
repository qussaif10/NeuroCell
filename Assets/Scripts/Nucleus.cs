using UnityEngine;

public class Nucleus : MonoBehaviour
{
    public GameObject nucleusSegment;
    public GameObject dna;
    public float radius;
    public int numberOfSegments;
    public float springiness;
    private Rigidbody2D _centerRigidBody;

    private GameObject[] _segments;

    private void Awake()
    {
        _segments = new GameObject[numberOfSegments];
        _centerRigidBody = new GameObject().AddComponent<Rigidbody2D>();
    }

    private void Start()
    {
        CreateNucleus();
        CreateNucleolus();
        CreateDeoxyRibonucleicAcid();
    }

    private void CreateNucleus()
    {
        var angleStep = 360f / numberOfSegments;
        _centerRigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
        _centerRigidBody.transform.position = new Vector3(-2, 1, 0);
        
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
            springs[1].connectedBody = _centerRigidBody;
            springs[0].frequency = springiness;
            springs[1].frequency = springiness;
        }
    }

    private void CreateNucleolus()
    {
        var nucleolus = Instantiate(nucleusSegment, new Vector3(-2, 1 , 0), Quaternion.identity, transform);
        nucleolus.transform.localScale = new Vector3(0.15f, 0.15f, 0);
        nucleolus.AddComponent<Rigidbody2D>();
        nucleolus.AddComponent<SpringJoint2D>();
        nucleolus.GetComponent<Rigidbody2D>().gravityScale = 0;
        nucleolus.GetComponent<Rigidbody2D>().drag = 1.2f;
        nucleolus.GetComponent<Rigidbody2D>().angularDrag = 0.2f;
        var nucleolusSpring = nucleolus.GetComponent<SpringJoint2D>();
        nucleolusSpring.connectedBody = _centerRigidBody;
        nucleolusSpring.frequency = springiness;
    }

    private void CreateDeoxyRibonucleicAcid()
    {
        for (var i = 0; i < 6; i++)
        {
            var dnas = new GameObject[6];
            var radiuss = 0.65f;
            var angle = 360f / 6 * i;
            var position = new Vector3(
                Mathf.Cos(angle * Mathf.Deg2Rad) * radiuss - 2,
                Mathf.Sin(angle * Mathf.Deg2Rad) * radiuss + 1,
                0);
            var randomRotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
            dnas[i] = Instantiate(dna, position, randomRotation, transform);
            dnas[i].AddComponent<Rigidbody2D>();
            dnas[i].GetComponent<Rigidbody2D>().gravityScale = 0;
            dnas[i].GetComponent<Rigidbody2D>().drag = 1.2f;
            dnas[i].GetComponent<Rigidbody2D>().angularDrag = 0.2f;
            dnas[i].AddComponent<SpringJoint2D>();
            dnas[i].GetComponent<SpringJoint2D>().connectedBody = _centerRigidBody;
            dnas[i].GetComponent<SpringJoint2D>().frequency = springiness;
        }
    }
}