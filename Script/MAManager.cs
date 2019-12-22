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
    [SerializeField] private Texture Icon = null;
    [SerializeField] private float AttaqueRate = 0f;
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
    [SerializeField] private IDArmeEmplacement[] IDAE = new IDArmeEmplacement[4];
    [SerializeField] private Arme[] ArmesContent = null;

    [SerializeField] private AudioManage AudioContent = null;
    void Update()
    {

    }
}