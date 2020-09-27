using System;
using System.Collections;
using System.Collections.Generic;
using IA;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public bool autoAim = false;
    public float maxAngle = 10;
    
    public Vector3 targetPosition;
    private bool targetFound = false;
    
    private SpriteRenderer _sprite;
    private GameManager _gameManager;
    public LayerMask peopleToShitOn;

    private Collider2D _colliderAutoAim;
    
    public float minRange = 0.5f;
    
    [SerializeField] private float sizeColliderFactor = 1;
    [SerializeField] private float lifeTime = 0.5f;

    private float _currentLifeTime;

    private Vector2 _velocity;
    // Start is called before the first frame update
    void Start()
    {
        if (autoAim)
        {
            _colliderAutoAim = Physics2D.OverlapCircle(transform.position,
                Vector3.Distance(transform.position, targetPosition),
                peopleToShitOn);
            
            if (_colliderAutoAim)
            {
                print("Collider found");
                if (Vector3.Distance(_colliderAutoAim.transform.position, transform.position) > minRange)
                {
                    print(transform.up);
                    print("Distance sufficient");
                    float cosAngle = Vector2.Dot(transform.up,
                        (_colliderAutoAim.transform.position - transform.position).normalized);
                    float angle = Mathf.Rad2Deg * Mathf.Acos(cosAngle);
                    print(angle);
                    if (Mathf.Abs(angle) < maxAngle)
                    {
                        targetPosition = _colliderAutoAim.transform.position;
                        targetFound = true;
                        print("target found");
                    }
                }
            }
        }
        _gameManager = FindObjectOfType<GameManager>();
        _sprite = GetComponent<SpriteRenderer>();
        _velocity = (targetPosition - transform.position) / lifeTime;
        _currentLifeTime = lifeTime;
    }

    // Update is called once per frame
    void Update()
    {
        _currentLifeTime -= Time.deltaTime;
        if (_currentLifeTime < 0)
        {
            if (autoAim)
            {
                HitAutoAim();
            }
            else
            {
                Hit();
            }
            return;
        }
        
        transform.Translate(new Vector3(_velocity.x, _velocity.y, 0) * Time.deltaTime, Space.World);
        transform.localScale = Vector3.one * Mathf.Lerp(5f, 2f, 1 - (_currentLifeTime / lifeTime));
    }

    private void Hit()
    {
        Collider2D people = Physics2D.OverlapCircle(transform.position, _sprite.size.x * sizeColliderFactor, peopleToShitOn);
        //Collision with people
        if (people)
        {
            _gameManager.TargetHit();

            HittableActor actor = people.GetComponent<HittableActor>();

            if (actor)
            {
                actor.TakeDamage(1);
            }
        }
        Destroy(transform.gameObject);
    }

    private void HitAutoAim()
    {
        if (!targetFound)
        {
            _colliderAutoAim = Physics2D.OverlapCircle(transform.position, _sprite.size.x * sizeColliderFactor, peopleToShitOn);

            if (!_colliderAutoAim)
            {
                Destroy(transform.gameObject);
                return;
            }
        }
        
        _gameManager.TargetHit();

        HittableActor actor = _colliderAutoAim.GetComponent<HittableActor>();

        if (actor)
        {
            actor.TakeDamage(1);
        }
        Destroy(transform.gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position,  Quaternion.AngleAxis(maxAngle, Vector3.forward) * transform.rotation * Vector3.up * 4);
        Gizmos.DrawRay(transform.position,  Quaternion.AngleAxis(-maxAngle, Vector3.forward) * transform.rotation * Vector3.up * 4);
    }
}
