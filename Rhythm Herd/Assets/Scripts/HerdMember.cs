using System.Collections;
using UnityEngine;

public class HerdMember : MonoBehaviour
{
    [SerializeField] private float moveRandomization = 0.5f;
    [SerializeField] private float followSpeed = 1f;
    [SerializeField] private float roamDistance = 1f;
    [SerializeField] private LayerMask mask;

    public enum MemberState
    {
        Joined, Rejoin, Roam
    }

    public Vector2 Offset { get; set; }
    private MemberState state;

    public static Vector2Int Target { get; set; }

    private Vector2 direction;
    private float currentFollowSpeed;

    private Vector2 startDisorientedPosition;
    private Vector2 targetDisorientedPosition;
    private float startDisoriented;

    public Vector2 Position
    {
        get => new Vector2
        {
            x = transform.position.x,
            y = transform.position.z,
        };
        set => transform.position = new Vector3
        {
            x = value.x,
            z = value.y,
        };
    }

    private void Start()
    {
        state = MemberState.Joined;
        currentFollowSpeed = followSpeed * Random.Range(1f - moveRandomization, 1f);
        startDisoriented = Time.time;
        GameManager.OnBeat += updateTargetDisorientedPosition;
    }

    private void OnValidate()
    {
        moveRandomization = Mathf.Clamp01(moveRandomization);
    }

    private void Update()
    {
        if (state == MemberState.Joined)
        {
            UpdateCustom();
        }
        else if (state == MemberState.Rejoin)
        {

        }
        else
        {
            Roam();
        }
    }

    private void UpdateCustom()
    {
        direction = Target + Offset - Position;
        direction = direction.magnitude > 1f ? direction.normalized : direction;
        Vector2 destination, normal;
        if (Herd.Tracer.Trace(Position, direction * Time.deltaTime * currentFollowSpeed, out destination, out normal))
        {
            Position = destination;
            var perp = Vector2.Perpendicular(normal);
            Position += perp * Vector2.Dot(perp, direction) * Time.deltaTime * currentFollowSpeed;
            Debug.DrawLine(transform.position, transform.position + new Vector3 { x = normal.x, z = normal.y, }, Color.red);
        }
        else
        {
            Position += direction * Time.deltaTime * currentFollowSpeed;
        }
    }

    private void Roam()
    {
        Vector2 direction = targetDisorientedPosition - Position;
        Position += direction * Time.deltaTime * currentFollowSpeed;
    }

    private void updateTargetDisorientedPosition()
    {
        float x = Random.Range(startDisorientedPosition.x - roamDistance, startDisorientedPosition.x + roamDistance);
        float y = Random.Range(startDisorientedPosition.y - roamDistance, startDisorientedPosition.y + roamDistance);
        targetDisorientedPosition = new Vector2(x, y);
    }

    public void setState(MemberState newState)
    {
        state = newState;
        if (state == MemberState.Roam)
        {
            Debug.Log("not member");
            startDisoriented = Time.time;
            startDisorientedPosition = Position;
            updateTargetDisorientedPosition();
        }
    }

    public bool IsDisoriented()
    {
        Debug.Log("disoriented: " + (Time.time - startDisoriented < 10f));
        return (Time.time - startDisoriented) < 10f;
    }

    public MemberState GetState()
    {
        return state;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            var velocity3 = new Vector3 { x = direction.x, z = direction.y };
            UnityEditor.Handles.color = Color.red;
            //UnityEditor.Handles.DrawLine(transform.position, transform.position + velocity3);
        }
    }
#endif

    public void Kill()
    {
        gameObject.SetActive(false);
    }
}
