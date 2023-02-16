using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BonusPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] bonusTextsGUI;

    [SerializeField] private TextMeshProUGUI totalTextGUI;

    private int scoreBeforeBonus;

    private Coroutine animationCoroutine;

    private readonly int singleBonus = 100;
    private readonly int doubleBonus = 400;
    private readonly int tripleBonus = 900;
    private readonly int tetrisBonus = 2500;


    private void Update()
    {
        if (Input.anyKeyDown) EndAnimation();
    }
    private void OnEnable()
    {
        scoreBeforeBonus = GameManager.instance.Score;
        animationCoroutine = StartCoroutine(StartTextsAnimation());
    }
    private void OnDisable()
    {
        ResetTexts();
    }
    private void EndAnimation()
    {
        StopCoroutine(animationCoroutine);

        for (int i = 0; i < bonusTextsGUI.Length; i++) SetText(bonusTextsGUI[i]);

        SetText(totalTextGUI);

        GameManager.instance.Score = scoreBeforeBonus + GetTotalBonus();

        GameManager.instance.skipBonusPanel = true;
    }
    private IEnumerator StartTextsAnimation()
    {
        int currentTotalScore = 0;

        for (int i = 0; i < bonusTextsGUI.Length; i++)
        {
            int tempBonus = 0;
            int limit = GetLimit(bonusTextsGUI[i].name);
            while (tempBonus < limit)
            {
                tempBonus++;
                GameManager.instance.Score++;

                SetText(bonusTextsGUI[i], tempBonus);

                yield return new WaitForEndOfFrame();
            }
            currentTotalScore += tempBonus;
            SetText(totalTextGUI, currentTotalScore);
        }
    }
    private int GetLimit(string bonusTextName)
    {
        int limit = 0;
        switch (bonusTextName)
        {
            case "Single Bonus Text":
                limit = GetTotalSingleBonus();
                break;
            case "Double Bonus Text":
                limit = GetTotalDoubleBonus();
                break;
            case "Triple Bonus Text":
                limit = GetTotalTripleBonus();
                break;
            case "Tetris Bonus Text":
                limit = GetTotalTetrisBonus();
                break;
        }
        return limit;
    }
    private void SetText(TextMeshProUGUI textGUI, int? score = null)
    {
        int tempScore = 0;
        switch (textGUI.name)
        {
            case "Single Bonus Text":
                tempScore = (score != null) ? (int)score : GetTotalSingleBonus();
                textGUI.SetText("{0}<space=2>SINGLE<br>X{1}=<space={2}>{3}", GameManager.instance.SingleCount, singleBonus, SetSpace(tempScore), tempScore);
                break;
            case "Double Bonus Text":
                tempScore = (score != null) ? (int)score : GetTotalDoubleBonus();
                textGUI.SetText("{0}<space=2>DOUBLE<br>X{1}=<space={2}>{3}", GameManager.instance.DoubleCount, doubleBonus, SetSpace(tempScore), tempScore);
                break;
            case "Triple Bonus Text":
                tempScore = (score != null) ? (int)score : GetTotalTripleBonus();
                textGUI.SetText("{0}<space=2>TRIPLE<br>X{1}=<space={2}>{3}", GameManager.instance.TripleCount, tripleBonus, SetSpace(tempScore), tempScore);
                break;
            case "Tetris Bonus Text":
                tempScore = (score != null) ? (int)score : GetTotalTetrisBonus();
                textGUI.SetText("{0}<space=2>TETRIS<br>{1}=<space={2}>{3}", GameManager.instance.TetrisCount, tetrisBonus, SetSpace(tempScore), tempScore);
                break;
            case "Total Bonus Text":
                tempScore = (score != null) ? (int)score : GetTotalBonus();
                textGUI.SetText("TOTAL<space={0}>{1}", SetSpace(tempScore), tempScore);
                break;
        }
    }
    private int SetSpace(int score)
    {
        if (score > 10000) return 0;

        else if (score < 10000 && score >= 1000) return 1;

        else if (score < 1000 && score >= 100) return 2;

        else if (score < 100 && score >= 10) return 3;

        return 4;
    }
    private void ResetTexts()
    {
        for (int i = 0; i < bonusTextsGUI.Length; i++)
        {
            SetText(bonusTextsGUI[i],0);
        }
    }
    private int GetTotalSingleBonus() => GameManager.instance.SingleCount * singleBonus;
    private int GetTotalDoubleBonus() => GameManager.instance.DoubleCount * doubleBonus;
    private int GetTotalTripleBonus() => GameManager.instance.TripleCount * tripleBonus;
    private int GetTotalTetrisBonus() => GameManager.instance.TetrisCount * tetrisBonus;
    private int GetTotalBonus() => GetTotalSingleBonus() + GetTotalDoubleBonus() + GetTotalTripleBonus() + GetTotalTetrisBonus();
}