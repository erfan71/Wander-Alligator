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
        Idle();
    }
   
    IEnumerator SetTargetRoutine()
    {

        yield return new WaitForSeconds(GetRandomeStoppingTime());
        Walk();
        yield return null;
        while (true)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (Mathf.Abs(agent.velocity.sqrMagnitude) < float.Epsilon)
                {
                    Debug.Log("idle");
                    Idle();
                    yield break;
                }
            }
            yield return null;
        }
    }

    private void Walk()
    {
        SetTarget();
        anim.Walk();
    }
    private void Idle()
    {
        anim.Idle();
        StartCoroutine(SetTargetRoutine());
    }

    void SetTarget()
    {
        Vector2 point = moveArea.GetRandomePoint();
        RaycastHit hitInfo;
        Physics.Raycast(new Vector3(point.x, 5, point.y), new Vector3(0, -1, 0), out hitInfo, 10);
        goal.transform.position = new Vector3(point.x, hitInfo.point.y, point.y);
        agent.SetDestination(goal.position);
    }

   float GetRandomeStoppingTime()
    {
        float selectedTime = Random.Range(stoppingTimeRange.x, stoppingTimeRange.y);
        return selectedTime;
    }
   
}
