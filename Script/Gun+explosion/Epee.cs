using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Epee : MonoBehaviour {
	
	public int IDArme;
	private int munitionMax = 32;
	private int munition = 32;
	private int munitionChargeur = 10;
	public float Portee;
	public float DgatSend;
	private bool AimAtack;
	public float Tempstire,TempTireSond;
	public float TempstireVar;
	public int TypeDeRechargement;
	private bool jeComprendPas;
	public float hitForce;
	private bool CoupParCoup;
	public Transform ImpacteSang,ImpacteSangbleu,Impactepierre,ImpacteSangvert,ImpacteAdaptation;
	public AudioClip SondAttaque1;
	public AudioClip SondAttaque2;
	public AudioClip SondAttaqueAim;
	private Transform Reasigneparent;
	public float AudioclipPlayer;
	private bool Ondialog,OnInventaire;
	private AudioSource m_AudioSource;
	private GameObject player;

    void Start ()
    {
		m_AudioSource = GetComponent<AudioSource>();
		AudioclipPlayer = -2;
		player = GameObject.FindWithTag("player");
		player.SendMessage("ReceiveMunition",munition);
		player.SendMessage("ReceiveMunitionChargeur",munitionChargeur);
		player.SendMessage("ReceiveMunitionMax",munitionMax);
		player.SendMessage("ReceiveCanreload",false);
		player.SendMessage("ReceiveTypeDeRechargement",TypeDeRechargement);
		player.SendMessage("ReceiveCoupParCoup",CoupParCoup);
		TempstireVar = Tempstire;
		AimAtack = false;
    }
	
	void Update () {
	if(!Ondialog && !OnInventaire){
		if(TempstireVar >= 0){TempstireVar -= Time.deltaTime;}
		if (Input.GetButtonDown("Fire1") && TempstireVar < 0){
			if(Input.GetButton("Fire2")){AimAtack = true;player.SendMessage("ReceiveAimAtackEpee",AimAtack);TempstireVar = Tempstire * 1.6f;}else{TempstireVar = Tempstire;}
			AudioclipPlayer = TempTireSond;
			player.SendMessage("ReceiveShoot",true);
		}
	}
	if(AudioclipPlayer > 0){AudioclipPlayer -= Time.deltaTime;}
		if(AudioclipPlayer > -1 && AudioclipPlayer <=0){
		int RandomAnime;
		RandomAnime = Random.Range(0,2);
		if(AimAtack){
		AimAtack = false;
		StartCoroutine(CouroutineAimAtack());
		m_AudioSource.clip = SondAttaqueAim;m_AudioSource.PlayOneShot(m_AudioSource.clip);	
		}else{
		if(RandomAnime == 1){m_AudioSource.clip = SondAttaque1;m_AudioSource.PlayOneShot(m_AudioSource.clip);}else{m_AudioSource.clip = SondAttaque1;m_AudioSource.PlayOneShot(m_AudioSource.clip);}
		}
		AudioclipPlayer = -2;
		AttaqueFunc();
	}
	
	}
	IEnumerator CouroutineAimAtack(){
	yield return new WaitForSeconds(0.2f);	
	AttaqueFunc();
	yield return new WaitForSeconds(0.2f);
	AttaqueFunc();
	yield return new WaitForSeconds(0.2f);
	AttaqueFunc();
	}
	void AttaqueFunc(){
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out hit)){
			if (hit.rigidbody != null){
				hit.rigidbody.AddForce(ray.direction * hitForce);	
			}
			if(hit.distance <= Portee){
				if(hit.transform.tag == "Enemie"){hit.collider.gameObject.SendMessage("ReceiveDamagePlayer",DgatSend);Reasigneparent = Instantiate(ImpacteSang,hit.point,Quaternion.FromToRotation(Vector3.forward, hit.normal));Reasigneparent.SetParent(hit.transform);}
				if(hit.transform.tag == "Crystal"){hit.collider.gameObject.SendMessage("ReceiveDamagePlayer",DgatSend);Reasigneparent = Instantiate(ImpacteAdaptation,hit.point,Quaternion.FromToRotation(Vector3.forward, hit.normal));Reasigneparent.SetParent(hit.transform);}
				if(hit.transform.tag == "EnemiePierre"){hit.collider.gameObject.SendMessage("ReceiveDamagePlayer",DgatSend);Reasigneparent = Instantiate(Impactepierre,hit.point,Quaternion.FromToRotation(Vector3.forward, hit.normal));Reasigneparent.SetParent(hit.transform);}
				if(hit.transform.tag == "EnemieBleu"){hit.collider.gameObject.SendMessage("ReceiveDamagePlayer",DgatSend);Reasigneparent = Instantiate(ImpacteSangbleu,hit.point,Quaternion.FromToRotation(Vector3.forward, hit.normal));Reasigneparent.SetParent(hit.transform);}
				if(hit.transform.tag == "EnemieVert"){hit.collider.gameObject.SendMessage("ReceiveDamagePlayer",DgatSend);Reasigneparent = Instantiate(ImpacteSangvert,hit.point,Quaternion.FromToRotation(Vector3.forward, hit.normal));Reasigneparent.SetParent(hit.transform);}
			}
		}	
	}
	public void ReceiveDialogArme(bool dialog){Ondialog = dialog;}
	public void infoArme(){player.SendMessage("ReceiveMunition",munition);player.SendMessage("ReceiveMunitionChargeur",munitionChargeur);player.SendMessage("ReceiveMunitionMax",munitionMax);player.SendMessage("ReceiveCanreload",true);player.SendMessage("ReceiveTypeDeRechargement",TypeDeRechargement);player.SendMessage("ReceiveCoupParCoup",CoupParCoup);}
	public void ReceiveMunition(int Mun){munition = Mun;}
	public void ReceiveMunitionChargeur(int Munc){munitionChargeur = Munc;}
	public void ReceiveInventaireState(bool inventaire){OnInventaire = inventaire;}
}