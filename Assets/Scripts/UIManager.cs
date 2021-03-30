using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Sprite[] _livesImages;
    [SerializeField] private Image _livesDisplay;
    [SerializeField] private Text _scoreText;
    [SerializeField] private GameObject _gameOverPanel;
    private bool _gameOver;

    void Start()
    {
        _livesDisplay.gameObject.SetActive(true);
        _gameOverPanel.SetActive(false);
        UpdateLivesDisplay(3);
        UpdateScoreText(0);
    }

    private void Update()
    {
        if(_gameOver && Input.GetKeyDown(KeyCode.R)) SceneManager.LoadScene(0);
    }

    public void UpdateLivesDisplay(int lives)
    {
        _livesDisplay.sprite = _livesImages[lives];
    }

    public void UpdateScoreText(int score)
    {
        _scoreText.text = $"Score\n{score}";
    }

    public void GameOver()
    {
        _gameOver = true;
        StartCoroutine(GameOverSequence());
    }

    private IEnumerator GameOverSequence()
    {
        _livesDisplay.gameObject.SetActive(false);
        _gameOverPanel.SetActive(true);
        GameObject restartText = _gameOverPanel.transform.Find("Restart_text").gameObject;
        while(true)
        {
            yield return new WaitForSeconds(0.5f);
            restartText.SetActive(true);
            yield return new WaitForSeconds(0.75f);
            restartText.SetActive(false);
        }
    }
    
}
