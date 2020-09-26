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
    private bool _flying;
    
    private float projectilesLeft = 5;

    private CooldownTimer timer;

    [Header("Shooting")] 
    [SerializeField] private float cooldownShoot = 3;
    public Transform target;
    public GameObject projectilePrefab;
    [SerializeField] private float maxProjectiles = 5;
    [SerializeField] private float range = 1;
    
    [Header("Movement")]
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
        timer = new CooldownTimer(0);
        _input = GetComponent<PlayerInput>();
        _input.currentActionMap["FlapLeft"].performed += OnFlapLeft;
        _input.currentActionMap["FlapRight"].performed += OnFlapRight;
        _input.currentActionMap["Move"].performed += OnMove;
        _input.currentActionMap["Move"].canceled += OnMove;
        _input.currentActionMap["Shit"].performed += OnShitting;
        _input.currentActionMap["Fly"].performed += OnFly;
    }

    private void OnFly (InputAction.CallbackContext obj)
    {
        if (_flying) return;
        _flying = true;
        _velocity = Vector2.up * flyAwaySpeed;
    }

    private void OnShitting (InputAction.CallbackContext obj)
    {
        if (!_flying || projectilesLeft == 0 || !timer.IsCompleted) return;
        timer.Start(cooldownShoot);
        projectilesLeft--;
        GameObject projectile = Instantiate(projectilePrefab) as GameObject;;
        projectile.transform.position = transform.position;
        Projectile p = projectile.GetComponent<Projectile>();
        if (p != null)
        {
            p.targetPosition = target.transform.position;
        }
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
        projectilesLeft = maxProjectiles;
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
        timer.Update(Time.deltaTime);
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

        target.position = transform.position;
        target.Translate(new Vector3(0,Mathf.Max(_velocity.y, 1), 0) * range);

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0f, 0.27f);
        Gizmos.DrawRay(transform.position, transform.up * 2);
    }
}
