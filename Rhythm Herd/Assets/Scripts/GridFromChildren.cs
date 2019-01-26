using UnityEngine;
using System.Collections.Generic;

public class GridFromChildren : MonoBehaviour
{
    public WorldGrid Grid { get; private set; }

    public Vector2Int cell;

    private void Start()
    {
        var positions = new List<Vector2Int>();
        var bounds = new WorldGrid.Bounds
        {
            min = new Vector2Int { x = 100000, y = 100000 },
            max = new Vector2Int { x = -100000, y = -100000 },
        };
        foreach (Transform child in transform)
        {
            var position3 = child.position;
            var position = new Vector2Int { x = (int)position3.x, y = (int)position3.z, }; 
            bounds.min = Vector2Int.Min(bounds.min, position);
            bounds.max = Vector2Int.Max(bounds.max, position);
            positions.Add(position);
        }
        Grid = new WorldGrid(bounds);
        foreach (var position in positions)
        {
            Grid.SetCell(true, position);
        }
        Debug.Log(string.Format("min: {0}, {1}  max: {2}, {3}", bounds.min.x, bounds.min.y, bounds.max.x, bounds.max.y));
        Debug.Log(string.Format("dimensions: {0}, {1}", bounds.Dimensions.x, bounds.Dimensions.y));
    }

    [ContextMenu("Print Cell")]
    public void PrintCell()
    {
        Debug.Log(Grid.GetCell(cell));
    }
}
