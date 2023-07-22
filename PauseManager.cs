using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance { get; private set; }
    [SerializeField] private GameObject _optionsMenu;
    private bool _isPaused = false;
    private MainMenu _mainMenu; //used to distinguish if this is first scene

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }    
        else if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InputManager.Instance.Controls.Player.Pause.started += PauseGame;
        }
        _mainMenu = FindObjectOfType<MainMenu>();
    }

    private void OnEnable() 
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    private void OnDisable() 
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _mainMenu = FindObjectOfType<MainMenu>();
    }

    //used in UI
    public void Unpause()
    {
        InputAction.CallbackContext context = new InputAction.CallbackContext();
        _isPaused = true; //this will immediately be set false by PauseGame
        PauseGame(context);
    }

    public void PauseGame(InputAction.CallbackContext context)
    {
        if (_mainMenu)
        {
            _mainMenu.gameObject.SetActive(true);
            _optionsMenu.SetActive(false);
        }
        else
        {
            _isPaused = !_isPaused;
            Time.timeScale = _isPaused ? 0f : 1f;
            InputManager.Instance.SetShootingEnabled(!_isPaused);
            _optionsMenu.SetActive(_isPaused);
        }
    }
}
