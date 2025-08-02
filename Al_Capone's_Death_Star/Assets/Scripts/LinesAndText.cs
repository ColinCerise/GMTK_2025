using JetBrains.Annotations;
using Unity.VisualScripting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using TMPro;
using UnityEngine.UI;
public class LinesAndText : MonoBehaviour
{
    public int Hours = 12;
    public int Minutes;
    public float AccumulatedTime;
    public LineRenderer Line1;
    public GameObject Connectpoint1a;
    public GameObject Connectpoint1b;
    public LineRenderer Line2;
    public GameObject Connectpoint2a;
    public GameObject Connectpoint2b;
    public LineRenderer Line3;
    public GameObject Connectpoint3a;
    public GameObject Connectpoint3b;
    public LineRenderer Line4;
    public GameObject Connectpoint4a;
    public GameObject Connectpoint4b;
    public LineRenderer Line5;
    public GameObject Connectpoint5a;
    public GameObject Connectpoint5b;
    public LineRenderer Line6;
    public GameObject Connectpoint6a;
    public GameObject Connectpoint6b;

    // Variables for clock rendering
    [SerializeField] GameObject clockDisplay;
    [SerializeField] Sprite[] clockSprites;
    private Image[] clockDigits;
    public float TimeIncrements = .5f;
    public float HeldTimeInc;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Line1 = GameObject.Find("Line1").GetComponent<LineRenderer>();
        Line2 = GameObject.Find("Line2").GetComponent<LineRenderer>();
        Line3 = GameObject.Find("Line3").GetComponent<LineRenderer>();
        Line4 = GameObject.Find("Line4").GetComponent<LineRenderer>();
        Line5 = GameObject.Find("Line5").GetComponent<LineRenderer>();
        Line6 = GameObject.Find("Line6").GetComponent<LineRenderer>();

        clockDigits = clockDisplay.GetComponentsInChildren<Image>();

        HeldTimeInc = TimeIncrements;
    }

    // Update is called once per frame
    void Update()
    {
        Line1.SetPosition(0, Connectpoint1a.transform.position);
        Line1.SetPosition(1, Connectpoint1b.transform.position);
        Line2.SetPosition(0, Connectpoint2a.transform.position);
        Line2.SetPosition(1, Connectpoint2b.transform.position);
        Line3.SetPosition(0, Connectpoint3a.transform.position);
        Line3.SetPosition(1, Connectpoint3b.transform.position);
        Line4.SetPosition(0, Connectpoint4a.transform.position);
        Line4.SetPosition(1, Connectpoint4b.transform.position);
        Line5.SetPosition(0, Connectpoint5a.transform.position);
        Line5.SetPosition(1, Connectpoint5b.transform.position);
        Line6.SetPosition(0, Connectpoint6a.transform.position);
        Line6.SetPosition(1, Connectpoint6b.transform.position);

        AccumulatedTime += Time.deltaTime;
        ClockDisplay(Hours, Minutes);
        if (AccumulatedTime >= .5)
        {
            for (float i = AccumulatedTime; i > TimeIncrements; i -= TimeIncrements)
            {
                AccumulatedTime -= TimeIncrements;
                Minutes++;
                if (Minutes > 59)
                {
                    Minutes -= 60;
                    Hours++;
                }
            }
        }
        if (Hours >= 22)
        {
            GameObject BossMan = GameObject.Find("BossManager");
            BossManager BossScript = BossMan.GetComponent<BossManager>();
            if (BossScript.Stage == 0)
            {
                //BossScript.Stage = 1;
                TimeIncrements = 60;
            }
        }
    }
    private void ClockDisplay(int hours, int minutes)
    {
        clockDigits[0].sprite = clockSprites[hours / 10];
        clockDigits[1].sprite = clockSprites[hours % 10];
        clockDigits[2].sprite = clockSprites[minutes / 10];
        clockDigits[3].sprite = clockSprites[minutes % 10];
    }
    public void WorldRevolves()
    {
        Hours = 12;
        Minutes = 0;
        AccumulatedTime = 0;
        GameObject BossMan = GameObject.Find("BossManager");
        BossManager BossScript = BossMan.GetComponent<BossManager>();
        BossScript.Stage = 0;
        TimeIncrements = HeldTimeInc;
    }
}
