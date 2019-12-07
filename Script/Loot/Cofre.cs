using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cofre : MonoBehaviour
{
    // Start is called before the first frame update
	public Transform[] Object;
	public bool IsOpen = false,hasLoot = false,needKey;
	public Animator m_Animator;
	public AudioClip OpenSound,CloseSound,KeyOpen,KeyClose;
	private AudioSource m_AudioSource;
	
	void Start()
	{
		m_Animator = GetComponent<Animator>();
		m_AudioSource = GetComponent<AudioSource>();
	}
	
	public void Open()
	{
		if(needKey)
		{
			GameObject Player = GameObject.FindWithTag("player");
			if(!Player.GetComponent<inventaire>().UseItemIdExtern(42))
			{
				return;
			}
		}
		IsOpen = !IsOpen;
		m_Animator.SetBool("isOpen",IsOpen);
		if(!hasLoot){StartCoroutine(Spawn());hasLoot = true;}
		if(IsOpen){m_AudioSource.clip = OpenSound;}else{m_AudioSource.clip = CloseSound;}
		m_AudioSource.PlayOneShot(m_AudioSource.clip);
	}
	
	IEnumerator Spawn()
	{
		GameObject Player = GameObject.FindWithTag("player");
		yield return new WaitForSeconds(0.8f);
		if(Object.Length != 0)
		{
			Transform r = Instantiate(Object[Random.Range(0,Object.Length)],transform.position - (transform.position - Player.transform.position)/2,Quaternion.identity);
			if(r.GetComponent<ObjectLoot>())
			{
				r.SendMessage("NotDisable");
			}
		}
	}
}
