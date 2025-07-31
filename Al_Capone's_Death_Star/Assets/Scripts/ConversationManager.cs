using JetBrains.Annotations;
using Unity.VisualScripting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using TMPro;
using System.IO;

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
    public GameObject AngerManager;
    public AngerBar AngryBar;
    public float TimeWaited = 0;
    public bool CallEnded = false;
    public bool CallMissed = false;
    public bool CallConnected = false;
    public bool CallStarted = false;
    public bool IAMTALKING = false;
    public bool ListenedToConvo;
    GameObject ConnectedPoint;
    GameObject ConnectedReciever;

    // Variables for parsing dialogue
    [SerializeField] string encounterName;
    private int totalCharLength;

    // Start is called before the first frame
    void Start()
    {
        Wiretap = GameObject.Find("Wiretap");
        Manager = GameObject.Find("Manager");
        DialogueBox = GameObject.Find("Dialogue Manager");
        PlayerReciever = GameObject.Find("PlayerReciever");
        AngerManager = GameObject.Find("AngryBossManager");
        WiretapScript = Wiretap.GetComponent<Wiretap>();
        ManagerScript = Manager.GetComponent<LinesAndText>();
        DialogueBoxScript = DialogueBox.GetComponent<DialogueManager>();
        AngryBar = AngerManager.GetComponent<AngerBar>();
        ConversationTargets = StarterOutput.name + " + " + TargetReciever.name;

        ParseDialogue();
    }

    private void ParseDialogue()
    {
        List<string> speakerList = new List<string>();
        List<string> dialogueList = new List<string>();
        string filepath = Application.dataPath + "\\" + encounterName + ".txt";

        if (File.Exists(filepath))
        {
            using StreamReader sr = new StreamReader(filepath);
            {
                string line;
                
                while ((line = sr.ReadLine()) != null)
                {
                    line.Trim();
                    speakerList.Add(line.Substring(0, line.IndexOf(": ")));
                    dialogueList.Add(line.Substring(line.IndexOf(": ") + 1));
                }
            }
        }

        totalCharLength = 0;
        foreach (string str in dialogueList)
        {
            totalCharLength += str.Length;
        }
    }

    void Update()
    {
        if (((ManagerScript.Hours == TimeOffsetHours && ManagerScript.Minutes >= TimeOffsetMinutes) || ManagerScript.Hours > TimeOffsetHours) && !CallEnded)
        {
            maxLeangth = Conversation.Length;
            TimeWaited += Time.deltaTime;
            ConnectedPoint = GameObject.Find(StarterOutput.name.Substring(0, (StarterOutput.name.Length - 1)) + "2");
            ConnectedReciever = ConnectedPoint.GetComponent<Grabbable>().TargettedReciever;
            if (StarterOutput.GetComponent<OutputJack>() != null && !StarterOutput.GetComponent<OutputJack>().LightActive)
            {
                StarterOutput.GetComponent<OutputJack>().SetLightActive(true);
            }
            if (!CallConnected && TimeWaited >= 5)
            {
                AngryBar.AddAnger(Time.deltaTime);
            }
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
                StarterOutput.GetComponent<OutputJack>().SetLightActive(false);
                CallMissed = true;
                AngryBar.DisconnectedCall();
            }
            else if (TimeWaited >= 30 && CallStarted)
            {
                CallEnded = true;
                StarterOutput.GetComponent<OutputJack>().SetLightActive(false);
                CallMissed = true;
                AngryBar.DisconnectedCall();
            }
            else if (CallStarted && !CallEnded)
            {
                if (ConnectedReciever != TargetReciever)
                {
                    CallEnded = true;
                    StarterOutput.GetComponent<OutputJack>().SetLightActive(false);
                    CallMissed = true;
                    AngryBar.DisconnectedCall();
                    Manager.GetComponent<ConvoLog>().AddConvo(CurrentDialog);
                }
                if ((TimeWaited - StartOffset) * LPS <= maxLeangth + 1)
                {
                    PlaceInConversation = (int)((TimeWaited - StartOffset) * LPS);
                    if (WiretapScript.Conversation != null && WiretapScript.Conversation.Equals(ConversationTargets))
                    {
                        if (PlaceInConversation - DialogueBoxScript.StartNum >= maxLeangth / 2)
                        {
                            ListenedToConvo = true;
                            //Debug.Log(PlaceInConversation - DialogueBoxScript.StartNum - maxLeangth / 2);
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
                            StarterOutput.GetComponent<OutputJack>().SetLightActive(false);
                            Manager.GetComponent<ConvoLog>().AddConvo(CurrentDialog);
                        }
                    }
                    else
                    {
                        IAMTALKING = false;
                        Manager.GetComponent<ConvoLog>().AddConvo(CurrentDialog);
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
                StarterOutput.GetComponent<OutputJack>().SetLightActive(false);
                Manager.GetComponent<ConvoLog>().AddConvo(CurrentDialog);
            }
        }
        if (CallEnded)
        {
            IAMTALKING = false;
            CurrentDialog = null;
        }
    }
}
