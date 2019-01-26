using UnityEngine;

public class Herd : MonoBehaviour
{
    [SerializeField] private HerdMember memberPrefab = null;
    [SerializeField] private int memberCount = 6;
    [SerializeField] private GridFromChildren grid = null;
    [SerializeField] private float herdRadius = 1f;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float minimumSeparation = 0.1f;

    private HerdMember[] members;

    private static float _followSpeed;
    public static float FollowSpeed => 1f - 1f / (_followSpeed + 1f);

    private void Start()
    {
        members = new HerdMember[memberCount];
        _followSpeed = moveSpeed;
        for (int i = 0; i < memberCount; i++)
        {
            members[i] = Instantiate(memberPrefab, transform);
        }
        members[0].GetComponent<MeshRenderer>().material.color = Color.red;
        members[0].FollowModifier = 1f;
        for (int i = 1; i < memberCount; i++)
        {
            var offset = Vector2.zero;
            while (ClosestMember(offset) < minimumSeparation)
            {
                offset = Random.insideUnitCircle * herdRadius;
            }
            members[i].Offset = offset;
        }
    }

    private void Update()
    {
        Vector2Int direction = InputDirection();
        if (direction != Vector2Int.zero)
        {
            //Leader.Move(InputDirection());
            HerdMember.Target += direction;
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
        _followSpeed = moveSpeed;
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
