using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Sprite[] _livesImages;
    [SerializeField] private Image _livesDisplay;
    [SerializeField] private Text _ammoText;
    [SerializeField] private Image _thrusterChargeDisplay;
    [SerializeField] private Text _scoreText;
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private GameObject _restartText;
    private bool _gameOver;
    private Camera _mainCamera;

    void Start()
    {
        _mainCamera = Camera.main;
        _livesDisplay.gameObject.SetActive(true);
        _gameOverPanel.SetActive(false);
        UpdateLivesDisplay(3);
        UpdateScoreText(0);
    }

    private void Update()
    {
        if(_gameOver && Input.GetKeyDown(KeyCode.R)) SceneManager.LoadScene(1);
    }

    public void UpdateAmmoText(int currentAmmo, int maxAmmo, bool unlimited=false)
    {
        if (unlimited) _ammoText.enabled = false;
        _ammoText.text = $"Ammo: {currentAmmo:D2}/{maxAmmo}";
    }

    public void UpdateThrusterDisplay(float current, float max)
    {
        _thrusterChargeDisplay.fillAmount = current / max;
    }

    public void UpdateLivesDisplay(int lives)
    {
        _livesDisplay.sprite = _livesImages[lives];
    }

    public void UpdateScoreText(int score)
    {
        _scoreText.text = $"Score\n{score}";
    }

    [ContextMenu("Simulate Damage")]
    public void CameraShake()
    {
        StartCoroutine(CameraShakeRoutine());
    }
    
    private IEnumerator CameraShakeRoutine()
    {
        // From http://wiki.unity3d.com/index.php/Camera_Shake
        // with some alterations
        float shakeDuration = 2.0f;
        float startDuration = 1f;
        float smoothAmount = 100f;
        float startAmount = 2f;
        float shakeAmount = 2f;
        while (shakeDuration > 0.01f)
        {
            Vector3 rotationAmount = Random.insideUnitSphere * shakeAmount;//A Vector3 to add to the Local Rotation
            rotationAmount.z = 0;//Don't change the Z; it looks funny.

            float shakePercentage = shakeDuration / startDuration;//Used to set the amount of shake (% * startAmount).

            shakeAmount = startAmount * shakePercentage;//Set the amount of shake (% * startAmount).
            shakeDuration = Mathf.Lerp(shakeDuration, 0, Time.deltaTime*2);//Lerp the time, so it is less and tapers off towards the end.
            _mainCamera.transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(rotationAmount), Time.deltaTime * smoothAmount);

            yield return null;
        }
        _mainCamera.transform.localRotation = Quaternion.identity;
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
        
        while(true)
        {
            yield return new WaitForSeconds(0.5f);
            _restartText.SetActive(true);
            yield return new WaitForSeconds(0.75f);
            _restartText.SetActive(false);
            if (Input.GetKeyDown(KeyCode.R)) break;
        }
    }
    
}
