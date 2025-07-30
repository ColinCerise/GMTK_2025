using JetBrains.Annotations;
using Unity.VisualScripting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using TMPro;
public class LinesAndText : MonoBehaviour
{
    public TMP_Text ClockText;
    public int Hours = 12;
    public int Minutes;
    public float AccumulatedTime;
    public LineRenderer Line1;
    public GameObject Connectpoint1a;
    public GameObject Connectpoint1b;
    public LineRenderer Line2;
    public GameObject Connectpoint2a;
    public GameObject Connectpoint2b;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Line1 = GameObject.Find("Line1").GetComponent<LineRenderer>();
        Line2 = GameObject.Find("Line2").GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Line1.SetPosition(0, Connectpoint1a.transform.position);
        Line1.SetPosition(1, Connectpoint1b.transform.position);
        Line2.SetPosition(0, Connectpoint2a.transform.position);
        Line2.SetPosition(1, Connectpoint2b.transform.position);

        AccumulatedTime += Time.deltaTime;
        if (Minutes < 10)
        {
            ClockText.text = Hours.ToString() + ":0" + Minutes.ToString();
        }
        else
        {
            ClockText.text = Hours.ToString() + ":" + Minutes.ToString();
        }
        if (AccumulatedTime >= .5)
        {
            for (float i = AccumulatedTime; i > .5f; i -= .5f)
            {
                AccumulatedTime -= .5f;
                Minutes++;
                if (Minutes > 59)
                {
                    Minutes -= 60;
                    Hours++;
                }
            }
        }
    }
}
