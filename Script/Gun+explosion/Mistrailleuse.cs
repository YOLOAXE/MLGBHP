using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Mistrailleuse : MonoBehaviour {
	
	public int IDArme;
	public int munitionMax = 32;
	public int munition = 32;
	public int munitionChargeur = 10;
	public float DgatSend;
	public float Tempstire = 0.08f,TimeReload,TempTireSond;
	private float TempstireVar;
	public int TypeDeRechargement;
	public bool onReload = false;
	private bool jeComprendPas;
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
	public AudioClip SondRecharge;
	public AudioClip SondRechargeOutofAmmo;
	public AudioClip SondNoAmmoTire;
	private Transform Reasigneparent;
	private float AudioclipPlayer;
	private bool taghelper,Ondialog,OnInventaire;
	private AudioSource m_AudioSource;
	private GameObject player;
	public Transform castingBullet;
	public GameObject pointDapparition;

    void Start ()
    {
		m_AudioSource = GetComponent<AudioSource>();
		Tire.Stop();
		AudioclipPlayer = TempTireSond;
		player = GameObject.FindWithTag("player");
		player.SendMessage("ReceiveMunition",munition);
		player.SendMessage("ReceiveMunitionChargeur",munitionChargeur);
		player.SendMessage("ReceiveMunitionMax",munitionMax);
		player.SendMessage("ReceiveCanreload",true);
		player.SendMessage("ReceiveTypeDeRechargement",TypeDeRechargement);
		player.SendMessage("ReceiveCoupParCoup",CoupParCoup);
		TempstireVar = Tempstire;
    }
	
	void Update () {
		if(!Ondialog && !OnInventaire){
		if(munition > munitionMax){
		munition = 	munition - munitionMax;
		munitionChargeur++;
		player.SendMessage("ReceiveMunition",munition);
		player.SendMessage("ReceiveMunitionChargeur",munitionChargeur);
		}
		if(Input.GetButtonDown("Fire1") && !Input.GetButton("Sprint") && !Input.GetButtonDown("Recharger") && munition == 0){m_AudioSource.clip = SondNoAmmoTire;m_AudioSource.PlayOneShot(m_AudioSource.clip);}
		if (Input.GetButton("Fire1") && !Input.GetButton("Sprint") && !Input.GetButtonDown("Recharger") && !jeComprendPas){
		if(munition >= 1){
			if(Input.GetButtonDown("Fire1")){TempstireVar = 0;AudioclipPlayer = 0;}
			if(!onReload){
			if(TireAnimPart){Tire.Play();TireAnimPart = false;player.SendMessage("ReceiveShoot",true);}
			TempstireVar -= Time.deltaTime;
			AudioclipPlayer -= Time.deltaTime;
			if(AudioclipPlayer <= 0){
				m_AudioSource.clip = SondTire;
				m_AudioSource.PlayOneShot(m_AudioSource.clip);
				AudioclipPlayer = TempTireSond;
			}
				if(TempstireVar < 0){
					TempstireVar = Tempstire;
					munition -= 1;
					player.SendMessage("ReceiveMunition",munition);
					player.SendMessage("ReceiveMunitionChargeur",munitionChargeur);
					if(castingBullet != null){
						Transform casting;
						casting = Instantiate(castingBullet,pointDapparition.transform.position,Quaternion.Euler(90,0,0));
						casting.transform.eulerAngles = pointDapparition.transform.eulerAngles;
						casting.GetComponent<Rigidbody>().AddForce(-pointDapparition.transform.forward * 200);
					}
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
						if(hit.transform.tag == "EnemiePierre"){hit.collider.gameObject.SendMessage("ReceiveDamagePlayer",DgatSend);Reasigneparent = Instantiate(Impactepierre,hit.point,Quaternion.FromToRotation(Vector3.forward, hit.normal));taghelper = true;}
						if(hit.transform.tag == "EnemieBleu"){hit.collider.gameObject.SendMessage("ReceiveDamagePlayer",DgatSend);Reasigneparent = Instantiate(ImpacteSangbleu,hit.point,Quaternion.FromToRotation(Vector3.forward, hit.normal));taghelper = true;}
						if(hit.transform.tag == "EnemieVert"){hit.collider.gameObject.SendMessage("ReceiveDamagePlayer",DgatSend);Reasigneparent = Instantiate(ImpacteSangvert,hit.point,Quaternion.FromToRotation(Vector3.forward, hit.normal));taghelper = true;}
						if(hit.transform.tag == "Crystal"){hit.collider.gameObject.SendMessage("ReceiveDamagePlayer",DgatSend);Reasigneparent = Instantiate(ImpacteAdaptation,hit.point,Quaternion.FromToRotation(Vector3.forward, hit.normal));taghelper = true;}
						if(!taghelper){if(hit.transform.tag == "PNJ" || hit.transform.tag == "Soldat" || hit.transform.tag == "player" || hit.transform.tag == "NoImpact" || hit.transform.tag == "Escalier"){}else{Reasigneparent = Instantiate(ImpacteNormal,hit.point,Quaternion.FromToRotation(Vector3.forward, hit.normal));}}
						if(!(hit.transform.tag == "PNJ" || hit.transform.tag == "Soldat" || hit.transform.tag == "player" || hit.transform.tag == "Terre" || hit.transform.tag == "Sable" || hit.transform.tag == "Beton" || hit.transform.tag == "NoImpact" || hit.transform.tag == "Escalier")){Reasigneparent.SetParent(hit.transform);}
					}
				}
			}
		}else{
		if(!TireAnimPart){TireAnimPart = true;player.SendMessage("ReceiveShoot",false);}
		Tire.Stop();
		}
		}else{
		if(!TireAnimPart){TireAnimPart = true;player.SendMessage("ReceiveShoot",false);}
		Tire.Stop();
		}
		if(!onReload){
			if(Input.GetButtonDown("Recharger") && !Input.GetButton("Fire1")){
				if(munition != munitionMax){
					if(munitionChargeur > 0){	
						onReload = true;jeComprendPas = true;
						player.SendMessage("ReceiveReload",onReload);
						StartCoroutine(RechargeCouroutine());
					}
				}
			}
	
		}
	}
	}
	IEnumerator RechargeCouroutine(){
		if(onReload){
			Tire.Stop();
			TireAnimPart = true;
			player.SendMessage("ReceiveCanShootAtack",false);
			if(munition > 0){m_AudioSource.clip = SondRecharge;	}else{m_AudioSource.clip = SondRechargeOutofAmmo;}
			m_AudioSource.PlayOneShot(m_AudioSource.clip);
			yield return new WaitForSeconds(TimeReload);	
			munition = munitionMax;
			munitionChargeur--;
			if(munitionChargeur < 0){munitionChargeur = 0;}
			onReload = false;
			player.SendMessage("ReceiveMunition",munition);
			player.SendMessage("ReceiveMunitionChargeur",munitionChargeur);
			player.SendMessage("ReceiveCanShootAtack",true);
		}
	jeComprendPas = false;
	}
	public void ReceiveDialogArme(bool dialog){Ondialog = dialog;}
	public void infoArme(){player.SendMessage("ReceiveMunition",munition);player.SendMessage("ReceiveMunitionChargeur",munitionChargeur);player.SendMessage("ReceiveMunitionMax",munitionMax);player.SendMessage("ReceiveCanreload",true);onReload = false;jeComprendPas = false;player.SendMessage("ReceiveTypeDeRechargement",TypeDeRechargement);player.SendMessage("ReceiveCoupParCoup",CoupParCoup);}
	public void ReceiveMunition(int Mun){munition = Mun;}
	public void ReceiveMunitionChargeur(int Munc){munitionChargeur = Munc;}
	public void ReceiveInventaireState(bool inventaire){OnInventaire = inventaire;}
}
