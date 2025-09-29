using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour, IPointerClickHandler
{

    public GameObject damageTextPrefab;
    public GameObject healthTextPrefab;
    public Canvas gameCanvas;
    MusicPlayer musicPlayer;

    [SerializeField] private GameOverScreen gameOverScreen;
    [SerializeField] private GameObject pauseScreen;

    public bool _isPaused;
    private bool _menuClicked;

    public bool getMenuCliked()
    {
        return _menuClicked;
    }


    private void Awake()
    {
        gameCanvas = FindObjectOfType<Canvas>();
        pauseScreen.SetActive(false);
    }

    private void Update()
    {
        if(_menuClicked)
            _menuClicked = false;
    }

    private void OnEnable()
    {
        CharacterEvents.characterDamaged += CharacterTookDamage;
        CharacterEvents.characterHealed += CharacterHealed;
    }

    private void OnDisable()
    {
        CharacterEvents.characterDamaged -= CharacterTookDamage;
        CharacterEvents.characterHealed -= CharacterHealed;
    }

    public void CharacterTookDamage(GameObject character, int damageReceived)
    {
        Vector3 spawnPosition = Camera.main.WorldToScreenPoint(character.transform.position);
        TMP_Text tmpText = Instantiate(damageTextPrefab, spawnPosition, Quaternion.identity, gameCanvas.transform)
            .GetComponent<TMP_Text>();

        tmpText.text = damageReceived.ToString();
    }
    public void CharacterHealed(GameObject character, int heal)
    {
        Vector3 spawnPosition = Camera.main.WorldToScreenPoint(character.transform.position);
        TMP_Text tmpText = Instantiate(healthTextPrefab, spawnPosition, Quaternion.identity, gameCanvas.transform)
            .GetComponent<TMP_Text>();

        tmpText.text = heal.ToString();
    }

    public void onExitGame(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            ExitGame();
        }
    }

    public  void ExitGame()
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

    #region Pause
    public void PauseGame(bool status)
    {
        //if status = true -> pause, if not unpause
        pauseScreen.SetActive(status);
        if (status)
        {
            Time.timeScale = 0;
            _isPaused = true;
        }
        else
        {
            _isPaused = false;
            Time.timeScale = 1;
        }
    }

    public void onPauseGame(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (pauseScreen.activeInHierarchy)
                PauseGame(false);
            else
                PauseGame(true);            
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_isPaused)
        {
            PauseGame(false);
            _menuClicked = true;
        }
    }

    public void RestartGame(int sceneIndex)
    {
        gameOverScreen.RestartButton(sceneIndex);
        PauseGame(false);
    }

    public void MainMenu(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
        Time.timeScale = 1;
    }

    //public void Volumn(float _change)
    //{

    //}

    //public void MusicVolumn()
    //{
    //    musicPlayer.ChangeMusicVolumn(0.2f);
    //}
    #endregion
}

