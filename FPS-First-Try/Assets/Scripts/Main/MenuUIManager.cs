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
    [SerializeField] private InputField playerNameField;
    [SerializeField] private Text bestPlayer;
    // Start is called before the first frame update
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

    public void GetPlayerName()
    {
        SceneFlow.Instance.playerName = playerNameField.text;
    }
}
