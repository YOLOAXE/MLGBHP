using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanceRocket : MonoBehaviour{
	
	public int IDArme;
	public int munitionMax = 1;
	public int munition = 1;
	public int munitionChargeur = 10;
	public float DgatSend;
	public float Tempstire = 1f,TimeReload,Timeanime;
	private float TempstireVar;
	public AudioClip SondTire;
	public AudioClip SondNoAmmoTire;
	public AudioClip SondRecharge;
	private bool Ondialog,OnInventaire;
	public int TypeDeRechargement;
	public bool onReload = false;
	private AudioSource m_AudioSource;
	private GameObject player;
	public GameObject pointDapparition;
	public GameObject PrincipalMissile;
	public Transform Rocket;
	
    void Start(){
		
		m_AudioSource = GetComponent<AudioSource>();  
		player = GameObject.FindWithTag("player");
		player.SendMessage("ReceiveMunition",munition);
		player.SendMessage("ReceiveMunitionChargeur",munitionChargeur);
		player.SendMessage("ReceiveMunitionMax",munitionMax);
		player.SendMessage("ReceiveCanreload",false);
		player.SendMessage("ReceiveCoupParCoup",true);
		TempstireVar = Tempstire;
		PrincipalMissile.SetActive(munition > 0);
    }

    void Update(){
		if(!Ondialog && !OnInventaire){
			if(munition > munitionMax){
				munition = 	munition - munitionMax;
				munitionChargeur++;
				player.SendMessage("ReceiveMunition",munition);
				player.SendMessage("ReceiveMunitionChargeur",munitionChargeur);
			}
			if(TempstireVar >= 0){TempstireVar -= Time.deltaTime;}
			if(TempstireVar < 0){
				if(Input.GetButtonDown("Fire1") && !Input.GetButton("Sprint") && !Input.GetButtonDown("Recharger") && munition == 0 && !onReload){m_AudioSource.clip = SondNoAmmoTire;m_AudioSource.PlayOneShot(m_AudioSource.clip);}
				if(Input.GetButtonDown("Fire1") && !Input.GetButton("Sprint") && !Input.GetButtonDown("Recharger") && munition >= 1 && !onReload){
					StartCoroutine(Shoot());
					TempstireVar = Tempstire;
					PrincipalMissile.SetActive(false);
					Instantiate(Rocket,pointDapparition.transform.position,Quaternion.Euler(0,0,0));
					munition -= 1;
					m_AudioSource.clip = SondTire;m_AudioSource.PlayOneShot(m_AudioSource.clip);
					player.SendMessage("ReceiveMunition",munition);
					player.SendMessage("ReceiveMunitionChargeur",munitionChargeur);
				}
				
			}
		}
		if(!onReload){
			if(Input.GetButtonDown("Recharger") && !Input.GetButton("Fire1")){
				if(munition != munitionMax){
					if(munitionChargeur > 0){	
						onReload = true;
						player.SendMessage("ReceiveReload",onReload);
						StartCoroutine(RechargeCouroutine());
					}
				}
			}
	
		}
    }
	IEnumerator RechargeCouroutine(){
		if(onReload){
			player.SendMessage("ReceiveCanShootAtack",false);
			m_AudioSource.clip = SondRecharge;
			m_AudioSource.PlayOneShot(m_AudioSource.clip);
			yield return new WaitForSeconds(Timeanime);
			PrincipalMissile.SetActive(true);
			yield return new WaitForSeconds(TimeReload);	
			munition = munitionMax;
			munitionChargeur--;
			if(munitionChargeur < 0){munitionChargeur = 0;}
			onReload = false;
			player.SendMessage("ReceiveMunition",munition);
			player.SendMessage("ReceiveMunitionChargeur",munitionChargeur);
			player.SendMessage("ReceiveCanShootAtack",true);
		}
	}
	IEnumerator Shoot(){
		player.SendMessage("ReceiveShoot",true);
		yield return new WaitForSeconds(0.1f);
		player.SendMessage("ReceiveShoot",false);
	}
	public void ReceiveDialogArme(bool dialog){Ondialog = dialog;}
	public void infoArme(){player.SendMessage("ReceiveMunition",munition);player.SendMessage("ReceiveMunitionChargeur",munitionChargeur);player.SendMessage("ReceiveMunitionMax",munitionMax);player.SendMessage("ReceiveCanreload",true);onReload = false;player.SendMessage("ReceiveTypeDeRechargement",TypeDeRechargement);player.SendMessage("ReceiveCoupParCoup",true);PrincipalMissile.SetActive(munition > 0);}
	public void ReceiveMunition(int Mun){munition = Mun;}
	public void ReceiveMunitionChargeur(int Munc){munitionChargeur = Munc;}
	public void ReceiveInventaireState(bool inventaire){OnInventaire = inventaire;}
}
