using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }

    public enum Scenes { MainMenu, Archery1, Archery2, EndScreen } //should reflect the build index for each scene in build settings
    [SerializeField] private Image _fadeImage;
    [SerializeField] private float _fadeOutDuration = 0.2f;
    [SerializeField] private float _fadeInDuration = 0.2f;

    public UnityAction OnFinishFadeIn;

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
            SceneManager.sceneLoaded += OnSceneLoaded;
        }     
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (_fadeImage.color.a >= 0.9f) //always fade in if we faded out
        {
            StartCoroutine(FadeIn());
        }
    }

    public void LoadScene(Scenes scene, bool fadeToBlack = false)
    {
        if (!fadeToBlack)
        {
            SceneManager.LoadScene((int)scene);
        }
        else
        {
            StartCoroutine(FadeOut(scene));
        }
    }

    public void ReloadScene()
    {
        StartCoroutine(FadeOut((Scenes)SceneManager.GetActiveScene().buildIndex));
    }

    private IEnumerator FadeOut(Scenes scene)
    {
        float alpha = 0f;
        float elapsedTime = 0f;
        Color fadeColor = _fadeImage.color;
        while (elapsedTime < _fadeOutDuration)
        {
            yield return null;
            elapsedTime += Time.deltaTime;
            alpha = elapsedTime / _fadeOutDuration;
            fadeColor.a = alpha;
            _fadeImage.color = fadeColor;
        }
        SceneManager.LoadScene((int)scene);
        StopAllCoroutines();
    }

    private IEnumerator FadeIn()
    {
        float alpha = 1f;
        float elapsedTime = 0f;
        Color fadeColor = _fadeImage.color;
        while (elapsedTime < _fadeInDuration)
        {
            yield return null;
            elapsedTime += Time.deltaTime;
            alpha = _fadeInDuration - (elapsedTime / _fadeInDuration);
            fadeColor.a = alpha;
            _fadeImage.color = fadeColor;
        }

        OnFinishFadeIn?.Invoke();
    }
}
