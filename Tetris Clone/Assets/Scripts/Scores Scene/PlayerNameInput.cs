using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerNameInput : MonoBehaviour
{
    private ScoreManager scoreManager;

    private int textCharacterIndex = 0;

    private int asciiIndex = 65;

    private readonly int minAsciiIndex = 65;
    private readonly int maxAsciiIndex = 90;

    private readonly int maxCharacterIndex = 5;
    private readonly int minCharacterIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        scoreManager = GetComponent<ScoreManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            asciiIndex = (asciiIndex + 1 <= maxAsciiIndex) ? ++asciiIndex : minAsciiIndex;
            ChangeCharacter();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            asciiIndex = (asciiIndex - 1 >= minAsciiIndex) ? --asciiIndex : maxAsciiIndex;
            ChangeCharacter();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            textCharacterIndex = (textCharacterIndex + 1 <= maxCharacterIndex) ? ++textCharacterIndex : minCharacterIndex;
            asciiIndex = GetASCII();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            textCharacterIndex = (textCharacterIndex - 1 >= minCharacterIndex) ? --textCharacterIndex : maxCharacterIndex;
            asciiIndex = GetASCII();
        }
    }
    private void ChangeCharacter()
    {
        StringBuilder sb = new StringBuilder(scoreManager.PlayerName);
        sb[textCharacterIndex] = GetCharacter();
        scoreManager.PlayerName = sb.ToString();
    }
    private int GetASCII()
    {
        StringBuilder sb = new StringBuilder(scoreManager.PlayerName);
        return (int)sb[textCharacterIndex];
    }
    private char GetCharacter() => (char)asciiIndex;
}
