using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetromino : MonoBehaviour
{
    public static int areaWidth = 10;
    public static int areaHeight = 20;

    public static Transform[,] area = new Transform[areaWidth, areaHeight];

    [SerializeField] private AudioClip dropEffect;
    [SerializeField] private float tetrominoSpeed;

    [SerializeField] private Vector3 centerPoint;

    private float previousTime;
    private float gravity;

    private bool tetrominoEnable = true;
    private bool fastDrop = false;

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.Pause && tetrominoEnable)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                transform.position += new Vector3(1, 0, 0);
                if (!ValidLeftRight()) transform.position += new Vector3(-1, 0, 0);
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                transform.position += new Vector3(-1, 0, 0);
                if (!ValidLeftRight()) transform.position += new Vector3(1, 0, 0);
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                StartCoroutine(DownFast());
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                transform.RotateAround(transform.TransformPoint(centerPoint), new Vector3(0, 0, 1), 90);
                if (!ValidLeftRight())
                {
                    transform.RotateAround(transform.TransformPoint(centerPoint), new Vector3(0, 0, -1), 90);
                }
            }
            if (Time.time - previousTime > TetrominoSpeed() && tetrominoEnable)
            {
                transform.position += new Vector3(0, -1, 0);
                if (!ValidDown())
                {
                    TetrominoDropped();
                }
                previousTime = Time.time;
            }
        }
    }

    private void AddScore(int TetrominoY)
    {
        GameManager.instance.Score += ((fastDrop ? 2 : 1) * GameManager.instance.Level * (GameManager.instance.Level + TetrominoY));
    }
    private void SaveToTheArea()
    {
        if (transform.position != GameObject.Find("Tetromino Spawn Position").transform.position)
        {
            foreach (Transform child in transform)
            {
                int roundedX = Mathf.RoundToInt(child.position.x);
                int roundedY = Mathf.RoundToInt(child.position.y);

                child.GetComponent<SpriteRenderer>().color = GameManager.instance.LevelColor;

                area[roundedX, roundedY] = child;
            }
            TetrominoAreaManager.instance.TetrominoSavedArea();
        }
        else
        {
            GameManager.instance.GameOver();
        }
    }
    private void TetrominoDropped()
    {
        transform.position += new Vector3(0, 1, 0);
        tetrominoEnable = false;
        SaveToTheArea();
    }
    private bool ValidDown()
    {
        foreach (Transform child in transform)
        {
            int roundedX = Mathf.RoundToInt(child.position.x);
            int roundedY = Mathf.RoundToInt(child.position.y);

            if (roundedY < 0 || roundedY >= areaHeight)
            {
                if (roundedY < 0) AddScore(roundedY + 1);
                return false;
            }
            if (area[roundedX, roundedY] != null)
            {
                AddScore(roundedY + 1);
                return false;
            }
        }
        return true;
    }
    private bool ValidLeftRight()
    {
        foreach (Transform child in transform)
        {
            int roundedX = Mathf.RoundToInt(child.position.x);
            int roundedY = Mathf.RoundToInt(child.position.y);

            if (roundedX < 0 || roundedX >= areaWidth)
            {

                return false;
            }

            if (area[roundedX, roundedY] != null)
            {
                return false;
            }
        }
        return true;
    }
    private float TetrominoSpeed()
    {
        return tetrominoSpeed - ((GameManager.instance.Level - 1) / 20);
    }
    public bool CheckChilds()
    {
        if (transform.childCount == 0)
        {
            return true;
        }
        return false;
    }
    private IEnumerator DownFast()
    {
        tetrominoEnable = false;
        fastDrop = true;
        while (true)
        {
            transform.position += new Vector3(0, -1, 0);

            if (!ValidDown())
            {
                TetrominoDropped();
                break;
            }
            yield return new WaitForSeconds(.01f); //10ms
        }
    }
}
