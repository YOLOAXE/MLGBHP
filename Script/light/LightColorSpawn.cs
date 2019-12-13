using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightColorSpawn : MonoBehaviour
{
    public int colorBoule;
	public Transform[] BouleSpawn;
	public Texture[] ImageBoule;
	private Transform AsingneDstroy = null;
	private bool aSpawn = false,OnStartNoEnable = true,OnDestroy = false;
	private GameObject ImageARenplace = null;
	private AudioSource m_Audio = null;
	public AudioClip Spawn,DeSpawn;
		
	void Start(){
		ImageARenplace = GameObject.Find("ImageBouleLight_sq");
		m_Audio = gameObject.GetComponent<AudioSource>();
		OnStartNoEnable = false;
		gameObject.GetComponent<LightColorSpawn>().enabled = false;
	}	
	void OnEnable(){
		if(!OnStartNoEnable){
			SetImageBoule();
		}
    }
    void Update()
    {
		if(OnDestroy){
			float DistanceLight = Vector3.Distance(AsingneDstroy.position,transform.position);
			float step =  (10 * (DistanceLight * 0.5f)) * Time.deltaTime;
			AsingneDstroy.position = Vector3.MoveTowards(AsingneDstroy.position, transform.position, step);
			if(DistanceLight <= 1f){
				Destroy(AsingneDstroy.gameObject);
				PlayOneShotSound(DeSpawn);
				OnDestroy = false;
			}
		}
		if(Input.GetButtonDown("Lumierre") && !OnDestroy){aSpawn = !aSpawn;}else{return;}
		if(aSpawn){
			AsingneDstroy = Instantiate(BouleSpawn[colorBoule],transform.position + new Vector3(0,2,0),Quaternion.Euler(0,0,0));
			SetImageBoule();
			PlayOneShotSound(Spawn);
		}else{
			OnDestroy = true;
			AsingneDstroy.GetComponent<LumierrePlayer>().enabled = false;
			AsingneDstroy.GetComponent<Rigidbody>().velocity = Vector3.zero; 
		}
    }
	void SetImageBoule(){
		ImageARenplace.GetComponent<UnityEngine.UI.RawImage>().texture = ImageBoule[colorBoule];
	}
	void PlayOneShotSound(AudioClip audioSound){
		m_Audio.clip = audioSound;m_Audio.PlayOneShot(m_Audio.clip);
	}
}
