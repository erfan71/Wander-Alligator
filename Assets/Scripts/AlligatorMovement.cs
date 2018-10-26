using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AlligatorMovement : MonoBehaviour
{

    // Use this for initialization
    public AlligatorMoveArea moveArea;
    public Transform goal;

    private AlligatorAnimation anim;
    private NavMeshAgent agent;

    public Vector2 stoppingTimeRange;
    public Vector2 walkingSpeedRange;

    void Start()
    {

        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<AlligatorAnimation>();
        if (isAlligatorInsideTheArea())
        {
            Idle();
        }
        else
        {
            Debug.LogException(new System.Exception("The alligator is not inside the area, Please move alligator inside and try again"));
        }
    }
   
    IEnumerator SetTargetRoutine(Vector3 point)
    {
        float time = GetRandomeStoppingTime();
        yield return new WaitForSeconds(time);
        MoveLogic(point);
        yield return null;
        while (true)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (Mathf.Abs(agent.velocity.sqrMagnitude) < float.Epsilon)
                {
                    Idle();
                    yield break;
                }
            }
            yield return null;
        }
    }

    private void Walk()
    {
        anim.Walk();
    }
    private void Idle()
    {
        anim.Idle();
        SetTarget();
    }
    void MoveLogic(Vector3 point)
    {
        RaycastHit hitInfo;
        Physics.Raycast(new Vector3(point.x, 5, point.y), new Vector3(0, -1, 0), out hitInfo, 10);
        goal.transform.position = new Vector3(point.x, hitInfo.point.y, point.y);
        agent.SetDestination(goal.position);
        agent.speed = (Random.Range(walkingSpeedRange.x, walkingSpeedRange.y));
        Walk();
    }

    void SetTarget()
    {
       moveArea.GetRandomePoint((point) => {
           StartCoroutine(SetTargetRoutine(point));
       });
        
    }

   float GetRandomeStoppingTime()
    {
        float selectedTime = Random.Range(stoppingTimeRange.x, stoppingTimeRange.y);
        return selectedTime;
    }
    bool isAlligatorInsideTheArea()
    {
       return moveArea.IsPointInsideArea(new Vector2(transform.position.x, transform.position.z));
    }
   
}
