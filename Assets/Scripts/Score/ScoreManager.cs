using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ScoreManager : MonoBehaviour {

    [SerializeField] TextMeshProUGUI killCountText;
    [SerializeField] TextMeshProUGUI elapsedTimeText;
    [SerializeField] float killCountWeight;
    [SerializeField] float elapsedTimeWeight;
    int killCount;
    int elapsedTime;
    PlayerPrefsManager playerPrefsManager;

	// Use this for initialization
	void Start () {
        playerPrefsManager = FindObjectOfType<PlayerPrefsManager>();
        killCount = 0;
        elapsedTime = 0;
	}
	
	// Update is called once per frame
	void Update () {
        elapsedTime = Mathf.RoundToInt(Time.timeSinceLevelLoad);
        UpdateText();
        UpdateHighScore();
	}

    public void AddOneKill()
    {
        killCount++;
    }

    private void UpdateText()
    {
        if (killCountText)
        {
            killCountText.text = killCount.ToString();
        }
        if (elapsedTimeText)
        {
            elapsedTimeText.text = elapsedTime.ToString();
        }
    }

    private void UpdateHighScore()
    {
        // UPDATE KILL COUNT HS
        if(killCount > playerPrefsManager.GetKillsHighScore())
        {
            playerPrefsManager.SetKillsHighScore(killCount);
        }

        // UPDATE TIME HS
        if(elapsedTime > playerPrefsManager.GetTimeHighScore())
        {
            playerPrefsManager.SetTimeHighScore(elapsedTime);
        }
    }
}
