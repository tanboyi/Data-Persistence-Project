using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuUIHandler : MonoBehaviour
{
    public static MenuUIHandler instance;

    public string userName;
    public string highScoreUserName;
    public int highScore;

    [SerializeField] private TMP_InputField userNameInputField;
    [SerializeField] private TextMeshProUGUI highScoreDisplay;

    private void Awake()
    {
        LoadResults();

        if (highScore == 0)
        {
            highScoreDisplay.text = "There are no high scores yet, play now!";
        }
        else
        {
            highScoreDisplay.text = "Best Score: " + highScoreUserName + ": " + highScore;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void GameStart()
    {
        if (userNameInputField.text.Length < 1)
        {
            highScoreDisplay.text = "Please enter your name with at least 1 letter";
        }
        else
        {
            SceneManager.LoadScene(1);
            userName = userNameInputField.text;
            Debug.Log(userName);
        }
    }

    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        EditorApplication.ExitPlayMode();
#endif
    }

    [System.Serializable]
    private class SaveData
    {
        public string highScoreUserName;
        public int highScore;
    }

    public void SaveResults()
    {
        SaveData data = new SaveData();

        data.highScoreUserName = userName;
        data.highScore = highScore;

        string json = JsonUtility.ToJson(data);

        Debug.Log(json);
        Debug.Log(Application.persistentDataPath.ToString());

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadResults()
    {
        string path = Application.persistentDataPath + "/savefile.json";

        Debug.Log("Check for whether the file exists: " + File.Exists(path));

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);

            SaveData data = JsonUtility.FromJson<SaveData>(json);

            highScore = data.highScore;
            highScoreUserName = data.highScoreUserName;
        }
    }
}
