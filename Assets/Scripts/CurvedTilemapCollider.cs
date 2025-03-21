using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

[RequireComponent(typeof(Tilemap))]
[RequireComponent(typeof(PolygonCollider2D))]

public class CurvedTilemapCollider : MonoBehaviour
{
    private Tilemap tilemap;
    private PolygonCollider2D polygonCollider;

    // You might want to adjust these values
    public float curveDetail = 0.5f; // Controls the smoothness of the curve (0 to 1)
    public float curveOffset = 0.1f; // How far in to offset the curve

    void Start()
    {
        tilemap = GetComponent<Tilemap>();
        polygonCollider = GetComponent<PolygonCollider2D>();

        GenerateCurvedCollider();
    }

    void GenerateCurvedCollider()
    {
        List<Vector2> points = new List<Vector2>();

        // Loop through all tiles in the tilemap
        BoundsInt bounds = tilemap.cellBounds;
        for (int x = bounds.min.x; x < bounds.max.x; x++)
        {
            for (int y = bounds.min.y; y < bounds.max.y; y++)
            {
                TileBase tile = tilemap.GetTile(new Vector3Int(x, y, 0));
                if (tile != null) // If there's a tile here
                {
                    // Get the world position of the tile's bottom-left corner
                    Vector3 worldPos = tilemap.GetCellCenterWorld(new Vector3Int(x, y, 0));

                    // Check for curved corners and add points
                    AddCurvedCornerPoints(points, worldPos, new Vector2(0, 0), new Vector2(1, 0), new Vector2(0, 1)); // Bottom-left
                    AddCurvedCornerPoints(points, worldPos, new Vector2(1, 0), new Vector2(1, 1), new Vector2(1, 0)); // Bottom-right
                    AddCurvedCornerPoints(points, worldPos, new Vector2(1, 1), new Vector2(0, 1), new Vector2(1, 1)); // Top-right
                    AddCurvedCornerPoints(points, worldPos, new Vector2(0, 1), new Vector2(0, 0), new Vector2(0, 1)); // Top-left
                }
            }
        }

        // Remove duplicate points (important for the PolygonCollider2D)
        points = RemoveDuplicatePoints(points);

        // Set the points to the PolygonCollider2D
        polygonCollider.SetPath(0, points.ToArray());
    }

    void AddCurvedCornerPoints(List<Vector2> points, Vector3 worldPos, Vector2 start, Vector2 corner, Vector2 end)
    {
        // Calculate the points for a curved corner
        Vector2 startPos = (Vector2)worldPos + start;
        Vector2 cornerPos = (Vector2)worldPos + corner;
        Vector2 endPos = (Vector2)worldPos + end;

        // Calculate offset
        Vector2 offsetDir = (cornerPos - (startPos + endPos) / 2).normalized * curveOffset;
        cornerPos += offsetDir;

        // Add the start point
        points.Add(startPos);

        // Add points along the curve
        for (float t = curveDetail; t < 1f; t += curveDetail)
        {
            Vector2 point = Vector2.Lerp(Vector2.Lerp(startPos, cornerPos, t), Vector2.Lerp(cornerPos, endPos, t), t);
            points.Add(point);
        }

        // Add the end point
        points.Add(endPos);
    }

    List<Vector2> RemoveDuplicatePoints(List<Vector2> points)
    {
        List<Vector2> uniquePoints = new List<Vector2>();
        foreach (Vector2 point in points)
        {
            if (!uniquePoints.Contains(point))
            {
                uniquePoints.Add(point);
            }
        }
        return uniquePoints;
    }
}