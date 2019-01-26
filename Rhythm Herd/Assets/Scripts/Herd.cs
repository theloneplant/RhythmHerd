using UnityEngine;
using UnityEngine.Assertions;

public class Herd : MonoBehaviour
{
    public GridFromChildren grid;

    private HerdMember leader;
    private int memberCount;

    private void Start()
    {
        HerdMember[] members = Members();
        memberCount = members.Length;
        leader = members[0];
        for (int i = 0; i < members.Length; i++)
        {
            HerdMember member = members[i];
            if (i + 1 < members.Length)
            {
                member.next = members[i + 1];
            }
            member.Position = new Vector2Int { x = i, };
            member.GetComponent<MeshRenderer>().material.color = Random.ColorHSV();
        }
    }

    private void Update()
    {
        Vector2Int direction = InputDirection();
        if (direction != Vector2Int.zero)
        {
            Vector2Int destination = leader.Position + direction;
            if (!HitsTail(destination) && !HitsObject(destination))
            {
                leader.Follow(destination);
            }
        }

        // tmp
        if (Input.GetKeyDown(KeyCode.A))
        {
            //KillMember(0);
            KillRandomMember();
        }
    }

    private bool HitsObject(Vector2Int destination)
    {
        if (grid != null)
        {
            return grid.Grid.GetCell(destination);
        }
        return false;
    }

    public void KillRandomMember()
    {
        int index = Random.Range(0, memberCount);
        KillMember(index);
    }

    private void KillMember(int index)
    {
        HerdMember previous = null;
        HerdMember member = leader;
        for (int i = 1; i < index - 1; i++)
        {
            previous = member;
            member = member.next;
        }
        if (member.next != null)
        {
            if (previous != null)
            {
                previous.next = member.next;
            }
            else
            {
                leader = member.next;
            }
            member.next.Follow(member.Position);
        }
        member.Kill();
        memberCount -= 1;
    }

    private bool HitsTail(Vector2Int destination)
    {
        bool hasHit = false;
        HerdMember member = leader.next;
        while (member != null)
        {
            hasHit |= member.Position == destination;
            member = member.next;
        }
        return hasHit;
    }

    private HerdMember[] Members()
    {
        HerdMember[] members = GetComponentsInChildren<HerdMember>();
        Assert.IsTrue(members.Length > 0, "No members in herd.");
        return members;
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
