using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public GameObject TutorialConvo;
    public ConversationManager TutorialScript;
    public GameObject Manager;
    public LinesAndText WorldScript;
    public GameObject TutorialInstructions;
    public GameObject TutorialInstructions2;
    public GameObject TutorialInstructions3;
    //public int TutStage = 1;
    // Start is called before the first frame update
    void Start()
    {
        Manager = GameObject.Find("Manager");
        if (Manager != null)
        {
            WorldScript = Manager.GetComponent<LinesAndText>();
        }
        TutorialConvo = GameObject.Find("Tutorial");
        if (TutorialConvo != null)
        {
            TutorialScript = TutorialConvo.GetComponent<ConversationManager>();
        }
        TutorialInstructions.SetActive(true);
        TutorialInstructions2.SetActive(false);
        TutorialInstructions3.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (WorldScript.Hours == 12 && WorldScript.Minutes == 29 && !TutorialScript.CallEnded)
        {
            WorldScript.TimeIncrements = 999999;
        }
        if (WorldScript.Minutes == 29 && TutorialScript.CallEnded)
        {
            WorldScript.RemoveAccumulated();
            WorldScript.TimeIncrements = WorldScript.HeldTimeInc;
            this.gameObject.SetActive(false);
        }
        if (TutorialScript.CallConnected && TutorialInstructions.activeSelf)
        {
            TutorialInstructions.SetActive(false);
            TutorialInstructions2.SetActive(true);
        }
        if (TutorialScript.CallStarted && TutorialInstructions2.activeSelf)
        {
            TutorialInstructions2.SetActive(false);
            TutorialInstructions3.SetActive(true);
        }
        if (TutorialInstructions3.activeSelf && TutorialScript.TimeWaited > 0)
        {
            TutorialInstructions3.SetActive(false);
        }
        
    }
}
