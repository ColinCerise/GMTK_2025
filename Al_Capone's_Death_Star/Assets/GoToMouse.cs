using UnityEngine;

public class GoToMouse : MonoBehaviour
{
    public Vector3 mousePos;
    public Vector3 TrueMousePos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Input.mousePosition;
        TrueMousePos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 0));
        transform.position = new Vector2(TrueMousePos.x, TrueMousePos.y);
    }
}
