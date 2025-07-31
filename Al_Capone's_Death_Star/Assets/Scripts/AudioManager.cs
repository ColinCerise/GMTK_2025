using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource mainSource;
    [SerializeField] string[] sfxNames;
    [SerializeField] AudioClip[] sfxClips;

    public void PlaySFX(string sfxName)
    {
        mainSource.PlayOneShot(sfxClips[Array.IndexOf(sfxNames, sfxName)]);
    }
}
