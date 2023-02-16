using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TetrominoAreaManager : MonoBehaviour
{
    public static TetrominoAreaManager instance;

    [SerializeField] private GameObject[] lineClearAnimations;

    [SerializeField] private AudioClip tetrominoDropEffect;
    [SerializeField] private AudioClip lineClearEffect;

    private int?[] destroyedLines = new int?[4];

    private void Awake()
    {
        if (instance != null) Destroy(gameObject);
        else instance = this;
    }
    public void TetrominoSavedArea()
    {
        AudioManager.instance.PlayEffectWithMusic(tetrominoDropEffect);
        CheckLines();
    }
    public void CheckLines()
    {
        bool lineFull = false;

        int destroyedCount = 0;

        for (int i = 0; i < Tetromino.areaHeight; i++)
        {
            for (int j = 0; j < Tetromino.areaWidth; j++)
            {
                if (Tetromino.area[j, i] == null)
                {
                    lineFull = false;
                    break;
                }
                if (!lineFull) lineFull = true;
            }
            if (lineFull)
            {
                destroyedLines[destroyedCount] = i;

                lineClearAnimations[destroyedCount].GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, i + 0.5f, 0);

                destroyedCount++;
                lineFull = false;
            }
        }
        ChangeAnimationsText(destroyedCount);
        switch (destroyedCount)
        {
            case 0:
                TetrominoManager.instance.CreateNewTetromino();
                break;
            case 1:
                GameManager.instance.SingleCount++;
                StartCoroutine(StartAnimation(destroyedCount));
                break;
            case 2:
                GameManager.instance.DoubleCount++;
                StartCoroutine(StartAnimation(destroyedCount));
                break;
            case 3:
                GameManager.instance.TripleCount++;
                StartCoroutine(StartAnimation(destroyedCount));
                break;
            case 4:
                GameManager.instance.TetrisCount++;
                StartCoroutine(StartAnimation(destroyedCount));
                break;
        }
    }
    public void ChangeTetrominosColorInArea()
    {
        foreach (Transform tetromino in Tetromino.area)
        {
            if (tetromino != null)
            {
                tetromino.GetComponent<SpriteRenderer>().color = GameManager.instance.LevelColor;
            }
        }
    }
    private void ChangeAnimationsText(int count)
    {
        if (count != 0)
        {
            for (int i = 0; i < count; i++)
            {
                switch (count)
                {
                    case 1:
                        lineClearAnimations[i].GetComponentInChildren<TextMeshProUGUI>().text = "SINGLE";
                        break;
                    case 2:
                        lineClearAnimations[i].GetComponentInChildren<TextMeshProUGUI>().text = "DOUBLE";
                        break;
                    case 3:
                        lineClearAnimations[i].GetComponentInChildren<TextMeshProUGUI>().text = "TRIPLE";
                        break;
                    case 4:
                        lineClearAnimations[i].GetComponentInChildren<TextMeshProUGUI>().text = "TETRIS";
                        break;
                }
            }
        }
    }
    private void DestroyLine(int?[] lineY)
    {
        for (int i = 0; i < destroyedLines.Length; i++)
        {
            if (destroyedLines[i] != null)
            {
                for (int j = 0; j < Tetromino.areaWidth; j++)
                {
                    Destroy(Tetromino.area[j, (int)destroyedLines[i] - i].gameObject);
                    Tetromino.area[j, (int)destroyedLines[i] - i] = null;
                }
                SetTetrominosPosition((int)destroyedLines[i] - i);
                destroyedLines[i] = null;
            }
        }
    }
    private void SetTetrominosPosition(int lineY)
    {
        for (int i = lineY + 1; i < Tetromino.areaHeight; i++)
        {
            for (int j = 0; j < Tetromino.areaWidth; j++)
            {
                if (Tetromino.area[j, i] == null) continue;

                Tetromino.area[j, i].position += new Vector3(0, -1, 0);
                Tetromino.area[j, i - 1] = Tetromino.area[j, i];
                Tetromino.area[j, i] = null;
            }
        }
    }
    private IEnumerator StartAnimation(int animationCount)
    {
        for (int i = 0; i < animationCount; i++)
        {
            lineClearAnimations[i].SetActive(true);
        }

        AudioManager.instance.PlayEffectWithMusic(lineClearEffect);

        yield return new WaitForSeconds(.5f);

        DestroyLine(destroyedLines);

        if(!GameManager.instance.Pause)  TetrominoManager.instance.CreateNewTetromino();
    }
}
