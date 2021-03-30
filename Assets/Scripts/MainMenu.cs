using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject _optionsPanel;
    private bool _optionsPanelActive = false;

    private void Awake()
    {
        _optionsPanel.SetActive(_optionsPanelActive);
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
    
    public void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExecuteMenuItem("Edit/Play");
#endif
        Application.Quit();
    }
}
