using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PnjDontMove : MonoBehaviour
{
    public void ReceiveDialogState(bool Dialog)
    {
        if (Dialog)
        {
            if (GetComponent<DialogueTrigger>() != null)
            {
                GetComponent<DialogueTrigger>().TriggerDialogue();
            }
        }
        else
        {
            GetComponent<DialogueTrigger>().ConditionDialogue();
        }
    }
}
