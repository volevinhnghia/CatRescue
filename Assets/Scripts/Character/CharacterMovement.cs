using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private float[] _borderRange;//left,right,top,bottom
    [SerializeField] private float[] _startPos;//x,y,z

    [SerializeField] private InputActionReference _inputRef;
    [SerializeField] private CharacterStats _stats;


    private CharacterController _characterController;

    private Vector3 _initPos;
    private Vector3 _prePos1;//Spawn object
    private Vector3 _prePos2;//Scale Tsunami
    private Vector3 _currentPos;
    private bool _isStartSpawn = false;

    private int phaseCnt = 0;
    public static event Action OnStartSpawn;
    public static event Action OnScaleTsunami;
    void Start()
    {
        _characterController = GetComponent<CharacterController>();

        //Get Player Init position
        _initPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Move(_inputRef.action.ReadValue<Vector2>());
        CalculatePlayerPos();
        CalculatePosForScaleTsunami();
    }

    private void Move(Vector2 inputValue)
    {
        if (inputValue == Vector2.zero) return;
        
        //Get speed from scriptable object stats and caclulate direction from input
        float speed = _stats.getSpeed();
        Vector3 moveDirection = new Vector3(inputValue.y, 0, -inputValue.x);
        _characterController.Move(moveDirection * speed * Time.deltaTime);

        //Rotate character depending on input
        Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 1000 * Time.deltaTime);

        //Keep update current position
        _currentPos = transform.position;
    }
    private void CalculatePlayerPos() 
    {
        //Start invoke spawn event when react certain location
        if (!_isStartSpawn && _currentPos.x - _initPos.x >= 40)
        {
            OnStartSpawn?.Invoke();
            _isStartSpawn = true;
            //Update Init position
            _prePos1 = _currentPos;
        }
        else if (_isStartSpawn && _currentPos.x - _prePos1.x >= 20) 
        {
            OnStartSpawn?.Invoke();
            //Update Init position
            _prePos1 = _currentPos;
        }
            
    }
    private void CalculatePosForScaleTsunami() 
    {
        //Start invoke scale Tsunami Speed event when react certain location
        Vector3 currentPos = transform.position;
        if (phaseCnt == 0 && currentPos.x - _initPos.x >= 400)
        {
            OnScaleTsunami?.Invoke();
            _prePos2 = currentPos;
            phaseCnt++;
        }
        else if (phaseCnt > 0 && currentPos.x - _prePos2.x >= 1000) 
        {
            OnScaleTsunami?.Invoke();
            _prePos2 = currentPos;
            phaseCnt++;
        }
    }
}
