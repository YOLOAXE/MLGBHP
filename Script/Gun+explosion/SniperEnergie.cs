using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SniperEnergie : MonoBehaviour {
	
	public int IDArme;
	public float DgatSend,EnergiePris;
	public float Tempstire = 0.08f,TempTireSond;
	private float TempstireVar;
	public float hitForce;
	public bool TireAnimPart;
	public bool CoupParCoup;
	public ParticleSystem Tire;
	public Transform ImpacteNormal;
	public Transform ImpacteSable;
	public Transform ImpacteTerre;
	public Transform ImpacteBeton;
	public Transform ImpacteMetal;
	public Transform ImpacteBois;
	public Transform ImpacteSang,ImpacteSangbleu,Impactepierre,ImpacteSangvert,ImpacteAdaptation;
	public AudioClip SondTire;
	public AudioClip SondNoAmmoTire;
	private Transform Reasigneparent,turnLaser;
	private float AudioclipPlayer;
	private bool taghelper,Ondialog,OnInventaire;
	private AudioSource m_AudioSource;
	private GameObject player,MainCamera;
	public GameObject pointDapparition;
	private bool AntiSpamMessageCamra = true;
	public Transform LaserSend;
	private bool Canreload = false;
	private float NULL = 0;
	public bool SniperZoom = true;
	public int TypeDeRechargement;

    void Start ()
    {
		m_AudioSource = GetComponent<AudioSource>();
		Tire.Stop();
		AudioclipPlayer = TempTireSond;
		player = GameObject.FindWithTag("player");
		MainCamera = GameObject.FindWithTag("MainCamera");
		player.SendMessage("ReceiveCanreload",false);
		player.SendMessage("ReceiveCoupParCoup",CoupParCoup);
		player.SendMessage("ReceiveTypeDeRechargement",TypeDeRechargement);
		TempstireVar = Tempstire;
    }
	
	void Update () {
		if(!Ondialog && !OnInventaire){
		if(SniperZoom){
			if(Input.GetButton("Fire2")){
				if(AntiSpamMessageCamra){MainCamera.SendMessage("ReceiveZoom",AntiSpamMessageCamra);AntiSpamMessageCamra = false;}
			}else{
				if(!AntiSpamMessageCamra){MainCamera.SendMessage("ReceiveZoom",AntiSpamMessageCamra);AntiSpamMessageCamra = true;}
			}
		}
		if(TempstireVar >= 0){TempstireVar -= Time.deltaTime;if(Input.GetButtonDown("Fire1")){m_AudioSource.clip = SondNoAmmoTire;m_AudioSource.PlayOneShot(m_AudioSource.clip);}}
		if(TempstireVar < 0){
		if(Input.GetButtonDown("Fire1") && !Input.GetButton("Sprint") && !Input.GetButtonDown("Recharger") && player.GetComponent<PlayerStat>().ManaJoueur > EnergiePris){StartCoroutine(Shoot());}
		if (Input.GetButtonDown("Fire1") && !Input.GetButton("Sprint") && !Input.GetButtonDown("Recharger") && player.GetComponent<PlayerStat>().ManaJoueur > EnergiePris){
			if(Input.GetButtonDown("Fire1")){TempstireVar = 0;AudioclipPlayer = 0;}
			if(TireAnimPart){Tire.Play();TireAnimPart = false;}
			AudioclipPlayer -= Time.deltaTime;
			if(AudioclipPlayer <= 0){
				m_AudioSource.clip = SondTire;
				m_AudioSource.PlayOneShot(m_AudioSource.clip);
				AudioclipPlayer = TempTireSond;
			}
				
					TempstireVar = Tempstire;
					RaycastHit hit;
					Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				
					if (Physics.Raycast(ray, out hit)){
					if (hit.rigidbody != null){
					hit.rigidbody.AddForce(ray.direction * hitForce);	
					}
						taghelper = false;
						if(hit.transform.tag == "Terre"){Reasigneparent = Instantiate(ImpacteTerre,hit.point,Quaternion.FromToRotation(Vector3.forward, hit.normal));taghelper = true;}
						if(hit.transform.tag == "Bois"){Reasigneparent = Instantiate(ImpacteBois,hit.point,Quaternion.FromToRotation(Vector3.forward, hit.normal));taghelper = true;}
						if(hit.transform.tag == "Sable"){Reasigneparent = Instantiate(ImpacteSable,hit.point,Quaternion.FromToRotation(Vector3.forward, hit.normal));taghelper = true;}
						if(hit.transform.tag == "Beton"){Reasigneparent = Instantiate(ImpacteBeton,hit.point,Quaternion.FromToRotation(Vector3.forward, hit.normal));taghelper = true;}
						if(hit.transform.tag == "Metal"){Reasigneparent = Instantiate(ImpacteMetal,hit.point,Quaternion.FromToRotation(Vector3.forward, hit.normal));taghelper = true;}
						if(hit.transform.tag == "Enemie"){hit.collider.gameObject.SendMessage("ReceiveDamagePlayer",DgatSend);Reasigneparent = Instantiate(ImpacteSang,hit.point,Quaternion.FromToRotation(Vector3.forward, hit.normal));taghelper = true;}
						if(hit.transform.tag == "Crystal"){hit.collider.gameObject.SendMessage("ReceiveDamagePlayer",DgatSend);Reasigneparent = Instantiate(ImpacteAdaptation,hit.point,Quaternion.FromToRotation(Vector3.forward, hit.normal));taghelper = true;}
						if(hit.transform.tag == "EnemiePierre"){hit.collider.gameObject.SendMessage("ReceiveDamagePlayer",DgatSend);Reasigneparent = Instantiate(Impactepierre,hit.point,Quaternion.FromToRotation(Vector3.forward, hit.normal));taghelper = true;}
						if(hit.transform.tag == "EnemieBleu"){hit.collider.gameObject.SendMessage("ReceiveDamagePlayer",DgatSend);Reasigneparent = Instantiate(ImpacteSangbleu,hit.point,Quaternion.FromToRotation(Vector3.forward, hit.normal));taghelper = true;}
						if(hit.transform.tag == "EnemieVert"){hit.collider.gameObject.SendMessage("ReceiveDamagePlayer",DgatSend);Reasigneparent = Instantiate(ImpacteSangvert,hit.point,Quaternion.FromToRotation(Vector3.forward, hit.normal));taghelper = true;}
						if(!taghelper){if(hit.transform.tag == "PNJ" || hit.transform.tag == "Soldat" || hit.transform.tag == "player" || hit.transform.tag == "NoImpact" || hit.transform.tag == "Escalier"){}else{Reasigneparent = Instantiate(ImpacteNormal,hit.point,Quaternion.FromToRotation(Vector3.forward, hit.normal));}}
						if(!(hit.transform.tag == "PNJ" || hit.transform.tag == "Soldat" || hit.transform.tag == "player" || hit.transform.tag == "Terre" || hit.transform.tag == "Sable" || hit.transform.tag == "Beton" || hit.transform.tag == "NoImpact" || hit.transform.tag == "Escalier")){Reasigneparent.SetParent(hit.transform);}
						turnLaser = Instantiate(LaserSend,pointDapparition.transform.position,Quaternion.identity);
						player.SendMessage("ReceiveTakeMana",EnergiePris);
					}
		}else{
		if(!TireAnimPart){TireAnimPart = true;}
		Tire.Stop();
		}
		}

	}
	}
	IEnumerator Shoot(){
		player.SendMessage("ReceiveShoot",true);
		yield return new WaitForSeconds(0.1f);
		player.SendMessage("ReceiveShoot",false);
	}
	public void ReceiveDialogArme(bool dialog){Ondialog = dialog;}
	public void infoArme(){player.SendMessage("ReceiveMunition",EnergiePris);player.SendMessage("ReceiveMunitionChargeur",NULL);player.SendMessage("ReceiveMunitionMax",NULL);player.SendMessage("ReceiveCanreload",Canreload);player.SendMessage("ReceiveCoupParCoup",CoupParCoup);player.SendMessage("ReceiveTypeDeRechargement",TypeDeRechargement);}
	public void ReceiveMunition(int Mun){}
	public void ReceiveMunitionChargeur(int Munc){}
	public void ReceiveInventaireState(bool inventaire){OnInventaire = inventaire;}
}
