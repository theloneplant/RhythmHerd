using UnityEngine;

public class HerdMember : MonoBehaviour
{
    public Vector2Int Position
    {
        get
        {
            return new Vector2Int
            {
                x = (int)transform.position.x,
                y = (int)transform.position.z,
            };
        }

        set
        {
            transform.position = new Vector3
            {
                x = value.x,
                z = value.y,
            };
        }
    }

    public HerdMember next { get; set; }

    public void Follow(Vector2Int position)
    {
        if (next != null) {
            next.Follow(Position);
        }
        Position = position;
    }

    public void Kill()
    {
        gameObject.SetActive(false);
    }
}
