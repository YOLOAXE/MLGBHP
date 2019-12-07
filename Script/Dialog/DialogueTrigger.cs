using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour {

	public Dialogue dialogue,dialogueApresGiveObject;
	public Transform ObjectSpawn;
	private bool GiveObject = false;

	public void TriggerDialogue()
	{
		if(GiveObject)
		{
			FindObjectOfType<DialogueManager>().StartDialogue(dialogueApresGiveObject);
		}
		else
		{
			FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
		}
	}
	public void ObjectSpawnDialogue()
	{
		if(ObjectSpawn != null && !GiveObject)
		{
			GameObject Joueur = GameObject.FindWithTag("player");
			GiveObject = true;
			Instantiate(ObjectSpawn,Joueur.transform.position,Quaternion.identity).SendMessage("NotDisable");
		}
	}
}
