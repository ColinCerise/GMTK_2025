using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwimToMouse : MonoBehaviour
{
    public Rigidbody2D rb;
    public float SwimSpeed = 1;
    public Vector2 MousePos;
    public Vector2 TrueMousePos;
    public Grabbable Grabscript;
    public float GrabStun;
    public bool GrabStunned;
    public float angle;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        Grabscript = this.GetComponent<Grabbable>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Grabscript.GrabbedLock && !GrabStunned)
        {
            
            MousePos = Input.mousePosition;
            TrueMousePos = Camera.main.ScreenToWorldPoint(new Vector3(MousePos.x, MousePos.y, 0));
            rb.velocity = new Vector2(TrueMousePos.x - transform.position.x, TrueMousePos.y - transform.position.y) * SwimSpeed;

            //Vector2 Angle = new Vector3(mousePos.x,mousePos.y, 0) - transform.position;
            //float angle = Mathf.Atan2(Angle.y, Angle.x) * Mathf.Rad2Deg;
            //angle += 1;
            /*
            Vector3 Angle = TrueMousePos - transform.position;
            float angle = Mathf.Atan2(Angle.y, Angle.x) * Mathf.Rad2Deg; // Convert to degrees

            transform.rotation = Quaternion.Euler(0, 0, angle - 90);
            transform.rotation = Quaternion.Euler(0, 0, angle);
            */
            Vector3 trueMousePos = Camera.main.ScreenToWorldPoint(new Vector3(MousePos.x, MousePos.y, Camera.main.nearClipPlane));
            Vector3 direction = trueMousePos - transform.position;
            //direction.z = 0f; // Important if you're in 2D
            // Rotate to face the direction
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            if (Mathf.Abs(direction.y) + Mathf.Abs(direction.x) > 2)
            {
                transform.rotation = Quaternion.Euler(0, 0, angle);
            }
        }
        else
        {
            if (Grabscript.GrabbedLock)
            {
                GrabStunned = true;
                GrabStun = 0;
            }
            else
            {
                GrabStun += Time.deltaTime;
                if (GrabStun >= 3)
                {
                    GrabStunned = false;
                }
            }
        }
    }
}
