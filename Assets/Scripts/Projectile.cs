﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector3 targetPosition;
    private SpriteRenderer sprite;
    [SerializeField] private LayerMask peopleToShitOn;
    
    [SerializeField] private float lifeTime = 0.5f;

    private float _currentLifeTime;

    private Vector2 _velocity;
    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        _velocity = (targetPosition - transform.position) / lifeTime;
        _currentLifeTime = lifeTime;
    }

    // Update is called once per frame
    void Update()
    {
        _currentLifeTime -= Time.deltaTime;
        if (_currentLifeTime < 0)
        {
            Collider2D people = Physics2D.OverlapCircle(transform.position, sprite.size.x * 0.5f, peopleToShitOn);
            //Collision with people
            if (people)
            {
                Color color = people.gameObject.GetComponent<SpriteRenderer>().color;
                people.gameObject.GetComponent<SpriteRenderer>().color = color == Color.red ? Color.yellow : Color.red;
            }
            Destroy(transform.gameObject);
            return;
        }
        transform.Translate(new Vector3(_velocity.x, _velocity.y, 0) * Time.deltaTime);
        transform.localScale = Vector3.one * Mathf.Lerp(5f, 2f, 1 - (_currentLifeTime / lifeTime));
    }
}