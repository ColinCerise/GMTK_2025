using JetBrains.Annotations;
using Unity.VisualScripting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using TMPro;

public class ConversationManager : MonoBehaviour
{
    public string Conversation;
    public string ConversationTargets;
    public float TimeOffsetHours = 12;
    public float TimeOffsetMinutes = 10;
    public float LPS = 20;
    public float StartOffset = 0;
    public int PlaceInConversation = 0;
    int maxLeangth;
    //public char CurrentChar;
    public string CurrentDialog;
    public GameObject Wiretap;
    public Wiretap WiretapScript;
    public GameObject Manager;
    public LinesAndText ManagerScript;
    public GameObject DialogueBox;
    public DialogueManager DialogueBoxScript;
    public float TimeWaited = 0;
    public bool CallEnded = false;
    public bool CallConnected = false;
    public bool CallStarted = false;
    public bool IAMTALKING = false;
    // Start is called before the first frame update
    void Start()
    {
        Wiretap = GameObject.Find("Wiretap");
        Manager = GameObject.Find("Manager");
        DialogueBox = GameObject.Find("Dialogue Manager");
        WiretapScript = Wiretap.GetComponent<Wiretap>();
        ManagerScript = Manager.GetComponent<LinesAndText>();
        DialogueBoxScript = DialogueBox.GetComponent<DialogueManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (((ManagerScript.Hours == TimeOffsetHours && ManagerScript.Minutes >= TimeOffsetMinutes) || ManagerScript.Hours > TimeOffsetHours) && !CallEnded)
        {
            TimeWaited += Time.deltaTime;
            
            if (CallStarted && StartOffset == 0)
            {
                StartOffset = TimeWaited;
            }
            if (TimeWaited >= 20 && !CallConnected)
            {
                CallEnded = true;
            }
            else if (TimeWaited >= 30 && !CallStarted)
            {
                CallEnded = true;
            }
            else if (CallStarted && !CallEnded)
            {
                maxLeangth = Conversation.Length;
                if ((TimeWaited - StartOffset) * LPS <= maxLeangth + 1)
                {
                    PlaceInConversation = (int)((TimeWaited - StartOffset) * LPS);
                    if (WiretapScript.Conversation.Equals(ConversationTargets))
                    {
                        if (!IAMTALKING)
                        {
                            IAMTALKING = true;
                            DialogueBoxScript.SetAsTalking(this.gameObject, PlaceInConversation);
                        }
                        CurrentDialog = Conversation.Substring(DialogueBoxScript.StartNum, PlaceInConversation);
                    }
                    else
                    {
                        IAMTALKING = false;
                        CurrentDialog = null;
                    }
                    //CurrentChar = Conversation[PlaceInConversation];
                }
            }
            if (PlaceInConversation >= maxLeangth)
            {
                CallEnded = true;
            }
        }
    }
}
