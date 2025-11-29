using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    
    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource goalSfx;
    
    [Header("BGM")]
    [SerializeField] private AudioClip titleBgm;
    [SerializeField] private AudioClip gameBGM;
    
    [Header("SFX")]
    [SerializeField] private AudioClip kickSound;
    [SerializeField] private AudioClip goalSound;
    [SerializeField] private AudioClip dashSound;
    [SerializeField] private AudioClip superModeGeodainoSound;
    [SerializeField] private AudioClip superModeLightningmanSound;
    [SerializeField] private AudioClip whistleSound;
    
    [Header("Settings")]
    [SerializeField] private float bgmVolume = 0.5f;
    [SerializeField] private float sfxVolume = 0.7f;
    [SerializeField] private float goalSfxVolume = 0.7f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        if (bgmSource == null)
        {
            bgmSource = gameObject.AddComponent<AudioSource>();
        }
        bgmSource.loop = true;
        bgmSource.volume = bgmVolume;
        
        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
        }
        sfxSource.loop = false;
        sfxSource.volume = sfxVolume;

        if (goalSfx == null)
        {
            goalSfx = gameObject.AddComponent<AudioSource>();
        }
        goalSfx.loop = false;
        goalSfx.volume = goalSfxVolume;
    }
    
    void Start()
    {
        PlayTitleBGM();
    }

    public void PlayTitleBGM()
    {
        if (titleBgm != null && bgmSource != null)
        {
            bgmSource.clip = titleBgm;
            bgmSource.Play();
        }
    }
    
    public void PlayGameBGM()
    {
        if (gameBGM != null && bgmSource != null)
        {
            bgmSource.clip = gameBGM;
            bgmSource.Play();
        }
    }
    
    public void StopBGM()
    {
        if (bgmSource != null)
        {
            bgmSource.Stop();
        }
    }
    
    public void PlayKickSound()
    {
        PlaySFX(kickSound);
    }
    
    public void PlayGoalSound()
    {
        if (goalSound != null && goalSfx != null)
        {
            goalSfx.PlayOneShot(goalSound);
        }
    }
    
    public void PlayDashSound()
    {
        PlaySFX(dashSound);
    }
    
    public void PlaySuperModeSound(CharacterType.Type characterType)
    {
        if (characterType == CharacterType.Type.Geodaino)
        {
            PlaySFX(superModeGeodainoSound);
        }
        else if (characterType == CharacterType.Type.Lightningman)
        {
            PlaySFX(superModeLightningmanSound);
        }
    }
    
    public void PlayWhistleSound()
    {
        PlaySFX(whistleSound);
    }
    
    void PlaySFX(AudioClip clip)
    {
        if (clip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }
    
    public void SetBGMVolume(float volume)
    {
        bgmVolume = Mathf.Clamp01(volume);
        if (bgmSource != null)
        {
            bgmSource.volume = bgmVolume;
        }
    }
    
    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        if (sfxSource != null)
        {
            sfxSource.volume = sfxVolume;
        }
    }

    public void SetGoalSFXVolume(float volume)
    {
        goalSfxVolume = Mathf.Clamp01(volume);
        if (goalSfx != null)
        {
            goalSfx.volume = goalSfxVolume;
        }
    }
}

