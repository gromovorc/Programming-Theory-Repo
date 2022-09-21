using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    bool m_started = false;
    public bool m_gameOver;
    public int currentScore;

    [SerializeField] GameObject gameOverText;

    void Start()
    {
        currentScore = 0;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) SceneManager.LoadScene(0);
        if (!m_started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneFlow.Instance.LoadHighScore();
                m_started = true;
            }
        }
        else if (m_gameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    public void GameOver()
    {
        if (SceneFlow.Instance.bestScore < currentScore)
        {
            SceneFlow.Instance.bestScore = currentScore;
            SceneFlow.Instance.bestPlayer = SceneFlow.Instance.playerName;
            SceneFlow.Instance.SaveHighScore();
        }
        m_gameOver = true;

        gameOverText.SetActive(true);
    }
}
