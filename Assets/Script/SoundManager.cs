using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    public AudioClip winSound;
    public AudioClip popSound;
    public AudioClip swapSound;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void ChangeStateSound()
    {
        if(PlayerPrefs.HasKey("Sound"))
        {
            PlayerPrefs.SetInt("Sound", PlayerPrefs.GetInt("Sound") == 1 ? 0 : 1);
        }
        else
        {
            PlayerPrefs.SetInt("Sound", 1);
        }
    }

    public void PlaySound(string  name)
    {
        if(PrefManager.GetInt("Sound") != 1) return;
        switch (name)
        {
            case "winSound":
                AudioSource.PlayClipAtPoint(winSound, Camera.main.transform.position);
                break;
            case "popSound":
                AudioSource.PlayClipAtPoint(popSound, Camera.main.transform.position);
                break;
            case "swapSound":
                AudioSource.PlayClipAtPoint(swapSound, Camera.main.transform.position);
                break;
        }
    }

}
