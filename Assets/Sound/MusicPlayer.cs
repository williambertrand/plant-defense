using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{

    public static MusicPlayer Instance; 

    private AudioSource _audioSource;
     private void Awake()
    {
        if(Instance == null)
        {
            DontDestroyOnLoad(transform.gameObject);
            _audioSource = GetComponent<AudioSource>();
            Instance = this;
        }
        else
        {
            // Ok there must be a better way to do this...
            this.gameObject.SetActive(false);
            return;
        }
        
    }

    public void PlayMusic()
    {
        if (_audioSource.isPlaying) return;
        _audioSource.Play();
    }

    public void StopMusic()
    {
        _audioSource.Stop();
    }
}
