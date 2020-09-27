using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private CooldownTimer _finalCountdown;
    private float _score = 0;
    private bool isGameOver = false;
    public Slider timeGauge;
    public GameObject gameOver;
    public TextMeshProUGUI scoreText;
    [SerializeField] private float additionalTime = 5;
    [SerializeField] private float initialCountdown = 20;

    public static GameManager instance;
    
    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null)
            instance = this;
        
        scoreText.text = "0";
        _finalCountdown = new CooldownTimer(initialCountdown);
        _finalCountdown.Start();
    }

    // Update is called once per frame
    void Update()
    {
        _finalCountdown.Update(Time.deltaTime);
        timeGauge.value = Mathf.Clamp(_finalCountdown.TimeRemaining / initialCountdown, 0,1);
        if (_finalCountdown.IsCompleted)
        {
            if (isGameOver) return;
            GameOver();
        }
    }

    public void TargetHit()
    {
        float timeToAdd;
        if ((additionalTime + _finalCountdown.TimeRemaining) >= initialCountdown)
        {
            timeToAdd = initialCountdown - _finalCountdown.TimeRemaining;
        }
        else
        {
            timeToAdd = additionalTime;
        }
        _finalCountdown.AddTime(timeToAdd);
        _score++;
        scoreText.text = _score.ToString(CultureInfo.InvariantCulture);
    }
    
    void GameOver()
    {
        isGameOver = true;
        gameOver.SetActive(true);
        Invoke(nameof(ReloadScene), 3);
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(0);
    }
    
}
