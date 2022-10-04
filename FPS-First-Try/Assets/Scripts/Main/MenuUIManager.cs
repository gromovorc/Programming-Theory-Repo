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
    [SerializeField] private InputField playerNameField, sensitivityField;
    [SerializeField] private Text bestPlayer;
    [SerializeField] private Slider volumeSlider, sensitivitySlider;

    [SerializeField] private Animator startButton, settingsButton, settingsDialog, playerName;

    public static float volume;
    [Range(0.1f, 6.0f)]public static float sensitivity;

    private void Awake()
    {
        SceneFlow.Instance.LoadHighScore();
        bestPlayer.text = $"The best player is {SceneFlow.Instance.bestPlayer} " +
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

    public void GetPlayerName() => SceneFlow.Instance.playerName = playerNameField.text;

    public void OpenSettings()
    {
        startButton.SetBool("isHidden", true);
        settingsButton.SetBool("isHidden", true);
        playerName.SetBool("isHidden", true);
        settingsDialog.SetBool("isHidden", false);
    }

    public void CloseSettings()
    {
        startButton.SetBool("isHidden", false);
        settingsButton.SetBool("isHidden", false);
        playerName.SetBool("isHidden", false);
        settingsDialog.SetBool("isHidden", true);
    }

    public void VolumeChange() => volume = volumeSlider.value;

    public void SensitivityChangeSlider()
    {
        sensitivity = sensitivitySlider.value;
        sensitivityField.text = sensitivitySlider.value.ToString();
    }

    public void SensitivityChangeText()
    {
        if (float.TryParse(sensitivityField.text, out float value))
        {
            sensitivityField.text = value.ToString();
            sensitivity = value;
            sensitivitySlider.value = sensitivity;
        }
        else
        {
            sensitivityField.text = 0.0f.ToString();
            sensitivitySlider.value = 0.0f;
        }
    }

    private void OnEnable()
    {
        volumeSlider.value = volume;
        sensitivitySlider.value = sensitivity;
        sensitivityField.text = sensitivity.ToString();
    }
}
