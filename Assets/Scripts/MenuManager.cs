using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    [SerializeField] private GameObject stageSelectScreen;
    [SerializeField] private GameObject TutorialScreen;

    private void Awake()
    {
        stageSelectScreen.SetActive(false);
        TutorialScreen.SetActive(false);
    }
    public void ExitGame()
    {
        #if (UNITY_EDITOR || DEVELOPMENT_BUILD)
                Debug.Log(this.name + " : " + this.GetType() + " : " + System.Reflection.MethodBase.GetCurrentMethod().Name);
        #endif
        #if (UNITY_EDITOR)
                UnityEditor.EditorApplication.isPlaying = false;
        #elif (UNITY_STANDALONE)
                                Application.Quit();
        #elif (UNITY_WEBGL)
                                SceneManager.LoadScene("QuitScene");
        #endif
    }

    public void PlayGame(bool status)
    {
        stageSelectScreen.SetActive(status);
        Tutorial(false);
    }

    public void Tutorial(bool status)
    {
        TutorialScreen.SetActive(status);
    }

    public void LoadStage(int sceneIndex)
    {
        if(sceneIndex < 0 || sceneIndex >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.LogError("Cant find seen with that index");
            return;
        }
        SceneManager.LoadScene(sceneIndex);
    }
}
