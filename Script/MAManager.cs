using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class ArmeInfoMun
{
    public int munition = 0;
    public int munitionChargeur = 0;
    public float ConsumeMana = 0f;
    public float cadence = 0.1f;
    public float damage = 0f;
    public bool useMana = false;
    public bool NoAmmo = false;
}

[System.Serializable]
public class UIArme
{
    public GameObject Emplacement;
    public GameObject CadreActive;
    public GameObject CadreDesactive;
}

[System.Serializable]
public class Arme
{
    public string Name = "";
    public GameObject Spawn = null;
    public GameObject ArmeInPlayer = null;
    public Vector3 HandPosRechage = new Vector3(0,0,0);
    public Vector3 HandPosNormal = new Vector3(0,0,0);
    public int IDIconTexture = 0;
    public int TypeArme = 0;
    public bool ArmeDejaPrisParJoueur = false;
}

[System.Serializable]
public class IDArmeEmplacement
{
    public int IDArme = 0;
    public int Munition = 0;
    public int MunitionMax = 0;
    public int Chargeur = 0;
}
[System.Serializable]
public class AudioManage
{
    public AudioSource m_AudioSource = null;
	public AudioClip HosterWeapon = null;
	public AudioClip TakeWeapon = null;
	public AudioClip pickup = null;
}
public class MAManager : MonoBehaviour
{
    [SerializeField] private IDArmeEmplacement[] IDAE = new IDArmeEmplacement[5];
    [SerializeField] private Arme[] ArmesContent = null;
    [SerializeField] private AudioManage AudioContent = null; 
    [SerializeField] private Texture[] IconArmeTexture = null;
    [SerializeField] private int EmplacementArme = 0;
    [SerializeField] private Animator Arm_Animator = null;
    [SerializeField] private UIArme[] UAM = new UIArme[5];

    private ArmeInfoMun AIM = new ArmeInfoMun();
    private GameObject ObjectTrigger = null;
    private TextMeshProUGUI AmmoTextMeshPro = null;
    private int i = 0;

    public bool OnDialog,OnInventaire;

    void Start()
    {
        chercheElment();
    }

    void Update()
    {
        if(!OnDialog && !OnInventaire)
        {
            for(i = 0;i < IDAE.Length; i++)
            {
                if (Input.GetButtonDown("Slot" + i.ToString()) && EmplacementArme != i)
                {
                    ResetBoolAnimator();
                    ArmeMiseAJour(i);
                }
            }
            if(Input.GetButtonDown("Drop"))
            {
                RemoveArmeSlot(EmplacementArme);
                ArmeMiseAJour(EmplacementArme);
            }
            GetArmeInfo();
            MunitionTexte();     
        }


    }

    void OnTriggerStay(Collider other)
    {
        if(other.tag == "ArmeLoot" && ObjectTrigger != other.transform.gameObject)
        {
            if(!other.GetComponent<ArmeLoot_S>().canTake){return;} // si le l'object vien juste d'apparetre.
            ObjectTrigger = other.transform.gameObject;
            AddObjectSlot(ObjectTrigger);
        }
    }

    void AddObjectSlot(GameObject Arme)
    {
        int i;
        for(i = 1;i < 5;i++)
        {
            if(IDAE[i].IDArme < 1)
            {
                IDAE[i].IDArme = Arme.GetComponent<ArmeLoot_S>().ID;
                IDAE[i].Munition = Arme.GetComponent<ArmeLoot_S>().Munition;
                IDAE[i].MunitionMax = Arme.GetComponent<ArmeLoot_S>().MunitionMax;
                IDAE[i].Chargeur = Arme.GetComponent<ArmeLoot_S>().Chargeur;
                Destroy(Arme);
                ResetBoolAnimator();
                ArmeMiseAJour(i);
                i = 5;
                AudioPlayOneShot(AudioContent.pickup);
            }
        }
        CadreEmplacementMiseAjour();
    }

    void RemoveArmeSlot(int Slot)
    {
        if(IDAE[Slot].IDArme == 0)
        {
            return;
        }
        GameObject Arme = (GameObject)Instantiate(ArmesContent[IDAE[Slot].IDArme].Spawn, transform.position + new Vector3(0,2,0), Quaternion.Euler(0,0,0));
        Arme.GetComponent<ArmeLoot_S>().ID = IDAE[Slot].IDArme;
        Arme.GetComponent<ArmeLoot_S>().Munition = IDAE[Slot].Munition;
        Arme.GetComponent<ArmeLoot_S>().MunitionMax = IDAE[Slot].MunitionMax;
        Arme.GetComponent<ArmeLoot_S>().Chargeur = IDAE[Slot].Chargeur;
        IDAE[Slot].IDArme = 0;
        IDAE[Slot].Munition = 0;
        IDAE[Slot].MunitionMax = 0;
        IDAE[Slot].Chargeur = 0;
        CadreEmplacementMiseAjour();
    }

    void AudioPlayOneShot(AudioClip Sound)
    {
        AudioContent.m_AudioSource.clip = Sound;
        AudioContent.m_AudioSource.PlayOneShot(AudioContent.m_AudioSource.clip);
    }

    void Chercher()
    {
        for(i = 0;i < UAM.Length;i++)
        {
            UAM[i].Emplacement = GameObject.Find("sxvfbvnry_" + i.ToString() + "_ElinT");
            UAM[i].CadreActive = GameObject.Find("sxvfbvnry_CinT_" + i.ToString());
            UAM[i].CadreDesactive = GameObject.Find("sxvfbvnry_CNinT_" + i.ToString());
        }
    }

