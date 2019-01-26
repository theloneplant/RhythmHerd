using UnityEngine;

public class WorldGrid
{
    public struct Bounds
    {
        public Vector2Int min;
        public Vector2Int max;

        public Vector2Int Dimensions
        {
            get
            {
                return max - min;
            }
        }
    }

    private bool[] grid;
    public Bounds bounds;

    public WorldGrid(Bounds bounds)
    {
        this.bounds = bounds;
        Vector2Int dimensions = bounds.Dimensions + Vector2Int.one;
        grid = new bool[dimensions.x * dimensions.y];
    }

    public bool GetCell(Vector2Int position)
    {
        return GetCell(position.x, position.y);
    }

    public bool GetCell(int x, int y)
    {
        int index = IndexForCell(x, y);
        return index != -1 ? grid[IndexForCell(x, y)] : false;
    }

    public void SetCell(bool value, Vector2Int position)
    {
        SetCell(value, position.x, position.y);
    }

    public void SetCell(bool value, int x, int y)
    {
        int index = IndexForCell(x, y);
        if (index != -1)
        {
            grid[IndexForCell(x, y)] = value;
        }
    }

    private int IndexForCell(int x, int y)
    {
        if (x >= bounds.min.x && x <= bounds.max.x && y >= bounds.min.y && y <= bounds.max.y)
        {
            Vector2Int offset = new Vector2Int { x = x, y = y } - bounds.min;
            return offset.x + offset.y * (bounds.Dimensions.x + 1);
        }
        return -1;
    }
}
