using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MyJoyStick : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static event Action PressedEvent;
    public static event Action ReleaseEvent;
    private Vector3 _cursorPos;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _cursorPos = Mouse.current.position.ReadValue();
        //Debug.Log(_cursorPos);
    }

    private void onPressMouse() 
    {
        if (!Mouse.current.leftButton.wasPressedThisFrame) return;
        this.enabled = true;
        _cursorPos = Mouse.current.position.ReadValue();
    }
}
