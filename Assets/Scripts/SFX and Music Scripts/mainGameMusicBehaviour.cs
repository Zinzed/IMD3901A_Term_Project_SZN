using UnityEngine;

public class mainGameMusicBehaviour : MonoBehaviour
{
    public FinalBossBehaviour bossBehaviour;

    public AudioSource musicSource;

    public AudioClip gameplayMusic;
    public AudioClip bossFightMusic;

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
        currentState = MusicState.Gameplay;
        PlayMusic(gameplayMusic);
    }

    // Update is called once per frame
    void Update()
    {
        if (bossBehaviour.IsBossDead)
        {
            if (currentState != MusicState.Gameplay)
            {
                PlayMusic(gameplayMusic);
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
                StopMusic();
                currentState = MusicState.Silence;
            }

            return;
        }

        if (bossBehaviour.bossActive && !bossBehaviour.IsBossDead)
        {
            if (currentState != MusicState.BossFight)
            {
                PlayMusic(bossFightMusic);
                currentState = MusicState.BossFight;
            }
            return;
        }

        if (currentState != MusicState.Gameplay)
        {
            PlayMusic(gameplayMusic);
            currentState = MusicState.Gameplay;
        }
    }

    void PlayMusic(AudioClip clip)
    {
        if (clip == null || musicSource == null)
        {
            return;
        }

        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    void StopMusic()
    {
        if (musicSource == null)
        {
            return;
        }

        musicSource.Stop();
    }
}
