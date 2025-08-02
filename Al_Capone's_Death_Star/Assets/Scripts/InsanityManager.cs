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
            AccumulatedTime += Time.deltaTime;
            if (BubblePrefab != null && AccumulatedTime >= SpawnDelay)
            {
                AccumulatedTime -= SpawnDelay;
                GameObject Bubble = Instantiate(BubblePrefab);
                Vector2 Bubbleran = new Vector2(Random.Range(-10, 10), -9);
                Bubble.transform.position = Bubbleran;
                SpriteRenderer sr = Bubble.GetComponent<SpriteRenderer>();
                Color color = sr.color;
                color = new Color(Random.value, Random.value, Random.value, Random.value);
                sr.color = color;
                float bubblescale = Random.Range(.5f, 10);
                Bubble.transform.localScale = new Vector2(bubblescale, bubblescale);
            }
            if (LoopScript.StageOfInsanity >= 2)
            {
                FishTime += Time.deltaTime;
                if (FishPrefab != null && FishTime >= FishSpawnDelay)
                {
                    FishTime -= FishSpawnDelay;
                    GameObject Fish = Instantiate(FishPrefab);
                    Vector2 Fishran = new Vector2(Random.Range(-10, 10), -9);
                    Fish.transform.position = Fishran;
                    SpriteRenderer sr = Fish.GetComponent<SpriteRenderer>();
                    Color color = sr.color;
                    color = new Color(Random.value, Random.value, Random.value, Random.value);
                    sr.color = color;
                    float Fishscale = Random.Range(.25f, 1);
                    Fish.transform.localScale = new Vector2(Fishscale, Fishscale);
                }
            }
        }
    }
}
