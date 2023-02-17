using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleMenu : MonoBehaviour
{

    [SerializeField] private GameObject musicMenuPanel;
    [SerializeField] private GameObject arrow;

    private RectTransform arrowTransform;

    private int arrowIndex = 0;

    private readonly int maxIndex = 1;
    private readonly int minIndex = 0;

    private readonly int positionChangeSize = -100;

    void Start()
    {
        arrowTransform = arrow.GetComponent<RectTransform>();

        AudioManager.instance.PlayTitleMusic();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            arrowIndex = (arrowIndex == maxIndex) ? --arrowIndex : maxIndex;
            SetArrowPosition();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            arrowIndex = (arrowIndex == minIndex) ? ++arrowIndex : minIndex;
            SetArrowPosition();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpaceBarPressed();
        }
    }
    private void SetArrowPosition()
    {
        Vector2 newPosition = new Vector2(arrowTransform.anchoredPosition.x, arrowIndex * positionChangeSize);
        arrowTransform.anchoredPosition = newPosition;
    }
    private void SpaceBarPressed()
    {
        if (arrowIndex == minIndex)
        {
            gameObject.SetActive(false);
            musicMenuPanel.SetActive(true);
        }
        else
        {
            Application.Quit();
        }
    }
}
