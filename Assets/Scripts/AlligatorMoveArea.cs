using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private void Update()
    {
#if UNITY_EDITOR
        points = new List<Transform>();
        for (int i = 0; i < transform.childCount; i++)
        {
            points.Add(transform.GetChild(i));

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
                Gizmos.DrawLine(points[i].position, points[i + 1].position);
            }
            else
            {
                Gizmos.DrawLine(points[i].position, points[0].position);
            }          
        }     
    }
    public List<Transform> GetPoints()
    {
        return points;
    }

}
