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
    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        InitialSprite = this.SpriteRenderer.sprite;
        if (!IsReciever)
        {
            Connector = GameObject.Find(this.gameObject.name.Substring(0, (this.gameObject.name.Length - 1)) + "2");
        }
        else
        {
            Connector = GameObject.Find(this.gameObject.name.Substring(0, (this.gameObject.name.Length - 8)) + "2");
        }
        ConnectorScript = Connector.GetComponent<Grabbable>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ConnectorScript.GrabbedLock || ConnectorScript.MousedOverReciever)
        {
            isActive = true;
        }
        else
        {
            isActive = false;
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
}
