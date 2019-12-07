using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	[SerializeField]
    private float lifeMax,Curentlife;
	public int NBSpawn;
	private int VarNBSpawn;
	public RectTransform BarDeVie;
	public GameObject Pos1,Pos2,TpPoint;
	public Transform ParticuleSpawn,ParticuleMort;
	public Transform[] EnnemieHasSpawn;
	[HideInInspector]
	public Transform[] ObjectSpawn;
	public GameObject Smoke;
	public bool isDead;
	
    void Start()
    {
        Curentlife = lifeMax;
		ObjectSpawn = new Transform[NBSpawn * 2];
		VarNBSpawn = NBSpawn;
    }
    void Update()
    {
        for(int i = 0;ObjectSpawn.Length > i;i++){
			if(ObjectSpawn[i] == null && !isDead && NBSpawn > i)
			{
				TpPoint.transform.position = new Vector3(Random.Range(Pos1.transform.position.x,Pos2.transform.position.x),Random.Range(Pos1.transform.position.y,Pos2.transform.position.y),Random.Range(Pos1.transform.position.z,Pos2.transform.position.z));
				ObjectSpawn[i] = Instantiate(EnnemieHasSpawn[Random.Range(0,EnnemieHasSpawn.Length)],TpPoint.transform.position,Quaternion.identity);
				Instantiate(ParticuleSpawn,TpPoint.transform.position,Quaternion.identity);
			}
		}
    }
	
	public void ReceiveDamagePlayer(float Damage)
	{
		if(!isDead)
		{
			Curentlife -= Damage;
			BarDeVie.sizeDelta = new Vector2(Curentlife,BarDeVie.sizeDelta.y);
			NBSpawn = VarNBSpawn * 2;
			if(Curentlife <= 0){isDead = true;Smoke.GetComponent<ParticleSystem>().Stop();StartCoroutine(Mort());}
		}
	}
	IEnumerator Mort()
	{
		Instantiate(ParticuleMort,transform.position,Quaternion.identity);
		yield return new WaitForSeconds(1f);
		gameObject.GetComponent<Animator>().SetBool("Dead", true);
		yield return new WaitForSeconds(2f);
		Destroy(gameObject);
	}
}
