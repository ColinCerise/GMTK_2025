using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutputJack : MonoBehaviour
{
    public GameObject Connector;
    public Grabbable ConnectorScript;
    public Sprite InitialSprite;
    public Sprite ActiveSprite;
    public SpriteRenderer SpriteRenderer;
    public bool isActive = false;
    public bool IsReciever;
    public int MultiTrigger = 0;
    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        InitialSprite = this.SpriteRenderer.sprite;
        if (!IsReciever)
        {
            Connector = GameObject.Find(this.gameObject.name.Substring(0, (this.gameObject.name.Length - 1)) + "2");
        }
        /*
        else
        {
            Connector = GameObject.Find(this.gameObject.name.Substring(0, (this.gameObject.name.Length - 8)) + "2");
        }
        */
        if (Connector != null)
        {
            ConnectorScript = Connector.GetComponent<Grabbable>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsReciever)
        {
            if (ConnectorScript.GrabbedLock || ConnectorScript.MousedOverReciever)
            {
                isActive = true;
            }
            else
            {
                isActive = false;
            }
        }
        else
        {
            if (ConnectorScript != null && !ConnectorScript.GrabbedLock)
            {
                isActive = true;
            }
            else
            {
                isActive = false;
            }
        }
        if (isActive)
        {
            SpriteRenderer.sprite = ActiveSprite;
        }
        else
        {
            SpriteRenderer.sprite = InitialSprite;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("triggered" + collision.gameObject.name);
        if (collision.gameObject.name != "Mouse" && collision.gameObject.name != "Wiretap")
        {
            MultiTrigger++;
            if (IsReciever && MultiTrigger == 1)
            {
                Connector = collision.gameObject;
                if (Connector.GetComponent<Grabbable>() != null)
                {
                    ConnectorScript = Connector.GetComponent<Grabbable>();
                }
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name != "Mouse" && collision.gameObject.name != "Wiretap")
        {
            MultiTrigger--;
            if (IsReciever && MultiTrigger == 0)
            {
                Connector = null;
                ConnectorScript = null;
            }
        }
    }
}
