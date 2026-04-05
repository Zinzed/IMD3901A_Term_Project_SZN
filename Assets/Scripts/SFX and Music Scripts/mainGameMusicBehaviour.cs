using System.Collections;
using UnityEngine;

public class mainGameMusicBehaviour : MonoBehaviour
{
    public FinalBossBehaviour bossBehaviour;

    public AudioSource musicSource;

    public AudioClip gameplayMusic;
    public AudioClip bossFightMusic;

    [SerializeField] private float fadeDuration = 2.0f;
    [SerializeField] private float gameplayVolume = 0.5f;
    [SerializeField] private float bossFightVolume = 1.0f;

    private Coroutine fadeCoroutine;

    private enum MusicState
    { 
        Gameplay,
        Silence,
        BossFight
    }

    private MusicState currentState;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        musicSource.volume = gameplayVolume;
        currentState = MusicState.Gameplay;
        FadeToMusic(gameplayMusic);
    }

    // Update is called once per frame
    void Update()
    {
        if (bossBehaviour.IsBossDead)
        {
            if (currentState != MusicState.Gameplay)
            {
                FadeToMusic(gameplayMusic);
                currentState = MusicState.Gameplay;
            }
            return;
        }

        if (bossBehaviour == null)
        {
            return;
        }

        if (bossBehaviour.introPlaying)
        {
            if (currentState != MusicState.Silence)
            {
                FadeToMusic(null);
                currentState = MusicState.Silence;
            }

            return;
        }

        if (bossBehaviour.bossActive && !bossBehaviour.IsBossDead)
        {
            if (currentState != MusicState.BossFight)
            {
                FadeToMusic(bossFightMusic);
                currentState = MusicState.BossFight;
            }
            return;
        }

        if (currentState != MusicState.Gameplay)
        {
            FadeToMusic(gameplayMusic);
            currentState = MusicState.Gameplay;
        }
    }

    void FadeToMusic(AudioClip newClip)
    {
        if (musicSource == null)
        {
            return;
        }

        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        float targetVolume = 0f;

        if (newClip == gameplayMusic)
        {
            targetVolume = gameplayVolume;
        }
        else if (newClip == bossFightMusic)
        {
            targetVolume = bossFightVolume;
        }

        fadeCoroutine = StartCoroutine(FadeMusicRoutine(newClip, targetVolume));
    }

    IEnumerator FadeMusicRoutine(AudioClip newClip, float targetVolume)
    {
        // fade out current music
        while (musicSource.volume > 0)
        {
            musicSource.volume -= Time.deltaTime / fadeDuration;
            yield return null;
        }

        musicSource.volume = 0;

        if (newClip == null)
        {
            musicSource.Stop();
            yield break;
        }

        musicSource.clip = newClip;
        musicSource.loop = true;
        musicSource.Play();

        // fade in to the correct target volume
        while (musicSource.volume < targetVolume)
        {
            musicSource.volume += Time.deltaTime / fadeDuration;
            yield return null;
        }

        musicSource.volume = targetVolume;
    }
}
