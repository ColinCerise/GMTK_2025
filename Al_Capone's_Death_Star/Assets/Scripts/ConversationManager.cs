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
    public bool TheCthuluException = false;
    public bool HeldCthulu = false;

    // Variables for parsing dialogue
    private int totalCharLength;
    public List<string> speakerList = new List<string>();
    public List<string> dialogueList = new List<string>();
    public int lineIndex;

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
                
                while ((line = sr.ReadLine()) != null)
                {
                    line.Trim();
                    speakerList.Add(line.Substring(0, line.IndexOf(": ")));
                    dialogueList.Add(line.Substring(line.IndexOf(": ") + 2));
                }
            }
        }
        else
        {
            Debug.Log(gameObject.name);
        }

        totalCharLength = 0;
        foreach (string str in dialogueList)
        {
            totalCharLength += str.Length;
        }

        string convo = "";
        foreach (string str in dialogueList)
        {
            convo += str;
        }
        Conversation = convo;
    }

    void Update()
    {
        if (TheCthuluException)
        {
            CallEnded = true;
            CallStarted = true;
            CallConnected = true;
            maxLeangth = totalCharLength;
            TimeWaited += Time.deltaTime;
            PlaceInConversation = (int)((TimeWaited - StartOffset) * LPS);
            if (!IAMTALKING)
            {
                IAMTALKING = true;
                DialogueBoxScript.SetAsTalking(this.gameObject, PlaceInConversation);
            }
            if (PlaceInConversation < maxLeangth)
            {
                //Debug.Log("I cant talk but must scream");
                CurrentDialog = Conversation.Substring(0, PlaceInConversation);
            }
            if (PlaceInConversation >= maxLeangth)
            {
                Debug.Log("Ending The convo");
                CurrentDialog = Conversation.Substring(0, maxLeangth);
                DialogueBoxScript.ForceUpdate();
                TheCthuluException = false;
                Manager.GetComponent<ConvoLog>().AddConvo(CurrentDialog);
            }
        }
        //check if enough time has passed and the call is still going
        if (((ManagerScript.Hours == TimeOffsetHours && ManagerScript.Minutes >= TimeOffsetMinutes) || ManagerScript.Hours > TimeOffsetHours) && !CallEnded)
        {
            maxLeangth = totalCharLength;
            //maxLeangth = Conversation.Length;
            TimeWaited += Time.deltaTime;
            ConnectedPoint = GameObject.Find(StarterOutput.name.Substring(0, (StarterOutput.name.Length - 1)) + "2");
            ConnectedReciever = ConnectedPoint.GetComponent<Grabbable>().TargettedReciever;
            if (StarterOutput.GetComponent<OutputJack>() != null && !StarterOutput.GetComponent<OutputJack>().LightActive)
            {
                StarterOutput.GetComponent<OutputJack>().SetLightActive(true, this.gameObject);
            }
            if (!CallConnected && TimeWaited >= 5)
            {
                AngryBar.AddAnger(Time.deltaTime);
            }
            if (ConnectedReciever == TargetReciever && !ConnectedPoint.GetComponent<Grabbable>().GrabbedLock)
            {
                CallStarted = true;
                CallConnected = true;
            }
            CheckIfActivate();
            CheckIfTimeout();
            if (CallStarted && !CallEnded)
            {
                if (ConnectedReciever != TargetReciever)
                {
                    CallEnded = true;
                    StarterOutput.GetComponent<OutputJack>().SetLightActive(false, this.gameObject);
                    CallMissed = true;
                    AngryBar.DisconnectedCall();
                    Manager.GetComponent<ConvoLog>().AddConvo(CurrentDialog);
                }
                RunConvo();
            }
            if (PlaceInConversation >= maxLeangth)
            {
                CurrentDialog = Conversation.Substring(DialogueBoxScript.StartNum, maxLeangth - DialogueBoxScript.StartNum);
                DialogueBoxScript.ForceUpdate();
                CallEnded = true;
                StarterOutput.GetComponent<OutputJack>().SetLightActive(false, this.gameObject);
                Manager.GetComponent<ConvoLog>().AddConvo(CurrentDialog);
            }
        }
        if (CallEnded && IAMTALKING && !TheCthuluException)
        {
            IAMTALKING = false;
            CurrentDialog = null;
        }
    }

    public string CurrentLine()
    {
        int charNum = PlaceInConversation;
        int lineIndex = -1;
        int remainder = charNum;
        if (charNum < totalCharLength)
        {
            for (int i = 0, count = charNum; count > 0; i++)
            {
                lineIndex = i;
                remainder = count;
                count -= dialogueList[i].Length;
            }
        }
        if (lineIndex >= 0 && lineIndex < dialogueList.Count)
        {
            return speakerList[lineIndex] + ": " + dialogueList[lineIndex].Substring(0, remainder);
        }
        else
        {
            return string.Empty;
        }
    }
    public void CheckIfTimeout()
    {
        if (TimeWaited >= 20 && !CallConnected)
        {
            CallEnded = true;
            StarterOutput.GetComponent<OutputJack>().SetLightActive(false, this.gameObject);
            CallMissed = true;
            AngryBar.DisconnectedCall();
        }
        else if (TimeWaited >= 30 && CallStarted)
        {
            CallEnded = true;
            StarterOutput.GetComponent<OutputJack>().SetLightActive(false, this.gameObject);
            CallMissed = true;
            AngryBar.DisconnectedCall();
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
        if ((TimeWaited - StartOffset) * LPS <= maxLeangth + 1) // Continue to progress conversation
        {
            // Set the integer value of the current character based on the passage of time since the beginning of the conversation
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
                    // Initialize dialogue encounter
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
                    StarterOutput.GetComponent<OutputJack>().SetLightActive(false, this.gameObject);
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
    public void Revolve()
    {
        CallEnded = false;
        CallConnected = false;
        CallStarted = false;
        CallMissed = false;
        TheCthuluException = HeldCthulu;
        PlaceInConversation = 0;
        TimeWaited = 0;
    }
}
