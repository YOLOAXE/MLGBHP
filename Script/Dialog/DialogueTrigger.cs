using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DtriggerCondition
{
    public bool giveObject = false;
    public bool quete = false;
}

[System.Serializable]
public class DT
{
    public Dialogue dialogue = new Dialogue();
    public DtriggerCondition cond = new DtriggerCondition();
    public QueteScript scriptQuete;
    public Transform ObjectSpawn;
}

public class DialogueTrigger : MonoBehaviour
{
    [Header("Dialogue")]
    [SerializeField] private DT[] dt = new DT[1];
    [SerializeField] private int indiceDialogue = 0;
    [Header("Sound")]
    [SerializeField] private AudioSource bAudioSource = null;
    [SerializeField] private AudioClip[] aC = new AudioClip[1]; 

    void Start()
    {
        /*Recupere les Infos (indiceDialogue) avec Get Players pref*/
    }

    public void TriggerDialogue()
	{
        PlaySound();
        FindObjectOfType<DialogueManager>().StartDialogue(dt[indiceDialogue].dialogue,gameObject);
	}

    public void ConditionDialogue()
	{
        bool plus = false;

        if (dt[indiceDialogue].cond.quete)
        {
            if(!dt[indiceDialogue].scriptQuete.success)
            {
                return;
            }
            plus = true;
        }

        if (dt[indiceDialogue].ObjectSpawn != null && dt[indiceDialogue].cond.giveObject)
		{
			Instantiate(dt[indiceDialogue].ObjectSpawn, GameObject.FindWithTag("player").transform.position,Quaternion.identity).SendMessage("NotDisable");
            plus = true;
        }
        if(plus){indiceDialogue++;}
    }

    void PlaySound()
    {
        int rd = Random.Range(0, aC.Length);
        if (aC[rd] != null)
        {
            bAudioSource.clip = aC[rd];
            bAudioSource.PlayOneShot(bAudioSource.clip);
        }
    }
}
