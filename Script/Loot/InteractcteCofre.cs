using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractcteCofre : MonoBehaviour
{
	public float DistanceInteractionCoffre;
	private bool OnDialog = true;

    void Update()
    {
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if(Input.GetButtonDown("Interagire")){
			if (Physics.Raycast(ray, out hit)){
				if(hit.transform.tag == "Coffre"){
					if(hit.distance <= DistanceInteractionCoffre){
							hit.transform.SendMessage("Open");
					}
				}
			}
		}
    }
}
