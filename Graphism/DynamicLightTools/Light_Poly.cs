using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light_Poly
{
    private PolygonCollider2D poly;

    public Light_Poly(PolygonCollider2D poly)
    {
        this.poly = poly;
    }
    public List<Vector3> GetVerticies()
    {
        List<Vector3> points = new List<Vector3>();


        Vector3 positionPoly = poly.transform.position;
        if (poly.transform.eulerAngles.z != 0)
        {
            float angle = poly.transform.eulerAngles.z * Mathf.Deg2Rad;

            foreach (Vector2 v in poly.points)
            {
                Vector3 rotatedPoint = Light_Tools.RotatePoint(new Vector2(v.x * poly.transform.localScale.x, v.y * poly.transform.localScale.y), angle);
                points.Add(rotatedPoint + positionPoly);
            }
        }
        else
        {
            foreach (Vector2 v in poly.points)
            {
                points.Add(new Vector3(v.x * poly.transform.localScale.x + positionPoly.x, v.y * poly.transform.localScale.y + positionPoly.y));
            }
        }
        return points;
    }

}
