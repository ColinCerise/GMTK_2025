using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public SpriteRenderer SpriteRenderer;
    public Sprite HideInScene;
    public bool Victory;
    public bool Loss;
    public bool AngerLoss;
    public int StageOfInsanity = 0;
    public GameObject Conversations;
    public GameObject FadeWall;
    public GameObject Anger;
    public AngerBar AngerBar;
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
    public float ANTIspam;
    //public bool BeforeYourEyes = true;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(SceneManager.GetActiveScene().name);
        if (!SceneManager.GetActiveScene().Equals("FakeSampleScene"))
        {
            Debug.Log("This Is Not The Sample!!!!!!!!");
            Anger = GameObject.Find("AngryBossManager");
            AngerBar = Anger.GetComponent<AngerBar>();
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
    }

    // Update is called once per frame
    void Update()
    {
        if (fading)
        {
            FadeIn();
        }
        if (ANTIspam != 0)
        {
            ANTIspam += Time.deltaTime;
            if (ANTIspam >= 11)
            {
                ANTIspam = 0;
            }
        }
    }
    public void Loop()
    {
        if (ANTIspam == 0)
        {
            ANTIspam++;
            if (Victory)
            {
                //Victory Cutscene IDK?
                TrueLoop();
            }
            if (Loss)
            {
                StageOfInsanity++;
                if (AngerBar.Anger >= 100)
                {
                    AngerLoss = true;
                }
                if (FadeWall != null)
                {
                    fading = true;
                }
            }
        }
    }
    public void TrueLoop()
    {
        FoundAll = false;
        bool BounceConnectors = false;
        DontBrickMe = 0;
        GameObject.Find("Manager").GetComponent<ConvoLog>().Revolve();
        while (!FoundAll && DontBrickMe < 1000)
        {
            DontBrickMe++;
            Conversations = GameObject.FindWithTag("Conversation");
            if (Conversations != null)
            {
                Conversations.tag = "Found";
                Conversations.GetComponent<ConversationManager>().Revolve();
            }
            else
            {
                GameObject LineManager = GameObject.Find("Manager");
                LineManager.GetComponent<LinesAndText>().WorldRevolves();
                AngerBar.RevolveBar();
                FoundAll = true;
            }
        }
        while (FoundAll && DontBrickMe < 1000)
        {
            DontBrickMe++;
            Conversations = GameObject.FindWithTag("Found");
            if (Conversations != null)
            {
                Conversations.tag = "Conversation";
            }
            else
            {
                FoundAll = false;
            }
        }
        while (!BounceConnectors && DontBrickMe < 1000)
        {
            DontBrickMe++;
            Conversations = GameObject.FindWithTag("Connector");
            if (Conversations != null)
            {
                Conversations.tag = "Found";
                Conversations.GetComponent<Grabbable>().Bounce();
            }
            else
            {
                BounceConnectors = true;
            }
        }
        while (BounceConnectors && DontBrickMe < 1000)
        {
            DontBrickMe++;
            Conversations = GameObject.FindWithTag("Found");
            if (Conversations != null)
            {
                Conversations.tag = "Connector";
            }
            else
            {
                BounceConnectors = false;
                Loss = false;
                AngerLoss = false;
                AngerBar.Anger = 0;
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
                    TrueLoop();
                }
            }
        }
        

    }

    public void ShiftScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
