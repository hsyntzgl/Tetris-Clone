using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool skipBonusPanel;

    [SerializeField] private GameObject bonusPanel;

    [SerializeField] private AudioClip levelUpEffect, gameOverEffect;

    [SerializeField] private TextMeshProUGUI scoreTextGUI, totalLineCountTextGUI, levelTextGUI;

    [SerializeField] private Color[] levelColors;

    private bool pause;
    private bool gameOver = false;

    private int level;
    private int score = 000000;

    private int totalLineCount;

    private int singleCount, doubleCount, tripleCount, tetrisCount;

    public int Level
    {
        get => level + 1;
    }

    public int Score
    {
        set
        {
            score = value;
            UpdateTexts();
        }
        get => score;
    }
    public int SingleCount
    {
        get => singleCount;
        set
        {
            singleCount = value;
            totalLineCount++;
            CheckLevel();
            UpdateTexts();
        }
    }
    public int DoubleCount
    {
        get => doubleCount;
        set
        {
            doubleCount = value;
            totalLineCount += 2;
            CheckLevel();
            UpdateTexts();
        }
    }
    public int TripleCount
    {
        get => tripleCount;
        set
        {
            tripleCount = value;
            totalLineCount += 3;
            CheckLevel();
            UpdateTexts();
        }
    }
    public int TetrisCount
    {
        get => tetrisCount;
        set
        {
            tetrisCount = value;
            totalLineCount += 4;
            CheckLevel();
            UpdateTexts();
        }
    }
    public Color LevelColor
    {
        get => levelColors[level];
    }

    public bool Pause
    {
        get => pause;
    }

    private void Awake()
    {
        if (instance != null) Destroy(gameObject);
        else instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        pause = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver && Input.anyKeyDown)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public void GameOver()
    {
        gameOver = true;
        AudioManager.instance.PlayEffectAndEndMusic(gameOverEffect);
    }
    private void CheckLevel()
    {
        if (level <= 4)
        {
            if (totalLineCount >= (level * 5) + 5)
            {
                StartCoroutine(LevelUp());
            }
        }
        else
        {
            if (totalLineCount >= (level * 30) - 50)
            {
                StartCoroutine(LevelUp());
            }
        }
    }
    private void UpdateTexts()
    {
        scoreTextGUI.text = "SCORE<br><br>" + score.ToString("000000");
        totalLineCountTextGUI.text = "LINES<br><br>" + totalLineCount.ToString();
        levelTextGUI.text = "LEVEL<br><br>" + (level + 1).ToString();
    }
    private void ResetBonus()
    {
        singleCount = 0;
        doubleCount = 0;
        tripleCount = 0;
        tetrisCount = 0;
    }
    private IEnumerator LevelUp()
    {
        pause = true;

        skipBonusPanel = false;

        level++;

        TetrominoAreaManager.instance.ChangeTetrominosColorInArea();

        UpdateTexts();

        AudioManager.instance.PauseMusic();

        yield return new WaitForSeconds(AudioManager.instance.PlayLevelUpEffect());

        bonusPanel.SetActive(true);

        for (float timer = AudioManager.instance.PlayLevelUpMusic(); timer > 0; timer -= Time.deltaTime)
        {
            if (skipBonusPanel)
            {
                AudioManager.instance.StopMusic();
                yield return new WaitForSeconds(1);
                break;
            }
            yield return null;
        }

        ResetBonus();

        bonusPanel.SetActive(false);

        AudioManager.instance.ContinueMusic();

        TetrominoManager.instance.ClearEmptyTetrominos();
        TetrominoManager.instance.CreateNewTetromino();

        pause = false;
    }
}
