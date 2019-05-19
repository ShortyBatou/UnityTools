using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light_Tools
{
    public static Vector3 RotatePoint(Vector3 point, float angle)
    {
        point = new Vector2(Mathf.Cos(angle) * point.x - Mathf.Sin(angle) * point.y,
                                Mathf.Cos(angle) * point.y + Mathf.Sin(angle) * point.x);
        return point;
    }

    public static float GetAngle(Vector2 point)
    {
        float angle = Mathf.Atan2(point.y, point.x);
        if (point.y < 0)
        {
            angle = 2 * Mathf.PI + angle;
        }
        return angle;
    }

    //récupère les intercections d'un segment AB sur le cercle
    public static bool GetIntersection(Vector3 A, Vector3 B, List<Vector3> points, DynamicLight light)
    {

        Vector3 C = light.transform.position;

        float R = light.GetRadius();

        Vector3 D = (B - A) / Vector2.Distance(A, B);

        float t = D.x * (C.x - A.x) + D.y * (C.y - A.y);

        Vector3 E = t * D + A;

        float EC = Vector2.Distance(E, C);

        if (EC < R)
        {

            float dt = Mathf.Sqrt(R * R - EC * EC);

            float AdB = Mathf.Pow(Vector2.Distance(A, B), 2);

            Vector3 AB = B - A;

            for (int i = 1; i > -2; i -= 2)
            {
                Vector3 F = (t + dt * i) * D + A;
                Vector3 AF = F - A;

                float ABdAF = Vector2.Dot(AB, AF);
                if (ABdAF > 0 && AdB > ABdAF)
                {
                    points.Add(F);
                }

            }
            return true;

        }

        return false;

    }

    public static List<Vector3> TrieAngle(List<Vector3> points)
    {

        int i, j;
        for (i = 1; i < points.Count; ++i)
        {
            Vector3 p = points[i];
            for (j = i; j > 0 && Mathf.Abs(points[j - 1].z) > Mathf.Abs(p.z); j--)
            {
                points[j] = points[j - 1];
            }

            points[j] = p;
        }
        return points;
    }
}
