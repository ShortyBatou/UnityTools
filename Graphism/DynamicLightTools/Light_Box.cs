using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light_Box
{
    private BoxCollider2D box;
    private Vector3 center;
    private Vector2 size;

    public Light_Box(BoxCollider2D box)
    {
        this.box = box;
        this.size = box.bounds.extents;
        this.center = box.bounds.center;
    }
    
    //récupère les points d'un box collider
    public List<Vector3> GetVertices()
    {
        List<Vector3> points = new List<Vector3>();
        if (box.transform.eulerAngles.z != 0)
        {
            float angle = box.transform.eulerAngles.z * Mathf.Deg2Rad;
            Vector2 vect = new Vector2(box.transform.localScale.x / 2, box.transform.localScale.y / 2);
            for (int i = -1; i < 2; i += 2)
            {
                for (int j = -1; j < 2; j += 2)
                {
                    Vector3 rotatedPoint = new Vector3(vect.x * i, vect.y * j);
                    rotatedPoint = Light_Tools.RotatePoint(rotatedPoint, angle);
                    points.Add(center + rotatedPoint);
                }
            }
        }
        else
        {
            points.Add(new Vector3(center.x - size.x, center.y - size.y, 0));
            points.Add(new Vector3(center.x - size.x, center.y + size.y, 0));
            points.Add(new Vector3(center.x + size.x, center.y + size.y, 0));
            points.Add(new Vector3(center.x + size.x, center.y - size.y, 0));
        }
        return points;
    }

    //trouve tous les points qui sont "visibles" par la lumière
    public List<Vector3> FindPoint(List<Vector3> points, DynamicLight light)
    {
        for (int i = 0; i < points.Count; i++)
        {

            Vector2 direction = points[i] - light.transform.position;

            float distance = Mathf.Sqrt(direction.x * direction.x + direction.y * direction.y) - 0.05f; ;
            if (distance <= light.GetRadius())
            {
                // on s'arrête juste avant le collider pour voir s'il touche quand même. Si c'est le cas alors le raycast se rentre dedans.
                RaycastHit2D hit = Physics2D.Raycast(light.transform.position, direction, distance - 0.05f, light.layer);
                
                // si le raycast ne touche rien alors le collider est en contacte direct avec la lumière
                if (!hit)
                {
                    float angle = Light_Tools.GetAngle(direction);
                    points[i] = new Vector3(points[i].x, points[i].y, angle);
                }
                // si le dernier point n'est pas le collider cela veut dire que, 
                // normalement il aurait due être pris en compte mais un objet bloque la lumière
                else
                {
                    points.Remove(points[i]);
                    i--;
                }
            }
            else
            {
                points.Remove(points[i]);
                i--;
            }
        }
        return points;

    }

    public List<Vector3> GetIntercections(List<Vector3> points, DynamicLight light)
    {
        for (int i = 0; i < points.Count; i++)
        {
            Light_Tools.GetIntersection(points[i], points[(i + 1) % (points.Count)], points, light);
        }
        return points;
    }
    public List<Vector3> CreateBoxLimit(List<Vector3> points, Collider2D collider, DynamicLight light)
    {
        int nbPoint = points.Count;
        for (int i = 0; i < nbPoint; i++)
        {
            float angle = Mathf.Abs(points[i].z);
            // le facteur permet de positionner un peu plus loin l'origine du point pour éviter que le raycast commence à la limite du collider
            Vector3 facteur = new Vector3(0.1f * Mathf.Cos(angle), 0.1f * Mathf.Sin(angle));

            Vector2 origine = points[i] + facteur;

            // la distance entre le centre de la lumière et le point d'origine de notre raycast c'est pour éviter que le raycast ne dépasse la limite du cercle
            if (!Physics2D.OverlapPoint(origine, light.layer))
            {
                Vector3 limite = new Vector3();

                Vector3 direction = points[i] - light.transform.position;

                float distance = Mathf.Sqrt(direction.x * direction.x + direction.y * direction.y);

                RaycastHit2D hit = Physics2D.Raycast(origine, direction, light.GetRadius() - distance, light.layer);

                if (hit)
                {
                    limite = hit.point;
                }
                else
                {
                    limite = new Vector3(light.transform.position.x + Mathf.Cos(angle) * light.GetRadius()
                                                , light.transform.position.y + Mathf.Sin(angle) * light.GetRadius()
                                                );
                }

                limite += new Vector3(0, 0, direction.z);
                float angleCollider = Light_Tools.GetAngle(collider.transform.position - light.transform.position);

                if (limite.z < angleCollider)
                {
                    if (Mathf.Abs(limite.z - angleCollider) > Mathf.PI)
                    {
                        points.Add(limite);
                    }
                    else
                    {
                        points.Insert(i, limite);
                        i++;
                        nbPoint++;
                    }

                }
                else if (limite.z > angleCollider)
                {
                    if (Mathf.Abs(limite.z - angleCollider) > Mathf.PI)
                    {
                        points.Insert(i, limite);
                        i++;
                        nbPoint++;

                    }
                    else
                    {
                        points.Add(limite);
                    }
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
