using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    public int CurrScore { get; private set; } = 0;
    public int LevelPar { get; private set; } = 0;
    private int _numShotsFired = 0;
    private int _scoreGainEachBelowPar = 100;
    private int _scoreLossEachAbovePar = 50;


    [SerializeField] private TextMeshProUGUI _parText, _shotsFiredText, _scoreText;
    [SerializeField] private AudioClip _badSound;
    [SerializeField] private AudioClip _okSound;
    [SerializeField] private AudioClip _goodSound;

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
        if (scene.buildIndex == 0)
        {
            _parText.enabled = false;
            _shotsFiredText.enabled = false;
            _scoreText.enabled = false;
        }
        else
        {
            _parText.enabled = true;
            _shotsFiredText.enabled = true;
            _scoreText.enabled = true;
        }
    }

    //called at end of level for par score
    public void EvaluatePar()
    {
        if (_numShotsFired < LevelPar)
        {
            ScoreManager.Instance.AddScore(_scoreGainEachBelowPar * (LevelPar - _numShotsFired));
            AudioManager.Instance.PlaySound(_goodSound);
        }
        else if (_numShotsFired == LevelPar)
        {
            //no bonus score if on par
            AudioManager.Instance.PlaySound(_okSound);
        }
        else
        {
            ScoreManager.Instance.SubtractScore(_scoreLossEachAbovePar * (_numShotsFired - LevelPar));
            AudioManager.Instance.PlaySound(_badSound);
        }
    }

    public void RegisterShotFired()
    {
        _numShotsFired++;
        _shotsFiredText.text = "Shots taken: " + _numShotsFired;
    }

    public void UpdatePar(int par)
    {
        LevelPar = par;
        _parText.text = "Par: " + par;
        _numShotsFired = 0; //assume update par means reset shots fired
        _shotsFiredText.text = "Shots taken: " + _numShotsFired;
    }

    public void AddScore(int score)
    {
        CurrScore += score;
        _scoreText.text = "Score: " + CurrScore;
    }

    public void SubtractScore(int score)
    {
        CurrScore -= score;
        _scoreText.text = "Score: " + CurrScore;
    }

    public void ResetScore()
    {
        CurrScore = 0;
        _scoreText.text = "Score: " + CurrScore;
    }
}
