using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITransmision : MonoBehaviour
{
	private GameObject Player;
	private int Send;
	
    void Start()
    {
        Player = GameObject.FindWithTag("player");
    }

	public void Numero1E(){Send = 0;Player.SendMessage("EquipementNumReceive",Send);}
	public void Numero2E(){Send = 1;Player.SendMessage("EquipementNumReceive",Send);}
	public void Numero3E(){Send = 2;Player.SendMessage("EquipementNumReceive",Send);}
	public void Resource1E(){Send = 0;Player.SendMessage("ResourceNumReceive",Send);}
	public void Resource2E(){Send = 1;Player.SendMessage("ResourceNumReceive",Send);}
}
