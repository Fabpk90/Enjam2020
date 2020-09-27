using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    private PlayerInput _input;
    private Vector2 _velocity = Vector2.zero;
    private bool _flying;

    private Animator anim;
    [SerializeField] private float animationSpeed = 1;

    private CooldownTimer timer;
    private CooldownTimer timerSexyStare;

    public PoopManager poopManager;
    
    private float projectilesLeft = 5;
    [Header("Shooting")] 
    [SerializeField] private float cooldownShoot = 3;
    private Transform _target;
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
    private static readonly int SexyStare = Animator.StringToHash("SexyStare");
    private static readonly int Flying = Animator.StringToHash("Flying");
    private static readonly int Turning = Animator.StringToHash("Turning");
    private static readonly int Speed = Animator.StringToHash("Speed");

    // Start is called before the first frame update
    private void Start()
    {
        anim = GetComponent<Animator>();
        _target = transform.GetChild(0).transform;
        timer = new CooldownTimer(0);
        _input = GetComponent<PlayerInput>();
        _input.currentActionMap["FlapLeft"].performed += OnFlapLeft;
        _input.currentActionMap["FlapRight"].performed += OnFlapRight;
        _input.currentActionMap["Move"].performed += OnMove;
        _input.currentActionMap["Move"].canceled += OnMove;
        _input.currentActionMap["Shit"].performed += OnShitting;
        _input.currentActionMap["Fly"].performed += OnFly;
        _input.currentActionMap["Restart"].performed += OnRestart;
        
        timerSexyStare = new CooldownTimer(5.0f);
        timerSexyStare.TimerCompleteEvent += () =>
        {
            if(Random.value > 0.5f && !_flying)
                anim.SetTrigger(SexyStare);
            timerSexyStare.Start();
        };
        timerSexyStare.Start();
    }

    private void OnRestart(InputAction.CallbackContext obj)
    {
        _input.currentActionMap["FlapLeft"].performed -= OnFlapLeft;
        _input.currentActionMap["FlapRight"].performed -= OnFlapRight;
        _input.currentActionMap["Move"].performed -= OnMove;
        _input.currentActionMap["Move"].canceled -= OnMove;
        _input.currentActionMap["Shit"].performed -= OnShitting;
        _input.currentActionMap["Fly"].performed -= OnFly;
        _input.currentActionMap["Restart"].performed -= OnRestart;
        SceneManager.LoadScene(0);
    }

    private void OnFly (InputAction.CallbackContext obj)
    {
        if (_flying) return;
        _flying = true;
        _velocity = Vector2.up * flyAwaySpeed;
        anim.SetBool(Flying, true);
    }

    private void OnShitting (InputAction.CallbackContext obj)
    {
        if (!_flying || projectilesLeft == 0 || !timer.IsCompleted) return;
        timer.Start(cooldownShoot);
        
        poopManager.UsePoop();
        
        projectilesLeft--;
        
        GameObject projectile = Instantiate(projectilePrefab) as GameObject;;
        projectile.transform.position = transform.position;
        Projectile p = projectile.GetComponent<Projectile>();
        if (p != null)
        {
            p.targetPosition = _target.transform.position;
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
        anim.SetTrigger(Turning);
        _velocity.x += turnSpeed;
        _velocity.y += speed;
    }
    
    private void OnFlapLeft(InputAction.CallbackContext obj)
    {
        if (!_flying) return;
        anim.SetTrigger(Turning);
        _velocity.x += -turnSpeed;
        _velocity.y += speed;

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //if (other.gameObject.layer != LayerMask.NameToLayer("Platform")) return;
        poopManager.ReloadAllPoop();
        projectilesLeft = maxProjectiles;
        _flying = false;
        _velocity = Vector2.zero;
        anim.SetBool(Flying, false);
        print("platform touched");
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        //if (other.gameObject.layer != LayerMask.NameToLayer("Platform")) return;
        _flying = true;
        _velocity = Vector2.up * flyAwaySpeed;
        anim.SetBool(Flying, true);
        print("platform left");
    }

    // Update is called once per frame
    private void Update()
    {
        anim.SetFloat(Speed, _velocity.y * animationSpeed);
        timer.Update(Time.deltaTime);
        timerSexyStare.Update(Time.deltaTime);
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

        _target.position = transform.position;
        _target.Translate(new Vector3(0,Mathf.Max(0, 1), 0) * range);

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0f, 0.27f);
        Gizmos.DrawRay(transform.position, transform.up * 2);
    }
}
