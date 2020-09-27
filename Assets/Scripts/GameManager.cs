using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using IA;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[Serializable]
public struct Wave
{
    public int amountToSpawn;
    public float secondsBeforeSpawning;
    public int scoreToReach;
}

public class GameManager : MonoBehaviour
{
    private CooldownTimer _finalCountdown;
    private uint _score = 0;
    private bool isGameOver = false;
    public Slider timeGauge;
    public GameObject gameOver;
    public TextMeshProUGUI scoreText;
    [SerializeField] private float additionalTime = 5;
    [SerializeField] private float initialCountdown = 20;

    public static GameManager instance;
    [HideInInspector]
    public List<Spawner> spawners;

    

    public List<Wave> waves;
    public int _waveIndex;

    private FMOD.Studio.EventInstance fmodinstance;

    [FMODUnity.EventRef]
    public string musicEvent;

    public uint scoreToAddOnShit;

    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null)
            instance = this;
        
        scoreText.text = "0";
        _finalCountdown = new CooldownTimer(initialCountdown);
        _finalCountdown.Start();

        _waveIndex = -1;
    }

    void Start()
    {
        fmodinstance = FMODUnity.RuntimeManager.CreateInstance(musicEvent);
        fmodinstance.start();

        Invoke("NextStage", 1.0f);
    }

    private void NextStage()
    {
        _waveIndex++;
        int amountPerSpawner = waves[_waveIndex].amountToSpawn / spawners.Count;

        foreach (var sp in spawners)
        {
            sp.NextWave(amountPerSpawner);
            sp.spawnCooldown = waves[_waveIndex].secondsBeforeSpawning;
        }
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
        _score += scoreToAddOnShit;
        scoreText.text = _score.ToString(CultureInfo.InvariantCulture);
        

        if (_score >= 500 && _score <= 1000)
        {
            fmodinstance.setParameterByName("Music_Intensity", 1);
        }

        if (_score >= 1100)
        {
            fmodinstance.setParameterByName("Music_Intensity", 2);
        }

        if (_score >= waves[_waveIndex].scoreToReach
        && _waveIndex != waves.Count - 1)
        {
            print("Next stage");
            NextStage();
        }

    }
    
    void GameOver()
    {
        var score = PlayerPrefs.GetInt("Score");

        if (score <= _score)
        {
            PlayerPrefs.SetInt("BestScore", 1);
            PlayerPrefs.SetInt("Score", score);
            
            PlayerPrefs.Save();
        }
        
        isGameOver = true;
        gameOver.SetActive(true);
        fmodinstance.setParameterByName("Music_Intensity", 4);
        Invoke(nameof(ReloadScene), 3);
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(2);
    }
    
}
