using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunImage : MonoBehaviour
{
	public GameObject[] Emplancement1;
	public GameObject[] Emplancement2;
	public GameObject[] Emplancement3;
	public GameObject[] Emplancement4;
	
	public GameObject[] EmplancementI1;
	public GameObject[] EmplancementI2;
	public GameObject[] EmplancementI3;
	public GameObject[] EmplancementI4;
	
    public void SetImage()
    {
		for(int i = 0;i < Emplancement1.Length;i++){
			EmplancementI1[i].SetActive(Emplancement1[i].activeSelf);
		}
		for(int i = 0;i < Emplancement2.Length;i++){
			EmplancementI2[i].SetActive(Emplancement2[i].activeSelf);
		}
		for(int i = 0;i < Emplancement3.Length;i++){
			EmplancementI3[i].SetActive(Emplancement3[i].activeSelf);
		}
		for(int i = 0;i < Emplancement4.Length;i++){
			EmplancementI4[i].SetActive(Emplancement4[i].activeSelf);
		}
    }
}
