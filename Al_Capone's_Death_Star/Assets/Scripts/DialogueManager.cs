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
    // Start is called before the first frame update
    void Start()
    {
        Wiretap = GameObject.Find("Wiretap");
        WiretapScript = Wiretap.GetComponent<Wiretap>();
        PlayerReciever = GameObject.Find("PlayerReciever");
        PlayerRecieverScript = PlayerReciever.GetComponent<OutputJack>();
    }

    // Update is called once per frame
    void Update()
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
            //DialogueBox.text = Conversation.GetComponent<ConversationManager>().CurrentDialog;
            DialogueBox.text = Conversation.GetComponent<ConversationManager>().CurrentLine();
            DialogueTimer = 0;
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
        if (Conversation != null && Conversation.GetComponent<ConversationManager>() && !PreconversationOverride)
        {
            DialogueBox.text = Conversation.GetComponent<ConversationManager>().CurrentDialog;
        }
        DialogueTimer = 0;
    }
}
