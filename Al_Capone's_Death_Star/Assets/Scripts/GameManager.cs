using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public SpriteRenderer SpriteRenderer;
    public Sprite HideInScene;
    public bool Victory;
    public bool Loss;
    public GameObject Conversations;
    public GameObject FadeWall;
    public bool FoundAll = false;
    public int DontBrickMe;
    public float FadeTime = 0;
    public bool FadedIn = false;
    public bool fading = false;
    public SpriteRenderer sr;
    public Color color;
    public float TimeDialation = .5f;
    public float FadeWallTime = 1.2f;
    public bool Pausewall = false;
    // Start is called before the first frame update
    void Start()
    {
        if (FadeWall != null)
        {
            sr = FadeWall.GetComponent<SpriteRenderer>();
            color = sr.color;
            color.a = 0;
            sr.color = color;
            SpriteRenderer = FadeWall.GetComponent<SpriteRenderer>();
            SpriteRenderer.sprite = HideInScene;
        }
        FadedIn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (fading)
        {
            FadeIn();
        }
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
            if (FadeWall != null)
            {
                fading = true;
            }
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
    public void FadeIn()
    {
        if (!Pausewall)
        {
            if (!FadedIn)
            {
                FadeTime += Time.deltaTime * TimeDialation;
                if (FadeTime > 1)
                {
                    FadeTime = 1;
                    FadedIn = true;
                    Pausewall = true;
                }
                color.a = FadeTime;
                sr.color = color;
            }
            else
            {
                FadeTime -= Time.deltaTime * TimeDialation;
                if (FadeTime < 0)
                {
                    FadeTime = 0;
                    FadedIn = false;
                    fading = false;
                    DontBrickMe = 0;
                }
                color.a = FadeTime;
                sr.color = color;
            }
        }
        else
        {
            if (FadedIn)
            {
                FadeTime += Time.deltaTime * TimeDialation;
                if (FadeTime >= 1 + (FadeWallTime / 2))
                {
                    FadedIn = false;
                }
            }
            else
            {
                FadeTime -= Time.deltaTime * TimeDialation;
                if (FadeTime <= 1)
                {
                    FadeTime = 1;
                    FadedIn = true;
                    Pausewall = false;
                }
            }
        }
        

    }
}
