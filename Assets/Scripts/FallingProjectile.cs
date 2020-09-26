using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingProjectile : MonoBehaviour
{
    [SerializeField] private float lifeTime = 0.5f;

    private float _currentLifeTime;
    
    // Start is called before the first frame update
    void Start()
    {
        _currentLifeTime = lifeTime;
    }

    // Update is called once per frame
    void Update()
    {
        _currentLifeTime -= Time.deltaTime;
        if (_currentLifeTime < 0)
        {
            //Collision with people
            Destroy(transform.gameObject);
            return;
        }
        transform.localScale = Vector3.one * Mathf.Lerp(2.0f, 0.5f, 1 - (_currentLifeTime / lifeTime));
    }
}