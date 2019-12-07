using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractePnj : MonoBehaviour
{
	public float DistanceInteractionPNJ;
	private bool OnDialog = true;
	
    void Update()
    {
        RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if(Input.GetButtonDown("Interagire") && !Input.GetButton("Inventaire")){
			if (Physics.Raycast(ray, out hit)){
				if(hit.transform.tag == "PNJ" || hit.transform.tag == "Soldat"){
					if(hit.distance <= DistanceInteractionPNJ){
						if(hit.transform.GetComponent<DialogueTrigger>() != null){
							hit.transform.SendMessage("ReceiveDialogState",OnDialog);
						}
					}
				}
			}
		}
    }
}
