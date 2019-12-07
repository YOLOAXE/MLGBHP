using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmeLoot : MonoBehaviour
{
	public int ID;
	public float Munition,Chargeur,MunitionMax;
	private GameObject Player;
	public int ModeRHM = 1;
	public int CursorId;

	void Start(){
	Player = GameObject.FindWithTag("player");
	}
	public void ReceiveSend(bool SendInfo){
		Player.SendMessage("ReceiveIDArme",ID);
		Player.SendMessage("ReceiveMunitionLoot",Munition);
		Player.SendMessage("ReceiveChargeurLoot",Chargeur);
		Player.SendMessage("ReceiveMunitionMaxLoot",MunitionMax);
		Player.SendMessage("CursorId",CursorId);
		Player.SendMessage("ModeRHM",ModeRHM);
	}
	public void ReceiveIDArme(int RID){ID = RID;}
	public void ReceiveMunitionLoot(float RMunition){Munition = RMunition;}
	public void ReceiveChargeurLoot (float RChargeur){Chargeur = RChargeur;}
	public void SendID(){Player.SendMessage("SendIDVerif",ID);}
}
