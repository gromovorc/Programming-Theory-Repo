using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class SceneFlow : MonoBehaviour
{
    public static SceneFlow Instance { get; private set; }
    
    public int bestScore;
    public string bestPlayer, playerName;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadHighScore();
    }
    [System.Serializable]
    class SaveData
    {
        public int bestScore;
        public string bestPlayerName;
    }

    public void SaveHighScore()
    {
        SaveData data = new SaveData();
        data.bestScore = bestScore;
        data.bestPlayerName = bestPlayer;
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadHighScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            bestScore = data.bestScore;
            bestPlayer = data.bestPlayerName;
        }
    }
}
