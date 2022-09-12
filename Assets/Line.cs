using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Line : MonoBehaviour
{
    public LineRenderer lineRenderer;

    List<Vector2> points;

    public void updateLine(Vector2 postion) {

        if (points == null) {
            points = new List<Vector2>();
            SetPoint(postion);
            return;
        }

        if (Vector2.Distance(points.Last(), postion) > 1f) {

            SetPoint(postion);
        }
    
    }
    void SetPoint(Vector2 point) {


        points.Add(point);

        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPosition(points.Count - 1, point);
    
    }


}
