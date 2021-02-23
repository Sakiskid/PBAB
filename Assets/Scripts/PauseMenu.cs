using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {

    [SerializeField] GameObject UI;
    float initialFixedDeltaTime;
    float timeScaleBeforePause;

    private void Start()
    {
        initialFixedDeltaTime = Time.fixedDeltaTime;
    }

    void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UI.SetActive(!UI.activeInHierarchy);
            if (UI.activeInHierarchy)
            {
                Pause();
            }
            else if (!UI.activeInHierarchy)
            {
                Resume();
            }
        }
	}

    public void Pause()
    {
        timeScaleBeforePause = Time.timeScale;
        Time.timeScale = 0;
        //Time.fixedDeltaTime = 5f;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Resume()
    {
        Time.timeScale = timeScaleBeforePause;
        //Time.fixedDeltaTime = initialFixedDeltaTime;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
