using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighScoreDisplay : MonoBehaviour {

    [SerializeField] PlayerPrefsManager playerPrefsManager;
    [SerializeField] TextMeshProUGUI mostKills;
    [SerializeField] TextMeshProUGUI longestSurvived;

    private void Start()
    {
        mostKills.text = playerPrefsManager.GetKillsHighScore().ToString();
        longestSurvived.text = playerPrefsManager.GetTimeHighScore().ToString();
    }

}
