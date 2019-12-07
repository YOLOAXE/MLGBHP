using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MunitionLoot : MonoBehaviour
{
	public int[] ID;
	public int Munition,Chargeur;
	private bool ComparaiSonID = false;
	private GameObject Player;
	public TextMeshProUGUI TexteMunition,TexteChargeur;

	void Start(){
	Player = GameObject.FindWithTag("player");
	TexteMunition.text = "Munition: " + Munition.ToString();
	TexteChargeur.text = "Chargeur: " + Chargeur.ToString();
	}
	public void IDTest(int IDTest){
		for(int i = 0;i< ID.Length;i++){
			if(!ComparaiSonID){ComparaiSonID = ID[i] == IDTest;}
		}
		Player.SendMessage("LOOTMunition",Munition);
		Player.SendMessage("LOOTChargeur",Chargeur);
		Player.SendMessage("ComparaiSonID",ComparaiSonID);
	}
}
