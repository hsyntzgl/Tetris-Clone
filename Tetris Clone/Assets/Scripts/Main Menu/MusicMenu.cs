using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicMenu : MonoBehaviour
{
    [SerializeField] private RectTransform arrow;

    private int arrowIndex = 0;

    private void OnEnable()
    {
        SetArrowPositionAndMusic();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow) && arrowIndex + 1 < AudioManager.instance.musics.Length)
        {
            arrowIndex++;
            SetArrowPositionAndMusic();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) && arrowIndex > 0)
        {
            arrowIndex--;
            SetArrowPositionAndMusic();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            LoadGameScene();
        }
    }
    private void SetArrowPositionAndMusic()
    {
        arrow.anchoredPosition = new Vector2(arrow.anchoredPosition.x, (-1 * 100 * arrowIndex));

        AudioManager.instance.PlayMusic(arrowIndex);
    }
    private void LoadGameScene()
    {
        SceneManager.LoadScene(1);
    }
}
