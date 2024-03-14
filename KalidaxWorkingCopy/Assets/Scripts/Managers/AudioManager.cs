using UnityEngine.Audio;
using System;
using UnityEngine;
using Random = UnityEngine.Random;
using Unity.VisualScripting;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Mixer Groups")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioMixerGroup sfxMixerGroup;
    [SerializeField] private AudioMixerGroup musicMixerGroup;
    [SerializeField] private readonly float startingVolume = 0.5f;
    private float prevMusicVolume;


    [Space()]
    public Sounds[] sounds;
    public BgSounds[] bgSounds;

    //a reference to the current instance of the audio manager we have in our scene
    public static AudioManager instance;
    private IEnumerator musicCoroutine;
    private IEnumerator musicCoroutine2;


    //Properties
    public AudioMixer m_AudioMixer { get => audioMixer;}

    public float m_StartingVolume => startingVolume;

    private void Awake()
    {
        //we want to check if there's only 1 in our scene. Singleton Pattern
        if(instance == null)
            instance = this;

        else
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);

        //set up audio managers so we can play them in the start
        //loop through each class array
        foreach (Sounds s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.clip = s.clip[Random.Range(0, s.clip.Length)];
            s.source.outputAudioMixerGroup = sfxMixerGroup;
        }

        //set up audio managers so we can play them in the start
        //loop through each class array
        foreach (BgSounds s in bgSounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.clip = s.clip;
            s.source.outputAudioMixerGroup = musicMixerGroup;
        }

    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("Master")) return;
        
        SetAudioMixer();
    }

    private void SetAudioMixer()
    {
        audioMixer.SetFloat("Master", Mathf.Log10(startingVolume) * 20);
        audioMixer.SetFloat("SFX",  Mathf.Log10(startingVolume) * 20);
        audioMixer.SetFloat("Music", Mathf.Log10(startingVolume) * 20);

        //Saving the value
        PlayerPrefs.SetFloat("MasterVolume", startingVolume);
        PlayerPrefs.SetFloat("SFXVolume", startingVolume);
        PlayerPrefs.SetFloat("MusicVolume", startingVolume);
    }


    public void Play(string name)
    {
        //randomize all the sounds that have multiple clips
        foreach (Sounds a in sounds)
        {
            a.source.clip = a.clip[Random.Range(0, a.clip.Length)];
        }

        //find a sound in the sounds array where sound.name == name the name inputted
        Sounds s = Array.Find(sounds, sound => sound.name == name);
      
        //if they didn't find a sound with that name in the array, throw an error
        if(s == null)
        {
            Debug.LogWarning("Sound: " + name + " Was not Found!");
            return;
        }

        s.source.Play();
    }
    public void BgPlay(BackgroundMusicSelector backgroundMusic)
    {

        //find a sound in the sounds array where sound.name == name the name inputted
        BgSounds s = Array.Find(bgSounds, sound => sound.backgroundMusic == backgroundMusic);

        //if they didn't find a sound with that name in the array, throw an error
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " Was not Found!");
            return;
        }

        //if the name is the same, then don't play a new background track
        if(s.source.isPlaying)
        {
            return;
        }
        //if it's not the same than the current one then stop all other bg musics and play this one
        else
        {
            //stop all the other background tracks and then play the new one
            foreach(BgSounds bg in bgSounds)
            {
                bg.source.Stop();

            }

            s.source.Play();
        }
        
    }

    public void LerpAudioToLevel(float _targetLevel)
    {
        musicCoroutine = IEChangeAudio(_targetLevel);
        StartCoroutine(musicCoroutine);
    }


    private IEnumerator IEChangeAudio(float _targetLevel)
    {
        //It can't be set to 0 for this math equation or else we'll get infinity
        _targetLevel = Mathf.Clamp(_targetLevel, 0.0001f, 1f);
        float waitTime = 0.5f;
        float currentTime = 0;
   
        float currentLevel = GetMusicVolume();

        prevMusicVolume = currentLevel;

        while (currentTime < waitTime)
        {
           
            float newVol = Mathf.Lerp(currentLevel, _targetLevel, currentTime / waitTime);

            audioMixer.SetFloat("MusicLowered", Mathf.Log10(newVol) * 20);

            currentTime += Time.unscaledDeltaTime;

            yield return null;
        }

        yield break;

    }

    public void LerpAudioToPrevLevel()
    {
        musicCoroutine2 = IERevertAudio();
        StartCoroutine(musicCoroutine2);
    }

    private IEnumerator IERevertAudio()
    {
        yield return new WaitForSeconds(0.1f);
        //It can't be set to 0 for this math equation or else we'll get infinity
    
        prevMusicVolume = Mathf.Clamp(prevMusicVolume, 0.0001f, 1f);

        float waitTime = 0.5f;
        float currentTime = 0;

        float currentLevel = GetMusicVolume();

        while (currentTime < waitTime)
        {

            float newVol = Mathf.Lerp(currentLevel, prevMusicVolume, currentTime / waitTime);

            audioMixer.SetFloat("MusicLowered", Mathf.Log10(newVol) * 20);

            currentTime += Time.deltaTime;

            yield return null;
        }

        yield break;

    }

    private float GetMusicVolume()
    {
        float currentLevel;
        bool result = m_AudioMixer.GetFloat("MusicLowered", out currentLevel);
      
        if (result)
        {
            return Mathf.Pow(10, currentLevel / 20);
        }
        else
        {
            return 0;
        }
    }
}



[System.Serializable]
public class Sounds
{
    public string name;
    public AudioClip[] clip;

    [Range(0f,1f)]
    public float volume = 1;
    [Range(0f, 3f)]
    public float pitch = 1;

    public bool loop;
    [HideInInspector]
    public AudioSource source;
}

[System.Serializable]
public class BgSounds
{
    public BackgroundMusicSelector backgroundMusic;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = 1;
    [Range(0f, 3f)]
    public float pitch = 1;

    public bool loop;
    [HideInInspector]
    public AudioSource source;
}
