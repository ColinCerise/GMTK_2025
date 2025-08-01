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
        lastSpaceIndex = 0;
        lastLineIndex = 0;
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
            ConversationManager convo = Conversation.GetComponent<ConversationManager>();

            tempCount = DialogueBox.textInfo.lineCount;
            DisplayLines(convo);
            DialogueTimer = 0;
            
            if (convo.Conversation[convo.PlaceInConversation] == ' ')
            {
                lastSpaceIndex = convo.PlaceInConversation;
            }
            else if (convo.Conversation[convo.PlaceInConversation] == '\n')
            {

            }

            DialogueBox.ForceMeshUpdate();
            if (DialogueBox.textInfo.lineCount > tempCount)
            {
                string lineSection = convo.Conversation.Substring(lastLineIndex, lastSpaceIndex - lastLineIndex);
                lastLineIndex = lastSpaceIndex;
                physicalLines.Add(lineSection);

                if (DialogueBox.textInfo.lineCount >= maxLines)
                {
                    physicalLines.RemoveAt(0);
                }
            }
        }
        else
        {
            DialogueTimer += Time.deltaTime;
            if (DialogueTimer >= 3)
            {
                DialogueBox.text = null;
                DialogueTimer = 0;
            }
        }
    }
    public void SetAsTalking(GameObject Conversationist, int startingNum)
    {
        Conversation = Conversationist;
        physicalLines.Clear();
        lastSpaceIndex = 0;
        lastLineIndex = 0;
        tempCount = 0;

        if (startingNum < 10)
        {
            StartNum = 0;
        }
        else
        {
            StartNum = startingNum - 10;
        }
    }
    public void ForceUpdate()
    {
        /*if (Conversation != null && Conversation.GetComponent<ConversationManager>() && !PreconversationOverride)
        {
            DialogueBox.text = Conversation.GetComponent<ConversationManager>().CurrentDialog;
        }*/
        DialogueTimer = 0;
    }

    private void DisplayLines(ConversationManager convo)
    {
        string fullDialogue = "";
        foreach (string str in physicalLines)
        {
            fullDialogue += str;
        }
        fullDialogue += convo.Conversation.Substring(lastLineIndex, convo.PlaceInConversation - lastLineIndex);

        DialogueBox.text = fullDialogue;
    }

    /*public string CurrentLine(ConversationManager convo)
    {
        int charNum = convo.PlaceInConversation;
        int lineIndex = -1;
        int remainder = charNum;
        int elapsedChars = 0;
        if (charNum < convo.totalCharLength)
        {
            for (int i = 0, count = charNum; count > 0; i++)
            {
                lineIndex = i;
                remainder = count;
                count -= convo.dialogueList[i].Length;
            }

            elapsedChars = charNum - remainder;
        }

        if (lineIndex >= 0 && lineIndex < convo.dialogueList.Count)
        {
            if (StartNum > elapsedChars)
            {
                int frontIndex = StartNum - elapsedChars;
                return convo.speakerList[lineIndex] + ": ..." + convo.dialogueList[lineIndex].Substring(frontIndex, remainder - frontIndex);
            }
            else
            {
                return convo.speakerList[lineIndex] + ": " + convo.dialogueList[lineIndex].Substring(0, remainder);
            }
        }
        else
        {
            return string.Empty;
        }
    }*/
}
