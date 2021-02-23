using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashScreenSkipper : MonoBehaviour {

    [SerializeField] LevelLoader levelLoader;

	void Update () {
        if (Input.anyKeyDown)
        {
            levelLoader.LoadScene(1);
        }
	}
}
