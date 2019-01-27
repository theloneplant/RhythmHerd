using System;
using UnityEngine;

public class DebugRaytracer : MonoBehaviour
{
    public GridFromChildren grid;
    public Transform target;
    public bool hit;

    public Vector3 Start { get; set; }
    public Vector3 End { get; set; }

    private Vector2 V3toV2(Vector3 value)
    {
        return new Vector2 { x = value.x, y = value.z };
    }

    private Vector3 V2toV3(Vector2 value)
    {
        return new Vector3 { x = value.x, z = value.y };
    }

    private void Update()
    {
        Vector2 start = V3toV2(Start);
        Vector2 end = V3toV2(End);
        hit = false;
        if (Query(end))
        {
            hit = true;
            float position = 0.5f;
            for (int i = 0; i < 8; i++)
            {
                var between = Vector2.Lerp(start, end, position);
                float magnitude = 1f / ((float)(i + 2) * (i + 2));
                position += magnitude * (Query(between) ? -1f : 1f);
            }
            target.position = V2toV3(Vector2.Lerp(start, end, position));
        }
    }

    private bool Query(Vector2 point)
    {
        var cast = new Vector2Int { x = Mathf.FloorToInt(point.x + 0.5f), y = Mathf.FloorToInt(point.y + 0.5f), };
        return grid.Grid.GetCell(cast);
    }
}
