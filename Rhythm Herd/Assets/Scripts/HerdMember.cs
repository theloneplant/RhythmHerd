using System.Collections;
using UnityEngine;

public class HerdMember : MonoBehaviour
{
    [SerializeField] private float moveRandomization = 0.5f;
    [SerializeField] private float followSpeed = 1f;
    [SerializeField] private LayerMask mask;

    public Vector2 Offset { get; set; }

    public static Vector2Int Target { get; set; }

    private Vector2 direction;

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
        followSpeed *= Random.Range(1f - moveRandomization, 1f);
    }

    private void OnValidate()
    {
        moveRandomization = Mathf.Clamp01(moveRandomization);
    }

    private void Update()
    {
        UpdateCustom();        
    }

    private void UpdateCustom()
    {
        direction = Target + Offset - Position;
        direction = direction.magnitude > 1f ? direction.normalized : direction;
        Vector2 destination, normal;
        if (Herd.Tracer.Trace(Position, direction * Time.deltaTime * followSpeed, out destination, out normal))
        {
            Position = destination;
            var perp = Vector2.Perpendicular(normal);
            Position += perp * Vector2.Dot(perp, direction) * Time.deltaTime * followSpeed;
            Debug.DrawLine(transform.position, transform.position + new Vector3 { x = normal.x, z = normal.y, }, Color.red);
        }
        else
        {
            Position += direction * Time.deltaTime * followSpeed;
        }
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
