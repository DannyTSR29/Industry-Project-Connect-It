using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    
    public static SoundManager instance;
    public AudioClip errorFxClip, correctFxClip, buttonClickFxClip, moveFxClip;
    public AudioSource fxSource;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    void Start()
    {
        fxSource = GetComponent<AudioSource>();
    }
    
    void Update()
    {
       
    }

    public static void PlayErroClip()
    {
        instance.fxSource.clip = instance.errorFxClip;
        instance.fxSource.Play();
    }


    public static void PlayCorrectClip()
    {
        instance.fxSource.clip = instance.correctFxClip;
        instance.fxSource.Play();
    }

    public static void PlayClickClip()
    {
        instance.fxSource.clip = instance.buttonClickFxClip;
        instance.fxSource.Play();
    }

    public static void PlayMoveClip()
    {
        instance.fxSource.clip = instance.moveFxClip;
        instance.fxSource.Play();
    }

    public static void StopPlay()
    {
        instance.fxSource.Stop();
    }
}

