using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class Arme
{
    public string Name = "";
    public GameObject Spawn = null;
    public GameObject ArmeInPlayer = null;
    public Vector3 HandPosRechage = new Vector3(0,0,0);
    public Vector3 HandPosNormal = new Vector3(0,0,0);
    public int IDIconTexture = 0;
    public float AttaqueRate = 0f;
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
    [SerializeField] private int EmplacementArme = 0; 
    [SerializeField] private Texture[] IConArmeTexture = null;
    private GameObject ObjectTrigger = null;

    public bool OnDialog,OnInventaire;
    void Update()
    {
        if(!OnDialog && !OnInventaire)
        {
            if(Input.GetButtonDown("Slot0") && EmplacementArme != 0)
            {
                EmplacementArme = 0;
            }
            if(Input.GetButtonDown("Slot1") && EmplacementArme != 1)
            {
                EmplacementArme = 1;
            }
            if(Input.GetButtonDown("Slot2") && EmplacementArme != 2)
            {
                EmplacementArme = 2;
            }
            if(Input.GetButtonDown("Slot3") && EmplacementArme != 3)
            {
                EmplacementArme = 3;
            }
            if(Input.GetButtonDown("Slot4") && EmplacementArme != 4)
            {
                EmplacementArme = 4;
            }
        }


    }

    void OnTriggerStay(Collider other)
    {
        if(other.tag == "ArmeLoot" && ObjectTrigger != other.transform.gameObject)
        {
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
                i = 5;
            }
        }
    }

    void RemoveArmeSlot(int Slot)
    {
        GameObject Arme = (GameObject)Instantiate(ArmesContent[IDAE[Slot].IDArme].Spawn, transform.position + new Vector3(0,2,0), Quaternion.Euler(0,0,0));
        Arme.GetComponent<ArmeLoot_S>().ID = IDAE[Slot].IDArme;
        Arme.GetComponent<ArmeLoot_S>().Munition = IDAE[Slot].Munition;
        Arme.GetComponent<ArmeLoot_S>().MunitionMax = IDAE[Slot].MunitionMax;
        Arme.GetComponent<ArmeLoot_S>().Chargeur = IDAE[Slot].Chargeur;
        IDAE[Slot].IDArme = 0;
        IDAE[Slot].Munition = 0;
        IDAE[Slot].MunitionMax = 0;
        IDAE[Slot].Chargeur = 0;
    }
}