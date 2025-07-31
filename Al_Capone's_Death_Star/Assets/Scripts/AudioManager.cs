using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] static AudioSource mainSource;

    [SerializeField] static string[] sfxNames;

    // Start is called before the first frame update
    void Start()
    {
        mainSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void PlaySFX(string soundEffect)
    {
        
    }
}
