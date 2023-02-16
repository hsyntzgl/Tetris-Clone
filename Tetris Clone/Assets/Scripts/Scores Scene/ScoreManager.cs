using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] scoreTextsGUI;

    [SerializeField] private TextMeshProUGUI saveOrMainMenuTextGUI;

    private ParentGameData gameData;

    private string playerName;

    private int playerPositionInList;

    private bool newHighScore;

    private readonly int textLimit = 15;

    private readonly int textSpace = 250;

    private readonly string fileName = "saves.txt";

    private readonly string defaultName = "AAAAAA";

    private readonly int[] defaultScores ={
        15000,14000,13000,12000,11000,10000,9000,8000,7000,6000,5000,4000,3000,2000,1000
    };

    public string PlayerName
    {
        get => playerName;
        set
        {
            if (newHighScore)
            {
                playerName = value;
                UpdateSelectedTextAndSavePlayerName();
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        playerName = defaultName;
        LoadScores();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpaceKeyFunction();
        }
    }

    public void SaveScores()
    {
        if (Directory.Exists(GetDirectoryPath()))
        {
            if (File.Exists(GetFilePath()))
            {
                string data = JsonConvert.SerializeObject(gameData);

                using (FileStream stream = new FileStream(GetFilePath(), FileMode.Open))
                {

                    using (StreamWriter sw = new StreamWriter(stream))
                    {

                        sw.WriteLine(data);

                        sw.Close();
                    }
                }
            }
            else
            {
                CreateNewSaveFile();
            }
        }
        else
        {
            CreateNewSaveFile();
        }
    }
    private void LoadScores()
    {
        string jsonData = GetStringFromJson();

        if (jsonData != null)
        {
            gameData = JsonConvert.DeserializeObject<ParentGameData>(jsonData);

            UpdateText();
        }
        else
        {
            CreateNewSaveFile();
        }
        CheckPlayerScore();
    }

    private void CreateNewSaveFile()
    {
        try
        {
            Directory.CreateDirectory(GetDirectoryPath());

            using (FileStream fs = File.Create(GetFilePath())) { }
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
            return;
        }
        CreateDefaultScores();
    }
    private void CreateDefaultScores()
    {
        gameData = new ParentGameData();

        gameData.childDatas = new GameData[textLimit];

        for (int i = 0; i < defaultScores.Length; i++)
        {
            scoreTextsGUI[i].SetText("{0}." + defaultName + "<space>{1}", (i + 1), defaultScores[i]);

            gameData.childDatas[i] = new GameData();

            gameData.childDatas[i].playerName = defaultName;
            gameData.childDatas[i].playerScore = defaultScores[i];
        }
        SaveScores();
    }
    private string GetStringFromJson()
    {
        if (Directory.Exists(GetDirectoryPath()))
        {
            if (File.Exists(GetFilePath()))
            {
                try
                {
                    using (FileStream stream = new FileStream(GetFilePath(), FileMode.Open))
                    {
                        using (StreamReader sr = new StreamReader(stream))
                        {
                            string data = sr.ReadToEnd();
                            return data;
                        }
                    }
                }
                catch (System.Exception e)
                {
                    Debug.Log(e.Message);
                    return null;
                }
            }
            return null;
        }
        return null;
    }
    private void CheckPlayerScore()
    {
        for (int i = 0; i < gameData.childDatas.Length; i++)
        {
            if (GameManager.instance.Score >= gameData.childDatas[i].playerScore)
            {
                newHighScore = true;
                saveOrMainMenuTextGUI.SetText("SAVE");
                scoreTextsGUI[i].color = Color.black;
                SetNewScorePositions(i);
                playerPositionInList = i;
                break;
            }
        }
    }
    private void SetNewScorePositions(int fromPosition)
    {
        for (int i = gameData.childDatas.Length - 1; i > fromPosition; i--)
        {
            gameData.childDatas[i].playerName = gameData.childDatas[i - 1].playerName;
            gameData.childDatas[i].playerScore = gameData.childDatas[i - 1].playerScore;
        }
        gameData.childDatas[fromPosition].playerName = defaultName;
        gameData.childDatas[fromPosition].playerScore = GameManager.instance.Score;

        SaveScores();

        UpdateText();
    }
    private void UpdateText()
    {
        for (int i = 0; i < textLimit; i++)
        {
            scoreTextsGUI[i].SetText("{0}." + gameData.childDatas[i].playerName + "<space={1}>{2}", (i + 1), textSpace, gameData.childDatas[i].playerScore);
        }
    }
    private void UpdateSelectedTextAndSavePlayerName()
    {
        scoreTextsGUI[playerPositionInList].SetText("{0}." + playerName + "<space={1}>{2}", playerPositionInList + 1, textSpace, GameManager.instance.Score);
        gameData.childDatas[playerPositionInList].playerName = playerName;
    }
    private void SpaceKeyFunction()
    {
        if (newHighScore)
        {
            newHighScore = false;
            SaveScores();
            saveOrMainMenuTextGUI.SetText("MAIN MENU");
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }

    private string GetFilePath() => Path.Combine(Application.persistentDataPath, fileName);

    private string GetDirectoryPath() => Application.persistentDataPath;

    private class GameData
    {
        public string playerName;
        public int playerScore;
    }
    private class ParentGameData
    {
        public GameData[] childDatas;
    }
}
