using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PNJMoveHead : MonoBehaviour
{
    private GameObject Player;
	public float DistanceRegards,vitesseRotation,Angle;
	private float DistancePlayer;
	private Vector3 PointDeVue;
	private Vector3 targetPoint;
	
    void Start()
    {
       Player = GameObject.FindWithTag("player");
	   RaycastHit hitForward;
	   if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitForward)){
		PointDeVue = hitForward.point;
	   }
    }

    // Update is called once per frame
    void Update()
    {
        DistancePlayer = Vector3.Distance(transform.position,Player.transform.position);
		if(DistancePlayer <= DistanceRegards){
			targetPoint = Player.transform.position - transform.position;
		}else{
			targetPoint = PointDeVue - transform.position;
		}
			float step = vitesseRotation * Time.deltaTime;
			Vector3 newDir = Vector3.RotateTowards(transform.forward, targetPoint, step, 0.0f);
			Vector3 Correction = new Vector3(newDir.x,0,newDir.z);
			if(Correction.x > Angle){
			transform.rotation = Quaternion.LookRotation(Correction);
			}
		
	}
}
