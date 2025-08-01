using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Wiretap : MonoBehaviour
{
    public GameObject Target;

    public bool OverOutput;
    public GameObject TargettedOutput;
    public GameObject ConnectedPoint;
    public GameObject ConnectedReciever;
    public string Conversation;

    private string previousConvo;
    private AudioManager audioManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioManager = GameObject.Find("Manager").GetComponent<AudioManager>();
        previousConvo = Conversation;
    }

    // Update is called once per frame
    void Update()
    {
        if (OverOutput && !this.gameObject.GetComponent<Grabbable>().GrabbedLock)
        {
            ConnectedPoint = GameObject.Find(TargettedOutput.name.Substring(0, (TargettedOutput.name.Length - 1)) + "2");
            ConnectedReciever = ConnectedPoint.GetComponent<Grabbable>().TargettedReciever;
            if (ConnectedReciever != null && TargettedOutput != null)
            {
                Conversation = TargettedOutput.name + " + " + ConnectedReciever.name;
                if (Conversation != previousConvo)
                {
                    audioManager.PlaySFX("tappingWires");
                }
                previousConvo = Conversation;
            }
            else
            {
                Conversation = null;
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name != "Mouse")
        {
            Target = collision.gameObject;
        }
        if (collision.gameObject.tag == "Output")
        {
            OverOutput = true;
            TargettedOutput = collision.gameObject;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Output")
        {
            OverOutput = false;
            TargettedOutput = null;
            ConnectedPoint = null;
            ConnectedReciever = null;
            Conversation = null;
        }
    }

}
