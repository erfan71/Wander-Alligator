using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[ExecuteInEditMode]
public class AlligatorMoveArea : MonoBehaviour
{
    [Header("Just for Info, No need to change.")]
    [SerializeField]
    private List<Transform> points;
    [SerializeField]
    private Vector2 xRange;
    [SerializeField]
    private Vector2 zRange;

    public float minDistancetoLastTarget;

    private Vector2 lastTarget = new Vector3(Mathf.Infinity, Mathf.Infinity);
    private void LateUpdate()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
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
                curentObs.center = new Vector3(0, -0.5f, curentObs.size.z / 2.0f);
                currentTra.LookAt(nextTra.transform);

                points.Add(currentTra);

                RaycastHit hitInfo;
                Physics.Raycast(new Vector3(currentTra.position.x, 5, currentTra.position.z), new Vector3(0, -1, 0), out hitInfo, 10);
                currentTra.position = new Vector3(currentTra.position.x, hitInfo.point.y + 0.5f, currentTra.position.z);
            }
            FindRanges();
        }
#endif
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;

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
    public Vector3 GetRandomePoint()
    {
        Vector2 randomePoint = new Vector2(Random.Range(xRange.x, xRange.y), Random.Range(zRange.x, zRange.y));

        int count = 0;
        while ( true)
        {
            if (Vector2.Distance(randomePoint, lastTarget) > minDistancetoLastTarget)
                if (IsPointInsideArea(randomePoint))
                    break;
            randomePoint = new Vector2(Random.Range(xRange.x, xRange.y), Random.Range(zRange.x, zRange.y));
            count++;
        }
        Debug.Log("Find after: " + count + "try");
        lastTarget = randomePoint;
        return randomePoint;
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
            Debug.Log("Yes sum: " + angleSum);
            return true;
        }
        else
        {
            Debug.Log("No sum: " + angleSum);

            return false;
        }
    }


}
