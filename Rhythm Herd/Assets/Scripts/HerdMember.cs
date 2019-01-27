using System.Collections;
using UnityEngine;

public class HerdMember : MonoBehaviour
{
    [SerializeField] private float moveRandomization = 0.5f;
    [SerializeField] private float followSpeed = 1f;
    [SerializeField] private LayerMask mask;

    public Vector2 Offset { get; set; }

    public static Vector2 Target { get; set; }

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
            Position += direction * Time.deltaTime * followSpeed;
        }
    }

    public void Kill()
    {
        gameObject.SetActive(false);
    }
}
