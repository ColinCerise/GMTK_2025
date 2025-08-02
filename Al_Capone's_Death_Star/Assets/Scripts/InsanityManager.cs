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
    public float AccumulatedTime;
    public float SpawnDelay = 1;
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
                float bubblescale = Random.Range(.5f, 2);
                Bubble.transform.localScale = new Vector2(bubblescale, bubblescale);
            }
        }
    }
}
