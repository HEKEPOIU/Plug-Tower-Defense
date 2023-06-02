using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    public delegate void GrabAction(Vector2 dir);
    public static PlayerInputManager Instance { get; private set; }
    
    [SerializeField] float scaleSpeed = 0.5f;
    [SerializeField] float screenMoveSpeed = 1f;
    float timer = 0;
    
    [SerializeField] CameraBehaviour cameraBehaviour;
    
    
    float prevMagnitude = 0;

    int touchCount = 0;

    public event GrabAction OnGrab;
    [SerializeField] InputAction touch,touchPosition,touchDelta,secondTouch,secondTouchPosition,secondTouchDelta;
    bool isMove = false;
    void OnEnable()
    {
        touch.Enable();
        touchDelta.Enable();
        touchPosition.Enable();
        secondTouch.Enable();
        secondTouchDelta.Enable();
        secondTouchPosition.Enable();
        // TouchSimulation.Enable();
    }

    void OnDisable()
    {
        // TouchSimulation.Disable();
        touch.Disable();
        touchDelta.Disable();
        touchPosition.Disable();
        secondTouch.Disable();
        secondTouchDelta.Disable();
        secondTouchPosition.Disable();
    }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

    }

    void Start()
    {
        touch.started += ctx =>
        {
            touchCount++;
            isMove = true;
        };
        touch.canceled += ctx =>
        {
            touchCount--;
            isMove = false;
            prevMagnitude = 0;
        };
        secondTouch.started += ctx =>
        {
            touchCount++;
        };
        secondTouch.canceled += ctx =>
        {
            touchCount--;
            prevMagnitude = 0;
        };
        touchDelta.performed += StartTouch;
        
        
        
        
        
        secondTouchPosition.performed += context =>
        {
            if(touchCount < 2) return;
            //count two finger magnitude.
            float magnitude = (secondTouchPosition.ReadValue<Vector2>() - touchPosition.ReadValue<Vector2>()).magnitude;
            //if first time, set prevMagnitude to magnitude.
            if(prevMagnitude == 0)
            {
                prevMagnitude = magnitude;
            }
            float dot = Vector2.Dot(secondTouchDelta.ReadValue<Vector2>(), touchDelta.ReadValue<Vector2>());
            //means two finger move in opposite direction.
            if (dot <= -.9f)
            {
                //calculate difference between magnitude and prevMagnitude.
                float difference = magnitude - prevMagnitude;
                //update prevMagnitude.
                prevMagnitude = magnitude;
                cameraBehaviour.CameraZoom(difference * scaleSpeed);
                cameraBehaviour.CameraRangeLimit();
                
            }
            else
            {
                cameraBehaviour.CameraMove((Vector3)(- secondTouchDelta.ReadValue<Vector2>() * Time.deltaTime * screenMoveSpeed));
                cameraBehaviour.CameraRangeLimit();
            }
        };

    }
    
    void StartTouch(InputAction.CallbackContext ctx)
    {
        if (!isMove || touchCount>=2) return;
        Vector2 dir = ctx.ReadValue<Vector2>().normalized;
        if (!Mathf.Approximately(dir.magnitude, 0))
        {
            OnGrab?.Invoke(dir);
        }
        
    }
    
}
