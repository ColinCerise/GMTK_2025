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
    public GameObject DialogueBox;
    public DialogueManager DialogueBoxScript;

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
        DialogueBox = GameObject.Find("Dialogue Manager");
        DialogueBoxScript = DialogueBox.GetComponent<DialogueManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManagerScript.Loss)
        {
            //Failed = true;
            A.SetActive(false);
            B.SetActive(false);
            C.SetActive(false);
            D.SetActive(false);
            Activebuttons = false;
            GameManagerScript.Loss = true;
            GameManagerScript.Loop();
            Stage = 0;
            CorrectInput = 0;
            CurrentInput = 0;
        }
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
                A.GetComponent<ButtonBetter>().ConnectedText.text = "22:30";
                B.GetComponent<ButtonBetter>().ConnectedText.text = "23:00";
                C.GetComponent<ButtonBetter>().ConnectedText.text = "23:30";
                D.GetComponent<ButtonBetter>().ConnectedText.text = "00:00";
                DialogueBoxScript.DialogueBox.text = "1: When will the assassination attempt take place?";
                if (CurrentInput != 0)
                {
                    CheckAnswer();
                }
                break;
            case 2:
                CorrectInput = 3;
                A.GetComponent<ButtonBetter>().ConnectedText.text = "Unit 5";
                B.GetComponent<ButtonBetter>().ConnectedText.text = "Unit 8";
                C.GetComponent<ButtonBetter>().ConnectedText.text = "Unit 13";
                D.GetComponent<ButtonBetter>().ConnectedText.text = "Unit 15";
                DialogueBoxScript.DialogueBox.text = "2: Where will the assassination attempt take place?";
                if (CurrentInput != 0)
                {
                    CheckAnswer();
                }
                break;
            case 3:
                CorrectInput = 1;
                A.GetComponent<ButtonBetter>().ConnectedText.text = "Blue with eagle ornament";
                B.GetComponent<ButtonBetter>().ConnectedText.text = "Blue with stag ornament";
                C.GetComponent<ButtonBetter>().ConnectedText.text = "Black with badger ornament";
                D.GetComponent<ButtonBetter>().ConnectedText.text = "Black with ox ornament";
                DialogueBoxScript.DialogueBox.text = "3: What car will the getaway driver be in?";
                if (CurrentInput != 0)
                {
                    CheckAnswer();
                }
                break;
            case 4:
                CorrectInput = 2;
                A.GetComponent<ButtonBetter>().ConnectedText.text = "Behind a shrub outside the main door";
                B.GetComponent<ButtonBetter>().ConnectedText.text = "In a box near the garage door";
                C.GetComponent<ButtonBetter>().ConnectedText.text = "In a locker on the second floor";
                D.GetComponent<ButtonBetter>().ConnectedText.text = "Under a floorboard by the garage door";
                DialogueBoxScript.DialogueBox.text = "4: Where are the guns hidden?";
                if (CurrentInput != 0)
                {
                    CheckAnswer();
                }
                break;
            case 5:
                CorrectInput = 1;
                A.GetComponent<ButtonBetter>().ConnectedText.text = "\"Phil Potatoes\"";
                B.GetComponent<ButtonBetter>().ConnectedText.text = "Frank Piney";
                C.GetComponent<ButtonBetter>().ConnectedText.text = "Louis Boone";
                D.GetComponent<ButtonBetter>().ConnectedText.text = "Crag \"One-Eye\"";
                DialogueBoxScript.DialogueBox.text = "5: Who is the inside agent?";
                if (CurrentInput != 0)
                {
                    CheckAnswer();
                }
                break;
            case 6:
                CorrectInput = 3;
                A.GetComponent<ButtonBetter>().ConnectedText.text = "Thickwood";
                B.GetComponent<ButtonBetter>().ConnectedText.text = "Whistlepig";
                C.GetComponent<ButtonBetter>().ConnectedText.text = "The Monax";
                D.GetComponent<ButtonBetter>().ConnectedText.text = "Eastburrow";
                DialogueBoxScript.DialogueBox.text = "6: Which police agency was bribed?";
                if (CurrentInput != 0)
                {
                    CheckAnswer();
                }
                break;
            case 7:
                if (Failed)
                {
                    Debug.Log("Failed so now loop");
                    A.SetActive(false);
                    B.SetActive(false);
                    C.SetActive(false);
                    D.SetActive(false);
                    Activebuttons = false;
                    GameManagerScript.Loss = true;
                    GameManagerScript.Loop();
                    Stage = 0;
                    CorrectInput = 0;
                    CurrentInput = 0;
                }
                else
                {
                    Debug.Log("You Win");
                    GameManagerScript.Victory = true;
                    GameManagerScript.Loop();
                }
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
            Debug.Log("Failed the fianl");
            Failed = true;
        }
        CurrentInput = 0;
    }
    public void Input(int input)
    {
        CurrentInput = input;
    }
    public void AllRed()
    {
        SpriteRenderer Asr = A.GetComponent<SpriteRenderer>();
        Color Acolor = Asr.color;
        Acolor = new Color(255, 0, 0, 255);
        Asr.color = Acolor;
        SpriteRenderer Bsr = B.GetComponent<SpriteRenderer>();
        Color Bcolor = Bsr.color;
        Bcolor = new Color(255, 0, 0, 255);
        Bsr.color = Bcolor;
        SpriteRenderer Csr = C.GetComponent<SpriteRenderer>();
        Color Ccolor = Csr.color;
        Ccolor = new Color(255, 0, 0, 255);
        Csr.color = Ccolor;
        SpriteRenderer Dsr = D.GetComponent<SpriteRenderer>();
        Color Dcolor = Dsr.color;
        Dcolor = new Color(255, 0, 0, 255);
        Dsr.color = Dcolor;
    }
    public void ALLWhite()
    {
        SpriteRenderer Asr = A.GetComponent<SpriteRenderer>();
        Color Acolor = Asr.color;
        Acolor = new Color(255, 255, 255, 255);
        Asr.color = Acolor;
        SpriteRenderer Bsr = B.GetComponent<SpriteRenderer>();
        Color Bcolor = Bsr.color;
        Bcolor = new Color(255, 255, 255, 255);
        Bsr.color = Bcolor;
        SpriteRenderer Csr = C.GetComponent<SpriteRenderer>();
        Color Ccolor = Csr.color;
        Ccolor = new Color(255, 255, 255, 255);
        Csr.color = Ccolor;
        SpriteRenderer Dsr = D.GetComponent<SpriteRenderer>();
        Color Dcolor = Dsr.color;
        Dsr.color = Dcolor;
    }
}
