using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool Victory;
    public bool Loss;
    public GameObject Conversations;
    public bool FoundAll = false;
    public int DontBrickMe;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Loop()
    {
        FoundAll = false;
        DontBrickMe = 0;
        if (Victory)
        {
            //Victory Cutscene IDK?
        }
        if (Loss)
        {
            //Fade to black?
        }
        while (!FoundAll && DontBrickMe < 100)
        {
            DontBrickMe++;
            Conversations = GameObject.FindWithTag("Conversation");
            if (Conversations != null)
            {
                Conversations.tag = "Found";
                Conversations.GetComponent<ConversationManager>().Revolve();
                //FoundAll = true;
            }
            else
            {
                GameObject LineManager = GameObject.Find("Manager");
                LineManager.GetComponent<LinesAndText>().WorldRevolves();
                FoundAll = true;
            }
        }
        while (FoundAll && DontBrickMe < 100)
        {
            DontBrickMe++;
            Conversations = GameObject.FindWithTag("Found");
            if (Conversations != null)
            {
                Conversations.tag = "Conversation";
                //Conversations.GetComponent<ConversationManager>().Revolve();
            }
            else
            {
                //GameObject LineManager = GameObject.Find("Manager");
                //LineManager.GetComponent<LinesAndText>().WorldRevolves();
                FoundAll = false;
            }
        }
    }
}
