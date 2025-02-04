using System.Collections.Generic;
using UnityEngine;

public class ScreenBoundaries : MonoBehaviour
{
    private void Start()
    {
        EdgeCollider2D boundaries = GetComponent<EdgeCollider2D>();

        List<Vector2> edges = new()
        {
            Camera.main.ViewportToWorldPoint(Vector2.zero + new Vector2(0.015f, 0)),
            Camera.main.ViewportToWorldPoint(Vector2.up + new Vector2(0.015f, 0)),
            Camera.main.ViewportToWorldPoint(new Vector2(1,1) + new Vector2(-0.0162f, 0)),
            Camera.main.ViewportToWorldPoint(Vector2.right + new Vector2(-0.0162f, 0)),
            Camera.main.ViewportToWorldPoint(Vector2.zero + new Vector2(0.015f, 0)),
        };

        boundaries.SetPoints(edges);
    }
}
