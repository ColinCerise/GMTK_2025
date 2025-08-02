using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutputJack : MonoBehaviour
{
    public GameObject Connector;
    public Grabbable ConnectorScript;
    public GameObject ConnectedLight;
    public Sprite InitialSprite;
    public Sprite ActiveSprite;
    public Sprite InitialLight;
    public Sprite ActiveLight;
    public SpriteRenderer SpriteRenderer;
    public bool isActive = false;
    public bool LightActive = false;
    public bool IsReciever;
    public int MultiTrigger = 0;
    public GameObject PendingConversation;
    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        InitialSprite = this.SpriteRenderer.sprite;
        if (!IsReciever)
        {
            Connector = GameObject.Find(this.gameObject.name.Substring(0, (this.gameObject.name.Length - 1)) + "2");
            ConnectedLight = GameObject.Find(this.gameObject.name.Substring(0, (this.gameObject.name.Length - 1)) + "3");
            InitialLight = ConnectedLight.GetComponent<SpriteRenderer>().sprite;
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
            if (LightActive)
            {
                ConnectedLight.GetComponent<SpriteRenderer>().sprite = ActiveLight;
            }
            else
            {
                ConnectedLight.GetComponent<SpriteRenderer>().sprite = InitialLight;
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
        if (collision.gameObject.name != "Mouse" && collision.gameObject.name != "Wiretap" && collision.gameObject.tag != "UselessObj")
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
        if (collision.gameObject.name != "Mouse" && collision.gameObject.name != "Wiretap" && collision.gameObject.tag != "UselessObj")
        {
            MultiTrigger--;
            if (IsReciever && MultiTrigger == 0)
            {
                Connector = null;
                ConnectorScript = null;
            }
        }
    }
    public void SetLightActive(bool active, GameObject Conversator)
    {
        LightActive = active;
        if (active)
        {
            PendingConversation = Conversator;
        }
        else
        {
            PendingConversation = null;
        }
    }
}
