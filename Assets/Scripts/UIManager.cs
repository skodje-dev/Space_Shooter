using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Sprite[] _livesImages;
    [SerializeField] private Image _livesDisplay;
    [SerializeField] private Text _scoreText;
    void Start()
    {
        UpdateLivesDisplay(3);
        UpdateScoreText(0);
    }

    public void UpdateLivesDisplay(int lives)
    {
        _livesDisplay.sprite = _livesImages[lives];
    }

    public void UpdateScoreText(int score)
    {
        _scoreText.text = $"Score: {score}";
    }
}
