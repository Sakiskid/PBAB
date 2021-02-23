using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthText : MonoBehaviour {

    GameManager gameManager;
    TextMeshProUGUI textBox;

	// Use this for initialization
	void Start () {
        gameManager = FindObjectOfType<GameManager>();
        textBox = GetComponent<TextMeshProUGUI>();
	}

    private void Update()
    {
        textBox.text = gameManager.player.health.ToString();
    }


}
