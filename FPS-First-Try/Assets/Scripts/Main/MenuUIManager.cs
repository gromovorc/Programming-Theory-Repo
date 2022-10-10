using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


[DefaultExecutionOrder(1000)]
public class MenuUIManager : MonoBehaviour
{
    [SerializeField] private InputField _playerNameField, _sensitivityField;
    [SerializeField] private Text _bestPlayer;
    [SerializeField] private Slider _volumeSlider, _sensitivitySlider;

    [SerializeField] private Animator _startButton, _settingsButton, _settingsDialog, _playerName;

    public static float volume;
    [Range(0.1f, 6.0f)]public static float sensitivity;

    private void Awake()
    {
        SceneFlow.Instance.LoadHighScore();
        _bestPlayer.text = $"The best player is {SceneFlow.Instance.bestPlayer} " +
            $"with amazing {SceneFlow.Instance.bestScore}! Try to beat him!";
    }
    public void StartNew()
    {
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    public void GetPlayerName() => SceneFlow.Instance.playerName = _playerNameField.text;

    public void OpenSettings()
    {
        _startButton.SetBool("isHidden", true);
        _settingsButton.SetBool("isHidden", true);
        _playerName.SetBool("isHidden", true);
        _settingsDialog.SetBool("isHidden", false);
    }

    public void CloseSettings()
    {
        _startButton.SetBool("isHidden", false);
        _settingsButton.SetBool("isHidden", false);
        _playerName.SetBool("isHidden", false);
        _settingsDialog.SetBool("isHidden", true);
    }

    public void VolumeChange() => volume = _volumeSlider.value;

    public void SensitivityChangeSlider()
    {
        sensitivity = _sensitivitySlider.value;
        _sensitivityField.text = _sensitivitySlider.value.ToString();
    }

    public void SensitivityChangeText()
    {
        if (float.TryParse(_sensitivityField.text, out float value))
        {
            _sensitivityField.text = value.ToString();
            sensitivity = value;
            _sensitivitySlider.value = sensitivity;
        }
        else
        {
            _sensitivityField.text = 0.0f.ToString();
            _sensitivitySlider.value = 0.0f;
        }
    }

    private void OnEnable()
    {
        _volumeSlider.value = volume;
        _sensitivitySlider.value = sensitivity;
        _sensitivityField.text = sensitivity.ToString();
    }
}
