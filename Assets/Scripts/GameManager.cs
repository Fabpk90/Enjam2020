using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private CooldownTimer _finalCountdown;

    [SerializeField] private float initialCountdown = 20;
    // Start is called before the first frame update
    void Start()
    {
        _finalCountdown = new CooldownTimer(initialCountdown);
        _finalCountdown.Start();
    }

    // Update is called once per frame
    void Update()
    {
        _finalCountdown.Update(Time.deltaTime);
        if (_finalCountdown.IsCompleted)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        print("u die");
    }
}
