using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    public Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(mainCamera.pixelWidth);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
