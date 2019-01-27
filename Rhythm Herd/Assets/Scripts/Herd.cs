using UnityEngine;

public class Herd : MonoBehaviour
{
    [SerializeField] private HerdMember memberPrefab = null;
    [SerializeField] private GridFromChildren grid = null;
    [SerializeField] private int memberCount = 6;
    [SerializeField] private float herdRadius = 1f;
    [SerializeField] private float minimumSeparation = 0.1f;

    public delegate void MoveAction();
    public static event MoveAction OnMove;

    private HerdMember[] members;

    public static GridRaytracer Tracer { get; set; }

    private void Start()
    {
        Tracer = new GridRaytracer(grid.Grid);
        members = new HerdMember[memberCount];
        for (int i = 0; i < memberCount; i++)
        {
            members[i] = Instantiate(memberPrefab, transform);
        }
        members[0].GetComponent<MeshRenderer>().material.color = Color.red;
        for (int i = 1; i < memberCount; i++)
        {
            var offset = Vector2.zero;
            while (ClosestMember(offset) < minimumSeparation)
            {
                offset = Random.insideUnitCircle * herdRadius;
            }
            members[i].Offset = offset;
            members[i].Position = offset;
        }
    }

    private void Update()
    {
        Vector2Int direction = InputDirection();
        if (direction != Vector2Int.zero)
        {
            var destination = HerdMember.Target + direction;
            if (!grid.Grid.GetCell(destination))
            {
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

    private Vector2Int InputDirection()
    {
        if (Input.GetButtonDown("Up"))
        {
            return Vector2Int.up;
        }
        else if (Input.GetButtonDown("Down"))
        {
            return Vector2Int.down;
        }
        else if (Input.GetButtonDown("Left"))
        {
            return Vector2Int.left;
        }
        else if (Input.GetButtonDown("Right"))
        {
            return Vector2Int.right;
        }
        return Vector2Int.zero;
    }
}
