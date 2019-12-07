using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectLoot : MonoBehaviour
{
	private GameObject player;
	public int id,Nombre;
	public bool Special,Material,OnStartRandomNombre;
	public AudioClip takeSound;
	public int MinR,MaxR;
	public Vector3 spawnAddPos;
	[HideInInspector]
	public bool noSend;	
	private bool NotWait;
	private Collider collide;
	
    void Start()
    {
        player = GameObject.FindWithTag("player");
		if(OnStartRandomNombre){Nombre = Random.Range(MinR,MaxR);}
		RaycastHit hitDown;
		collide = gameObject.GetComponent<Collider>();
		if(Physics.Raycast(transform.position, Vector3.down, out hitDown))
		{
			if(!(hitDown.distance > 50) && hitDown.transform.tag != "Enemie")
			{
				transform.position = hitDown.point + spawnAddPos;
				if(!NotWait){StartCoroutine(WaitSpawn());}
				transform.eulerAngles = new Vector3(transform.eulerAngles.x,Random.Range(0f,360f),transform.eulerAngles.z);
			}
			else
			{
				noSend = false;
			}
		}
		
    }
	public void SendInfoObjectLoot()
	{
		if(!noSend)
		{
			player.SendMessage("ReceiveSendMessageInfoisSpecial",Special);
			player.SendMessage("ReceiveSendMessageInfoisMaterial",Material);
			player.SendMessage("ReceiveSendMessageInfoAudioClip",takeSound);
			player.SendMessage("ReceiveSendMessageInfoID",id);
			player.SendMessage("ReceiveSendMessageInfoNombre",Nombre);
		}
	}
	public void NotDisable()
	{
		NotWait = true;
	}
	
	public void ModificationNombre(int NB)
	{
		Nombre = NB;	
	}
	IEnumerator WaitSpawn()
	{
		noSend = true;
		collide.enabled = false;
		yield return new WaitForSeconds(3f);
		collide.enabled = true;
		noSend = false;
	}
}
