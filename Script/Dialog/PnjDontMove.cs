using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PnjDontMove : MonoBehaviour
{
	public void ReceiveDialogState(bool Dialog){
		if(Dialog){
			if(gameObject.transform.GetComponent<DialogueTrigger>() != null){
				gameObject.transform.GetComponent<DialogueTrigger>().TriggerDialogue();	
			}
		}else{
			gameObject.transform.GetComponent<DialogueTrigger>().ObjectSpawnDialogue();
		}
	}
}
