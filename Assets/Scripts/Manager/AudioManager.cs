using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    private AudioSource mAudioSource;   
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        mAudioSource = GetComponent<AudioSource>(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Play(AudioClip audioClip)
    {
        if (mAudioSource != null)
        {
            mAudioSource.clip = audioClip;
            mAudioSource.Play();
        }
    }
    public void Play()
    {
        if (mAudioSource != null)
        {
            mAudioSource.Play();
        }
    }
    public void Pause()
    {
        if (mAudioSource != null)
        {
            mAudioSource.Pause();
        }
    }
    public void Stop()
    {
        if (mAudioSource != null)
        {
            mAudioSource.Stop();
        }
    }
    public void Mute(bool isTrue)
    {
        if (mAudioSource != null)
        {
            mAudioSource.mute = isTrue;
        }
    }
}
