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
    public bool NoSnapping = false;
    public bool DeleteAtBorder = false;
    private AudioManager audioManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        InitialSprite = this.SpriteRenderer.sprite;
        StartPos = transform.position;
        //this.GetComponent<Wiretap>
        audioManager = GameObject.Find("Manager").GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckGrab();
        if (this.GetComponent<Wiretap>() != null)
        {
            if (this.GetComponent<Wiretap>().Conversation != null)
            {
                LockOnNode = true;
            }
            else
            {
                LockOnNode = false;
            }
        }
        if (!GrabbedLock)
        {
            if (!NoSnapping)
            {
                SpriteRenderer.sprite = InitialSprite;
                if (MultiTrigger == 1 || MultiTrigger == 0)
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
            else if (transform.position.y >= 20 || transform.position.y <= -10)
            {
                if (DeleteAtBorder)
                {
                    Destroy(this.gameObject);
                }
                else
                {
                    transform.position = StartPos;
                }
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
        else if (collision.gameObject.tag == "Reciever" && SnapToReciever)
        {
            MousedOverReciever = true;
            TargettedReciever = collision.gameObject;
        }
        else if (collision.gameObject.tag == "Output" && SnapToOutput)
        {
            MousedOverOutput = true;
            TargettedReciever = collision.gameObject;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("triggered" + collision.gameObject.name);
        if (collision.gameObject.name != "Mouse" && collision.gameObject.name != "Wiretap" && collision.gameObject.tag != "UselessObj")
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
            audioManager.PlaySoundEffect("wireConnect");
        }
        else if (collision.gameObject.tag == "Output")
        {
            MousedOverOutput = false;
            TargettedReciever = null;

            audioManager.PlaySoundEffect("grabWire");
        }
        if (collision.gameObject.name != "Mouse" && collision.gameObject.name != "Wiretap" && collision.gameObject.tag != "UselessObj")
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
    public void Bounce()
    {
        transform.position = StartPos;
        MousedOverReciever = false;
        MousedOverOutput = false;
        LockOnNode = false;
    }
}
