using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngerBar : MonoBehaviour
{
    public float Anger = 0;
    public float MaxAnger = 100;
    public GameObject GameManager;
    public GameManager GameManagerScript;
    // Start is called before the first frame update
    void Start()
    {
        GameManager = GameObject.Find("LoopManager");
        GameManagerScript = GameManager.GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Anger >= MaxAnger)
        {
            Debug.Log("GameOver");
            GameManagerScript.Loss = true;
            GameManagerScript.Loop();
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
