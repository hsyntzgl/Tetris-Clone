using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleMenu : MonoBehaviour
{
    
    [SerializeField] private GameObject musicMenuPanel;
    
/*
    [SerializeField] private AudioClip titleMenuMusic;
*/
    void Start()
    {
        AudioManager.instance.PlayTitleMusic();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            gameObject.SetActive(false);
            musicMenuPanel.SetActive(true);
        }
    }
}
