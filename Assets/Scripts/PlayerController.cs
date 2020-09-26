using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    private PlayerInput _input;
    private Vector2 _velocity = Vector2.zero;
    private bool _flying = false;
    private Rigidbody2D _rb;
    
    [SerializeField] private float groundSpeed = 2;
    [SerializeField] private float groundTurnSpeed = 100;
    [SerializeField] private float turnSpeed = 30;
    [SerializeField] private float turnSpeedDecreaseRate = 1;
    [SerializeField] private float flyAwaySpeed = 4;
    [SerializeField] private float speed = 2;
    [SerializeField] private float speedDecreaseRate = 1;

    // Start is called before the first frame update
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _input = GetComponent<PlayerInput>();
        _input.currentActionMap["FlapLeft"].performed += OnFlapLeft;
        _input.currentActionMap["FlapRight"].performed += OnFlapRight;
        _input.currentActionMap["Move"].performed += OnMove;
        _input.currentActionMap["Move"].canceled += OnMove;
    }

    private void OnMove(InputAction.CallbackContext obj)
    {
        if (_flying) return;
        _velocity = obj.ReadValue<Vector2>();
    }
    private void OnFlapRight(InputAction.CallbackContext obj)
    {
        if (!_flying) return;
        _velocity.x += turnSpeed;
        _velocity.y += speed;
    }
    
    private void OnFlapLeft(InputAction.CallbackContext obj)
    {
        if (!_flying) return;
        _velocity.x += -turnSpeed;
        _velocity.y += speed;

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        _flying = false;
        _velocity = Vector2.zero;
        print("platform touched");
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        _flying = true;
        _velocity = Vector2.up * flyAwaySpeed;
        print("platform left");
    }

    // Update is called once per frame
    private void Update()
    {
        if (!_flying)
        {
            transform.Rotate(Vector3.forward * (groundTurnSpeed * Time.deltaTime * _velocity.x));
            transform.Translate(Vector3.up * (groundSpeed * Time.deltaTime * _velocity.y)); 
        }
        else
        {
            _velocity.x = Mathf.Lerp(_velocity.x, 0, turnSpeedDecreaseRate * Time.deltaTime);
            _velocity.y = Mathf.Lerp(_velocity.y, 0, speedDecreaseRate * Time.deltaTime);
            transform.Rotate(Vector3.forward * (turnSpeed * Time.deltaTime * _velocity.x));
            transform.Translate(Vector3.up * (speed * Time.deltaTime * _velocity.y));
        }
        

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0f, 0.27f);
        Gizmos.DrawRay(transform.position, transform.up * 2);
    }
}
