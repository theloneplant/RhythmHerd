using System.Collections.Generic;
using UnityEngine;

public class Herd : MonoBehaviour
{
    [SerializeField] private HerdMember memberPrefab = null;
    [SerializeField] private HerdGatherer gatherer = null;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private int memberCount = 6;
    [SerializeField] private float herdRadius = 1f;
    [SerializeField] private float minimumSeparation = 0.1f;

    private LinkedList<HerdMember> members;

    private void Start()
    {
        members = new LinkedList<HerdMember>();
        for (int i = 0; i < memberCount; i++)
        {
            HerdMember member = Instantiate(memberPrefab, transform);
            members.AddLast(member);
            member.SetState(HerdMember.MemberState.Joined);
        }
        foreach (HerdMember member in members)
        {
            Vector2 offset = Vector2.zero;
            const int MAX_TRIES = 20;
            for (int j = 0; j < MAX_TRIES && ClosestMember(offset) < minimumSeparation; j++)
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
        Vector2 direction = InputDirection();
        if (direction != Vector2.zero)
        {
            DropMembers();
            var direction3D = new Vector3 { x = direction.x, z = direction.y, };
            var origin = new Vector3 { x = HerdMember.Target.x, z = HerdMember.Target.y, };
            var ray = new Ray(origin, direction3D);
            bool found = Physics.Raycast(ray, out RaycastHit hit, direction3D.magnitude, layerMask);
            if (found)
            {
                Debug.Log("Hit - " + Time.time);
                Vector3 position = hit.point - direction3D.normalized * 0.05f;
                HerdMember.Target = new Vector2 { x = position.x, y = position.z, };
            }
            else
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
            if (score > 0.98)
            {
                Cheer(5);
            }
            else if (score > 0.95)
            {
                Cheer(3);
            }
            else if (score > 0.93)
            {
                Cheer(2);
            }
            else
            {
                Cheer(1);
            }
        }
        else if (score < 0.75f)
        {
            members.Last?.Value.SetState(HerdMember.MemberState.Roam);
            if (members.Count > 0)
            {
                members.RemoveLast();
            }
        }
    }

    private void Cheer(int numberOfCheers)
    {
        LinkedListNode<HerdMember> currentMember = members.First;
        for (int i = 0; i <= numberOfCheers; i++)
        {
            HerdMember member = currentMember?.Value;
            member?.Cheer();
            currentMember = currentMember?.Next;
        }
    }

    public void CheerAll()
    {
        Cheer(members.Count);
    }

    public void AddMember(HerdMember newMember)
    {
        newMember.SetState(HerdMember.MemberState.Roam);
        members.AddFirst(newMember);
    }

    public int JoinedCount()
    {
        int count = 0;
        LinkedListNode<HerdMember> member = members.First;
        while (member != null && member.Next != null)
        {
            count += member.Value.GetState() == HerdMember.MemberState.Joined ? 1 : 0;
            member = member.Next;
        }
        return count;
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
