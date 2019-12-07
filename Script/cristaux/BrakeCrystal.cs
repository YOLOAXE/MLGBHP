using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrakeCrystal : MonoBehaviour
{
	//public Transform effet;
	//private AudioSource m_AudioSource;
	//public AudioClip SondBrake;
	public GameObject[] crystaux;
	public float Vie;
	
	//void Start(){m_AudioSource.GetComponent<AudioSource>();}
	
	public void ReceiveDamage(float Damage){
		if(Vie <= 0){
			for(int i = 0;i < crystaux.Length; i++){
				crystaux[i].GetComponent<Rigidbody>().isKinematic = false;
				crystaux[i].transform.SetParent(null);
				crystaux[i].SendMessage("detach");
			}
			Destroy(gameObject);
		}else{
			Vie -= Damage;
			//m_AudioSource.clip = SondBrake;
			//m_AudioSource.PlayOneShot(m_AudioSource.clip);	
			//si ta envie de rajouter un effet de particule a chaque fois quil prend des degat 
			//Instantiate(effet,transform.position,Quaternion.identity);
		}
	}
}
