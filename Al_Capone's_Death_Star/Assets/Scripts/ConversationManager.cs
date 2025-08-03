using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
//using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class ConversationManager : MonoBehaviour
{
    private string Conversation;
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
    public float ListenReq = .5f;
    GameObject ConnectedPoint;
    GameObject ConnectedReciever;
    public bool TheCthuluException = false;
    public bool HeldCthulu = false;
    public bool TheBossException = false;
    //public bool HeldBoss = false;
    public bool TutorialException = false;

    // Variables for parsing dialogue
    private int totalCharLength;
    public List<string> speakerList = new List<string>();
    public List<string> dialogueList = new List<string>();
    private int currentLineIndex = 0;
    public List<int> dialogueIndices = new List<int>();

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
        if (TheCthuluException)
        {
            HeldCthulu = true;
        }
        if (TheBossException)
        {
            //HeldBoss = true;
        }
        currentLineIndex = 0;
    }

    private void Awake()
    {
        ParseDialogue();
    }

    private void ParseDialogue()
    {
        string filepath = Application.dataPath + "/TextFiles/" + gameObject.name + ".txt";
        if (File.Exists(filepath))
        {
            using StreamReader sr = new StreamReader(filepath);
            {
                string line;
                int count = 0;
                
                while ((line = sr.ReadLine()) != null)
                {
                    line.Trim();
                    speakerList.Add(line.Substring(0, line.IndexOf(": ")));
                    string dialogue = line.Substring(line.IndexOf(": ") + 2);
                    count += dialogue.Length;
                    dialogueIndices.Add(count - 1);
                    dialogueList.Add(dialogue);
                }
            }
        }
        else
        {
            Debug.Log("Failed to parse conversation for " + gameObject.name);
        }

        totalCharLength = 0;
        foreach (string str in dialogueList)
        {
            totalCharLength += str.Length;
        }

        string convo = "";
        foreach (string str in dialogueList)
        {
            convo += str + "\n";
        }
        Conversation = convo;
    }

    void Update()
    {
        // Cthulu
        if (TheCthuluException && ((ManagerScript.Hours == TimeOffsetHours && ManagerScript.Minutes >= TimeOffsetMinutes) || ManagerScript.Hours > TimeOffsetHours))
        {
            CallEnded = true;
            CallStarted = true;
            CallConnected = true;
            maxLeangth = totalCharLength;
            TimeWaited += Time.deltaTime;

            int temp = PlaceInConversation;

            PlaceInConversation = (int)((TimeWaited - StartOffset) * LPS);
            if (PlaceInConversation < 0)
            {
                PlaceInConversation = 0;
            }

            if (PlaceInConversation != temp)
            {
                if (!IAMTALKING)
                {
                    IAMTALKING = true;
                    DialogueBoxScript.SetAsTalking(this.gameObject, PlaceInConversation);
                }
                if (PlaceInConversation < maxLeangth)
                {
                    //Debug.Log("I cant talk but must scream");
                    CurrentDialog = Conversation.Substring(0, PlaceInConversation);
                    DialogueBoxScript.CallDialogueUpdate();
                }
                if (PlaceInConversation >= maxLeangth)
                {
                    CurrentDialog = Conversation.Substring(0, maxLeangth);
                    DialogueBoxScript.ForceUpdate();
                    Manager.GetComponent<ConvoLog>().AddConvo(DialogueBoxScript.FormattedCurrentText());
                    TheCthuluException = false;
                    DialogueBoxScript.Conversation = null;
                    if (DialogueBoxScript.Conversation == this.gameObject)
                    {
                        DialogueBoxScript.Conversation = null;
                    }
                }
            }

        }

        //check if enough time has passed and the call is still going
        if (((ManagerScript.Hours == TimeOffsetHours && ManagerScript.Minutes >= TimeOffsetMinutes) || ManagerScript.Hours > TimeOffsetHours) && !CallEnded)
        {
            maxLeangth = totalCharLength;
            if (!TutorialException || IAMTALKING)
            {
                TimeWaited += Time.deltaTime;
            }
            ConnectedPoint = GameObject.Find(StarterOutput.name.Substring(0, (StarterOutput.name.Length - 1)) + "2");
            ConnectedReciever = ConnectedPoint.GetComponent<Grabbable>().TargettedReciever;
            if (StarterOutput.GetComponent<OutputJack>() != null && !StarterOutput.GetComponent<OutputJack>().LightActive)
            {
                StarterOutput.GetComponent<OutputJack>().SetLightActive(true, this.gameObject);
                Manager.GetComponent<AudioManager>().PlaySoundEffect("lightOn");
            }
            if (!CallConnected && TimeWaited >= 10 && !TutorialException)
            {
                AngryBar.AddAnger(Time.deltaTime);
            }
            if (ConnectedReciever == TargetReciever && !ConnectedPoint.GetComponent<Grabbable>().GrabbedLock)
            {
                CallStarted = true;
                CallConnected = true;
            }
            CheckIfActivate();
            if (!TheBossException)
            {
                CheckIfTimeout();
            }
            if (CallStarted && !CallEnded)
            {
                if (ConnectedReciever != TargetReciever) // If we start a call then disconnect
                {
                    CallEnded = true;
                    StarterOutput.GetComponent<OutputJack>().SetLightActive(false, this.gameObject);
                    CallMissed = true;
                    AngryBar.DisconnectedCall();
                    Debug.Log("Disconnected 1 on " + gameObject.name);
                    if (IAMTALKING)
                    {
                        LeaveConvo();
                    }
                }
                RunConvo();
            }
            if (PlaceInConversation >= maxLeangth) // If we have supposedly reached the end of the convo naturally
            {
                CurrentDialog = Conversation.Substring(DialogueBoxScript.StartNum, maxLeangth - DialogueBoxScript.StartNum);
                DialogueBoxScript.ForceUpdate();
                CallEnded = true;
                StarterOutput.GetComponent<OutputJack>().SetLightActive(false, this.gameObject);
                if (IAMTALKING)
                {
                    Manager.GetComponent<ConvoLog>().AddConvo(DialogueBoxScript.FormattedCurrentText());
                }
            }
        }
        if (CallEnded && IAMTALKING && !TheCthuluException)
        {
            IAMTALKING = false;
            CurrentDialog = null;
            if (DialogueBoxScript.Conversation == this.gameObject)
            {
                DialogueBoxScript.Conversation = null;
            }
        }
    }

    public void CheckIfTimeout()
    {
        if (TimeWaited >= 20 && !CallConnected)
        {
            CallEnded = true;
            Debug.Log("Ending call via connection timeout, i0.1");
            StarterOutput.GetComponent<OutputJack>().SetLightActive(false, this.gameObject);
            CallMissed = true;
            AngryBar.DisconnectedCall();
            Debug.Log("Disconnected 2 on " + gameObject.name);
        }
        else if (TimeWaited >= 30 && !CallStarted)
        {
            CallEnded = true;
            Debug.Log("Ending call via start timeout, i0.2");
            StarterOutput.GetComponent<OutputJack>().SetLightActive(false, this.gameObject);
            CallMissed = true;
            AngryBar.DisconnectedCall();
            Debug.Log("Disconnected 3 on " + gameObject.name);
        }
    }
    public void CheckIfActivate()
    {
        if (CallStarted && StartOffset == 0)
        {
            StartOffset = TimeWaited;
        }
        if (ConnectedReciever == PlayerReciever)
        {
            CallConnected = true;
        }
    }
    public void RunConvo()
    {
        //Debug.Log("Starts running");
        if ((TimeWaited - StartOffset) * LPS <= maxLeangth + 1) // Continue to progress conversation
        {
            //Debug.Log("Stage1");
            int temp = PlaceInConversation;
            // Set the integer value of the current character based on the passage of time since the beginning of the conversation
            PlaceInConversation = (int)((TimeWaited - StartOffset) * LPS);

            if (TutorialException && !IAMTALKING && PlaceInConversation == temp) //Jumpstarts the conversation bc otherwise it wont progress
            {
                PlaceInConversation++;
            }
            // If we have progressed to a new conversation index
            if (temp != PlaceInConversation)
            {
                //Debug.Log("Stage2");
                if ((TheBossException && !TutorialException) || (WiretapScript.Conversation != null && WiretapScript.Conversation.Equals(ConversationTargets)))
                {
                    //Debug.Log("Stage3");
                    if (PlaceInConversation - DialogueBoxScript.StartNum >= maxLeangth * ListenReq)
                    {

                        ListenedToConvo = true;
                        //Debug.Log(PlaceInConversation - DialogueBoxScript.StartNum - maxLeangth / 2);
                    }

                    if (!IAMTALKING)
                    {
                        //Debug.Log("Stage4 I will talk!");
                        IAMTALKING = true;

                        int index = 0;
                        for (int i = 0, count = PlaceInConversation; count > 0; i++)
                        {
                            index = i;
                            count -= dialogueList[i].Length;
                        }

                        currentLineIndex = index;

                        DialogueBoxScript.SetAsTalking(this.gameObject, PlaceInConversation);
                    }
                    else if (DialogueBoxScript.Conversation != this.gameObject)
                    {
                        DialogueBoxScript.SetAsTalking(this.gameObject, PlaceInConversation);
                    }
                    if (PlaceInConversation < maxLeangth && DialogueBoxScript.StartNum < maxLeangth)
                    {
                        CurrentDialog = Conversation.Substring(DialogueBoxScript.StartNum, PlaceInConversation - DialogueBoxScript.StartNum);
                        DialogueBoxScript.CallDialogueUpdate();
                    }
                    else if (DialogueBoxScript.StartNum < maxLeangth)
                    {
                        CurrentDialog = Conversation.Substring(DialogueBoxScript.StartNum, maxLeangth - DialogueBoxScript.StartNum);
                        DialogueBoxScript.CallDialogueUpdate();
                    }
                    else
                    {
                        CallEnded = true;
                        StarterOutput.GetComponent<OutputJack>().SetLightActive(false, this.gameObject);
                    }
                }
                else if (IAMTALKING)
                {
                    LeaveConvo();
                    IAMTALKING = false;
                    CurrentDialog = null;
                    if (DialogueBoxScript.Conversation == this.gameObject)
                    {
                        DialogueBoxScript.Conversation = null;
                    }
                }
            }
        }
    }
    public void Revolve()
    {
        if (!TutorialException)
        {
            CallEnded = false;
            CallConnected = false;
            CallStarted = false;
            CallMissed = false;
            IAMTALKING = false;
            TheCthuluException = HeldCthulu;
            PlaceInConversation = 0;
            TimeWaited = 0;
            StarterOutput.GetComponent<OutputJack>().SetLightActive(false, null);
        }
        //ConnectedPoint
    }

    public string NextLine()
    {
        currentLineIndex++;
        Manager.GetComponent<AudioManager>().PlayVoiceFX(speakerList[currentLineIndex - 1]);

        if (Array.IndexOf(DialogueBoxScript.troublesomeNames, speakerList[currentLineIndex - 1]) != -1)
        {
            StartCoroutine(DialogueBoxScript.PlaceHolderLine());
        }

        return "<color=#0e0b02>" + speakerList[currentLineIndex - 1] + "</color>\n";
    }

    public string GetConversation()
    {
        return Conversation;
    }

    public void LeaveConvo()
    {
        Debug.Log("Leaving convo");
        Manager.GetComponent<ConvoLog>().AddConvo(DialogueBoxScript.FormattedCurrentText() + "...");
    }

    public string StartTime()
    {
        return $"{(int)TimeOffsetHours}:{(int)TimeOffsetMinutes:D2} 02/02/1923\n\n";
    }
}
