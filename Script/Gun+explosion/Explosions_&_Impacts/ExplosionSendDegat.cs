using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionSendDegat : MonoBehaviour
{
	public float DegatSend,DistanceSend;
	public GameObject Player;
	public GameObject Camera;
	public float EECS_duration,EECS_magnitude;
	public GameObject[] Enemie;
	public GameObject[] EnemiePierre;
	public GameObject[] EnemieBleu;
	public GameObject[] EnemieVert;
	public GameObject[] Crystal;
    void Start()
    {
		Camera = GameObject.FindWithTag("MainCamera");
		Player = GameObject.FindWithTag("player");
        Enemie = GameObject.FindGameObjectsWithTag("Enemie");
		EnemiePierre = GameObject.FindGameObjectsWithTag("EnemiePierre");
		EnemieBleu = GameObject.FindGameObjectsWithTag("EnemieBleu");
		EnemieVert = GameObject.FindGameObjectsWithTag("EnemieVert");
		Crystal = GameObject.FindGameObjectsWithTag("Crystal");
		float distanceCalcule1 = Vector3.Distance(Player.transform.position,transform.position);
		if(distanceCalcule1 < DistanceSend){Player.SendMessage("ReceiveDamage",DegatSend * (1-(distanceCalcule1/DistanceSend)));}
		
			for(int i = 0;i < Enemie.Length;i++){
				float distanceCalcule2 = Vector3.Distance(Enemie[i].transform.position,transform.position);
				if(distanceCalcule2 < DistanceSend){
					float CalculeDegat = Mathf.RoundToInt(DegatSend * (1-(distanceCalcule2/DistanceSend)));
					SendDamageEnemie(Enemie[i],CalculeDegat);
				}
			}
		

			for(int i = 0;i < EnemiePierre.Length;i++){
				float distanceCalcule3 = Vector3.Distance(EnemiePierre[i].transform.position,transform.position);
					if(distanceCalcule3 < DistanceSend){
						float CalculeDegat = Mathf.RoundToInt(DegatSend * (1-(distanceCalcule3/DistanceSend)));
						SendDamageEnemie(EnemiePierre[i],CalculeDegat);
					}
			}

			for(int i = 0;i < EnemieBleu.Length;i++){
				float distanceCalcule4 = Vector3.Distance(EnemieBleu[i].transform.position,transform.position);
				if(distanceCalcule4 < DistanceSend){
					float CalculeDegat = Mathf.RoundToInt(DegatSend * (1-(distanceCalcule4/DistanceSend)));
					SendDamageEnemie(EnemieBleu[i],CalculeDegat);
				}
			}

			for(int i = 0;i < EnemieVert.Length;i++){
				float distanceCalcule5 = Vector3.Distance(EnemieVert[i].transform.position,transform.position);
				if(distanceCalcule5 < DistanceSend){
					float CalculeDegat = Mathf.RoundToInt(DegatSend * (1-(distanceCalcule5/DistanceSend)));
					SendDamageEnemie(EnemieVert[i],CalculeDegat);
				}
			}

			for(int i = 0;i < Crystal.Length;i++){
				float distanceCalcule6 = Vector3.Distance(Crystal[i].transform.position,transform.position);
				if(distanceCalcule6 < DistanceSend){
					float CalculeDegat = Mathf.RoundToInt(DegatSend * (1-(distanceCalcule6/DistanceSend)));
					SendDamageEnemie(Crystal[i],CalculeDegat);
				}
			}
		if(distanceCalcule1 < 200){
			StartCoroutine(Camera.GetComponent<ExplosionShake>().Shake(EECS_duration,EECS_magnitude * (1-(distanceCalcule1/200))));
		}
    }
	void SendDamageEnemie(GameObject Enemie,float Degat){
		Enemie.SendMessage("ReceiveDamagePlayer",Degat);
	}
}
