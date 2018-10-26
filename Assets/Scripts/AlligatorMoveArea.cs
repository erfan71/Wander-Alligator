using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[ExecuteInEditMode]
public class AlligatorMoveArea : MonoBehaviour
{
    [Header("Just for Info, No need to change.")]
    [SerializeField]
    private List<Transform> points;
    private List<Edge> lines;
    [SerializeField]
    private Vector2 xRange;
    [SerializeField]
    private Vector2 zRange;

    public float minDistancetoLastTarget;

    private Vector2 lastTarget = new Vector3(Mathf.Infinity, Mathf.Infinity);

    private Color gizmozColor;
    private void LateUpdate()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            points = new List<Transform>();
            lines = new List<Edge>();
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

                NavMeshObstacle currentObs = currentTra.GetComponent<NavMeshObstacle>();
                currentObs.size = new Vector3(0.5f, 1, segmentObs.magnitude);
                currentObs.center = new Vector3(0, -0.5f, currentObs.size.z / 2.0f);
                currentTra.LookAt(nextTra.transform);

                points.Add(currentTra);

                RaycastHit hitInfo;
                Physics.Raycast(new Vector3(currentTra.position.x, 5, currentTra.position.z), new Vector3(0, -1, 0), out hitInfo, 10);
                currentTra.position = new Vector3(currentTra.position.x, hitInfo.point.y + 0.5f, currentTra.position.z);

                lines.Add(new Edge(new Vector2(currentTra.position.x, currentTra.position.z), new Vector2(nextTra.position.x, nextTra.position.z)));
            }
            bool badEdges = false;
            for (int i = 0; i < lines.Count; i++)
            {
                for (int j = 0; j < lines.Count; j++)
                {
                    if (i >= j) continue;
                    Vector2 hitPoint;
                    if (Edge.Check_Intersection(lines[i], lines[j], out hitPoint))
                    {
                        Debug.LogError("Edges of the polygon intersect to each other. Consider moving bad vertexes");
                        gizmozColor = Color.red;
                        badEdges = true;
                        break;

                    }
                }
            }
            if (!badEdges)
                gizmozColor = Color.blue;
            if (points.Count <= 2)
            {
                Debug.LogError("No area could be make with less that 3 vertex. Consider adding vertex");
            }
            FindRanges();
        }
#endif
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = gizmozColor;

        for (int i = 0; i < points.Count; i++)
        {
            if (i != points.Count - 1)
            {
                Gizmos.DrawLine(points[i].position, points[i + 1].position);

            }
            else
            {
                Gizmos.DrawLine(points[i].transform.position, points[0].transform.position);
            }
        }

    }


    void FindRanges()
    {
        float minX = Mathf.Infinity;
        float maxX = -Mathf.Infinity;
        float minZ = Mathf.Infinity;
        float maxZ = -Mathf.Infinity;

        foreach (Transform tr in points)
        {
            float x = tr.position.x;
            if (x > maxX)
                maxX = x;
            if (x < minX)
                minX = x;
            float z = tr.position.z;
            if (z > maxZ)
                maxZ = z;
            if (z < minZ)
                minZ = z;
        }

        xRange = new Vector2(minX, maxX);
        zRange = new Vector2(minZ, maxZ);
    }
    public void GetRandomePoint(System.Action<Vector3> callBack)
    {
        StartCoroutine(SetTargetRoutine(callBack));
    }
    IEnumerator SetTargetRoutine(System.Action<Vector3> callBack)
    {
        Vector2 randomePoint = new Vector2(Random.Range(xRange.x, xRange.y), Random.Range(zRange.x, zRange.y));

        int count = 0;
        while (true)
        {
            if (Vector2.Distance(randomePoint, lastTarget) > minDistancetoLastTarget)
                if (IsPointInsideArea(randomePoint))
                    break;
            randomePoint = new Vector2(Random.Range(xRange.x, xRange.y), Random.Range(zRange.x, zRange.y));
            count++;
            yield return null;
        }
        lastTarget = randomePoint;
        callBack?.Invoke(randomePoint);
    }
    public bool IsPointInsideArea(Vector2 testPoint)
    {
        float angleSum = 0;
        for (int i = 0; i < points.Count; i++)
        {
            Vector2 pq = Vector2.zero;
            Vector2 pr = Vector2.zero;
            if (i != points.Count - 1)
            {
                pq = new Vector2(points[i].position.x - testPoint.x, points[i].position.z - testPoint.y);
                pr = new Vector2(points[i + 1].position.x - testPoint.x, points[i + 1].position.z - testPoint.y);
            }
            else
            {
                pq = new Vector2(points[i].position.x - testPoint.x, points[i].position.z - testPoint.y);
                pr = new Vector2(points[0].position.x - testPoint.x, points[0].position.z - testPoint.y);
            }
            float angleBetweenPqPr = Vector2.SignedAngle(pq, pr);
            angleSum += angleBetweenPqPr;
        }
        angleSum = Mathf.Abs(angleSum);
        if (angleSum >= 359.9 && angleSum <= 360.1)
        {
           // Debug.Log("Yes sum: " + angleSum);
            return true;
        }
        else
        {
            //Debug.Log("No sum: " + angleSum);
            return false;
        }
    }



}
