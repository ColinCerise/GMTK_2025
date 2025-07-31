using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] static AudioSource mainSource;
    [SerializeField] static string[] sfxNames;
    [SerializeField] static AudioClip[] sfxClips;

    public static void PlaySFX(string sfxName)
    {
        mainSource.PlayOneShot(sfxClips[Array.IndexOf(sfxNames, sfxName)]);
    }
}
