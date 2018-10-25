using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[ExecuteInEditMode]
public class AlligatorMoveArea : MonoBehaviour
{
    [SerializeField] 
    private List<Transform> points;
    void Start()
    {

        points = new List<Transform>();
        for (int i = 0; i < transform.childCount; i++)
        {
            points.Add(transform.GetChild(i));
        }
    }
    private void LateUpdate()
    {
#if UNITY_EDITOR
        points = new List<Transform>();
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform currentTra = transform.GetChild(i);
            Transform nextTra;
            Vector3 segmentObs = Vector3.zero;
            if (i != transform.childCount - 1)
            {
                nextTra = transform.GetChild(i + 1);
            }
            else
            {
                nextTra = transform.GetChild(0);
            }
            segmentObs = nextTra.transform.position - currentTra.transform.position;

            NavMeshObstacle curentObs = currentTra.GetComponent<NavMeshObstacle>();
            curentObs.size = new Vector3(0.5f, 1, segmentObs.magnitude);
            curentObs.center = new Vector3(0, -0.5f, curentObs.size.z/ 2.0f);
            currentTra.LookAt(nextTra.transform);

            points.Add(currentTra);

            RaycastHit hitInfo;
            Physics.Raycast(new Vector3(currentTra.position.x,5, currentTra.position.z), new Vector3(0, -1, 0),out hitInfo,10);
            currentTra.position = new Vector3(currentTra.position.x, hitInfo.point.y + 0.5f, currentTra.position.z);
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
                Gizmos.DrawLine(points[i]. position, points[i + 1]. position);
              //  Gizmos.DrawRay(new Vector3(points[i].position.x, 5, points[i].position.z), new Vector3(0, -1, 0));

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
        foreach(Transform tr in this.points)
        {
            points.Add(tr.position);
        }
        return points;
    }

}
