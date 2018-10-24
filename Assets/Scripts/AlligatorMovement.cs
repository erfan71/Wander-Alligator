using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlligatorMovement : MonoBehaviour {

    // Use this for initialization
    public AlligatorMoveArea moveArea;
    List<Transform> points;

    void Start () {
        MoveAlligatorOntheEdgesPoints();

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
