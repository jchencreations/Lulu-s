using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(LineRenderer))]
public class InkTracer : MonoBehaviour
{
    private LineRenderer lineRenderer = null;
    private List<Vector3> points = new List<Vector3>();
    [SerializeField] private float drawingThreshold = 0.001f;

    private void Awake()
    {
        this.lineRenderer = GetComponent<LineRenderer>();
    }

    public void UpdateLineRenderer(Vector3 newPosition)
    {
        if (this.IsUpdateRequired(newPosition))
        {
            this.points.Add(newPosition);
            this.lineRenderer.positionCount = this.points.Count;
            this.lineRenderer.SetPosition(this.points.Count - 1, newPosition);
        }
    }

    private bool IsUpdateRequired(Vector3 position)
    {
        if (this.points.Count == 0)
            return true;
        return Vector3.Distance(this.points.Last(), position) > this.drawingThreshold;
    }
}
