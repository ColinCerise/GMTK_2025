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
    [SerializeField] string[] songNames;
    [SerializeField] AudioClip[] songClips;
    [SerializeField] bool musicOn = true;

    private void Start()
    {
        if (musicOn)
        {
            StartCoroutine(LoopMusic("main"));
        }
    }

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

    private IEnumerator LoopMusic(string song)
    {
        mainSource.clip = songClips[Array.IndexOf(songNames, song)];
        mainSource.Play();
        yield return new WaitForSeconds(mainSource.clip.length);
        if (musicOn)
        {
            StartCoroutine(LoopMusic(song));
        }
    }
}
