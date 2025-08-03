using JetBrains.Annotations;
using Unity.VisualScripting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using TMPro;
using UnityEngine.UI;

public class AngerBar : MonoBehaviour
{
    public float Anger = 0;
    public float MaxAnger = 100;
    public GameObject GameManager;
    public GameManager GameManagerScript;

    [SerializeField] GameObject angerMeter;
    [SerializeField] Sprite[] meterSprites;
    private Sprite meterSprite;
    private GameObject angerMeterObject;
    private SpriteRenderer meterSpriteRenderer;
    public bool DontUpdateBar = false;
    // Start is called before the first frame update
    void Start()
    {
        GameManager = GameObject.Find("LoopManager");
        GameManagerScript = GameManager.GetComponent<GameManager>();

        angerMeterObject = GameObject.Find("AngerMeter");
        meterSpriteRenderer = angerMeterObject.GetComponent<SpriteRenderer>();
        meterSprite = meterSpriteRenderer.sprite;
    }

    // Update is called once per frame
    void Update()
    {
        if (Anger >= MaxAnger)
        {
            DontUpdateBar = true;
            meterSpriteRenderer.sprite = meterSprites[10];
            Anger = 0;
            Debug.Log("GameOver");
            GameManagerScript.Loss = true;
            GameManagerScript.Loop();
        }
        else
        {
            if (!DontUpdateBar && Anger < 100)
            {
                meterSpriteRenderer.sprite = meterSprites[(int)(Anger / 10)];
            }
        }
    }
    public void AddAnger(float anger)
    {
        Anger += anger;
    }
    public void DisconnectedCall()
    {
        Anger += MaxAnger / 4;
    }
    public void RevolveBar()
    {
        DontUpdateBar = false;
    }
}
