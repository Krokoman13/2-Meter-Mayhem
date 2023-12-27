using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;


//MUST BE PART OF A DONTDESTROYONLOAD GAMEOBJECT, SUCH AS THE APPHANDLER
public class AudioHandler : MonoBehaviour
{
    public static AudioHandler instance = null;
    float volume = 1;

    [Header("AudioSource Components")]
    [SerializeField] AudioSource musicSource = null;
    [SerializeField] AudioSource sfxSource = null;

    [Header("Music Clips")]
    public AudioClip[] music_gameplayBackgroundMusic = null;

    [Header("SFX Clips")]
    public AudioClip sfx_pickup = null;
    public AudioClip[] sfx_coughing = null;
    public AudioClip[] sfx_sneeze = null;





    public void PlayMusic(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogError("Music clip is null", this);
            return;
        }

        if(musicSource.isPlaying)
            musicSource.Stop();

        musicSource.clip = clip;        
        musicSource.Play();
    }
    public void PlayRandomMusicFromArray(AudioClip[] clips)
    {
        int rndm = Random.Range(0, clips.Length);
        PlayMusic(clips[rndm]);
    }

    public void StopMusic()
    {
        if (!musicSource.isPlaying)
            return;

        musicSource.Stop();
    }


    public void PlaySFX(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogError("SFX clip is null", this);
            return;
        }
        sfxSource.PlayOneShot(clip, volume);
    }
    public void PlayRandomSFXFromArray(AudioClip[] clips)
    {
        int rndm = Random.Range(0, clips.Length);
        PlaySFX(clips[rndm]);
    }




    void Awake()
    {
        //Make sure only one instance exists
        if (instance == null)
        {
            instance = this;
        }

        else if (instance != this)
            Destroy(gameObject);
    }

    public void GameplayLevelLoaded()
    {
        string volumeSettingName = SettingsHandler.instance.allSettings[2].name;
        float savedVolume = PlayerPrefs.GetInt(volumeSettingName, 100);
        volume = savedVolume / 100;
        musicSource.volume = volume;

        PlayRandomMusicFromArray(music_gameplayBackgroundMusic);
    }
}