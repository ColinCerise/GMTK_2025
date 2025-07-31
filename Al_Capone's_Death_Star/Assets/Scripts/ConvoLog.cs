using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvoLog : MonoBehaviour
{
    [SerializeField] List<string> conversationsLogged = new List<string>();
    [SerializeField] 

    public void AddConvo(string convo)
    {
        conversationsLogged.Add(convo);
    }
}
