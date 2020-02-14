using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{

    public Text nameText;
    public Text dialogueText;
    [SerializeField] private Animator animator = null;
    private bool OnDialog;
    private GameObject Player = null;
    private GameObject CameraJoueur = null;
    private GameObject pnjTarget = null;

    private Queue<string> sentences;

    void Start()
    {
        sentences = new Queue<string>();
        Player = GameObject.FindWithTag("player");
        CameraJoueur = GameObject.FindWithTag("MainCamera");
        OnDialog = false;
    }

    public void StartDialogue(Dialogue dialogue,GameObject PNJ)
    {
        animator.SetBool("IsOpen", true);
        OnDialog = true;
        pnjTarget = PNJ;
        CameraJoueur.GetComponent<MouseLook>().enabled = !OnDialog;
        Player.SendMessage("ReceiveDialogState", OnDialog);
        nameText.text = dialogue.name;

        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
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

    IEnumerator TypeSentence(string sentence)
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
        Player.SendMessage("ReceiveDialogState", OnDialog);
        CameraJoueur.GetComponent<MouseLook>().enabled = !OnDialog;
        pnjTarget.SendMessage("ReceiveDialogState", OnDialog);
    }


}
