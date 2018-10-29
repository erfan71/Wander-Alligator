using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Line with Equation of Y=Ax+B
/// </summary>
public class Edge
{

    /// <summary>
    /// Slope
    /// </summary>
    public float A { get; set; }
    /// <summary>
    /// Constant
    /// </summary>
    public float B { get; set; }

    public Vector2 VertexA { get; set; }
    public Vector2 VertexB { get; set; }

    /// <summary>
    /// Create a like with slope and constant: y=slope*X + constant;
    /// </summary>
    /// <param name="slope"></param>
    /// <param name="constant"></param>

    public Edge(Vector2 vertexA, Vector2 vertexB)
    {

        A = (vertexA.y - vertexB.y) / (vertexA.x - vertexB.x);
        if (A == Mathf.Infinity)
        {
            B = vertexA.x; // X=B line that are parallel to  y axis
        }
        else 
            B = -A * vertexA.x + vertexA.y;
        VertexA = vertexA;
        VertexB = vertexB;
    }
    /// <summary>
    /// Warning: We suppose that point is already on the line
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    private bool IsPointBetweenEndVertexes(Vector2 point)
    {
        float minX = Mathf.Min(VertexA.x, VertexB.x);
        float maxX = Mathf.Max(VertexA.x, VertexB.x);
        float minY = Mathf.Min(VertexA.y, VertexB.y);
        float maxY = Mathf.Max(VertexA.y, VertexB.y);

        if (point.x>=minX && point.x <= maxX)
            if (point.y>=minY && point.y <= maxY)
                return true;
        return false;
    }
    /// <summary>
    /// Two edges are only interset if te hitpoint is not any vertex and 
    /// </summary>
    /// <param name="line1"></param>
    /// <param name="line2"></param>
    /// <param name="hitPoint"></param>
    /// <returns></returns>
    public static bool Check_Intersection(Edge line1, Edge line2, out Vector2 hitPoint)
    {
        hitPoint = Vector2.zero;
        if (line1.A == line2.A)//Parallel
        {
            return false;
        }
        else
        {
            Vector2 intersectPoint = new Vector2();
            
            if (line1.A == Mathf.Infinity && line2.A!=Mathf.Infinity)
            {
                intersectPoint.x = line1.B;
                intersectPoint.y = line2.A * intersectPoint.x + line2.B;

            }
            else if ( line1.A != Mathf.Infinity)

            {
                if (line2.A == Mathf.Infinity)
                {
                    intersectPoint.x = line2.B;
                }
                else
                {
                    intersectPoint.x = (line1.B - line2.B) / (line2.A - line1.A);
                }
                intersectPoint.y = line1.A * intersectPoint.x + line1.B;
            }                   
            hitPoint = intersectPoint;
            if (line1.VertexA == hitPoint || line1.VertexB == hitPoint)
                return false;
            //Hit point is in the Edge (between two end vertexes)

            if (line1.IsPointBetweenEndVertexes(hitPoint) && line2.IsPointBetweenEndVertexes(hitPoint))
                return true;

            return false;
        }
    }
}
