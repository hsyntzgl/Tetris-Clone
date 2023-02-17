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
    [SerializeField] private GameObject gameOverPanel;

    [SerializeField] private AudioClip levelUpEffect, gameOverEffect;

    [SerializeField] private TextMeshProUGUI scoreTextGUI, totalLineCountTextGUI, levelTextGUI;

    [SerializeField] private Color[] levelColors;

    private bool pause;
    private bool gameOver = false;
    private bool gameOverCoroutineFinish = false;

    private int level;
    private int score = 0;

    private int totalLineCount;

    private int singleCount, doubleCount, tripleCount, tetrisCount;

    private readonly int lastLevel = 17;
    private readonly int secondPhaseAfterThisLevel = 5;
    private readonly int levelUpLimitForPhaseOne = 30;
    private readonly int levelUpLimitForPhaseTwo = 50;

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
        get => levelColors[(level % (lastLevel - 1))];
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
        if (gameOver && Input.anyKeyDown && gameOverCoroutineFinish)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public void GameOver()
    {
        gameOver = true;
        gameOverPanel.SetActive(true);
        StartCoroutine(GameOverCoroutine());
    }
    private void CheckLevel()
    {
        int levelNotIndex = level + 1;
        if (levelNotIndex <= secondPhaseAfterThisLevel)
        {
            if (totalLineCount >= levelNotIndex * levelUpLimitForPhaseOne)
            {
                StartCoroutine(LevelUp());
            }
        }
        else
        {
            int subLevelForPhaseTwo = levelNotIndex - 5;
            if (totalLineCount >= ((levelNotIndex * levelUpLimitForPhaseOne) + (subLevelForPhaseTwo * levelUpLimitForPhaseTwo)))
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
    private IEnumerator GameOverCoroutine()
    {
        gameOverCoroutineFinish = (gameOverCoroutineFinish) ? false : gameOverCoroutineFinish;

        yield return new WaitForSeconds(AudioManager.instance.GameOverMusic());

        gameOverCoroutineFinish = true;
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
