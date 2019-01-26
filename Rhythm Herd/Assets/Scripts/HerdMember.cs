using System.Collections;
using UnityEngine;

public class HerdMember : MonoBehaviour
{
    public float moveRandomization = 0.5f;

    public Vector2 Offset { get; set; }

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

    public static Vector2 Target { get; set; }

    public float FollowModifier { get; set; }

    private void Start()
    {
        FollowModifier = Random.Range(1f - moveRandomization, 1f);
    }

    private void OnValidate()
    {
        moveRandomization = Mathf.Clamp01(moveRandomization);
    }

    private void Update()
    {
        Position = Vector2.Lerp(Position, Target + Offset, Herd.FollowSpeed * FollowModifier);
    }

    public void Move(Vector2Int direction)
    {
        StartCoroutine(MoveCoroutine(direction));
    }

    private IEnumerator MoveCoroutine(Vector2 direction)
    {
        Vector2 start = Position;
        Vector2 end = start + direction;
        for (float time = 0f; time < 1f; time += Time.deltaTime * 10f)
        {
            Position = Vector2.Lerp(start, end, time);
            yield return null;
        }
        Position = end;
    }

    public void Kill()
    {
        gameObject.SetActive(false);
    }
}
