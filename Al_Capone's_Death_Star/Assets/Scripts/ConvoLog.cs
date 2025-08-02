using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ConvoLog : MonoBehaviour
{
    [SerializeField] List<string> conversationsLogged = new List<string>();
    [SerializeField] GameObject logbookDisplay;
    private bool noEntries;
    [SerializeField] string defaultText = "No entries available (listen in on conversations and they will be recorded here for your reference).";
    private TextMeshProUGUI currLog;
    private int logCounter;

    private void Start()
    {
        logbookDisplay.SetActive(false);
        currLog = logbookDisplay.GetComponentInChildren<TextMeshProUGUI>();
        conversationsLogged.Clear();
        conversationsLogged.Add(defaultText);
        noEntries = true;
    }

    public void AddConvo(string convo)
    {
        if (!string.IsNullOrEmpty(convo))
        {
            if (noEntries)
            {
                noEntries = false;
                conversationsLogged.Clear();
            }
            conversationsLogged.Add(convo);
        }
    }

    public void LogbookDisplay()
    {
        logbookDisplay.SetActive(!logbookDisplay.activeSelf);

        currLog.text = conversationsLogged[0];
        logCounter = 0;
    }

    public void ProgressLogbook()
    {
        if (logbookDisplay.activeSelf && logCounter < conversationsLogged.Count - 1)
        {
            logCounter++;
            currLog.text = conversationsLogged[logCounter];
        }
    }

    public void AntiProgressLogbook()
    {
        if (logbookDisplay.activeSelf && logCounter > 0)
        {
            logCounter--;
            currLog.text = conversationsLogged[logCounter];
        }
    }
}