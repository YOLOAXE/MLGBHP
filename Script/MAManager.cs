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
    public Vector3 HandPos = new Vector3(0, 0, 0);
    public Vector3 HandAngle = new Vector3(0, 0, 0);
    public int IDIconTexture = 0;
    public int TypeArme = 0;
    public int idReticule = 0;
    public bool ArmeDejaPrisParJoueur = false;
    public bool AsRifle = false, AsMelee = false, AsHandGun = false;
    public bool Scope = false;
    public Vector3 DB = new Vector3(0, 0, 0);
    public Vector3 GB = new Vector3(0, 0, 0);
    public Vector3 GBRechagement = new Vector3(0, 0, 0);
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
    [Header("Manager")]
    [SerializeField] private IDArmeEmplacement[] IDAE = new IDArmeEmplacement[5];
    [SerializeField] private Arme[] ArmesContent = null;
    [SerializeField] private AudioManage AudioContent = null;
    [SerializeField] private Texture[] IconArmeTexture = null;
    [SerializeField] private GameObject[] ArmeObject = null;
    [SerializeField] private int EmplacementArme = 0;
    [SerializeField] private Animator Arm_Animator = null;
    [SerializeField] private UIArme[] UAM = new UIArme[5];
    [SerializeField] private GameObject SpawnArmePoint = null;
    [SerializeField] private float ForceImpulseArme = 1f;
    [SerializeField] private StrafeMovement SM = null;
    [Header("Bras")]
    [SerializeField] private GameObject BrasGauche = null;
    [SerializeField] private GameObject BrasDroite = null;
    [SerializeField] private GameObject[] IAT = new GameObject[5];
    [Header("TakeObject")]
    [SerializeField] private GameObject takeObject = null;
    [SerializeField] private float takeDistance = 3f;

    private ArmeInfoMun AIM = new ArmeInfoMun();
    private GameObject ObjectTrigger = null;
    private TextMeshProUGUI AmmoTextMeshPro = null;
    private bool canScope = false;
    private int i = 0;

    public bool OnDialog, OnInventaire;

    void Awake()
    {
        chercheElment();
    }

    void Update()
    {
        if (!OnDialog && !OnInventaire)
        {
            for (i = 0; i < IDAE.Length; i++)
            {
                if (Input.GetButtonDown("Slot" + i.ToString()) && EmplacementArme != i)
                {
                    ResetBoolAnimator();
                    ArmeMiseAJour(i);
                }
            }
            if (Input.GetButtonDown("Drop"))
            {
                RemoveArmeSlot(EmplacementArme);
                ArmeMiseAJour(EmplacementArme);
            }
            if (canScope && Input.GetButtonDown("Fire2"))
            {
                gameObject.transform.GetChild(0).SendMessage("ReceiveZoom", true);
            }
            if (canScope && Input.GetButtonUp("Fire2"))
            {
                gameObject.transform.GetChild(0).SendMessage("ReceiveZoom", false);
            }
            if(Input.GetButtonDown("Interagire"))
            {
                TakeObject();
            }
            GetArmeInfo();
            MunitionTexte();
        }


    }

    void LateUpdate()
    {
        Arm_Animator.transform.localPosition = ArmesContent[IDAE[EmplacementArme].IDArme].HandPos;
        Arm_Animator.transform.localEulerAngles = ArmesContent[IDAE[EmplacementArme].IDArme].HandAngle;
        BrasDroite.transform.localPosition = BrasDroite.transform.localPosition + ArmesContent[IDAE[EmplacementArme].IDArme].DB;
        BrasGauche.transform.localPosition = BrasGauche.transform.localPosition + ArmesContent[IDAE[EmplacementArme].IDArme].GB;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "ArmeLoot" && ObjectTrigger != other.transform.gameObject)
        {
            if (!other.GetComponent<ArmeLoot_S>().canTake) { return; } // si l'object vien juste d'apparetre.
            ObjectTrigger = other.transform.gameObject;
            AddObjectSlot(ObjectTrigger);
            RemoveObject();
        }
        if(other.tag == "Object")
        {
            RemoveObject();
        }
    }

    void AddObjectSlot(GameObject Arme)
    {
        int i;
        for (i = 1; i < 5; i++)
        {
            if (IDAE[i].IDArme == Arme.GetComponent<ArmeLoot_S>().ID)// si le joueur possede deja l'arme.
            {
                if (ArmesContent[IDAE[i].IDArme].ArmeInPlayer.activeSelf)
                {
                    ArmesContent[IDAE[EmplacementArme].IDArme].ArmeInPlayer.GetComponent<ArmeShoot>().munition += Arme.GetComponent<ArmeLoot_S>().Munition;
                    ArmesContent[IDAE[EmplacementArme].IDArme].ArmeInPlayer.GetComponent<ArmeShoot>().munitionChargeur += Arme.GetComponent<ArmeLoot_S>().Chargeur;
                    IDAE[i].Munition = ArmesContent[IDAE[EmplacementArme].IDArme].ArmeInPlayer.GetComponent<ArmeShoot>().munition;
                    IDAE[i].Chargeur = ArmesContent[IDAE[EmplacementArme].IDArme].ArmeInPlayer.GetComponent<ArmeShoot>().munitionChargeur;
                }
                else
                {
                    ObjectTrigger = null;
                    return;
                }
                Destroy(Arme);
                AudioPlayOneShot(AudioContent.pickup);
                return;
            }
        }
        for (i = 1; i < 5; i++)
        {
            if (IDAE[i].IDArme < 1)// si le joueur possede un emplacement vide.
            {
                IDAE[i].IDArme = Arme.GetComponent<ArmeLoot_S>().ID;
                IDAE[i].Munition = Arme.GetComponent<ArmeLoot_S>().Munition;
                IDAE[i].MunitionMax = Arme.GetComponent<ArmeLoot_S>().MunitionMax;
                IDAE[i].Chargeur = Arme.GetComponent<ArmeLoot_S>().Chargeur;
                ArmesContent[IDAE[i].IDArme].ArmeInPlayer.SetActive(true);
                ArmesContent[IDAE[i].IDArme].ArmeInPlayer.GetComponent<ArmeShoot>().munition = IDAE[i].Munition;
                ArmesContent[IDAE[i].IDArme].ArmeInPlayer.GetComponent<ArmeShoot>().munitionChargeur = IDAE[i].Chargeur;
                ArmesContent[IDAE[i].IDArme].ArmeInPlayer.SetActive(false);
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
        if (IDAE[Slot].IDArme == 0)
        {
            return;
        }
        RaycastHit hit;
        Ray ray = new Ray(transform.GetChild(0).position, transform.GetChild(0).forward);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            ArmesContent[IDAE[EmplacementArme].IDArme].ArmeInPlayer.SetActive(false);
            GameObject Arme = (GameObject)Instantiate(ArmesContent[IDAE[Slot].IDArme].Spawn, SpawnArmePoint.transform.position, Quaternion.Euler(transform.GetChild(0).eulerAngles));
            Arme.GetComponent<ArmeLoot_S>().ID = IDAE[Slot].IDArme;
            Arme.GetComponent<ArmeLoot_S>().Munition = ArmesContent[IDAE[EmplacementArme].IDArme].ArmeInPlayer.GetComponent<ArmeShoot>().munition;
            Arme.GetComponent<ArmeLoot_S>().MunitionMax = ArmesContent[IDAE[EmplacementArme].IDArme].ArmeInPlayer.GetComponent<ArmeShoot>().munitionMax;
            Arme.GetComponent<ArmeLoot_S>().Chargeur = ArmesContent[IDAE[EmplacementArme].IDArme].ArmeInPlayer.GetComponent<ArmeShoot>().munitionChargeur;
            IDAE[Slot].IDArme = 0;
            IDAE[Slot].Munition = 0;
            IDAE[Slot].MunitionMax = 0;
            IDAE[Slot].Chargeur = 0;
            if (hit.distance > 1.5f)
            {
                Arme.GetComponent<Rigidbody>().AddForce(transform.GetChild(0).forward * ForceImpulseArme, ForceMode.Impulse);
            }
            else
            {
                Arme.transform.position = transform.GetChild(0).position + new Vector3(0f, 1f, 0f);
            }
        }
        CadreEmplacementMiseAjour();
    }

    void AudioPlayOneShot(AudioClip Sound)
    {
        AudioContent.m_AudioSource.clip = Sound;
        AudioContent.m_AudioSource.PlayOneShot(AudioContent.m_AudioSource.clip);
    }

    void Chercher()
    {
        for (i = 0; i < UAM.Length; i++)
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
        for (i = 0; i < IAT.Length && IAT[i] != null; i++)
        {
            IAT[i].GetComponent<UnityEngine.UI.RawImage>().texture = IconArmeTexture[ArmesContent[IDAE[i].IDArme].IDIconTexture];
        }
        for (i = 0; i < ArmeObject.Length; i++)
        {
            ArmeObject[i].SetActive(ArmesContent[IDAE[EmplacementArme].IDArme].IDIconTexture == i);
        }
        Arm_Animator.transform.localPosition = ArmesContent[IDAE[EmplacementArme].IDArme].HandPos;
        Arm_Animator.transform.localEulerAngles = ArmesContent[IDAE[EmplacementArme].IDArme].HandAngle;
        SM.AsRifle = ArmesContent[IDAE[EmplacementArme].IDArme].AsRifle;
        SM.AsMelee = ArmesContent[IDAE[EmplacementArme].IDArme].AsMelee;
        SM.AsHandGun = ArmesContent[IDAE[EmplacementArme].IDArme].AsHandGun;
        SendMessage("ChangeCursor", ArmesContent[IDAE[EmplacementArme].IDArme].idReticule);
        canScope = ArmesContent[IDAE[EmplacementArme].IDArme].Scope;
    }

    void chercheElment()
    {
        Chercher();
        CadreEmplacementMiseAjour();
        AmmoTextMeshPro = GameObject.Find("AmmoTextMeshPro").GetComponent<TextMeshProUGUI>();
        for (i = 0; i < 5; i++)
        {
            IAT[i] = GameObject.Find("IAT_Invent_" + i.ToString());
        }
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
                AmmoTextMeshPro.text = "-" + AIM.ConsumeMana.ToString() + "MP";
            }
            else if (AIM.NoAmmo)
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
        Arm_Animator.SetBool("Inspect", false);
        Arm_Animator.SetBool("Rechargement", false);
        Arm_Animator.SetBool("RechargementOutOfAmmo", false);
        Arm_Animator.SetBool("RechargementOutOfAmmo", false);
        Arm_Animator.SetBool("Shoot", false);
        Arm_Animator.SetBool("Holster", false);
        Arm_Animator.SetBool("Run", false);
        Arm_Animator.SetBool("Aim", false);
        Arm_Animator.SetBool("Walk", false);
    }

    void TakeObject()
    {
        if (takeObject != null)
        {
            if (takeObject.GetComponent<FixedJoint>())
            {
                Destroy(takeObject.GetComponent<FixedJoint>());
            }
            takeObject = null;
        }
        else
        {
            RaycastHit hit;
            Ray ray = new Ray(transform.GetChild(0).position, transform.GetChild(0).forward);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.transform.tag == "Object" && hit.distance <= takeDistance)
                {
                    takeObject = hit.transform.gameObject;
                    takeObject.AddComponent<FixedJoint>();
                    takeObject.GetComponent<FixedJoint>().connectedBody = transform.GetChild(0).GetComponent<Rigidbody>();
                    takeObject.GetComponent<FixedJoint>().breakForce = 10000;
                    takeObject.GetComponent<FixedJoint>().breakTorque = 10000;
                    StartCoroutine(ConstanteUpdate());
                }
                if(hit.transform.tag == "ArmeLoot" && hit.distance <= (takeDistance + 1f))
                {
                    hit.transform.position = transform.position;
                }
            }
        }
    }

    void RemoveObject()
    {
        if (takeObject != null)
        {
            if (takeObject.GetComponent<FixedJoint>())
            {
                Destroy(takeObject.GetComponent<FixedJoint>());
            }
            takeObject = null;
        }
    }

    IEnumerator ConstanteUpdate()
    {
        bool Stop = false;
        while (!Stop)
        {
            if (takeObject != null)
            {
                Stop = !takeObject.GetComponent<FixedJoint>();
            }
            else
            {
                Stop = true;
            }
            yield return new WaitForSeconds(0.1f);
            transform.GetChild(0).transform.eulerAngles = new Vector3(transform.GetChild(0).transform.eulerAngles.x, transform.GetChild(0).transform.eulerAngles.y, transform.GetChild(0).transform.eulerAngles.z);
        }
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