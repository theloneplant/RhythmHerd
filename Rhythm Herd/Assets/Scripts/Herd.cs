using UnityEngine;

public class Herd : MonoBehaviour
{
    [SerializeField] private HerdMember memberPrefab = null;
    [SerializeField] private int memberCount = 6;
    [SerializeField] private float herdRadius = 1f;
    [SerializeField] private float minimumSeparation = 0.1f;

    public delegate void MoveAction();
    public static event MoveAction OnMove;

    private HerdMember[] members;

    public static GridRaytracer Tracer { get; set; }

    private void Start()
    {
        members = new HerdMember[memberCount];
        for (int i = 0; i < memberCount; i++)
        {
            members[i] = Instantiate(memberPrefab, transform);
        }
        members[0].GetComponent<MeshRenderer>().material.color = Color.red;
        for (int i = 1; i < memberCount; i++)
        {
            Vector2 offset = Vector2.zero;
            const int MAX_TRIES = 20;
            for (int j = 0; j < MAX_TRIES && ClosestMember(offset) < minimumSeparation; j++)
            {
                offset = Random.insideUnitCircle * herdRadius;
            }
            members[i].Offset = offset;
            members[i].Position = offset;
        }
    }

    private void Update()
    {
        Vector2 direction = InputDirection();
        if (direction != Vector2.zero)
        {
            var direction3D = new Vector3 { x = direction.x, z = direction.y, };
            var origin = new Vector3 { x = HerdMember.Target.x, z = HerdMember.Target.y, };
            var ray = new Ray(origin, direction3D);
            bool found = Physics.Raycast(ray, out RaycastHit hit, direction3D.magnitude);
            if (found)
            {
                Debug.Log("Hit");
                Vector3 position = hit.point - direction3D.normalized * 0.05f;
                HerdMember.Target = new Vector2 { x = position.x, y = position.z, };
            }
            else
            {
                Debug.Log("Not hit.");
                HerdMember.Target += direction;
            }
            OnMove?.Invoke();
        }
    }

    private float ClosestMember(Vector2 point)
    {
        float closest = Mathf.Infinity;
        foreach (HerdMember member in members)
        {
            float distance = Vector2.Distance(member.Offset, point);
            closest = Mathf.Min(closest, distance);
        }
        return closest;
    }

    private void OnValidate()
    {
        memberCount = memberCount > 0 ? memberCount : 1;
    }

    private Vector2 InputDirection()
    {
        if (Input.GetButtonDown("Up"))
        {
            return Vector2.up;
        }
        else if (Input.GetButtonDown("Down"))
        {
            return Vector2.down;
        }
        else if (Input.GetButtonDown("Left"))
        {
            return Vector2.left;
        }
        else if (Input.GetButtonDown("Right"))
        {
            return Vector2.right;
        }
        return Vector2.zero;
    }
}
