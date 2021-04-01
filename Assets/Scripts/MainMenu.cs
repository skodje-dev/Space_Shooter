using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject _optionsPanel;
    [SerializeField] private Slider _masterVolumeSlider;
    [SerializeField] private Text _masterVolumeText;
    [SerializeField] private Slider _musicVolumeSlider;
    [SerializeField] private Text _musicVolumeText;
    [SerializeField] private Toggle _muteAudioToggle;
    
    private bool _optionsPanelActive = false;
    private Animator _anim;
    

    private void Start()
    {
        _anim = GetComponent<Animator>();
        _optionsPanel.SetActive(_optionsPanelActive);
        int[] audioSettings = AudioManager.Instance.GetVolumePlayerPrefs();
        _masterVolumeSlider.value = audioSettings[0];
        _masterVolumeText.text = audioSettings[0] + "%";
        _musicVolumeSlider.value = audioSettings[1];
        _musicVolumeText.text = audioSettings[1] + "%";
        _muteAudioToggle.isOn = audioSettings[2] == 1;
    }

    public void PlayGame()
    {
        _anim.SetTrigger("HitPlay");
    }
    
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ToggleOptionsPanel()
    {
        _optionsPanelActive = !_optionsPanelActive;
        _optionsPanel.SetActive(_optionsPanelActive);
    }

    public void OpenUrl(string destination)
    {
        string url = "";
        switch (destination)
        {
            case "linkedin":
                url = "https://www.linkedin.com/in/skodje/";
                break;
            case "github":
                url = "https://github.com/skodje-dev";
                break;
            case "medium":
                url = "https://skodje.medium.com/";
                break;
            case "website":
                url = "https://skodje-digital.com";
                break;
        }
        Application.OpenURL(url);
    }
    
    public void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExecuteMenuItem("Edit/Play");
#endif
        Application.Quit();
    }

    public void StartBackgroundMusic()
    {
        AudioManager.Instance.StartBackgroundMusic();
    } 
    public void OnMusicSliderValueChanged(float value)
    {
        AudioManager.Instance.MusicVolumeChanged(value);
        _musicVolumeText.text = value + "%";
    }
    public void OnMasterSliderValueChanged(float value)
    {
        AudioManager.Instance.MasterVolumeChanged(value);
        _masterVolumeText.text = value + "%";
    }

    public void OnMuteToggleValueChanged(bool value)
    {
        AudioManager.Instance.ToggleMuteAudio(value);
    }
}
