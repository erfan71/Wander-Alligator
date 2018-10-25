using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[ExecuteInEditMode]
public class AlligatorMoveArea : MonoBehaviour
{
    [SerializeField] 
    private List<NavMeshObstacle> points;
    void Start()
    {

        points = new List<NavMeshObstacle>();
        for (int i = 0; i < transform.childCount; i++)
        {
            points.Add(transform.GetChild(i).GetComponent<NavMeshObstacle>());
        }
    }
    private void Update()
    {
#if UNITY_EDITOR
        points = new List<NavMeshObstacle>();
        for (int i = 0; i < transform.childCount; i++)
        {
            NavMeshObstacle currentObs = transform.GetChild(i).GetComponent<NavMeshObstacle>();
            NavMeshObstacle nextObs;
            Vector3 segmentObs = Vector3.zero;
            if (i != transform.childCount - 1)
            {
                nextObs = transform.GetChild(i+1).GetComponent<NavMeshObstacle>();
            }
            else
            {
                nextObs = transform.GetChild(0).GetComponent<NavMeshObstacle>();
            }
            segmentObs = nextObs.transform.position - currentObs.transform.position;

            currentObs.size = new Vector3(0.5f, 1, segmentObs.magnitude);
            currentObs.center = new Vector3(0, -0.5f, currentObs.size.z/ 2.0f);
            currentObs.transform.LookAt(nextObs.transform);

            points.Add(currentObs);
        }
#endif
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        
        for ( int i=0; i < points.Count; i++)
        {
            if (i != points.Count - 1)
            {
                Gizmos.DrawLine(points[i].transform. position, points[i + 1].transform. position);
            }
            else
            {
                Gizmos.DrawLine(points[i].transform.position, points[0].transform.position);
            }          
        }     
    }
    public List<Vector3> GetPoints()
    {
        List<Vector3> points = new List<Vector3>();
        foreach(NavMeshObstacle tr in this.points)
        {
            points.Add(tr.transform.position);
        }
        return points;
    }

}
