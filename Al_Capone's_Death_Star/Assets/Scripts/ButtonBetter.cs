using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ButtonBetter : MonoBehaviour
{
    public GameObject BossMan;
    public BossManager BossScript;
    public int ButtonNum = 0;
    public bool MouseDown;
    public bool MousedOver;
    public bool MouseLockOut;
    public Sprite InitialSprite;
    public SpriteRenderer SpriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        BossMan = GameObject.Find("BossManager");
        BossScript = BossMan.GetComponent<BossManager>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        InitialSprite = this.SpriteRenderer.sprite;
    }

    // Update is called once per frame
    void Update()
    {
        CheckGrab();
        /*
        if (BossScript.Stage == 0 )
        {
            SpriteRenderer.sprite = null;
            this.GetComponent<BoxCollider2D>().enabled = false;
        }
        else
        {
            SpriteRenderer.sprite = InitialSprite;
            this.GetComponent<BoxCollider2D>().enabled = true;
        }
        */
    }
    public void CheckGrab()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("Mouse click");
            MouseDown = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            MouseDown = false;
            MouseLockOut = false;
            //Debug.Log("Mouse Unclick");
        }
        if (MouseDown && !MousedOver)
        {
            MouseLockOut = true;
        }
        if (MouseDown && MousedOver && !MouseLockOut)
        {
            Grab();
            //Debug.Log("Tring to Move");
        }
    }
    public void Grab()
    {
        BossScript.Input(ButtonNum);
        MouseLockOut = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Mouse")
        {
            MousedOver = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    { 
        if (collision.gameObject.name == "Mouse")
        {
            MousedOver = true;
        }
    }
}
