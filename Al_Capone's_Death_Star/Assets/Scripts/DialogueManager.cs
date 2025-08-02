using JetBrains.Annotations;
using Unity.VisualScripting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using TMPro;

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

    // Start is called before the first frame update
    void Start()
    {
        Wiretap = GameObject.Find("Wiretap");
        WiretapScript = Wiretap.GetComponent<Wiretap>();
        PlayerReciever = GameObject.Find("PlayerReciever");
        PlayerRecieverScript = PlayerReciever.GetComponent<OutputJack>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (PlayerRecieverScript.isActive)
        {
            GameObject TempNotConv = GameObject.Find(PlayerRecieverScript.Connector.gameObject.name.Substring(0, (PlayerRecieverScript.Connector.gameObject.name.Length - 1)) + "1");
            if (TempNotConv.GetComponent<OutputJack>().PendingConversation != null)
            {
                DialogueBox.text = TempNotConv.GetComponent<OutputJack>().PendingConversation.GetComponent<ConversationManager>().PreConversation;
                PreconversationOverride = true;
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

        if (startingNum < 10)
        {
            StartNum = 0;
        }
        else
        {
            int firstIndex = startingNum - 10;
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
        ConversationManager convo = Conversation.GetComponent<ConversationManager>();
        if (Conversation != null && convo && !PreconversationOverride)
        {
            convo.PlaceInConversation = convo.GetConversation().Length - 1;
            DisplayLines(convo);

            for (int i = 0; i < physicalLines.Count - maxLines || (string.IsNullOrWhiteSpace(physicalLines[0]) && physicalLines[0] != null); i++)
            {
                physicalLines.RemoveAt(0);
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
        if (!PlayerRecieverScript.isActive)
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

        for (int i = 0; i < physicalLines.Count - maxLines || (string.IsNullOrWhiteSpace(physicalLines[0]) && physicalLines[0] != null); i++)
        {
            physicalLines.RemoveAt(0);
        }

        if (convo.GetConversation()[convo.PlaceInConversation] == ' ')
        {
            lastSpaceIndex = convo.PlaceInConversation;
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
            string result = line;
            if (StartNum != 0 && lastLineIndex == StartNum)
            {
                result = "..." + result;
            }
            physicalLines.Add(result);
        }
    }

    private void AddLineBreak()
    {
        physicalLines.Add("\n\n");
    }
}
