using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectMainArmeManager : MonoBehaviour
{
	//atrapper les objects
	public GameObject PlayerCamera; // pour atribuit le fixed joint dessu
	public float DistanceMaxTakeObject = 3; //distance pour atraper un object
	public bool asObject; //possede un object
	private bool asObjectVar; //possede un object varie pour detecter un changement
	private FixedJoint FJ; // le joint
	public float DPlayerObject; // distance entre le joueur et l'object
	public float hitForcePouser = 1000; // force du joeur pour envoye des truc 
	public RaycastHit hit;
	public Ray ray;
	//atrapper les objects
	
	//Gun/Arme/Wand
	public int NombreArmePlace,ArmePlaceSelection,NombreArmeMax = 4;
	public GameObject[] ArmePorterParJoueur;
	private GameObject[] ArmeCadreSelection,ArmeCadreNonSelection,Arme_1,Arme_2,Arme_3,Arme_4,ArmeGameObjectShadow;
	public Transform[] ArmeLootJoueur;
	public int[] IDArmePorterParJoueur,TypeArmePorterParJoueur,IDArmeDejaPrisParJoueur,IDCursor;
	public bool AsRifle,AsHandGun,AsMelee;
	public Animator m_AnimatorMainRifle,m_AnimatorMainHandGun,m_AnimatorMainMelee;
	public GameObject Rifle,HandGun,Melee;
	private float TimerAnimationCange,TimerReload;
	private bool StartTime = false;
	private int changeAnimation = 0;
	public bool CanShootAtack = true;
	public bool OnReload = false,CanRelod = true,NoAmmo = false,TypeArmeCoupParCoup = false;
	public int TypeRechargement;
	public float TempsDeRechargement = 1f;
	public TextMeshProUGUI TexteMunition;
	public float[] Munition,MunitionMax,MunitionChargeur;
	public AudioSource m_AudioSource;
	public AudioClip HosterWeapon;
	public AudioClip TakeWeapon;
	public AudioClip pickup;
	public GameObject Loot,MunitionLoot;
	public bool OnLoot,OnLootMunition;
	public int IDLoot;
	public float MuniLoot,MuniMaxLoot,ChargLoot;
	public bool ChangeArme;
	private int ModRHM,IDReticule,MunitionLootInt,MunitionChargerLootInt;
	public int CursorBase = 0;
	private bool Ondrop;
	private Transform ObjectADrop;
	private float TimerRedrop = 1;
	public GameObject DropObject;
	private float timerReload = 1.2f;
	private bool receiveReload,NoReceive;
	private int SendAntichange;
	private float TimerChangeSlot = 1;
	private bool AttaqueEpee,AimAtaqueEpee;
	//Gun/Arme/Wand
	//dialog
	public bool OnDialog,OnInventaire;
	private bool OnDialogVar,OnInventaireVar;
	//
	void Start (){
	TexteMunition = GameObject.Find("AmmoTextMeshPro").GetComponent<TextMeshProUGUI>();
	Munition = new float[NombreArmeMax];
	MunitionMax = new float[NombreArmeMax];
	MunitionChargeur = new float[NombreArmeMax];
	IDArmePorterParJoueur = new int[NombreArmeMax];
	TypeArmePorterParJoueur = new int[NombreArmeMax];
	IDCursor = new int[NombreArmeMax];
	TimerAnimationCange = 0.5f;	
	TimerReload = TempsDeRechargement;
	asObjectVar = asObject;
	CanShootAtack = true;
	OnDialog = false;
	OnInventaire = false;
	OnLoot = false;
	ChangeArme = false;
	Ondrop = false;
	receiveReload = false;
	NoReceive = false;
	FindCadreArme();
	}

	void Update () {
	if(TimerChangeSlot > -1){TimerChangeSlot-=Time.deltaTime;}
	if(Input.GetButtonDown("Slot0") && TimerChangeSlot < 0 && ArmePlaceSelection != 0 && !Ondrop && !asObject && !OnLootMunition && !Input.GetButton("Interagire") && !OnDialog && !OnInventaire){
		ArmePlaceSelection = 0;SendAntichange = 0;
		ChangeArme = true;TimerChangeSlot = 1;
		m_AudioSource.clip = HosterWeapon;
		m_AudioSource.PlayOneShot(m_AudioSource.clip);
		Ondrop = true;TimerRedrop = 1;
		ChangeCadre();MunitionLoot = null;
		ChangeArmeLogo();SendArmeInfoRHA();
	}
	if(Input.GetButtonDown("Slot1") && TimerChangeSlot < 0 && ArmePlaceSelection != 1 && !Ondrop && !asObject && !OnLootMunition && !Input.GetButton("Interagire") && !OnDialog && !OnInventaire){
		ArmePlaceSelection = 1;SendAntichange = 1;TimerChangeSlot = 1;
		if(IDArmePorterParJoueur[ArmePlaceSelection -1] == 0){AsRifle = false;AsHandGun = false;AsMelee = false;ArmePorterParJoueur[IDArmePorterParJoueur[ArmePlaceSelection - 1]].SetActive(false);}else{ChangeArme = true;}
		m_AudioSource.clip = TakeWeapon;m_AudioSource.PlayOneShot(m_AudioSource.clip);
		Ondrop = true;TimerRedrop = 1;
		ChangeCadre();MunitionLoot = null;
		ChangeArmeLogo();SendArmeInfoRHA();
	}
	if(Input.GetButtonDown("Slot2") && TimerChangeSlot < 0 && ArmePlaceSelection != 2 && !Ondrop && !asObject && !OnLootMunition && !Input.GetButton("Interagire") && !OnDialog && !OnInventaire){
		ArmePlaceSelection = 2;SendAntichange = 2;TimerChangeSlot = 1;
		if(IDArmePorterParJoueur[ArmePlaceSelection -1] == 0){AsRifle = false;AsHandGun = false;AsMelee = false;ArmePorterParJoueur[IDArmePorterParJoueur[ArmePlaceSelection - 1]].SetActive(false);}else{ChangeArme = true;}
		m_AudioSource.clip = TakeWeapon;m_AudioSource.PlayOneShot(m_AudioSource.clip);
		Ondrop = true;TimerRedrop = 1;
		ChangeCadre();MunitionLoot = null;
		ChangeArmeLogo();SendArmeInfoRHA();
	}
	if(Input.GetButtonDown("Slot3") && TimerChangeSlot < 0 && ArmePlaceSelection != 3 && !Ondrop && !asObject && !OnLootMunition && !Input.GetButton("Interagire") && !OnDialog && !OnInventaire){
		ArmePlaceSelection = 3;SendAntichange = 3;TimerChangeSlot = 1;
		if(IDArmePorterParJoueur[ArmePlaceSelection -1] == 0){AsRifle = false;AsHandGun = false;AsMelee = false;ArmePorterParJoueur[IDArmePorterParJoueur[ArmePlaceSelection - 1]].SetActive(false);}else{ChangeArme = true;}
		m_AudioSource.clip = TakeWeapon;m_AudioSource.PlayOneShot(m_AudioSource.clip);
		Ondrop = true;TimerRedrop = 1;
		ChangeCadre();MunitionLoot = null;
		ChangeArmeLogo();SendArmeInfoRHA();
	}
	if(Input.GetButtonDown("Slot4") && TimerChangeSlot < 0 && ArmePlaceSelection != 4 && !Ondrop && !asObject && !OnLootMunition && !Input.GetButton("Interagire") && !OnDialog && !OnInventaire){
		ArmePlaceSelection = 4;SendAntichange = 4;TimerChangeSlot = 1;
		if(IDArmePorterParJoueur[ArmePlaceSelection -1] == 0){AsRifle = false;AsHandGun = false;AsMelee = false;ArmePorterParJoueur[IDArmePorterParJoueur[ArmePlaceSelection - 1]].SetActive(false);}else{ChangeArme = true;}
		m_AudioSource.clip = TakeWeapon;m_AudioSource.PlayOneShot(m_AudioSource.clip);
		Ondrop = true;TimerRedrop = 1;
		ChangeCadre();MunitionLoot = null;
		ChangeArmeLogo();SendArmeInfoRHA();
	}
	if(Input.GetButton("Drop") && !OnLoot && !(ArmePlaceSelection == 0) && !(IDArmePorterParJoueur[ArmePlaceSelection -1] == 0 ) && !Ondrop && !asObject && !OnLootMunition && !Input.GetButton("Interagire") && !OnDialog && !OnInventaire){
		Ondrop = true;
		Loot = null;
		ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out hit)){
			if(hit.distance < 1){
			ObjectADrop = Instantiate(ArmeLootJoueur[IDArmePorterParJoueur[ArmePlaceSelection -1]], transform.position + new Vector3(0,2,0), Quaternion.Euler(0,0,0));	
			}else{
			ObjectADrop = Instantiate(ArmeLootJoueur[IDArmePorterParJoueur[ArmePlaceSelection -1]], DropObject.transform.position, Quaternion.Euler(0,0,0));	
			}
		}
		if(ObjectADrop != null){
		ObjectADrop.transform.eulerAngles = DropObject.transform.eulerAngles;
		ObjectADrop.SendMessage("ReceiveIDArme",IDArmePorterParJoueur[ArmePlaceSelection - 1]);
		ObjectADrop.SendMessage("ReceiveMunitionLoot",Munition[ArmePlaceSelection - 1]);
		ObjectADrop.SendMessage("ReceiveChargeurLoot",MunitionChargeur[ArmePlaceSelection - 1]);
		ObjectADrop.GetComponent<Rigidbody>().AddForce(ray.direction * hitForcePouser);
		IDArmePorterParJoueur[ArmePlaceSelection - 1] = 0;
		Munition[ArmePlaceSelection - 1] = 0;
		MunitionChargeur[ArmePlaceSelection - 1] = 0;
		MunitionMax[ArmePlaceSelection - 1] = 0;
		TypeArmePorterParJoueur[ArmePlaceSelection - 1] = 0;
		IDCursor[ArmePlaceSelection - 1] = 0;
		NombreArmePlace--;
		ArmePlaceSelection = 0;
		ChangeArme = true;
		ChangeArmeLogo();
		TexteMunition.text = "No Ammo";
		}else{
		Ondrop = false;
		}
	}
	if(Ondrop){
		TimerRedrop -= Time.deltaTime;
		if(TimerRedrop < 0){
			Ondrop = false;	
			TimerRedrop = 1;
		}
	}
	if(ChangeArme && !OnLoot){
	for(int p = 0;p < ArmePorterParJoueur.Length;p++){
		if(ArmePlaceSelection == 0){
		AsRifle = false;AsHandGun = false;AsMelee = false;
		ArmePorterParJoueur[p].SetActive(false);
		}else{
			if(IDArmePorterParJoueur[ArmePlaceSelection - 1] == p){
				ArmePorterParJoueur[p].SetActive(true);
				AsRifle = false;AsHandGun = false;AsMelee = false;
				if(TypeArmePorterParJoueur[ArmePlaceSelection - 1] == 1 || TypeArmePorterParJoueur[ArmePlaceSelection - 1] == 5 || TypeArmePorterParJoueur[ArmePlaceSelection - 1] == 4){AsRifle = true;}
				if(TypeArmePorterParJoueur[ArmePlaceSelection - 1] == 2){AsHandGun = true;}
				if(TypeArmePorterParJoueur[ArmePlaceSelection - 1] == 3){AsMelee = true;}
				StartCoroutine(InfoCouroutine());
			}else{
				ArmePorterParJoueur[p].SetActive(false);	
			}
		}
	}
	ChangeArme = false;
	gameObject.SendMessage("AUneArmeDeTypeRifle",AsRifle);
	gameObject.SendMessage("AUneArmeDeTypeHandGun",AsHandGun);
	gameObject.SendMessage("AUneArmeDeTypeMelee",AsMelee);
	}
	if(OnLoot){
		for(int i = 0;i < NombreArmeMax;i++){
			if(IDLoot == IDArmePorterParJoueur[i]){
				NoReceive = true;
				Munition[i] += MuniLoot;
				MunitionChargeur[i] += ChargLoot;
				MunitionMax[i] = MuniMaxLoot;
				SendAntichange = i + 1;
				ArmePlaceSelection = i + 1;
				ChangeArme = true;
				StartCoroutine(InfoCouroutine());
				m_AudioSource.clip = TakeWeapon;m_AudioSource.PlayOneShot(m_AudioSource.clip);
				//ajoute les munitions
				Destroy(Loot);
				TimerChangeSlot = 1;
				OnLoot = false;
				m_AudioSource.clip = pickup;
				m_AudioSource.PlayOneShot(m_AudioSource.clip);
				ChangeCadre();
			}

		}
		for(int n = 0;n < NombreArmeMax;n++){
				if(NombreArmePlace < NombreArmeMax){
					if(IDArmePorterParJoueur[n] == 0){
						if(OnLoot){
						NoReceive = true;
						Munition[n] = MuniLoot;
						MunitionChargeur[n] = ChargLoot;
						IDArmePorterParJoueur[n] = IDLoot; 
						TypeArmePorterParJoueur[n] = ModRHM;
						IDCursor[n] = IDReticule;
						MunitionMax[n] = MuniMaxLoot;
						SendAntichange = n + 1;
						StartCoroutine(InfoCouroutine());
						m_AudioSource.clip = TakeWeapon;m_AudioSource.PlayOneShot(m_AudioSource.clip);
						//ajoute arme et c'est mune
						NombreArmePlace++;
						ArmePlaceSelection = n + 1;
						Destroy(Loot);
						TimerChangeSlot = 1;
						OnLoot = false;
						m_AudioSource.clip = pickup;
						m_AudioSource.PlayOneShot(m_AudioSource.clip);
						ChangeCadre();
						ChangeArmeLogo();
						}
					}
				}
			}
		IDLoot = 0;
		MuniLoot = 0;
		MuniMaxLoot = 0;
		ChargLoot = 0;
	}
	if(!OnDialog && !OnInventaire){
		if(OnDialogVar || OnInventaireVar){
			if(changeAnimation == 2){m_AnimatorMainRifle.SetBool("Holster",false);}
			if(changeAnimation == 3){m_AnimatorMainHandGun.SetBool("Holster",false);}
			if(changeAnimation == 1){m_AnimatorMainMelee.SetBool("Holster",false);}	
		}
	if(timerReload > 0){timerReload -= Time.deltaTime;}
	if(receiveReload){
		receiveReload = false;
			if(CanRelod && timerReload <= 0.1f && Munition[ArmePlaceSelection - 1] != MunitionMax[ArmePlaceSelection - 1]){
			timerReload = 1.2f;
			ArmePorterParJoueur[IDArmePorterParJoueur[ArmePlaceSelection -1]].SendMessage("infoArme");
			OnReload = true;
			CanShootAtack = false;
			if(NoAmmo || Munition[ArmePlaceSelection - 1] < 1){
				if(TypeRechargement == 0){if(changeAnimation == 2){m_AnimatorMainRifle.Play("HandGun Reload Out Of Ammo", 0, 0.25f);}if(changeAnimation == 3){m_AnimatorMainHandGun.Play("Reload Out Of Ammo", 0, 0.25f);}}
				if(TypeRechargement == 1){if(changeAnimation == 2){m_AnimatorMainRifle.Play("Reload Out Of Ammo", 0, 0.25f);}if(changeAnimation == 3){m_AnimatorMainHandGun.Play("Rifle Reload Out of Ammo", 0, 0.25f);}}
				if(TypeRechargement == 2){m_AnimatorMainRifle.Play("LanceRocket", 0, 0.25f);}
			}
			else{
				if(TypeRechargement == 0){if(changeAnimation == 2){m_AnimatorMainRifle.Play("HandGun Reload Ammo Left", 0, 0.25f);}if(changeAnimation == 3){m_AnimatorMainHandGun.Play("Reload Ammo Left", 0, 0.25f);}}
				if(TypeRechargement == 1){if(changeAnimation == 2){m_AnimatorMainRifle.Play("Reload Ammo Left", 0, 0.25f);}if(changeAnimation == 3){m_AnimatorMainHandGun.Play("Rifle Reload Ammo left", 0, 0.25f);}}
				if(TypeRechargement == 2){m_AnimatorMainRifle.Play("LanceRocket", 0, 0.25f);}
			}
			}
	}
	if(OnReload){
		TimerReload -= Time.deltaTime;
		if(TimerReload <= 0){
			TimerReload = TempsDeRechargement;
			OnReload = false;CanShootAtack = true;
		}
	}
	if(AsMelee){
		TexteMunition.text = "No Ammo";
		if(Input.GetButton("Fire2")){m_AnimatorMainMelee.SetBool("Aim",true);}else{m_AnimatorMainMelee.SetBool("Aim",false);}
		if(AimAtaqueEpee){
			if(CanShootAtack && AttaqueEpee){
				m_AnimatorMainMelee.SetBool("AttaqueAim",true);		
				AttaqueEpee = false;
				AimAtaqueEpee = false;
			}
		}else{
			if(CanShootAtack && AttaqueEpee){
				int RandomAnime;
				AttaqueEpee = false;
				RandomAnime = Random.Range(0,2);
				if(RandomAnime == 1){
					m_AnimatorMainMelee.SetBool("Attaque1",true);	
				}else{
					m_AnimatorMainMelee.SetBool("Attaque2",true);	
				}
			}
		}
		if(Input.GetButton("Horizontal") || Input.GetButton("Vertical")){
			if(Input.GetButton("Sprint") && !Input.GetButton("Jump") && !Input.GetButton("Fire2")){
			m_AnimatorMainMelee.SetBool("Run",true);
			m_AnimatorMainMelee.SetBool("Walk",false);	
			}
			else{
			m_AnimatorMainMelee.SetBool("Run",false);
			m_AnimatorMainMelee.SetBool("Walk",true);	
			}
		}
		else{
		m_AnimatorMainMelee.SetBool("Walk",false);	
		m_AnimatorMainMelee.SetBool("Run",false);
		}
		if(changeAnimation != 1){
			if(changeAnimation == 2){m_AnimatorMainRifle.SetBool("Holster",true);}
			if(changeAnimation == 3){m_AnimatorMainHandGun.SetBool("Holster",true);}
			StartTime = true;
			changeAnimation = 1;
		}
	}
	if(AsRifle){
		if(MunitionChargeur[ArmePlaceSelection - 1] == 0 && MunitionMax[ArmePlaceSelection - 1] == 0){
			TexteMunition.text = "-" + Munition[ArmePlaceSelection - 1];
		}else{
			TexteMunition.text = Munition[ArmePlaceSelection - 1] + " / " + MunitionChargeur[ArmePlaceSelection - 1];
		}
		if(!OnReload){
		if(CanShootAtack && Munition[ArmePlaceSelection - 1] > 0 ){
			if(!Input.GetButton("Sprint")){
				if(TypeArmeCoupParCoup){
					if(Input.GetButtonDown("Fire1")){StartCoroutine(CoupParCoup());}
				}else{
					if(Input.GetButton("Fire1")){m_AnimatorMainRifle.SetBool("Shoot",true);}else{m_AnimatorMainRifle.SetBool("Shoot",false);}	
				}
			}else{
				m_AnimatorMainRifle.SetBool("Shoot",false);
			}
		}
		if(Input.GetButton("Fire2") && !Input.GetButton("Sprint") && TypeRechargement != 2){m_AnimatorMainRifle.SetBool("Aim",true);}else{m_AnimatorMainRifle.SetBool("Aim",false);}
		}
		if(Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
		{
			if(Input.GetButton("Sprint") && !Input.GetButton("Jump") && !Input.GetButton("Fire2"))
			{
				m_AnimatorMainRifle.SetBool("Run",true);
				m_AnimatorMainRifle.SetBool("Walk",false);	
				CanShootAtack = false;
			}
			else
			{
				m_AnimatorMainRifle.SetBool("Run",false);
				m_AnimatorMainRifle.SetBool("Walk",true);	
				CanShootAtack = true;
			}
		}
		else
		{
			m_AnimatorMainRifle.SetBool("Walk",false);	
			m_AnimatorMainRifle.SetBool("Run",false);
		}
		if(changeAnimation != 2){
			if(changeAnimation == 3){m_AnimatorMainHandGun.SetBool("Holster",true);}
			if(changeAnimation == 1){m_AnimatorMainMelee.SetBool("Holster",true);}
			StartTime = true;
			changeAnimation = 2;
		}	
	}
	if(AsHandGun){
		TexteMunition.text = Munition[ArmePlaceSelection - 1] + " / " + MunitionChargeur[ArmePlaceSelection - 1];
		if(!OnReload){
		if(CanShootAtack && Munition[ArmePlaceSelection - 1] > 0 ){
			if(TypeArmeCoupParCoup){
				if(Input.GetButtonDown("Fire1")){m_AnimatorMainHandGun.SetBool("Shoot",true);}else{m_AnimatorMainHandGun.SetBool("Shoot",false);}	
			}else{
				if(Input.GetButton("Fire1")){m_AnimatorMainHandGun.SetBool("Shoot",true);}else{m_AnimatorMainHandGun.SetBool("Shoot",false);}	
			}
		}else{
		m_AnimatorMainHandGun.SetBool("Shoot",false);	
		}
		if(Input.GetButton("Fire2")){m_AnimatorMainHandGun.SetBool("Aim",true);}else{m_AnimatorMainHandGun.SetBool("Aim",false);}
		}
		if(Input.GetButton("Horizontal") || Input.GetButton("Vertical")){
			if(Input.GetButton("Sprint") && !Input.GetButton("Jump") && !Input.GetButton("Fire2")){
			m_AnimatorMainHandGun.SetBool("Run",true);
			m_AnimatorMainHandGun.SetBool("Walk",false);	
			CanShootAtack = false;
			}
			else{
			m_AnimatorMainHandGun.SetBool("Run",false);
			m_AnimatorMainHandGun.SetBool("Walk",true);	
			CanShootAtack = true;
			}
		}
		else{
		m_AnimatorMainHandGun.SetBool("Walk",false);	
		m_AnimatorMainHandGun.SetBool("Run",false);
		}
		if(changeAnimation != 3){
			if(changeAnimation == 2){m_AnimatorMainRifle.SetBool("Holster",true);}
			if(changeAnimation == 1){m_AnimatorMainMelee.SetBool("Holster",true);}
			StartTime = true;
			changeAnimation = 3;
		}	
	}
	if(StartTime){
	TimerAnimationCange -= Time.deltaTime;	
	}
	if(TimerAnimationCange <= 0){
	if(!asObject){
		if(changeAnimation == 2){m_AnimatorMainRifle.SetBool("Holster",false);}
		if(changeAnimation == 3){m_AnimatorMainHandGun.SetBool("Holster",false);}
		if(changeAnimation == 1){m_AnimatorMainMelee.SetBool("Holster",false);}
		Rifle.SetActive(AsRifle);
		HandGun.SetActive(AsHandGun);
		Melee.SetActive(AsMelee);
		TimerAnimationCange = 0.5f;
		StartTime = false;
	}
	}
	
	if(!AsMelee && !AsRifle && !AsHandGun || asObject != asObjectVar && asObject){
		if(changeAnimation != 0){
			if(changeAnimation == 2){m_AnimatorMainRifle.SetBool("Holster",true);}
			if(changeAnimation == 3){m_AnimatorMainHandGun.SetBool("Holster",true);}
			if(changeAnimation == 1){m_AnimatorMainMelee.SetBool("Holster",true);}
			StartTime = true;
			changeAnimation = 0;
		}	
	}
	asObjectVar = asObject;
		
	if(asObject){CanShootAtack = !asObject;}
	if(FJ == null){
		asObject = false;	
	}
	if(Input.GetButtonDown("Interagire")){
		if(FJ == null)
		{
			asObject = false;	
		}
		if(asObject)
		{
			Destroy(FJ);
			asObject = false;
		}
		else
		{
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit))
			{
				if (hit.transform.gameObject.tag == "Object")
				{
					DPlayerObject =  Vector3.Distance(hit.transform.position,transform.position);
					if(DPlayerObject <= DistanceMaxTakeObject)
					{
						Debug.Log("test");
						FJ = PlayerCamera.AddComponent(typeof(FixedJoint)) as FixedJoint;
						FJ.connectedBody = hit.transform.gameObject.GetComponent<Rigidbody>();
						FJ.breakForce = 100;
						FJ.breakTorque = 100;
						FJ.massScale = 100;
						FJ.connectedMassScale = 100;
						asObject = true;
					}
				}
			}
		}
	}
	if(Input.GetButtonDown("Fire1"))
	{
		if(asObject)
		{
			Destroy(FJ);
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (hit.rigidbody != null){hit.rigidbody.AddForce(ray.direction * hitForcePouser);}
			asObject = false;
		}
	}
	}else{
	OnDialogVar = OnDialog;
	OnInventaireVar = OnInventaire;
	if(changeAnimation == 2){m_AnimatorMainRifle.SetBool("Holster",true);}
	if(changeAnimation == 3){m_AnimatorMainHandGun.SetBool("Holster",true);}
	if(changeAnimation == 1){m_AnimatorMainMelee.SetBool("Holster",true);}	
	}
	}
	void OnTriggerStay(Collider other){
		if(other.tag == "Object"){
			Destroy(FJ);
			asObject = false;	
		}
		if(other.tag == "ArmeLoot" && !Ondrop && !OnLootMunition && !OnDialog && !OnInventaire){
			if(Loot != other.gameObject || Loot == null){
				Loot = other.gameObject;	
				MunitionLoot = null;
				if(NombreArmePlace >= NombreArmeMax){
					IDLoot = -1;
					Loot.SendMessage("SendID");
					
					for(int i = 0;i < NombreArmeMax;i++){
						if(IDLoot == -1){i = 0;}
						if(IDLoot == IDArmePorterParJoueur[i]){
							Loot.SendMessage("ReceiveSend",true);
							Ondrop = true;TimerRedrop = 1;
						}
					}
				}else{
					if(asObject){Destroy(FJ);asObject = false;}
					Loot.SendMessage("ReceiveSend",true);
					Ondrop = true;TimerRedrop = 1;	
				}
			}
		}
		if(other.tag == "MunitionLoot" && !Ondrop && !OnLootMunition && ArmePlaceSelection != 0 && !OnDialog && !OnInventaire){
			if(MunitionLoot != other.gameObject || MunitionLoot == null){
				MunitionLoot = other.gameObject;
				OnLootMunition = true;
				MunitionLoot.SendMessage("IDTest",IDArmePorterParJoueur[ArmePlaceSelection -1]);
			}
		}
	}
	IEnumerator CoupParCoup(){
		m_AnimatorMainRifle.SetBool("Shoot",true);
		yield return new WaitForSeconds(0.2f);	
		m_AnimatorMainRifle.SetBool("Shoot",false);
	}
	IEnumerator InfoCouroutine(){	
	yield return new WaitForSeconds(0.5f);	
	if(ArmePlaceSelection == SendAntichange){
		ArmePorterParJoueur[IDArmePorterParJoueur[ArmePlaceSelection - 1]].SendMessage("infoArme");	
		ArmePorterParJoueur[IDArmePorterParJoueur[ArmePlaceSelection - 1]].SendMessage("ReceiveMunition",Munition[SendAntichange - 1]);
		ArmePorterParJoueur[IDArmePorterParJoueur[ArmePlaceSelection - 1]].SendMessage("ReceiveMunitionChargeur",MunitionChargeur[ArmePlaceSelection - 1]);
	}
	if(IDArmeDejaPrisParJoueur[IDArmePorterParJoueur[ArmePlaceSelection - 1]] == 0){
		if(AsRifle){m_AnimatorMainRifle.SetBool("Inspect",true);}
		if(AsHandGun){m_AnimatorMainHandGun.SetBool("Inspect",true);}
		if(AsMelee){m_AnimatorMainMelee.SetBool("Inspect",true);}
		IDArmeDejaPrisParJoueur[IDArmePorterParJoueur[ArmePlaceSelection - 1]] = 1;
	}
	NoReceive = false;
	}
	void FindCadreArme(){
		Arme_1 = new GameObject[7];
		Arme_2 = new GameObject[7];
		Arme_3 = new GameObject[7];
		Arme_4 = new GameObject[7];
		ArmeGameObjectShadow = new GameObject[7];
		ArmeCadreSelection = new GameObject[5];
		ArmeCadreNonSelection = new GameObject[5];
		for(int a = 0; a < ArmeCadreSelection.Length;a++){
			ArmeCadreSelection[a] = GameObject.Find("sxvfbvnry_CinT_" + a.ToString());  
		}
		for(int b = 0; b < ArmeCadreSelection.Length;b++){
			ArmeCadreNonSelection[b] = GameObject.Find("sxvfbvnry_CNinT_" + b.ToString());  
		}
		
		for(int i = 0; i < Arme_1.Length;i++){Arme_1[i] = GameObject.Find("sxvfbvnry_1_ElinT_" + i.ToString());}
		for(int i = 0; i < Arme_2.Length;i++){Arme_2[i] = GameObject.Find("sxvfbvnry_2_ElinT_" + i.ToString());}
		for(int i = 0; i < Arme_3.Length;i++){Arme_3[i] = GameObject.Find("sxvfbvnry_3_ElinT_" + i.ToString());}
		for(int i = 0; i < Arme_4.Length;i++){Arme_4[i] = GameObject.Find("sxvfbvnry_4_ElinT_" + i.ToString());}	
		for(int z = 0; z < ArmeGameObjectShadow.Length;z++){ArmeGameObjectShadow[z] = GameObject.Find("abcdef_Arme_" + z.ToString());}
		ChangeCadre();
		ChangeArmeLogo();
	}
	void ChangeCadre(){
		bool EtatCadre;
		for(int i = 0; i < ArmeCadreSelection.Length;i++){
			EtatCadre = ArmePlaceSelection == i;
			ArmeCadreSelection[i].SetActive(EtatCadre);
			ArmeCadreNonSelection[i].SetActive(!EtatCadre);
		}
		TexteMunition.text = "No Ammo";
	}
	void ChangeArmeLogo(){
		for(int i = 0; i < Arme_1.Length;i++){
			Arme_1[i].SetActive(i == TypeArmePorterParJoueur[0]);
		}
		for(int i = 0; i < Arme_2.Length;i++){
			Arme_2[i].SetActive(i == TypeArmePorterParJoueur[1]);
		}
		for(int i = 0; i < Arme_3.Length;i++){
			Arme_3[i].SetActive(i == TypeArmePorterParJoueur[2]);
		}
		for(int i = 0; i < Arme_1.Length;i++){
			Arme_4[i].SetActive(i == TypeArmePorterParJoueur[3]);
		}
		for(int i = 0; i < ArmeGameObjectShadow.Length;i++){
			if(!(ArmePlaceSelection == 0)){
				ArmeGameObjectShadow[i].SetActive(i == TypeArmePorterParJoueur[ArmePlaceSelection - 1]);
			}else{
				ArmeGameObjectShadow[i].SetActive(false);	
			}
		}
		if(ArmePlaceSelection == 0){
			gameObject.SendMessage("ChangeCursor",CursorBase);
		}else{
			gameObject.SendMessage("ChangeCursor",IDCursor[ArmePlaceSelection - 1]);
		}
	}
	void SendArmeInfoRHA(){
		gameObject.SendMessage("AUneArmeDeTypeRifle",AsRifle);
		gameObject.SendMessage("AUneArmeDeTypeHandGun",AsHandGun);
		gameObject.SendMessage("AUneArmeDeTypeMelee",AsMelee);	
	}
	//Autre
	public void ReceiveDialogState(bool Dialog){OnDialog = Dialog; if(AsHandGun || AsRifle || AsMelee){ArmePorterParJoueur[IDArmePorterParJoueur[ArmePlaceSelection - 1]].SendMessage("ReceiveDialogArme",Dialog);}}
	public void ReceiveInventaireState(bool Inventaire){OnInventaire = Inventaire; if(AsHandGun || AsRifle || AsMelee){ArmePorterParJoueur[IDArmePorterParJoueur[ArmePlaceSelection - 1]].SendMessage("ReceiveInventaireState",Inventaire);}}
	public void ReceiveShoot(bool Shoot){if(changeAnimation == 2){if(!Input.GetButton("Sprint")){m_AnimatorMainRifle.SetBool("Shoot",Shoot);}if(changeAnimation == 3){m_AnimatorMainHandGun.SetBool("Shoot",Shoot);}}if(changeAnimation == 1){AttaqueEpee = Shoot;}}
	//Autre
	//Arme
	public void ReceiveCanShootAtack(bool CSHOOT){CanShootAtack = CSHOOT;}
	public void ReceiveMunition(float Mun){if(!NoReceive){Munition[ArmePlaceSelection - 1] = Mun;}}
	public void ReceiveMunitionMax(float MunMax){if(!NoReceive){MunitionMax[ArmePlaceSelection - 1] = MunMax;}}
	public void ReceiveMunitionChargeur(float MunChar){if(!NoReceive){MunitionChargeur[ArmePlaceSelection - 1] = MunChar;}}
	public void CursorId(int Id){IDReticule = Id;}
	public void ModeRHM(int RHM){ModRHM = RHM;AsRifle = false;AsHandGun = false;AsMelee = false;if(RHM == 1 || RHM == 5 || RHM == 4){AsRifle = true;}if(RHM == 2){AsHandGun = true;}if(RHM == 3){AsMelee = true;}}
	public void ReceiveCanreload(bool Can){CanRelod = Can;}
	public void ReceiveReload(bool can){receiveReload = can;}//active lanime comme si on apuiyer sur R
	public void ReceiveTypeDeRechargement(int TypeDeRechargement){TypeRechargement = TypeDeRechargement;}
	public void ReceiveCoupParCoup(bool CoupParCoup){TypeArmeCoupParCoup = CoupParCoup;}
	public void ReceiveAimAtackEpee(bool AimAtackEpee){AimAtaqueEpee = AimAtackEpee;}
	//Arme
	//Loot
	public void ReceiveIDArme(int ID){IDLoot = ID;OnLoot = true;ChangeArme = true;}
	public void SendIDVerif(int IDverif){IDLoot = IDverif;}
	public void ReceiveMunitionLoot(float MunLoot){MuniLoot = MunLoot;}
	public void	ReceiveMunitionMaxLoot(float MunMaxLoot){MuniMaxLoot = MunMaxLoot;}
	public void ReceiveChargeurLoot(float ChargeurLoot){ChargLoot = ChargeurLoot;}
	//MunitionLoot
	public void ComparaiSonID(bool SIOUI){
		if(SIOUI){
			Munition[ArmePlaceSelection -1] += MunitionLootInt;
			MunitionChargeur[ArmePlaceSelection -1] += MunitionChargerLootInt;
			m_AudioSource.clip = pickup;m_AudioSource.PlayOneShot(m_AudioSource.clip);
			Destroy(MunitionLoot);
			ArmePorterParJoueur[IDArmePorterParJoueur[ArmePlaceSelection - 1]].SendMessage("ReceiveMunition",Munition[ArmePlaceSelection - 1]);
			ArmePorterParJoueur[IDArmePorterParJoueur[ArmePlaceSelection - 1]].SendMessage("ReceiveMunitionChargeur",MunitionChargeur[ArmePlaceSelection - 1]);
			ArmePorterParJoueur[IDArmePorterParJoueur[ArmePlaceSelection - 1]].SendMessage("infoArme");	
		}
		OnLootMunition = false;
		MunitionLootInt = 0;
		MunitionChargerLootInt = 0;
	}
	public void LOOTMunition(int _Mun){MunitionLootInt = _Mun;}
	public void LOOTChargeur(int _Charg){MunitionChargerLootInt =_Charg;}
	//Loot
	
}
