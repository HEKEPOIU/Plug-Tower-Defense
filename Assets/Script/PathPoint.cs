using UnityEngine;

public class PathPoint : MonoBehaviour
{
    [SerializeField] Transform[] nextPoints;
    [HideInInspector]
    
    public Transform NextPoint()
    {
        return nextPoints[Random.Range(0, nextPoints.Length)];
    }
}
