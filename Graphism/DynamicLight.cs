using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
// EN COURS DE DEV - Optimisation et strucutre à revoir  // 

public class DynamicLight : MonoBehaviour {

    public int nbRayon;
    public LayerMask layer;
    private CircleCollider2D circleCol;
    private Mesh mesh;
    private MeshFilter mf;

    private Collider2D[] colliders;


    private List<Vector2> verticeList;
    private int[] triangleList;
    public List<Vector3> pointList;


    private void Awake()
    {
        circleCol = gameObject.GetComponent<CircleCollider2D>();
        mf = gameObject.GetComponent<MeshFilter>();
        verticeList = new List<Vector2>();
        pointList = new List<Vector3>();

    }

    // A chaques frame on récupère tous les colliders 
    // et on fait le traitement approprié pour construire la lumière
    private void Update()
    {
        verticeList.Clear();
        pointList.Clear();
        colliders = Physics2D.OverlapCircleAll(transform.position, circleCol.radius, layer);

        
        foreach(Collider2D c in colliders)
        {
            if(c.GetType() == typeof(BoxCollider2D))
            {
                GetBoxVertices((BoxCollider2D)c);
            }
            else if(c.GetType() == typeof(CircleCollider2D))
            {
                GetCircleVertices((CircleCollider2D)c);
            }
            else if(c.GetType() == typeof(PolygonCollider2D))
            {
                GetPolyVertices((PolygonCollider2D)c);
            }
        }

        for (int i=0; i < nbRayon; i++)
        {
            float angle = 2 * Mathf.PI * i / nbRayon;
            Vector2 direction = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle));
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, circleCol.radius, layer);
            
            // un collider même s'il n'est pas touché par un raycast doit être pris en compte.
            // d'avors on déduis les point des colliders et après on remplis le vide avec les raycasts.
            if (hit)
            {
                pointList.Add(new Vector3(hit.point.x, hit.point.y, angle));
            }
            else
            {
                pointList.Add(new Vector3(transform.position.x + direction.x * circleCol.radius
                                            , transform.position.y +direction.y * circleCol.radius
                                            ,angle
                                            ));
            }            
        }
       
        //trie les points celons leurs angles de manière croissante
        pointList = TrieAngle(pointList);
        //ajoute un point pour avoir le centre du cercle ( utilse pour la construction du mesh ) 
        pointList.Insert(0,new Vector3(transform.position.x, transform.position.y, -1));
        pointList = pointList.Distinct().ToList();
        mf.mesh = BuildMesh();
    }

    public float GetRadius()
    {
        return circleCol.radius;
    }

    // récupèretous les points du collider et les traites pour les rajouter à la liste de points //
    private void GetBoxVertices(BoxCollider2D box)
    {
        Light_Box l_box = new Light_Box(box);

        List<Vector3> points = l_box.GetVertices();

        
        float box_radius = Vector2.Distance(box.transform.position, points[0]);
        float box_distance = Vector2.Distance(box.transform.position, transform.position);
        if (box_radius+ box_distance > GetRadius())
        {
            points = l_box.GetIntercections(points, this);
        }
        points = l_box.FindPoint(points, this);

        // si tout les points ne sont pas dans la lumière alors on regarde s'il n'y a pas d'intersection

        if (points.Count > 0)
        { 
            points = Light_Tools.TrieAngle(points);
            
            points = l_box.CreateBoxLimit(points, box, this);
            
            foreach (Vector3 v in points)
            {
                pointList.Add(v);
            }
        }
    }
    
    private void GetCircleVertices(CircleCollider2D circle)
    {
        List<Vector3> points = new List<Vector3>();
        Light_Circle l_circle = new Light_Circle(circle);
        points = l_circle.GetVertices(points,this);
        
        points = l_circle.FindCirclePoint(points,this);
        if (points.Count > 0)
        {
            
            points = Light_Tools.TrieAngle(points);
            points = l_circle.CreateCircleLimit(points, this);
            foreach (Vector3 v in points)
            {
                pointList.Add(v);
            }
        }
        
    }

    private void GetPolyVertices(PolygonCollider2D poly)
    {
        List<Vector3> points = new List<Vector3>();
        Light_Poly l_poly = new Light_Poly(poly);
        points = l_poly.GetVerticies();

        points = FindPolyPoint(points);

        if (points.Count > 0)
        {
            points = TrieAngle(points);            
            foreach (Vector3 v in points)
            {
                pointList.Add(v);
            }
        }
    }
    // -------- //

    // touve tous les points qui sont en "contact" direct avec la lumière //
    private List<Vector3> FindPoint(List<Vector3> points)
    {
        for (int i = 0; i < points.Count; i++)
        {

            Vector2 direction = points[i] - transform.position;

            float distance = Mathf.Sqrt(direction.x * direction.x + direction.y * direction.y) - 0.05f; ;
            if(distance <= circleCol.radius)
            {
                // on s'arrête juste avant le collider pour voir s'il touche quand même. Si c'est le cas alors le raycast se rentre dedans.
                RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance - 0.05f, layer);
                // si le raycast ne touche rien alors le collider est en contacte direct avec la lumière
                if (!hit)
                {
                    float angle = GetAngle(direction);
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


    private List<Vector3> FindPolyPoint(List<Vector3> allPoints)
    {
        List<Vector3> points = new List<Vector3>();
        points = FindIntersections(allPoints);
        points = FindPoint(points);

        AfficheList(points);
        //regarde par pair de 2 points pour faire comme si on traitait des segments un par un au lieu d'un grand polygone
        for(int i = 0; i < allPoints.Count; i+=2)
        {
            Vector3[] segment;

            //évite d'utilise deux fois le point de départ
            if (i < allPoints.Count -1 )
            {
                
                segment = new Vector3[2] { allPoints[i], allPoints[i + 1] };
                segment[0].z = GetAngle(segment[0] - transform.position);
                segment[1].z = GetAngle(segment[1] - transform.position);

            }
            else
            {
                //si on est revenu au point de départ on prend le point précédent
                segment = new Vector3[2] { allPoints[i], allPoints[i - 1] };
                segment[0].z = GetAngle(segment[0] - transform.position);
                segment[1].z = GetAngle(segment[1] - transform.position);
            }


        
            float midAngle = GetAngle((segment[0] + segment[1])/2 - transform.position);

            // met un angle négatif sur le doublon pour ne pas l'ajouter plus tard
            if (i > allPoints.Count - 1)
            {
                segment[1].z *= -1;
            }

            
            foreach (Vector3 v in segment)
            {

                Vector2 direction = v - transform.position;
                float distance = Mathf.Sqrt(direction.x * direction.x + direction.y * direction.y);
                
                //si le point est dans la lumière
                if (distance < circleCol.radius)
                {
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance - 0.05f, layer);
                    // s'il ne touche pas, alors le point est en contacte directe avec la lumière
                    if (!hit)
                    {
                        //si ce n'est pas un doublon, on rajoute
                        if(v.z > 0)
                        {
                            points.Add(v);

                            Vector3 facteur = new Vector3(0.1f * Mathf.Cos(v.z), 0.1f * Mathf.Sin(v.z));

                            Vector2 origine = v + facteur;

                            if (!Physics2D.OverlapPoint(origine, layer))
                            {
                                Vector3 limite;

                                hit = Physics2D.Raycast(origine, direction, circleCol.radius - distance);

                                if (hit)
                                {
                                    limite = hit.point;
                                }
                                else
                                {
                                    limite = new Vector3(transform.position.x + Mathf.Cos(v.z) * circleCol.radius
                                                                , transform.position.y + Mathf.Sin(v.z) * circleCol.radius
                                                                );
                                }

                                limite.z = v.z;
                                if (limite.z < midAngle)
                                {
                                    if (Mathf.Abs(limite.z - midAngle) > Mathf.PI)
                                    {
                                        points.Add(limite);
                                    }
                                    else
                                    {
                                        points.Insert(points.Count - 1, limite);
                                    }
                                    
                                }
                                else if (limite.z > midAngle)
                                {
                                    if (Mathf.Abs(limite.z - midAngle) > Mathf.PI)
                                    {
                                        points.Insert(points.Count - 1, limite);
                                       
                                    }
                                    else
                                    {
                                        points.Add(limite);
                                    }
                                }
                            }
                        }
                    }
                }
               
            }
        }
        return points;
    }
    // -------- //


    
     
    

    private List<Vector3> FindIntersections(List<Vector3> allPoints)
    {
        List<Vector3> points = new List<Vector3>();
        for (int i = 0; i < allPoints.Count; i++)
        {
            GetIntersection(allPoints[i], allPoints[(i + 1) % (allPoints.Count)], points);
            
        }
        return points;
    }

    // affiche une liste de vecteur sur une seule ligne ( utile pour le débug )   
    private void AfficheList(List<Vector3> list)
    {
        string s = "";
        foreach(Vector3 v in list)
        {
            s += v+ "  ";
            Debug.DrawLine(transform.position, v);
        }
    }

    private float GetAngle(Vector3 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x);
        if (direction.y < 0)
        {
            angle = 2 * Mathf.PI + angle;
        }
        return angle;
    }
   

    public Vector2 RotatePoint(Vector2 point,float angle)
    {
        Vector2 newPoint;
        newPoint = new Vector2(Mathf.Cos(angle) * point.x - Mathf.Sin(angle) * point.y,
                                Mathf.Cos(angle) * point.y + Mathf.Sin(angle) * point.x);
        return newPoint;
    }

    // trie de manière croissante la liste en entrée en fonction des angles
    private List<Vector3> TrieAngle(List<Vector3> points)
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

    //récupère les intercections d'un segment AB sur le cercle
    private bool GetIntersection(Vector2 A, Vector2 B, List<Vector3> points)
    {
        
        Vector2 C = transform.position;

        float R = circleCol.radius;

        Vector2 D = (B - A) / Vector2.Distance(A, B);

        float t = D.x * (C.x - A.x) + D.y * (C.y - A.y);

        Vector2 E = t * D + A;

        float EC = Vector2.Distance(E,C);

        if (EC < R)
        {

            float dt = Mathf.Sqrt(R*R - EC * EC);

            float AdB = Mathf.Pow(Vector2.Distance(A, B),2);

            Vector2 AB = B - A;
            
            for(int i = 1; i>-2; i-= 2)
            {
                Vector2 F = (t + dt*i) * D + A;
                Vector2 AF = F - A;

                float ABdAF = Vector2.Dot(AB,AF);
                if (ABdAF > 0 && AdB > ABdAF )
                {
                    points.Add(F);
                }
               
            }
            return true;
            
        }

        return false;
        
    }

    // permet la construction du mesh en fonction des points qu'on lui donne 
    private Mesh BuildMesh()
    {
        Mesh m = mf.mesh;
        
        // on les mets en local
        foreach (Vector3 v in pointList)
        {
           verticeList.Add(new Vector3(v.x - transform.position.x, v.y - transform.position.y));
        }

        // pas à moi
        CircleTriangulator tr = new CircleTriangulator(verticeList.ToArray());
       
        int[] indice = tr.Triangulate();

        Vector3[] vert = new Vector3[verticeList.Capacity];

        int i = 0;
        foreach(Vector2 v in verticeList)
        {
            vert[i] = new Vector3(v.x , v.y ,0);
            i++;
        }
        m.Clear();
        m.vertices = vert;
        m.triangles = indice;
        m.RecalculateNormals();
        return m;

    }
    
    public void SetRadius(float radius)
    {
        circleCol.radius = radius;
    }

    // dessine des traits vers tous les points de notre mesh
    private void OnDrawGizmos()
    {
        // la couleur des raycasts
        Gizmos.color = Color.green;
        // pour chaques raycasts on l'affiche avec Gizmos
        if(verticeList != null)
        {
            foreach (Vector2 v in verticeList)
            {
                Gizmos.DrawLine(transform.position, v + new Vector2(transform.position.x, transform.position.y));
            }
        }

    }
}