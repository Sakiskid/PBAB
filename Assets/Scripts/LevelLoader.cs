using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour {

    [Header("AUTO LOAD Options")]
    [SerializeField] bool loadSceneAutomatically;
    [SerializeField] int automaticLoadSceneIndex;
    [SerializeField] float automaticLoadDelay;
    GameObject loadingScreenContainer;

    private IEnumerator Start()
    {
        loadingScreenContainer = GameObject.Find("Loading Container");
        if (loadSceneAutomatically)
        {
            yield return new WaitForSeconds(automaticLoadDelay);
            LoadScene(automaticLoadSceneIndex);
        }
    }

    public void LoadScene(int sceneIndex)
    {

        SceneManager.LoadScene(sceneIndex);
    }

    public void LoadSurvivalScene()
    {
        SceneManager.LoadScene("Game_Survival");
    }

    public void LoadChampionScene()
    {
        SceneManager.LoadScene("Game_Champion");
    }

    public void LoadSandboxScene()
    {
        SceneManager.LoadScene("Game_Sandbox");
    }

    private void EnableLoadingScreen()
    {
        if (loadingScreenContainer)
        {
            loadingScreenContainer.SetActive(true);
        }
    }
}
