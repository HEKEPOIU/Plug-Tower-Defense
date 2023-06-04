using System;
using Unity.VisualScripting;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    Camera mainCamera;
    [SerializeField] Transform tR, bL;
    float xMax,xMin,yMax,yMin;
    float size;

    void Awake()
    {
        mainCamera = GetComponent<Camera>();
        Vector3 position = tR.position;
        Vector3 position1 = bL.position;
        xMax = position.x;
        xMin = position1.x;
        yMax = position.y;
        yMin = position1.y;
    }

    void Start()
    {
        //reset camera position
        Vector3 transformSelf = mainCamera.transform.position;
        float disX = Mathf.Min(transformSelf.x - xMin, xMax - transformSelf.x);
        mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize, 5f, disX/2);
        size = disX / 2;
    }

    public void CameraMove(Vector3 move)
    {
        mainCamera.transform.position += move;
    }

    public void TouchDetect(Vector3 touchPosition)
    {
        Ray ray = mainCamera.ScreenPointToRay(touchPosition +new Vector3(0,0,3));
        RaycastHit2D hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(touchPosition), Vector2.zero);
        if (hit.collider == null) return;
        TowerPlace towerPlace = hit.collider.GetComponent<TowerPlace>();
        towerPlace.Build();
        BuildManager.Instance.BuildCancel();
    }

    public void CameraZoom(float increment)
    {
        //size *2 = distance
        mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize - increment, 5f, size);
    }
    
    public void CameraRangeLimit()
    {
        float cameraHalfWidth = mainCamera.orthographicSize * mainCamera.aspect;
        float cameraHalfHeight = mainCamera.orthographicSize;
        Vector3 newCameraPosition = mainCamera.transform.position;
        newCameraPosition.x = Mathf.Clamp(newCameraPosition.x, xMin + cameraHalfWidth, xMax - cameraHalfWidth);
        newCameraPosition.y = Mathf.Clamp(newCameraPosition.y,  yMin+ cameraHalfHeight, yMax - cameraHalfHeight);
        mainCamera.transform.position = newCameraPosition;
    }
}
