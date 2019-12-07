using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnnimationGun : MonoBehaviour
{
	public GameObject target;
	private Vector3 Base,Calcule;
	
	void Start(){
		Base = transform.position;
		transform.position = target.transform.position;
		Calcule = Base - target.transform.position;
	}
    void Update(){
        transform.position = target.transform.position + Calcule;
    }
}
