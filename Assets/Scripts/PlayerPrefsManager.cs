using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour {

    const string HIGH_SCORE_KEY = "high score";
    const string HIGH_SCORE_KILLS_KEY = "kills high score";
    const string HIGH_SCORE_TIME_KEY = "time high score";

	public void SetHighScore(int newHighScore)
    {
        PlayerPrefs.SetInt(HIGH_SCORE_KEY, newHighScore);
    }

    public int GetHighScore()
    {
        return PlayerPrefs.GetInt(HIGH_SCORE_KEY);
    }

    public void SetKillsHighScore(int newHighScore)
    {
        PlayerPrefs.SetInt(HIGH_SCORE_KILLS_KEY, newHighScore);
    }

    public int GetKillsHighScore()
    {
        return PlayerPrefs.GetInt(HIGH_SCORE_KILLS_KEY);
    }

    public void SetTimeHighScore(int newHighScore)
    {
        PlayerPrefs.SetInt(HIGH_SCORE_TIME_KEY, newHighScore);
    }

    public int GetTimeHighScore()
    {
        return PlayerPrefs.GetInt(HIGH_SCORE_TIME_KEY);
    }
}
