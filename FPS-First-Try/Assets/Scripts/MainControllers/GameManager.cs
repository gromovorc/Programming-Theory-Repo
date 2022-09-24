using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    bool m_started = false;
    public bool m_gameOver;
    public int currentScore;

    public Text healthText, scoreText;
    [SerializeField] GameObject gameOverText;
    [SerializeField] TextMesh playerName;

    void Start()
    {
        currentScore = 0;
        playerName.text = SceneFlow.Instance.playerName;
        scoreText.text = $"Score: {currentScore}";

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
    public void ChangeScore(int amount)
    {
        currentScore += amount;
        scoreText.text = $"Score: {currentScore}";
    }
}
