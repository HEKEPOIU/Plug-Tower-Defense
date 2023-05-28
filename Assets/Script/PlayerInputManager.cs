using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    public delegate void GrabAction(Vector2 dir);
    public static PlayerInputManager Instance { get; private set; }

    public event GrabAction OnGrab;
    [SerializeField] InputAction touch,touchPosition;
    bool isMove = false;
    void OnEnable()
    {
        touch.Enable();
        touchPosition.Enable();
        // TouchSimulation.Enable();
    }

    void OnDisable()
    {
        // TouchSimulation.Disable();
        touch.Disable();
        touchPosition.Disable();
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
        touch.started += ctx => isMove = true;
        touch.canceled += ctx => isMove = false;
        touchPosition.performed += StartTouch;
    }

    void StartTouch(InputAction.CallbackContext ctx)
    {
        if (!isMove) return;
        
        Vector2 dir = ctx.ReadValue<Vector2>().normalized;
        if (!Mathf.Approximately(dir.magnitude, 0))
        {
            OnGrab?.Invoke(dir);
        }
        
    }
    
}
