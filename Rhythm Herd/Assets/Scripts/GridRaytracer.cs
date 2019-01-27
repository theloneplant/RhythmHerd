using UnityEngine;

public class GridRaytracer
{
    private WorldGrid grid;

    public GridRaytracer(WorldGrid grid)
    {
        this.grid = grid;
    }

    public bool Trace(Vector2 from, Vector2 direction, out Vector2 destination, out Vector2 normal)
    {
        Vector2 to = from + direction;
        if (Query(to))
        {
            float position = 0.5f;
            for (int i = 0; i < 12; i++)
            {
                var between = Vector2.Lerp(from, to, position);
                float magnitude = 1f / ((float)(i + 2) * (i + 2));
                position += magnitude * (Query(between) ? -1f : 1f);
            }

            const float shellWidth = 0.005f;
            Vector2 shell = direction.normalized * shellWidth;
            destination = Vector2.Lerp(from, to, position) - shell;
            normal = Normal(destination);
            return true;
        }
        destination = Vector2.zero;
        normal = Vector2.zero;
        return false;
    }

    private Vector2 Normal(Vector2 destination)
    {
        float left = destination.x - Mathf.Floor(destination.x);
        float bottom = destination.y - Mathf.Floor(destination.y);
        float right = 1f - left;
        float top = 1f - bottom;
        if (left < bottom && left < right && left < top)
        {
            return Vector2.up;
        }
        else if (bottom < right && bottom < top)
        {
            return Vector2.right;
        }
        else if (right < top)
        {
            return Vector2.down;
        }
        return Vector2.left;
    }

    private bool Query(Vector2 point)
    {
        return grid.GetCell(CellForPoint(point));
    }

    private Vector2Int CellForPoint(Vector2 point)
    {
        return new Vector2Int { x = Mathf.FloorToInt(point.x + 0.5f), y = Mathf.FloorToInt(point.y + 0.5f), };
    }
}
