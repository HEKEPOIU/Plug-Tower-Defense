using UnityEngine;
using System;

public class Path : MonoBehaviour
{
    Transform[] points;
    PathPoint[] pathPoints;
    float[] endDistance;
    // Start is called before the first frame update
    void Awake()
    {
        points = new Transform[transform.childCount];
        endDistance = new float[points.Length];
        pathPoints = new PathPoint[points.Length];
        // points = GetComponentsInChildren<Transform>(); //會包含自己
        for (int i = 0; i < points.Length; i++)
        {
            points[i] = transform.GetChild(i);
            pathPoints[i] = points[i].GetComponent<PathPoint>();
        }
        
        //紀錄每個點與終點的距離。
        for (int i = 0; i < points.Length; i++)
        {
            endDistance[i] = Vector3.Distance(points[0].position, points[i].position);
        }
        Array.Sort(endDistance);
    }


    public Transform FindNearestPoint(Vector3 position)
    {
        Transform nearestPoint = null;
        float minDistance = Mathf.Infinity;
        foreach (Transform point in points)
        {
            float distance = Vector3.Distance(position, point.position);
            if (!(distance < minDistance)) continue;
            minDistance = distance;
            nearestPoint = point;

        }

        return nearestPoint;
    }
    public Transform NextPoint(Transform nowPoint)
    {
        Transform nextPoint = null;
        int index = Array.IndexOf(points, nowPoint);
        nextPoint = pathPoints[index].NextPoint();
        return nextPoint;
    }

}
