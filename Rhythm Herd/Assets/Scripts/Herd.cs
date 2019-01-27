using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Herd : MonoBehaviour
{
    [SerializeField] private HerdMember memberPrefab = null;
    [SerializeField] private GridFromChildren grid = null;
    [SerializeField] private HerdGatherer gatherer = null;
    [SerializeField] private int memberCount = 6;
    [SerializeField] private float herdRadius = 1f;
    [SerializeField] private float minimumSeparation = 0.1f;

    private LinkedList<HerdMember> members;

    public static GridRaytracer Tracer { get; set; }

    private void Start()
    {
        Tracer = new GridRaytracer(grid.Grid);
        members = new LinkedList<HerdMember>();
        for (int i = 0; i < memberCount; i++)
        {
            HerdMember member = Instantiate(memberPrefab, transform);
            members.AddLast(member);
            member.setState(HerdMember.MemberState.Joined);
        }
        foreach (HerdMember member in members)
        {
            var offset = Vector2.zero;
            while (ClosestMember(offset) < minimumSeparation)
            {
                offset = Random.insideUnitCircle * herdRadius;
            }
            member.Offset = offset;
            member.Position = offset;
        }
    }

    private void Update()
    {
        gatherer.transform.position = new Vector3(HerdMember.Target.x, 0, HerdMember.Target.y);
        Vector2Int direction = InputDirection();
        if (direction != Vector2Int.zero)
        {
            DropMembers();
            var destination = HerdMember.Target + direction;
            if (!grid.Grid.GetCell(destination))
            {
                HerdMember.Target += direction;
            }
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

    private void DropMembers()
    {
        float score = GameManager.instance.getBeatScore();
        if (score > 0.9f)
        {
            // Cheer
        }
        else if (score < 0.75f)
        {
            Debug.Log("removing");
            members.Last.Value.setState(HerdMember.MemberState.Roam);
            members.RemoveLast();
        }
    }

    public void AddMember(HerdMember newMember)
    {
        newMember.setState(HerdMember.MemberState.Roam);
        members.AddFirst(newMember);
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