    void CadreEmplacementMiseAjour()
    {
        for (i = 0; i < UAM.Length; i++)
        {
            UAM[i].Emplacement.GetComponent<UnityEngine.UI.RawImage>().texture = IconArmeTexture[ArmesContent[IDAE[i].IDArme].IDIconTexture];
            UAM[i].CadreActive.SetActive(EmplacementArme == i);
            UAM[i].CadreDesactive.SetActive(EmplacementArme != i);
        }     
    }

    void chercheElment()
    {
        Chercher();
        CadreEmplacementMiseAjour();
        AmmoTextMeshPro = GameObject.Find("AmmoTextMeshPro").GetComponent<TextMeshProUGUI>();
    }

    void ArmeMiseAJour(int i)
    {
        ArmesContent[IDAE[EmplacementArme].IDArme].ArmeInPlayer.SetActive(false);
        EmplacementArme = i;
        if (i == 0)
        {
            AudioPlayOneShot(AudioContent.HosterWeapon);
        }
        else
        {
            AudioPlayOneShot(AudioContent.TakeWeapon);
        }
        CadreEmplacementMiseAjour();
        Arm_Animator.SetInteger("TypeArme", ArmesContent[IDAE[EmplacementArme].IDArme].TypeArme);
        Arm_Animator.Play("Enter", 0, 0.25f);
        ArmesContent[IDAE[EmplacementArme].IDArme].ArmeInPlayer.SetActive(true);
        if (IDAE[EmplacementArme].IDArme > 0)
        {
            AIM.munition = ArmesContent[IDAE[EmplacementArme].IDArme].ArmeInPlayer.GetComponent<ArmeShoot>().munition;
            AIM.munitionChargeur = ArmesContent[IDAE[EmplacementArme].IDArme].ArmeInPlayer.GetComponent<ArmeShoot>().munitionChargeur;
            AIM.ConsumeMana = ArmesContent[IDAE[EmplacementArme].IDArme].ArmeInPlayer.GetComponent<ArmeShoot>().ConsumeMana;
            AIM.cadence = ArmesContent[IDAE[EmplacementArme].IDArme].ArmeInPlayer.GetComponent<ArmeShoot>().cadence;
            AIM.damage = ArmesContent[IDAE[EmplacementArme].IDArme].ArmeInPlayer.GetComponent<ArmeShoot>().damage;
            AIM.useMana = ArmesContent[IDAE[EmplacementArme].IDArme].ArmeInPlayer.GetComponent<ArmeShoot>().useMana;
            AIM.NoAmmo = ArmesContent[IDAE[EmplacementArme].IDArme].ArmeInPlayer.GetComponent<ArmeShoot>().NoAmmo;
        }
    }

    void MunitionTexte()
    {
        if (IDAE[EmplacementArme].IDArme != 0)
        {
            if (AIM.useMana)
            {
                AmmoTextMeshPro.text = "-" + AIM.ConsumeMana.ToString() +"MP";
            }
            else if(AIM.NoAmmo)
            {
                AmmoTextMeshPro.text = "No Ammo";
            }
            else
            {
                AmmoTextMeshPro.text = AIM.munition.ToString() + " / " + AIM.munitionChargeur.ToString();
            }
        }
        else 
        {
            AmmoTextMeshPro.text = "No Ammo";
        }
    }

    void GetArmeInfo()
    {
        if (IDAE[EmplacementArme].IDArme > 0)
        {
            AIM.munition = ArmesContent[IDAE[EmplacementArme].IDArme].ArmeInPlayer.GetComponent<ArmeShoot>().munition;
            AIM.munitionChargeur = ArmesContent[IDAE[EmplacementArme].IDArme].ArmeInPlayer.GetComponent<ArmeShoot>().munitionChargeur;
        }
    }

    void ResetBoolAnimator()
    {
        Arm_Animator.SetBool("Inspect",false);
        Arm_Animator.SetBool("Rechargement", false);
        Arm_Animator.SetBool("RechargementOutOfAmmo", false);
        Arm_Animator.SetBool("RechargementOutOfAmmo", false);
        Arm_Animator.SetBool("Shoot", false);
        Arm_Animator.SetBool("Holster", false);
        Arm_Animator.SetBool("Run", false);
        Arm_Animator.SetBool("Aim", false);
        Arm_Animator.SetBool("Walk", false);
    }

    public void ReceiveDialogState(bool OD)
    {
        OnDialog = OD;
        if (IDAE[EmplacementArme].IDArme > 0)
        {
            if (ArmesContent[IDAE[EmplacementArme].IDArme].ArmeInPlayer != null)
            {
                if (ArmesContent[IDAE[EmplacementArme].IDArme].ArmeInPlayer.activeSelf)
                {
                    ResetBoolAnimator();
                    Arm_Animator.SetBool("Holster", OnDialog);
                    ArmesContent[IDAE[EmplacementArme].IDArme].ArmeInPlayer.GetComponent<ArmeShoot>().OnAction = OnDialog;
                }
            }
        }
    }

    public void ReceiveInventaireState(bool OI)
    {
        OnInventaire = OI;
        if (IDAE[EmplacementArme].IDArme > 0)
        {
            if (ArmesContent[IDAE[EmplacementArme].IDArme].ArmeInPlayer != null)
            {
                if (ArmesContent[IDAE[EmplacementArme].IDArme].ArmeInPlayer.activeSelf)
                {
                    ResetBoolAnimator();
                    Arm_Animator.SetBool("Holster", OnInventaire);
                    ArmesContent[IDAE[EmplacementArme].IDArme].ArmeInPlayer.GetComponent<ArmeShoot>().OnAction = OnInventaire;
                }
            }
        }
    }
}