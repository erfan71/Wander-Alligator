using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AlligatorMovement : MonoBehaviour {

    // Use this for initialization
    public AlligatorMoveArea moveArea;
    public Transform goal;


    private AlligatorAnimation animator;
    private List<Vector3> points;
    private NavMeshAgent agent;
    private Vector2 xRange;
    private Vector2 zRange;

    void Start () {

        animator = GetComponent<AlligatorAnimation>();
        agent = GetComponent<NavMeshAgent>();

        points = moveArea.GetPoints();      
        FindRangesofDimention();


        Vector3 point= GetRandomePoint();
        RaycastHit hitInfo;

        Physics.Raycast(new Vector3(point.x, 5, point.z), new Vector3(0, -1, 0), out hitInfo, 10);
        goal.transform.position = new Vector3(point.x, hitInfo.point.y, point.z);
        agent.SetDestination(goal.position);
        animator.Walk();
      


    }
    void FindRangesofDimention()
    {
        float minX = Mathf.Infinity;
        float maxX = -Mathf.Infinity;
        float minZ= Mathf.Infinity;
        float maxZ = -Mathf.Infinity;

        foreach(Vector3 tr in points)
        {
            float x = tr.x;
            if (x > maxX)
                maxX = x;
            if (x < minX)
                minX = x;
            float z = tr.z;
            if (z > maxZ)
                maxZ = z;
            if (z < minZ)
                minZ = z;
        }
        xRange = new Vector2(minX, maxX);
        zRange = new Vector2(minZ, maxZ);
    }
    Vector3  GetRandomePoint()
    {
        Vector3 randomePoint= new Vector3(Random.Range(xRange.x, xRange.y), points[0].y,Random.Range(zRange.x, zRange.y) );
        
        while (!IsPointInsideArea(randomePoint))
        {
            randomePoint = new Vector3(Random.Range(xRange.x, xRange.y), points[0].y, Random.Range(zRange.x, zRange.y));
        }
        return  randomePoint;
    }
    bool IsPointInsideArea(Vector3 testPoint)
    {
        print(testPoint);
        float angleSum = 0;
        for (int i = 0; i < points.Count; i++)
        {
            Vector2 pq = Vector2.zero;
            Vector2 pr = Vector2.zero;
            if (i != points.Count - 1)
            {
                pq = new Vector2(points[i].x - testPoint.x, points[i].z - testPoint.z);
                pr = new Vector2(points[i + 1].x - testPoint.x, points[i + 1].z - testPoint.z);
            }
            else
            {
                pq = new Vector2(points[i].x - testPoint.x, points[i].z - testPoint.z);
                pr = new Vector2(points[0].x - testPoint.x, points[0].z - testPoint.z);
            }

            float angleBetweenPqPr = Vector2.Angle(pq, pr);
            angleSum += angleBetweenPqPr;

        }
        if (angleSum>=358 && angleSum<=362)
        {
            Debug.Log("yes: "+ angleSum);
            return true;
        }
        else
        {
            Debug.Log("No: " + angleSum);

            return
                   false;

        }
        

    }
   


}
