using System.Collections.Generic;
using UnityEngine;

public class GridFromColliders : GridGenerator
{
    private void Awake()
    {
        var positions = new List<Vector2Int>();
        var bounds = new WorldGrid.Bounds
        {
            min = new Vector2Int { x = 100000, y = 100000 },
            max = new Vector2Int { x = -100000, y = -100000 },
        };
        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders)
        {
            Vector3 position3 = collider.transform.position;
            var position = new Vector2Int { x = (int)position3.x, y = (int)position3.z, };
            bounds.min = Vector2Int.Min(bounds.min, position);
            bounds.max = Vector2Int.Max(bounds.max, position);
            positions.Add(position);
        }
        Grid = new WorldGrid(bounds);
        foreach (Vector2Int position in positions)
        {
            Grid.SetCell(true, position);
        }
        Debug.Log(string.Format("min: {0}, {1}  max: {2}, {3}", bounds.min.x, bounds.min.y, bounds.max.x, bounds.max.y));
        Debug.Log(string.Format("dimensions: {0}, {1}", bounds.Dimensions.x, bounds.Dimensions.y));
    }
}
