using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InsanityManager : MonoBehaviour
{
    public GameObject LoopMan;
    public GameManager LoopScript;
    public GameObject BubblePrefab;
    public GameObject FishPrefab;
    public float AccumulatedTime;
    public float FishTime = 5;
    public float SpawnDelay = 1;
    public float FishSpawnDelay = 9;
    public int TrackedInsanity;
    // Start is called before the first frame update
    void Start()
    {
        LoopMan = GameObject.Find("LoopManager");
        LoopScript = LoopMan.GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (LoopScript.StageOfInsanity >= 1)
        {
            if (LoopScript.StageOfInsanity != TrackedInsanity)
            {
                TrackedInsanity = LoopScript.StageOfInsanity;
                switch(TrackedInsanity)
                {
                    case 1:
                        SpawnDelay = 2;
                        break;
                        case 2:
                        SpawnDelay = 1;
                        FishSpawnDelay = 20;
                        break;
                        case 3:
                        SpawnDelay = .75f;
                        FishSpawnDelay = 15;
                        break;
                        case 4:
                        FishSpawnDelay = 10;
                        break;
                    case 5:
                        break;
                        default:
                        TrackedInsanity = 5;
                        SpawnDelay = .75f;
                        FishSpawnDelay = 10;
                        break;

                }
            }
            AccumulatedTime += Time.deltaTime;
            if (BubblePrefab != null && AccumulatedTime >= SpawnDelay)
            {
                AccumulatedTime -= SpawnDelay;
                GameObject Bubble = Instantiate(BubblePrefab);
                Vector2 Bubbleran = new Vector2(Random.Range(-10, 10), -9);
                Bubble.transform.position = Bubbleran;
                SpriteRenderer sr = Bubble.GetComponent<SpriteRenderer>();
                Color color = sr.color;
                color = new Color(255, 255, 255, Random.value);
                sr.color = color;
                float bubblescale = Random.Range(0, 1) + (TrackedInsanity / 2);
                Bubble.transform.localScale = new Vector2(bubblescale, bubblescale);
            }
            if (TrackedInsanity >= 2)
            {
                FishTime += Time.deltaTime;
                if (FishPrefab != null && FishTime >= FishSpawnDelay)
                {
                    //FishSpawnDelay = 20;
                    FishTime -= FishSpawnDelay;
                    GameObject Fish = Instantiate(FishPrefab);
                    Vector2 Fishran = new Vector2(Random.Range(-10, 10), -9);
                    Fish.transform.position = Fishran;
                    SpriteRenderer sr = Fish.GetComponent<SpriteRenderer>();
                    Color color = sr.color;
                    color = new Color(Random.value, Random.value, Random.value, Random.Range(70,130));
                    sr.color = color;
                    float Fishscale = Random.Range(5, 10);
                    Fish.transform.localScale = new Vector2(Fishscale, Fishscale);
                }
            }
        }
    }
}
