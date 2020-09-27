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

    private FMOD.Studio.EventInstance fmodinstance;

    [FMODUnity.EventRef]
    public string musicEvent;

    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null)
            instance = this;
        
        scoreText.text = "0";
        _finalCountdown = new CooldownTimer(initialCountdown);
        _finalCountdown.Start();
    }

    void Start()
    {
        fmodinstance = FMODUnity.RuntimeManager.CreateInstance(musicEvent);
        fmodinstance.start();
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

        if (_score >= 1 && _score <= 3)
        {
            fmodinstance.setParameterByName("Music_Intensity", 1);
        }

        if (_score >= 4 && _score <= 8)
        {
            fmodinstance.setParameterByName("Music_Intensity", 2);
        }

    }
    
    void GameOver()
    {
        isGameOver = true;
        gameOver.SetActive(true);
        fmodinstance.setParameterByName("Music_Intensity", 4);
        Invoke(nameof(ReloadScene), 3);
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(0);
    }
    
}
