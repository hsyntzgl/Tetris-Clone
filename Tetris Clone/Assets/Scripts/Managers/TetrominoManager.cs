using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TetrominoManager : MonoBehaviour
{
    public static TetrominoManager instance;

    [SerializeField] private Image nextTetrominoImage;

    
    [SerializeField] private GameObject[] tetrominos;

    [SerializeField] private Sprite[] tetrominos_Sprite;

    [SerializeField] private Transform tetrominoSpawnTransform;

    private List<GameObject> createdTetrominos;

    private int? nextTetrominoIndex = null;

    private void Awake()
    {
        if (instance != null) Destroy(gameObject);
        else instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        createdTetrominos = new List<GameObject>();
        CreateNewTetromino();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void CreateNewTetromino()
    {
        if (nextTetrominoIndex == null) nextTetrominoIndex = Random.Range(0, tetrominos.Length);
        GameObject newTetromino = Instantiate(tetrominos[(int)nextTetrominoIndex], tetrominoSpawnTransform.position, Quaternion.Euler(0, 0, 0));

        createdTetrominos.Add(newTetromino);

        NextTetromino();
    }
    public void ClearEmptyTetrominos()
    {
        for (int i = 0; i < createdTetrominos.Count; i++)
        {
            if (createdTetrominos[i].GetComponent<Tetromino>().CheckChilds())
            {
                GameObject temp = createdTetrominos[i];
                createdTetrominos.Remove(temp);
                Destroy(temp);
            }
        }
    }
    private void NextTetromino()
    {
        nextTetrominoIndex = Random.Range(0, tetrominos_Sprite.Length);

        nextTetrominoImage.color = tetrominos[(int)nextTetrominoIndex].transform.GetChild(0).GetComponent<SpriteRenderer>().color;
        
        
        nextTetrominoImage.sprite = GetTetrominoSprite(tetrominos[(int)nextTetrominoIndex].name);
    }
    private Sprite GetTetrominoSprite(string tetrominoName){
        foreach (Sprite sprite in tetrominos_Sprite)
        {
            if(sprite.name == tetrominoName) return sprite;
        }
        return null;
    }
}
