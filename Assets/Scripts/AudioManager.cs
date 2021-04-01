using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource _audio;
    [SerializeField] private AudioClip _backgroundMusic;
    private float _musicVolume;
    private bool _audioMuted;
    private static AudioManager _instance;
    private float _masterVolume;

    public static AudioManager Instance
    {
        get
        {
            if (!_instance)
            {
                Debug.Log("AudioManager is NULL");
            }

            return _instance;
        }
    }

    private void Awake()
    {
        if (!_instance) _instance = this;
        else Destroy(gameObject);
        
        DontDestroyOnLoad(gameObject);
        GetVolumePlayerPrefs();
    }

    public int[] GetVolumePlayerPrefs()
    {
        int musicVolume = PlayerPrefs.GetInt("music_vol", 100);
        _musicVolume = musicVolume / 100;
        int masterVolume = PlayerPrefs.GetInt("master_vol", 100);
        _masterVolume = masterVolume / 100;
        int audioMuted = PlayerPrefs.GetInt("mute_audio");
        _audioMuted = audioMuted == 1;
        
        return new [] {masterVolume, musicVolume, audioMuted};
    }

    public void StartBackgroundMusic()
    {
        _audio.volume = _musicVolume;
        _audio.clip = _backgroundMusic;
        _audio.Play();
    }

    public void MasterVolumeChanged(float volume)
    {
        PlayerPrefs.SetInt("master_vol", (int)volume);
        _masterVolume = volume / 100;
        AudioListener.volume = _masterVolume;
    }

    public void MusicVolumeChanged(float volume)
    {
        PlayerPrefs.SetInt("music_vol", (int)volume);
        _musicVolume = volume / 100;
        _audio.volume = _musicVolume;
    }

    public void ToggleMuteAudio(bool mute)
    {
        _audioMuted = mute;
        AudioListener.volume = mute ? 0 : _masterVolume;
        PlayerPrefs.SetInt("mute_audio", _audioMuted ? 1 : 0);
    }

    public void PlayAudioClip(AudioClip audioClip)
    {
        if(audioClip) _audio.PlayOneShot(audioClip);
    }
}
