using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    public bool Failed = false;
    public int CurrentInput = 0;
    public int CorrectInput = 1;
    public int Stage = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch(Stage)
        {
            case 1:
                CorrectInput = 1;
                if (CurrentInput != 0)
                {
                    CheckAnswer();
                }
                break;
        }
    }
    public void CheckAnswer()
    {
        if (CurrentInput == CorrectInput)
        {
            Stage++;
        }
        else
        {
            Failed = true;
            Stage = 0;
        }
    }
    public void Input(int input)
    {
        CurrentInput = input;
    }
}
