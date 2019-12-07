

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LumierrePlayer : MonoBehaviour
{
	public float VitesseDeplacement,VitesseDeplacementWarning,DistanceArret;
	public GameObject Player,LightBoule;
	private float HitDistanceUp,HitDistanceDown,HitDistanceRight,HitDistanceLeft,HitDistanceForward,HitDistanceBackward,Distance;
	private Vector3 hitPointCam,XYZ;
	private Rigidbody rb;
	private bool Impulse,mode;
	
    void Start(){
		StartCoroutine(OnStartIntensitySet());
		rb = GetComponent<Rigidbody>();
		Player = GameObject.FindWithTag("player") ;
    }
	
    void Update(){
		RaycastHit hitUp;
		RaycastHit hitDown;
		RaycastHit hitLeft;
		RaycastHit hitRight;
		RaycastHit hitForward;
		RaycastHit hitBackward;
		RaycastHit hitCam;
		
		Ray rayCam = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(rayCam, out hitCam)){hitPointCam = hitCam.point;}
		
		if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up), out hitUp)){HitDistanceUp = hitUp.distance;}
		if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hitDown)){HitDistanceDown = hitDown.distance;}
		if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.left), out hitLeft)){HitDistanceLeft = hitLeft.distance;}
		if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), out hitRight)){HitDistanceRight = hitRight.distance;}
		if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitForward)){HitDistanceForward = hitForward.distance;}
		if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.back), out hitBackward)){HitDistanceBackward = hitBackward.distance;}
		
		if(Vector3.Distance(transform.position,hitPointCam) > 8 ){XYZ = Player.transform.position + new Vector3(0,2,0);}else{XYZ = hitPointCam;}
		if(Input.GetButtonDown("Interagire")){mode = !mode;}
    }
	void FixedUpdate(){
		if(HitDistanceUp <= DistanceArret){rb.AddForce(Vector3.down * VitesseDeplacementWarning);}
		if(HitDistanceDown <= DistanceArret){rb.AddForce(Vector3.up * VitesseDeplacementWarning);}
		if(HitDistanceLeft <= DistanceArret){rb.AddForce(Vector3.right * VitesseDeplacementWarning);}
		if(HitDistanceRight <= DistanceArret){rb.AddForce(Vector3.left * VitesseDeplacementWarning);}
		if(HitDistanceForward <= DistanceArret){rb.AddForce(Vector3.back * VitesseDeplacementWarning);}
		if(HitDistanceBackward <= DistanceArret){rb.AddForce(Vector3.forward * VitesseDeplacementWarning);}
		if(mode){
			Distance = Vector3.Distance(Player.transform.position,transform.position);
			if(Distance > DistanceArret){
				float step = (VitesseDeplacement * (Distance * 0.5f)) * Time.deltaTime;
				transform.position = Vector3.MoveTowards(transform.position, Player.transform.position, step);
			}
			rb.velocity = Vector3.zero; 
			Impulse = true;
		}else{
			if(Impulse){
				Vector3 DirectionTarget = Player.transform.position - transform.position;
				if(Distance < 10){
					rb.AddForce(DirectionTarget * (Distance * 0.1f), ForceMode.Impulse);
				}else{
					rb.AddForce(DirectionTarget * (10 * 0.1f), ForceMode.Impulse);
				}
				Impulse = false;
			}
			if(XYZ.x > transform.position.x){rb.AddRelativeForce(Vector3.right * VitesseDeplacement);}else{rb.AddRelativeForce(Vector3.left * VitesseDeplacement);}
			if(XYZ.y > transform.position.y){rb.AddRelativeForce(Vector3.up * VitesseDeplacement);}else{rb.AddRelativeForce(Vector3.down * VitesseDeplacement);}
			if(XYZ.z > transform.position.z){rb.AddRelativeForce(Vector3.forward * VitesseDeplacement);}else{rb.AddRelativeForce(Vector3.back * VitesseDeplacement);}
		}
    }
	IEnumerator OnStartIntensitySet(){
		LightBoule.GetComponent<Light>().intensity = 0f;
		while(LightBoule.GetComponent<Light>().intensity < 2f){
			LightBoule.GetComponent<Light>().intensity += 0.1f;
			yield return new WaitForSeconds(0.01f); 			
		}
		yield return new WaitForSeconds(0.1f); 
	}
}