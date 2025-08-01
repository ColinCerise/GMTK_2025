using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    public bool Failed = false;
    public GameObject GameManager;
    public GameManager GameManagerScript;
    public int CurrentInput = 0;
    public int CorrectInput = 1;
    public int Stage = 0;
    public bool Activebuttons = false;
    public GameObject A;
    public GameObject B;
    public GameObject C;
    public GameObject D;

    // Start is called before the first frame update
    void Start()
    {
        A = GameObject.Find("A");
        B = GameObject.Find("B");
        C = GameObject.Find("C");
        D = GameObject.Find("D");
        GameManager = GameObject.Find("LoopManager");
        GameManagerScript = GameManager.GetComponent<GameManager>();
        A.SetActive(false);
        B.SetActive(false);
        C.SetActive(false);
        D.SetActive(false);
        Activebuttons = false;
    }

    // Update is called once per frame
    void Update()
    {
        switch(Stage)
        {
            case 1:
                if (!Activebuttons)
                {
                    A.SetActive(true);
                    B.SetActive(true);
                    C.SetActive(true);
                    D.SetActive(true);
                    Activebuttons = true;
                }
                CorrectInput = 4;
                if (CurrentInput != 0)
                {
                    CheckAnswer();
                }
                break;
            case 2:
                CorrectInput = 3;
                if (CurrentInput != 0)
                {
                    CheckAnswer();
                }
                break;
            case 3:
                CorrectInput = 1;
                if (CurrentInput != 0)
                {
                    CheckAnswer();
                }
                break;
            case 4:
                CorrectInput = 2;
                if (CurrentInput != 0)
                {
                    CheckAnswer();
                }
                break;
            case 5:
                CorrectInput = 1;
                if (CurrentInput != 0)
                {
                    CheckAnswer();
                }
                break;
            case 6:
                CorrectInput = 3;
                if (CurrentInput != 0)
                {
                    CheckAnswer();
                }
                break;
            case 7:
                Debug.Log("You Win");
                GameManagerScript.Victory = true;
                GameManagerScript.Loop();
                break;


        }
    }
    public void CheckAnswer()
    {
        if (CurrentInput == CorrectInput)
        {
            Stage++;
        }
        else
        {
            Failed = true;
            A.SetActive(false);
            B.SetActive(false);
            C.SetActive(false);
            D.SetActive(false);
            Activebuttons = false;
            GameManagerScript.Loss = true;
            GameManagerScript.Loop();
            Stage = 0;
        }
        CurrentInput = 0;
    }
    public void Input(int input)
    {
        CurrentInput = input;
    }
}
