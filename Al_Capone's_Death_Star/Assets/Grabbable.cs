using UnityEngine;

public class Grabbable : MonoBehaviour
{
    //private int MultiTrigger;
    public Sprite InitialSprite;
    public Sprite ActiveSprite;
    public bool MousedOver;
    public bool GrabbedLock;
    public bool MouseLockOut;
    public bool MouseDown;
    public bool MousedOverReciever;
    public bool MousedOverOutput;
    public bool LockOnNode = false;
    //public bool MousedOverOutput;
    public Vector2 mousePos;
    public Vector2 TrueMousePos;
    public Vector2 StartPos;
    public float GrabSpeed = 20;
    public int MultiTrigger = 0;
    public Rigidbody2D rb;
    public SpriteRenderer SpriteRenderer;
    public GameObject Target;
    public GameObject TargettedReciever;
    public bool SnapToReciever = false;
    public bool SnapToOutput = false;
    public bool AbleToLock = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        InitialSprite = this.SpriteRenderer.sprite;
        StartPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        CheckGrab();
        if (!GrabbedLock)
        {
            SpriteRenderer.sprite = InitialSprite;
            if (MultiTrigger == 1)
            {
                rb.velocity = Vector2.zero;
                if (SnapToReciever)
                {
                    if (MousedOverReciever)
                    {
                        LockOnNode = true;
                        transform.position = TargettedReciever.transform.position;
                    }
                    else
                    {
                        transform.position = StartPos;
                    }
                }
                if (SnapToOutput)
                {
                    if (MousedOverOutput)
                    {
                        if (AbleToLock)
                        {
                            LockOnNode = true;
                        }
                        transform.position = TargettedReciever.transform.position;
                    }
                    else
                    {
                        transform.position = StartPos;
                    }
                }
            }
            else if (!LockOnNode)
            {
                transform.position = StartPos;
            }
        }
        else
        {
            if (ActiveSprite != null)
            {
                //needs to exist
                SpriteRenderer.sprite = ActiveSprite;
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name != "Mouse")
        {
            Target = collision.gameObject;
        }
        //Debug.Log("triggered" + collision.gameObject.name);
        if (collision.gameObject.name == "Mouse")
        {
            MousedOver = true;
        }
        else if (collision.gameObject.tag == "Reciever")
        {
            MousedOverReciever = true;
            TargettedReciever = collision.gameObject;
        }
        else if (collision.gameObject.tag == "Output")
        {
            MousedOverOutput = true;
            TargettedReciever = collision.gameObject;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("triggered" + collision.gameObject.name);
        if (collision.gameObject.name != "Mouse" && collision.gameObject.name != "Wiretap")
        {
            MultiTrigger++;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Mouse")
        {
            MousedOver = false;
        }
        else if (collision.gameObject.tag == "Reciever")
        {
            MousedOverReciever = false;
            TargettedReciever = null;
        }
        else if (collision.gameObject.tag == "Output")
        {
            MousedOverOutput = false;
            TargettedReciever = null;
        }
        if (collision.gameObject.name != "Mouse" && collision.gameObject.name != "Wiretap")
        {
            MultiTrigger--;
        }
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
            //Debug.Log("Mouse Unclick");
        }
        if (MouseDown && !MousedOver)
        {
            MouseLockOut = true;
        }
        if (!MouseDown && (MouseLockOut || GrabbedLock))
        {
            MouseLockOut = false;
            GrabbedLock = false;
        }
        if ((MouseDown && MousedOver && !MouseLockOut) || GrabbedLock)
        {
            Grab();
            //Debug.Log("Tring to Move");
        }
    }
    public void Grab()
    {
        GrabbedLock = true;
        LockOnNode = false;
        mousePos = Input.mousePosition;
        TrueMousePos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 0));
        rb.velocity = new Vector2(TrueMousePos.x - transform.position.x, TrueMousePos.y - transform.position.y) * GrabSpeed;
    }
}
