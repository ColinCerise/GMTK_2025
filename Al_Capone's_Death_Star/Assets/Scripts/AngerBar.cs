using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngerBar : MonoBehaviour
{
    public float Anger = 0;
    public float MaxAnger = 100;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Anger >= MaxAnger)
        {
            Debug.Log("GameOver");
        }
    }
    public void AddAnger(float anger)
    {
        Anger += anger;
    }
    public void DisconnectedCall()
    {
        Anger += MaxAnger / 2;
    }
}
