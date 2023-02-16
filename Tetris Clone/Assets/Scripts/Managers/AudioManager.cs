using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioClip[] musics;

    [SerializeField] private AudioClip titleMusic, levelUpMusic;

    [SerializeField] private AudioClip arrowEffect, gameOverEffect, levelUpEffect, lineClearEffect, selectEffect, tetrominoDropEffect;

    private bool silence = false;

    private int selectMusicIndex;
    private float time = 0f;

    private AudioSource audioSource;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else instance = this;

        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();

        SceneManager.activeSceneChanged += SceneChanged;
    }
    public void PlayTitleMusic()
    {
        audioSource.PlayOneShot(titleMusic);
    }
    public void PlayArrowEffect()
    {
        audioSource.PlayOneShot(arrowEffect);
    }
    public void PlayEffectWithMusic(AudioClip effect)
    {
        audioSource.PlayOneShot(effect);
    }
    public void PlayEffectAndEndMusic(AudioClip effect)
    {
        audioSource.Stop();
        audioSource.PlayOneShot(effect);
    }
    public void PlayMusic(int musicIndex)
    {
        if (musics[musicIndex] != null)
        {
            audioSource.Stop();
            audioSource.clip = musics[musicIndex];
            audioSource.Play();

            silence = false;
            selectMusicIndex = musicIndex;
        }
        else
        {
            if (audioSource.clip != null) audioSource.clip = null;
            audioSource.Stop();
            silence = true;
        }
    }
    public void PauseMusic()
    {
        time = audioSource.time;
        audioSource.Stop();
    }
    public void ContinueMusic()
    {
        if (!silence)
        {
            audioSource.clip = musics[selectMusicIndex];
            audioSource.time = time;
            audioSource.Play();
        }
    }
    public void StopMusic()
    {
        audioSource.Stop();
    }
    public float PlayLevelUpMusic()
    {
        audioSource.clip = levelUpMusic;
        audioSource.time = 0f;
        audioSource.Play();

        return levelUpMusic.length;
    }
    public float PlayLevelUpEffect()
    {
        audioSource.PlayOneShot(levelUpEffect);

        return levelUpEffect.length;
    }
    private void SceneChanged(Scene current, Scene next)
    {
        if (audioSource.clip != null)
        {
            if (next.buildIndex == current.buildIndex || next.buildIndex == 1)
            {
                audioSource.time = 0;

                if (!audioSource.isPlaying && !silence) audioSource.Play();
            }
            else
            {
                audioSource.Stop();
            }
        }
    }
}
