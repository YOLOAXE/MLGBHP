using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class Arme
{
    [SerializeField] private string Name = "";
    [SerializeField] private GameObject Spawn = null;
    [SerializeField] private GameObject ArmeInPlayer = null;
    [SerializeField] private Vector3 HandPosRechage = new Vector3(0,0,0);
    [SerializeField] private Vector3 HandPosNormal = new Vector3(0,0,0);
    [SerializeField] private int IDIconTexture = 0;
    [SerializeField] private float AttaqueRate = 0f;
    [SerializeField] private int TypeArme = 0;
    [SerializeField] private bool ArmeDejaPrisParJoueur = false;
}

[System.Serializable]
public class IDArmeEmplacement
{
    [SerializeField] private int IDArme;
    [SerializeField] private int Munition;
    [SerializeField] private int MunitionMax;
    [SerializeField] private int Chargeur;
}
[System.Serializable]
public class AudioManage
{
    [SerializeField] private AudioSource m_AudioSource = null;
	[SerializeField] private AudioClip HosterWeapon = null;
	[SerializeField] private AudioClip TakeWeapon = null;
	[SerializeField] private AudioClip pickup = null;
}
public class MAManager : MonoBehaviour
{
    [SerializeField] private IDArmeEmplacement[] IDAE = new IDArmeEmplacement[5];
    [SerializeField] private Arme[] ArmesContent = null;
    [SerializeField] private AudioManage AudioContent = null;
    [SerializeField] private int EmplacementArme = 0; 
    [SerializeField] private Texture[] IConArmeTexture = null;

    public bool OnDialog,OnInventaire;
    void Update()
    {
        if(!OnDialog && !OnInventaire)
        {
            if(Input.GetButtonDown("Slot0") && EmplacementArme != 0)
            {

            }
            if(Input.GetButtonDown("Slot1") && EmplacementArme != 1)
            {

            }
            if(Input.GetButtonDown("Slot2") && EmplacementArme != 2)
            {

            }
            if(Input.GetButtonDown("Slot3") && EmplacementArme != 3)
            {

            }
            if(Input.GetButtonDown("Slot4") && EmplacementArme != 4)
            {

            }
        }


    }
}