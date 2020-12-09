using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Class to play music. Call PlayMusic("index of music track") for each scene
/// </summary>
public class MusicPlayer : MonoBehaviour
{
    private AudioSource _audioSource;

    public List<AudioClip> audioClips;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayMusic(int audioClipIndex)
    {
        if (_audioSource.clip != audioClips[audioClipIndex]) {
            _audioSource.clip = audioClips[audioClipIndex];
            _audioSource.Play();
        }
    }

    public void StopMusic()
    {
        _audioSource.Stop();
    }
}