using System.Collections;
using UnityEngine;

public class HerdMember : MonoBehaviour
{
    [SerializeField] private float moveRandomization = 0.5f;
    [SerializeField] private float followSpeed = 1f;
    [SerializeField] private float roamDistance = 1f;
    [SerializeField] private float cheerChance = 0.2f;
    [SerializeField] private float lowChance = 0.2f;
    [SerializeField] private LayerMask mask;
    [SerializeField] private ParticleSystem particle;
    [SerializeField] private Transform memberModel;
    [SerializeField] private MeshRenderer question;
    [SerializeField] private MeshRenderer exclamation;
    [SerializeField] private AudioClip ow;
    [SerializeField] private AudioClip owLow;
    [SerializeField] private AudioClip cheer;
    [SerializeField] private AudioClip cheerLow;

    public enum MemberState
    {
        Joined, Rejoin, Roam
    }

    public Vector2 Offset { get; set; }
    private MemberState state;

    public static Vector2 Target { get; set; }

    private Vector2 direction;
    private float currentFollowSpeed;

    private Vector2 startRoamPosition;
    private Vector2 targetRoamPosition;
    private float startDisoriented;
    private Vector3 startLookAtOffset;

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
        startDisoriented = Time.time - 10f;
        GameManager.OnBeat += UpdateTargetRoamPosition;
        startLookAtOffset = memberModel.position - transform.position;
    }

    private void OnValidate()
    {
        moveRandomization = Mathf.Clamp01(moveRandomization);
    }

    private void Update()
    {
        if (state == MemberState.Joined)
        {
            question.enabled = false;
            exclamation.enabled = false;
            UpdateCustom();
        }
        else
        {
            if (state == MemberState.Rejoin)
            {
                question.enabled = false;
                exclamation.enabled = true;
            }
            else
            {
                question.enabled = true;
                exclamation.enabled = false;
            }
            GoToTargetRoamPosition();
        }

        if (state == MemberState.Rejoin && (Target - Position).magnitude <= roamDistance * 3f)
        {
            SetState(MemberState.Joined);
        }

        memberModel.LookAt(transform.position - new Vector3(direction.x, 0, direction.y));
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

    private void GoToTargetRoamPosition()
    {
        direction = targetRoamPosition - Position;
        Position += direction * Time.deltaTime * currentFollowSpeed;
    }

    private void UpdateTargetRoamPosition()
    {
        if (state == MemberState.Rejoin)
        {
            direction = Target + Offset - Position;
            if (direction.magnitude < 1)
            {
                targetRoamPosition = Position + direction;
            }
            else
            {
                targetRoamPosition = Position + direction.normalized * roamDistance;
            }
        }
        else if (state == MemberState.Roam)
        {
            float x = Random.Range(startRoamPosition.x - roamDistance, startRoamPosition.x + roamDistance);
            float y = Random.Range(startRoamPosition.y - roamDistance, startRoamPosition.y + roamDistance);
            targetRoamPosition = new Vector2(x, y);
        }
    }

    public void Cheer()
    {
        if (state == MemberState.Joined)
        {
            if (isVocal())
            {
                if (isLow())
                {
                    GameManager.PlaySound(cheer, 1.2f, true, 0.2f);
                }
                else
                {
                    GameManager.PlaySound(cheerLow, 0.7f, true, 0.2f);
                }
            }
            particle.Emit(30);
        }
    }

    public void Leave()
    {
        if (isVocal())
        {
            if (isLow())
            {
                GameManager.PlaySound(ow, 1.5f, true, 0.2f);
            }
            else
            {
                GameManager.PlaySound(owLow, 0.7f, true, 0.2f);
            }
        }
    }

    public bool IsDisoriented()
    {
        return (Time.time - startDisoriented) < 10f;
    }

    public void SetState(MemberState newState)
    {
        state = newState;
        if (state == MemberState.Roam)
        {
            startDisoriented = Time.time;
            startRoamPosition = Position;
            UpdateTargetRoamPosition();
        }
    }

    private bool isVocal()
    {
        return Random.Range(0, 1) < cheerChance;
    }

    private bool isLow()
    {
        return Random.Range(0, 1) < lowChance;
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
