using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorTexture : MonoBehaviour
{
    public Texture2D[] cursorTexture;
	public int CursorID;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2[] hotSpot;
	public bool OnDialog,OnInventaire;
	
	void Start(){
		Cursor.SetCursor(cursorTexture[CursorID], hotSpot[CursorID], cursorMode);
		Cursor.lockState = CursorLockMode.Locked;  
		Cursor.visible = true;
	}
	void Update()
    {
	  if(Input.GetButton("Fire1") || Input.GetButton("Fire2")){
		  if(!OnDialog && !OnInventaire){
			Cursor.SetCursor(cursorTexture[CursorID], hotSpot[CursorID], cursorMode);
			Cursor.lockState = CursorLockMode.Locked;  
			Cursor.visible = true;
		  }
	  }
	  if(Input.GetButton("Cancel")){
		Cursor.SetCursor(null, Vector2.zero, cursorMode);
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	  }
	  
    }
		
	public void ChangeCursor(int Id){
		CursorID = Id;
		Cursor.SetCursor(cursorTexture[CursorID], hotSpot[CursorID], cursorMode);
	}
	
	public void ReceiveDialogState(bool Dialog){
	OnDialog = Dialog;
		if(OnDialog){
			Cursor.lockState = CursorLockMode.None;
			Cursor.SetCursor(null, Vector2.zero, cursorMode);
			Cursor.visible = true;
		}else{
			Cursor.lockState = CursorLockMode.Locked;
			ChangeCursor(CursorID);
			Cursor.visible = true;
		}
	}
	public void ReceiveInventaireState(bool inventaire){
		OnInventaire = inventaire;
		if(OnInventaire){
			Cursor.lockState = CursorLockMode.None;
			Cursor.SetCursor(null, Vector2.zero, cursorMode);
			Cursor.visible = true;
		}else{
			Cursor.lockState = CursorLockMode.Locked;
			ChangeCursor(CursorID);
			Cursor.visible = true;
		}
	}
}
