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
    private Vector2 _dir = Vector2.zero;
    [SerializeField]
    private float speed = 2;
    [SerializeField]
    private float turnSpeed = 30;

    private bool _flying = false;

    // Start is called before the first frame update
    void Start()
    {
        _input = GetComponent<PlayerInput>();
        _input.currentActionMap["Fly"].performed += OnFly;
        _input.currentActionMap["Flap"].performed += OnFlap;
        _input.currentActionMap["Flap"].canceled += OnFlap;
    }

    private void OnFly(InputAction.CallbackContext obj)
    {
        _flying = !_flying;
    }

    private void OnFlap(InputAction.CallbackContext obj)
    {
        _dir = obj.ReadValue<Vector2>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * (-_dir.x * turnSpeed * Time.deltaTime));
        if (_flying)
        {
            transform.Translate(Vector3.up *(speed * Time.deltaTime));
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0f, 0.27f);
        Gizmos.DrawRay(transform.position, transform.up * 5);
    }
}
