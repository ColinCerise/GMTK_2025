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
    public string PreConversation;
    public string ConversationTargets;
    public GameObject PlayerReciever;
    public GameObject StarterOutput;
    public GameObject TargetReciever;
    public float TimeOffsetHours = 12;
    public float TimeOffsetMinutes = 10;
    public float LPS = 20;
    public float StartOffset = 0;
    public int PlaceInConversation = 0;
    public int maxLeangth;
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
    public bool CallMissed = false;
    public bool CallConnected = false;
    public bool CallStarted = false;
    public bool IAMTALKING = false;
    public bool ListenedToConvo = false;
    GameObject ConnectedPoint;
    GameObject ConnectedReciever; 
    // Start is called before the first frame
    void Start()
    {
        Wiretap = GameObject.Find("Wiretap");
        Manager = GameObject.Find("Manager");
        DialogueBox = GameObject.Find("Dialogue Manager");
        PlayerReciever = GameObject.Find("PlayerReciever");
        WiretapScript = Wiretap.GetComponent<Wiretap>();
        ManagerScript = Manager.GetComponent<LinesAndText>();
        DialogueBoxScript = DialogueBox.GetComponent<DialogueManager>();
        ConversationTargets = StarterOutput.name + " + " + TargetReciever.name;
    }

    // Update is called once per
    void Update()
    {
        if (((ManagerScript.Hours == TimeOffsetHours && ManagerScript.Minutes >= TimeOffsetMinutes) || ManagerScript.Hours > TimeOffsetHours) && !CallEnded)
        {
            maxLeangth = Conversation.Length;
            TimeWaited += Time.deltaTime;
            ConnectedPoint = GameObject.Find(StarterOutput.name.Substring(0, (StarterOutput.name.Length - 1)) + "2");
            ConnectedReciever = ConnectedPoint.GetComponent<Grabbable>().TargettedReciever;
            if (ConnectedReciever == TargetReciever)
            {
                CallStarted = true;
                CallConnected = true;
            }
            if (CallStarted && StartOffset == 0)
            {
                StartOffset = TimeWaited;
            }
            if (ConnectedReciever == PlayerReciever)
            {
                CallConnected = true;
            }
            if (TimeWaited >= 20 && !CallConnected)
            {
                CallEnded = true;
                CallMissed = true;
            }
            else if (TimeWaited >= 30 && CallStarted)
            {
                CallEnded = true;
                CallMissed = true;
            }
            else if (CallStarted && !CallEnded)
            {
                if (ConnectedReciever != TargetReciever)
                {
                    CallEnded = true;
                    CallMissed = true;
                }
                if ((TimeWaited - StartOffset) * LPS <= maxLeangth + 1)
                {
                    PlaceInConversation = (int)((TimeWaited - StartOffset) * LPS);
                    if (WiretapScript.Conversation != null && WiretapScript.Conversation.Equals(ConversationTargets))
                    {
                        if (PlaceInConversation - DialogueBoxScript.StartNum >= maxLeangth / 2)
                        {
                            ListenedToConvo = true;
                            Debug.Log(PlaceInConversation - DialogueBoxScript.StartNum - maxLeangth / 2);
                        }
                        if (!IAMTALKING)
                        {
                            IAMTALKING = true;
                            DialogueBoxScript.SetAsTalking(this.gameObject, PlaceInConversation);
                        }
                        if (PlaceInConversation < maxLeangth && DialogueBoxScript.StartNum < maxLeangth)
                        {
                            CurrentDialog = Conversation.Substring(DialogueBoxScript.StartNum, PlaceInConversation - DialogueBoxScript.StartNum);
                        }
                        else if (DialogueBoxScript.StartNum < maxLeangth)
                        {
                            CurrentDialog = Conversation.Substring(DialogueBoxScript.StartNum, maxLeangth - DialogueBoxScript.StartNum);
                        }
                        else
                        {
                            CallEnded = true;
                        }
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
                CurrentDialog = Conversation.Substring(DialogueBoxScript.StartNum, maxLeangth - DialogueBoxScript.StartNum);
                DialogueBoxScript.ForceUpdate();
                CallEnded = true;
            }
        }
        if (CallEnded)
        {
            IAMTALKING = false;
            CurrentDialog = null;
        }
    }
}
