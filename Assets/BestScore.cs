using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BestScore : MonoBehaviour
{
    public TextMeshProUGUI text;
    
    // Start is called before the first frame update
    void Start()
    {
        var i = PlayerPrefs.GetInt("BestScore");

        if (i == 1)
        {
            text.gameObject.SetActive(true);
            text.text = "Best Score: " + PlayerPrefs.GetInt("Score");
            
            PlayerPrefs.SetInt("BestScore", 0);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(0);
        }
    }
}
