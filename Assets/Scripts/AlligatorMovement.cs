﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AlligatorMovement : MonoBehaviour {

    // Use this for initialization
    public AlligatorMoveArea moveArea;
    private AlligatorAnimation animator;
    List<Transform> points;
    public bool OnEdge;
    public bool Target;
    private NavMeshAgent agent;
    public Transform goal;

    void Start () {
        animator = GetComponent<AlligatorAnimation>();

        if (OnEdge)
         MoveAlligatorOntheEdgesPoints();
        else if (Target)
        {
            agent = GetComponent<NavMeshAgent>();
            agent.SetDestination(goal.position);
            animator.Walk();
        }

    }
    void MoveAlligatorOntheEdgesPoints()
    {
        points = moveArea.GetPoints();
        if (points.Count >= 2)
        {
            MoveBetweenTwoPoint(0);

        }
    }
    void MoveBetweenTwoPoint(int index)
    {
        if ( index!= points.Count-1)
        StartCoroutine(MoveBetweenTwoPointRoutine(points[index].position, points[index + 1].position,2, () => MoveBetweenTwoPoint(index + 1)));
        else
            StartCoroutine(MoveBetweenTwoPointRoutine(points[index].position, points[0].position, 0.5f, () => MoveBetweenTwoPoint(0)));

    }
    IEnumerator MoveBetweenTwoPointRoutine(Vector3 pointFrom, Vector3 pointTo, float speed,System.Action finishedCallback)
    {
        float i = 0.0f;
        float rate = speed;
        while (i < 1)
        {
            i += rate * Time.deltaTime;
            transform.position = Vector3.Lerp(pointFrom, pointTo,i);
            yield return null;
        }
        transform.position = pointTo;
        finishedCallback?.Invoke();
    }
	
	
}
