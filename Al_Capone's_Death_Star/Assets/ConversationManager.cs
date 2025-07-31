using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationManager : MonoBehaviour
{
    public string Conversation;
    public string ConversationTargets;
    public float TimeOffsetHours = 12;
    public float TimeOffsetMinutes = 10;
    public float LPS = 20;
    public int PlaceInConversation = 0;
    public char CurrentChar;
    public GameObject Wiretap;
    public Wiretap WiretapScript;
    public GameObject Manager;
    public LinesAndText ManagerScript;
    // Start is called before the first frame update
    void Start()
    {
        Wiretap = GameObject.Find("Wiretap");
        Manager = GameObject.Find("Manager");
        WiretapScript = Wiretap.GetComponent<Wiretap>();
        ManagerScript = Manager.GetComponent<LinesAndText>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ManagerScript.Hours >= TimeOffsetHours && ManagerScript.Minutes >= TimeOffsetMinutes)
        {

        }
    }
}
