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

    public static Vector2 Target { get; set; }

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
        var direction3D = new Vector3 { x = direction.x, z = direction.y, };
        var ray = new Ray(transform.position, direction3D);
        bool found = Physics.Raycast(ray, out RaycastHit hit, direction3D.magnitude * Time.deltaTime * followSpeed);
        if (found)
        {
            transform.position = hit.point - direction3D.normalized * 0.05f;
            var perpendicular = Vector3.Cross(Vector3.up, hit.normal);
            transform.position += perpendicular * Vector3.Dot(perpendicular, direction3D) * Time.deltaTime * followSpeed;
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

    public void Kill()
    {
        gameObject.SetActive(false);
    }
}
