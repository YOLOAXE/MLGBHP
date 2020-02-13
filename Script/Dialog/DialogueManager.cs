using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

	public Text nameText;
	public Text dialogueText;
	public Animator animator;
	private bool OnDialog;
	private GameObject Player,CameraJoueur;
	private GameObject[] PNJ;
	private GameObject[] Soldat;

	private Queue<string> sentences;

	void Start ()
    {
		sentences = new Queue<string>();
		OnDialog = false;
		Player = GameObject.FindWithTag("player");
		PNJ = GameObject.FindGameObjectsWithTag("PNJ");
		Soldat = GameObject.FindGameObjectsWithTag("Soldat");
		CameraJoueur = GameObject.FindWithTag("MainCamera");
	}

	public void StartDialogue (Dialogue dialogue)
	{
		animator.SetBool("IsOpen", true);
		OnDialog = true;
		CameraJoueur.GetComponent<MouseLook>().enabled = !OnDialog;
		Player.SendMessage("ReceiveDialogState",OnDialog);
		nameText.text = dialogue.name;

		sentences.Clear();

		foreach (string sentence in dialogue.sentences)
		{
			sentences.Enqueue(sentence);
		}

		DisplayNextSentence();
	}

	public void DisplayNextSentence ()
	{
		if (sentences.Count == 0)
		{
			EndDialogue();
			return;
		}

		string sentence = sentences.Dequeue();
		StopAllCoroutines();
		StartCoroutine(TypeSentence(sentence));
	}

	IEnumerator TypeSentence (string sentence)
	{
		dialogueText.text = "";
		foreach (char letter in sentence.ToCharArray())
		{
			dialogueText.text += letter;
			yield return null;
		}
	}

	void EndDialogue()
	{
		animator.SetBool("IsOpen", false);
		OnDialog = false;
		Player.SendMessage("ReceiveDialogState",OnDialog);
		CameraJoueur.GetComponent<MouseLook>().enabled = !OnDialog;
		for(int i = 0;i<PNJ.Length;i++){
			if(PNJ[i].GetComponent<DialogueTrigger>() != null){
				PNJ[i].SendMessage("ReceiveDialogState",OnDialog);
			}
		}
		for(int i = 0;i<Soldat.Length;i++){
			if(Soldat[i].GetComponent<DialogueTrigger>() != null){
				Soldat[i].SendMessage("ReceiveDialogState",OnDialog);
			}
		}
	}
	
	
}
