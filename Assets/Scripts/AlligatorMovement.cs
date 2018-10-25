using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AlligatorMovement : MonoBehaviour {

    // Use this for initialization
    public AlligatorMoveArea moveArea;
    private AlligatorAnimation animator;
    List<Vector3> points;
    public bool Target;
    public bool RandomeTarget;
    private NavMeshAgent agent;
    public Transform goal;

    private Vector2 xRange;
    private Vector2 yRange;

    void Start () {
        points = moveArea.GetPoints();

        animator = GetComponent<AlligatorAnimation>();

    
        if (Target)
        {
            agent = GetComponent<NavMeshAgent>();
            agent.SetDestination(goal.position);
            animator.Walk();
        }
        else if (RandomeTarget)
        {

        }
    }
    void FindRangesofDimention()
    {
        float minX = Mathf.Infinity;
        float maxX = -Mathf.Infinity;
        float minY= Mathf.Infinity;
        float maxY = -Mathf.Infinity;

        foreach(Vector3 tr in points)
        {
            float x = tr.x;
            if (x > maxX)
                maxX = x;
            if (x < minX)
                minX = x;
            float y = tr.y;
            if (y > maxY)
                maxY = y;
            if (y < minY)
                minY = y;
        }
        xRange = new Vector2(minX, maxX);
        yRange = new Vector2(minY, maxY);
    }
    void InstantiateRandomTarget()
    {
        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        obj.transform.position = new Vector3(Random.Range(xRange.x, xRange.y), Random.Range(yRange.x, yRange.y), points[0].z);  
    }

   


}
