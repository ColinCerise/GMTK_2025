using JetBrains.Annotations;
using Unity.VisualScripting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using TMPro;
using System;

public class DialogueManager : MonoBehaviour
{
    public TMP_Text DialogueBox;
    public int StartNum = 0;
    public GameObject Wiretap;
    public Wiretap WiretapScript;
    public GameObject PlayerReciever;
    public OutputJack PlayerRecieverScript;
    public GameObject Conversation;
    public float DialogueTimer = 0;
    public bool PreconversationOverride = false;

    [SerializeField] List<string> physicalLines = new List<string>();

    [SerializeField] int maxLines = 4;
    private int lastSpaceIndex = 0;
    private int lastLineIndex = 0;
    private int tempCount = 0;

    private ConvoLog convoLog;
    public string[] troublesomeNames;
    [SerializeField] string formattedCurrentText;

    // Start is called before the first frame update
    void Start()
    {
        Wiretap = GameObject.Find("Wiretap");
        WiretapScript = Wiretap.GetComponent<Wiretap>();
        PlayerReciever = GameObject.Find("PlayerReciever");
        PlayerRecieverScript = PlayerReciever.GetComponent<OutputJack>();
        convoLog = GameObject.Find("Manager").GetComponent<ConvoLog>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (PlayerRecieverScript.isActive)
        {
            GameObject TempNotConv = GameObject.Find(PlayerRecieverScript.Connector.gameObject.name.Substring(0, (PlayerRecieverScript.Connector.gameObject.name.Length - 1)) + "1");
            if (TempNotConv.GetComponent<OutputJack>().PendingConversation != null)
            {
                ConversationManager Convo = TempNotConv.GetComponent<OutputJack>().PendingConversation.GetComponent<ConversationManager>();
                if (Convo != null && (Convo.TheBossException && !Convo.TutorialException))
                {

                }
                else
                {
                    DialogueBox.text = Convo.PreConversation;
                    PreconversationOverride = true;
                }
            }
        }
        else
        {
            PreconversationOverride = false;
        }
        if (Conversation != null && Conversation.GetComponent<ConversationManager>().CurrentDialog != null && !PreconversationOverride)
        {
            //CallDialogueUpdate();
        }
        else
        {
            DialogueTimer += Time.deltaTime;
            if (DialogueTimer >= 3 && !PlayerRecieverScript.isActive)
            {
                DialogueBox.text = null;
                DialogueTimer = 0;
            }
        }
    }
    public void SetAsTalking(GameObject Conversationist, int startingNum)
    {
        lastSpaceIndex = 0;
        lastLineIndex = 0;
        Conversation = Conversationist;
        ConversationManager convo = Conversation.GetComponent<ConversationManager>();
        formattedCurrentText = convo.StartTime();

        if (startingNum < 20)
        {
            StartNum = 0;
        }
        else
        {
            int firstIndex = startingNum - 20;
            int index = firstIndex;

            if (convo.GetConversation()[firstIndex] != ' ')
            {
                for (int i = firstIndex; index == firstIndex && i < convo.maxLeangth; i++)
                {
                    if (convo.GetConversation()[i] == ' ')
                    {
                        index = i;
                    }
                }
            }
            
            StartNum = index;
        }

        physicalLines.Clear();
        tempCount = 0;
        AddLine(convo.NextLine());
        lastSpaceIndex = StartNum;
        lastLineIndex = StartNum;
    }
    public void ForceUpdate()
    {
        
        if (Conversation != null && !PreconversationOverride)
        {
            ConversationManager convo = Conversation.GetComponent<ConversationManager>();
            convo.PlaceInConversation = convo.GetConversation().Length - 1;

            for (int i = 0; i < physicalLines.Count - maxLines || (string.IsNullOrWhiteSpace(physicalLines[0]) && physicalLines[0] != null); i++)
            {
                physicalLines.RemoveAt(0);
            }

            if (convo.IAMTALKING)
            {
                DisplayLines(convo);
                //convoLog.AddConvo(FormattedCurrentText());
            }
        }
        DialogueTimer = 0;
    }

    private void DisplayLines(ConversationManager convo)
    {
        string fullDialogue = "";
        foreach (string str in physicalLines)
        {
            fullDialogue += str;
        }

        if (StartNum != 0 && lastLineIndex == StartNum)
        {
            fullDialogue += "...";
        }

        fullDialogue += convo.GetConversation().Substring(lastLineIndex, convo.PlaceInConversation - lastLineIndex);
        if (!PlayerRecieverScript.isActive || convo.TheBossException)
        {
            DialogueBox.text = fullDialogue;
        }
    }

    public void CallDialogueUpdate()
    {
        
        ConversationManager convo = Conversation.GetComponent<ConversationManager>();

        tempCount = DialogueBox.textInfo.lineCount;
        DisplayLines(convo);
        DialogueTimer = 0;

        DialogueBox.ForceMeshUpdate();
        if (DialogueBox.textInfo.lineCount > tempCount)
        {
            string lineSection = convo.GetConversation().Substring(lastLineIndex, lastSpaceIndex - lastLineIndex);
            AddLine(lineSection);
            lastLineIndex = lastSpaceIndex;
        }

        if (convo.GetConversation()[convo.PlaceInConversation] == ' ')
        {
            lastSpaceIndex = convo.PlaceInConversation + 1;
        }
        else if (convo.GetConversation()[convo.PlaceInConversation] == '\n')
        {
            AddLine(convo.GetConversation().Substring(lastLineIndex, convo.PlaceInConversation - lastLineIndex));
            lastSpaceIndex = convo.PlaceInConversation + 1;
            lastLineIndex = convo.PlaceInConversation + 1;

            AddLineBreak();
            AddLine(convo.NextLine());
        }
    }

    private void AddLine(string line)
    {
        if (!string.IsNullOrEmpty(line))
        {
            int temp = DialogueBox.textInfo.lineCount;
            string result = line;
            if (StartNum != 0 && lastLineIndex == StartNum)
            {
                result = "..." + result;
            }
            physicalLines.Add(result);

            formattedCurrentText += result;

            for (int i = 0; i < physicalLines.Count - maxLines || (string.IsNullOrWhiteSpace(physicalLines[0]) && physicalLines[0] != null); i++)
            {
                physicalLines.RemoveAt(0);
            }

            //DisplayLines(Conversation.GetComponent<ConversationManager>());
        }
    }

    private void AddLineBreak()
    {
        physicalLines.Add("\n\n");
        formattedCurrentText += "\n\n";
    }

    public IEnumerator PlaceHolderLine()
    {
        yield return new WaitForEndOfFrame();
        physicalLines.Add(string.Empty);
    }

    public string FormattedCurrentText()
    {
        ConversationManager convo = Conversation.GetComponent<ConversationManager>();
        formattedCurrentText += convo.GetConversation().Substring(lastLineIndex, convo.PlaceInConversation - lastLineIndex);
        return formattedCurrentText;
    }
}
