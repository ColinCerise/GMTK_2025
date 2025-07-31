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
    public GameObject Conversation;
    // Start is called before the first frame update
    void Start()
    {
        Wiretap = GameObject.Find("Wiretap");
        WiretapScript = Wiretap.GetComponent<Wiretap>();
    }

    // Update is called once per frame
    void Update()
    {
        DialogueBox.text = Conversation.GetComponent<ConversationManager>().CurrentDialog;
    }
    public void SetAsTalking(GameObject Conversationist, int startingNum)
    {
        Conversation = Conversationist;
        StartNum = startingNum;
    }
}
