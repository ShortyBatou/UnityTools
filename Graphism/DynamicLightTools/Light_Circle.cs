using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light_Circle
{

    private CircleCollider2D circle;
    private Vector3 center;
    private float radius;

    public Light_Circle(CircleCollider2D circle)
    {
        this.circle = circle;
        this.radius = circle.radius;
        this.center = circle.bounds.center;
    }


    public List<Vector3> GetVertices(List<Vector3> points, DynamicLight light)
    {

        // --- 
        bool inversion = false;

        // on cherche la tangentes à du cercle passant par l'origine de la lumiére

        // le cercle du colider
        float x0 = center.x - light.transform.position.x;
        float y0 = center.y - light.transform.position.y;
        float r0 = circle.radius * circle.transform.localScale.x;

        // le cercle qui à pour point l'origine de la lumière, le centre du cercle collider, et les points des tangentes 
        float x1 = x0 / 2;
        float y1 = y0 / 2;
        float r1 = Mathf.Sqrt(x1 * x1 + y1 * y1);

        if (Mathf.Abs(y0 - y1) < 0.3f)
        {
            inversion = true;
            float _ = x0;
            x0 = y0;
            y0 = _;

            _ = x1;
            x1 = y1;
            y1 = _;

        }

        float A, B, C, N, a, delta;
        // un coef utile pour ne pas répéter le calcul
        a = (x0 - x1) / (y0 - y1);

        // pareil
        N = (r1 * r1 - r0 * r0 - x1 * x1 + x0 * x0 - y1 * y1 + y0 * y0) / (2 * (y0 - y1));

        // on a une équation du second degrés du type Ax² + Bx + C =0
        A = a * a + 1;
        B = 2 * y0 * a - 2 * N * a - 2 * x0;
        C = x0 * x0 + y0 * y0 + N * N - r0 * r0 - 2 * y0 * N;

        // on trouve le delta
        delta = Mathf.Sqrt(B * B - 4 * A * C);

        // on en déduit les deux solutions
        for (int i = -1; i < 2; i += 2)
        {
            float x = (-B - delta * i) / (2 * A);
            float y = N - x * a;
            if (inversion)
            {
                float _ = y;
                y = x;
                x = _;
            }
            points.Add(new Vector3(x + light.transform.position.x, y + light.transform.position.y));
        }
        for (int i = 0; i < 2; i++)
        {
            Vector3 direction = points[i] - light.transform.position;
            points[i] = new Vector3(points[i].x, points[i].y, Light_Tools.GetAngle(direction));
        }
        return points;

    }

    public List<Vector3> FindCirclePoint(List<Vector3> points, DynamicLight light)
    {
        for (int i = 0; i < points.Count; i++)
        {
            Vector2 direction = points[i] - light.transform.position;

            float distance = Mathf.Sqrt(direction.x * direction.x + direction.y * direction.y);

            RaycastHit2D[] hits = Physics2D.RaycastAll(light.transform.position, direction, distance, light.layer);

            bool touchAnotherCollider = false;
            foreach (RaycastHit2D h in hits)
            {
                if (h.collider != circle)
                {
                    touchAnotherCollider = true;
                    break;
                }
            }
            if (!touchAnotherCollider)
            {
                float angle = Light_Tools.GetAngle(direction);
                points[i] = new Vector3(points[i].x, points[i].y, angle);
            }
            else
            {
                points.Remove(points[i]);
                i--;
            }
        }
        return points;
    }
    // fait les tests pour les projections pour les ombres (  cercle ) //
    public List<Vector3> CreateCircleLimit(List<Vector3> points, DynamicLight light)
    {
        for (int i = 0; i < points.Count; i++)
        {
            Vector3 limite = Vector3.zero;
            Vector3 direction = points[i] - light.transform.position;
            float distance = Mathf.Sqrt(direction.x * direction.x + direction.y * direction.y);
            if (light.GetRadius() - distance > 0)
            {
                RaycastHit2D[] hit = Physics2D.RaycastAll(points[i], direction, light.GetRadius() - distance, light.layer);
                float angle = direction.z;
                foreach (RaycastHit2D h in hit)
                {

                    if (h.collider != circle)
                    {

                        limite = h.point;
                        break;
                    }
                }
                if (limite == Vector3.zero)
                {
                    limite = new Vector3(light.transform.position.x + Mathf.Cos(angle) * light.GetRadius()
                                                , light.transform.position.y + Mathf.Sin(angle) * light.GetRadius()
                                                );

                }
                limite.z = angle;

                // redondance
                if (i < points.Count - 1)
                {
                    points.Insert(i, limite);
                    i++;
                }
                else if (points.Count == 1)
                {
                    float angleCollider = Light_Tools.GetAngle(circle.transform.position - light.transform.position);
                    if (points[0].z < angleCollider || points[0].z - angleCollider > Mathf.PI)
                    {
                        points.Insert(0, limite);
                    }
                    else
                    {
                        points.Add(limite);
                    }
                }
                else
                {
                    points.Add(limite);
                    i++;
                }
            }
        }
        if (Mathf.PI < Mathf.Abs(points[points.Count - 1].z) - Mathf.Abs(points[0].z))
        {
            Vector3 _ = points[0];
            points[0] = points[1];
            points[1] = _;

            _ = points[points.Count - 1];
            points[points.Count - 1] = points[points.Count - 2];
            points[points.Count - 2] = _;
        }
        return points;
    }
}
