using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleTriangulator {

    private List<Vector2> m_points = new List<Vector2>();


    public CircleTriangulator(Vector2[] points)
    {
        m_points = new List<Vector2>(points);
    }

    public int[] Triangulate()
    {
        List<int> indices = new List<int>();

        int length = m_points.Capacity;

        for(int i = 1; i<length;i++)
        {
            indices.Add(0);
            if(i<length-1)
            {
                indices.Add(i + 1);
            }
            else
            {
                indices.Add(1);
            }
            
            indices.Add(i);
        }


        return indices.ToArray();
    }
}
