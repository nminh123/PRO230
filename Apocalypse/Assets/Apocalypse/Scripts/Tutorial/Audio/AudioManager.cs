using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Volume")]
    [Range(0, 1)] public float musicVolume = 1f;
    [Range(0, 1)] public float sfxVolume = 1f;


    /// <summary>
    /// Phát âm thanh ngắn
    /// </summary>
    public void PlaySFX(string sfxName)
    {
        AudioClip clip = Resources.Load<AudioClip>("Audio/SFX/" + sfxName);
        if (clip)
        {
            sfxSource.PlayOneShot(clip, sfxVolume);
        }
        else
        {
            Debug.LogWarning("SFX không tìm thấy: " + sfxName);
        }
    }

    /// <summary>
    /// Phát nhạc nền
    /// </summary>
    public void PlayMusic(string musicName)
    {
        AudioClip clip = Resources.Load<AudioClip>("Audio/Music/" + musicName);
        if (clip && musicSource.clip != clip)
        {
            musicSource.clip = clip;
            musicSource.volume = musicVolume;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void SetMusicVolume(float value)
    {
        musicVolume = value;
        musicSource.volume = musicVolume;
    }

    public void SetSFXVolume(float value)
    {
        sfxVolume = value;
    }

    GameObject Linh;
    Transform viTri;
    private void Start()
    {
        PoolManager.Instance.ReturnToPool(Linh);
    }
}
