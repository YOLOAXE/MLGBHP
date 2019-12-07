using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oiseau : MonoBehaviour
{
    
	public GameObject[] CircuitOiseau;
	public float DistancePoint,VitesseDeplacement = 1,vitesseRotation;
	public int Point = 0;

    void Update()
    {
		DistancePoint = Vector3.Distance(CircuitOiseau[Point].transform.position,transform.position);
		if(DistancePoint <= 2){Point++;}
		transform.Translate(Vector3.forward * Time.deltaTime * VitesseDeplacement);
		Vector3 targetPoint = CircuitOiseau[Point].transform.position - transform.position;
		float step = vitesseRotation * Time.deltaTime;
		Vector3 newDir = Vector3.RotateTowards(transform.forward, targetPoint, step, 0.0f);
		transform.rotation = Quaternion.LookRotation(newDir);
		if(Point >= CircuitOiseau.Length - 1){
		Point = 0;
		}			
    }
}
