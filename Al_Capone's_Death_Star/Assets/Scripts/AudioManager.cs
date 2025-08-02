using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource mainSource;
    [SerializeField] string[] sfxNames;
    [SerializeField] AudioClip[] sfxClips;
    [SerializeField] string[] voiceNames;
    [SerializeField] AudioClip[] voiceClips;

    public void PlaySFX(string sfxName)
    {
        mainSource.PlayOneShot(sfxClips[Array.IndexOf(sfxNames, sfxName)]);
    }

    public void PlayVoiceFX(string name)
    {
        if (Array.IndexOf(voiceNames, name) != -1)
        {
            mainSource.PlayOneShot(voiceClips[Array.IndexOf(voiceNames, name)]);
        }
    }
}
