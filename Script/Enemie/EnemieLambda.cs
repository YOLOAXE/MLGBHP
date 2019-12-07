using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine;

public class EnemieLambda : MonoBehaviour
{
	public NavMeshAgent Agent;
	public float Vie,VieMax;
	public bool Idel,Chase,Attaque,isDead;
	public bool HaveAnimeReciveDamage;
	public Transform MortParticule;
	public Vector3 SpawnMortParticule;
	private GameObject Player;
	private Vector3 targetPointfrape;
	public RectTransform BarDeVie;
	Animator m_Animator;
	public float DegatSend,HitForce,DistanceDeFrape,TempsAttaque,DistanceDeChase;
	public float VitesseNoChase,VitesseChase,CoeffDeDetection,TimerDeadDestroy,TimerChangePosition,CoeffDeDeplacement;
	public float DistancePlayer,CoeffDeDetectionVar,TempsAttaqueVar,TimerChangePositionVar;
	public Vector3 RandomDeplacement; 
	public AudioSource m_AudioSource;
	public AudioClip Mort;
	public Transform[] Loot;

    void Start()
	{
		m_AudioSource = GetComponent<AudioSource>();
		Player = GameObject.FindWithTag("player") ;
		CoeffDeDetectionVar = CoeffDeDetection; 
		TempsAttaqueVar = TempsAttaque;
		m_Animator = GetComponent<Animator>();
		TimerChangePositionVar = TimerChangePosition;
		Vie = VieMax;
		BarDeVie.sizeDelta = new Vector2((Vie/VieMax) * 100,BarDeVie.sizeDelta.y);
    }

    void Update()
	{
		if(!isDead)
		{
			if(Vie <= 0)
			{
				isDead = true;
				m_AudioSource.clip = Mort;
				m_AudioSource.PlayOneShot(m_AudioSource.clip);
			}
			DistancePlayer = Vector3.Distance(transform.position,Player.transform.position);
			if(DistancePlayer <= DistanceDeChase && DistanceDeFrape < DistanceDeChase)
			{
				if(!Chase)
				{
					TrigChase();
				}
			}
			else
			{
				Chase = false;
			}
			if(DistancePlayer <= DistanceDeFrape)
			{
				Attaque = true;
				TempsAttaqueVar -= Time.deltaTime;
				if(TempsAttaqueVar <= 0)
				{
					RaycastHit hit;
					Vector3 ray = transform.TransformDirection(Vector3.forward);
					if (Physics.Raycast(transform.position,transform.TransformDirection(Vector3.forward), out hit))
					{
						if (hit.rigidbody != null){hit.rigidbody.AddForce(Vector3.up * HitForce);hit.rigidbody.AddForce(ray * HitForce);}
					}
					Player.SendMessage("ReceiveDamage",DegatSend);
					TempsAttaqueVar = TempsAttaque;	
				}
			}
			else
			{
				Attaque = false;
			}
			if(TimerChangePositionVar <= 0)
			{
				RandomDeplacement = new Vector3(Random.Range(-CoeffDeDeplacement,CoeffDeDeplacement),Random.Range(-CoeffDeDeplacement,CoeffDeDeplacement),Random.Range(-CoeffDeDeplacement,CoeffDeDeplacement)) + transform.position;
				Agent.SetDestination(RandomDeplacement);
				TimerChangePositionVar = TimerChangePosition + Random.Range(-TimerChangePosition,TimerChangePosition);	
			}
			if(Attaque || Chase)
			{	
				if(DistancePlayer > DistanceDeFrape)
				{
					Agent.SetDestination(Player.transform.position);
				}
				float step = 2 * Time.deltaTime;
				targetPointfrape = Player.transform.position -  transform.position;
				Vector3 newDir = Vector3.RotateTowards(transform.forward, targetPointfrape,step, 0.0f);
				Vector3 Correction = new Vector3(newDir.x,0,newDir.z);
				transform.rotation = Quaternion.LookRotation(Correction);
				Agent.speed = VitesseChase;
			}
			else
			{
				TimerChangePositionVar -= Time.deltaTime;
				Agent.speed = VitesseNoChase;
			}
			m_Animator.SetBool("Walk",(Agent.remainingDistance > Agent.stoppingDistance || Agent.desiredVelocity.magnitude > 0.2));
			m_Animator.SetBool("Attack",Attaque);
		}
		else
		{
			m_Animator.SetBool("Death",isDead);
			if(Vie <= 0)
			{ 
				Vie = 100;
				Agent.isStopped = true;
				Instantiate(MortParticule,transform.position + SpawnMortParticule,Quaternion.identity);
				SpawnObjectLoot();
				Destroy(gameObject, TimerDeadDestroy);
			}

		}
	}
	void TrigChase()
	{
		CoeffDeDetectionVar -= Time.deltaTime;
		if(CoeffDeDetectionVar <= 0)
		{
			if(Random.Range(0,2) == 1)
			{
				Chase = true;
			}
			CoeffDeDetectionVar = CoeffDeDetection; 
		}
	}
	public void ReceiveDamagePlayer(float Damage)
	{
		if(!isDead)
		{
			Vie -= Damage;
			BarDeVie.sizeDelta = new Vector2((Vie/VieMax) * 100,BarDeVie.sizeDelta.y);
			if(HaveAnimeReciveDamage){m_Animator.SetBool("RecieveDamage",HaveAnimeReciveDamage);}
			if(DistancePlayer <= DistanceDeChase && DistanceDeFrape < DistanceDeChase){Chase = true;}
			Agent.SetDestination(Player.transform.position);
			Chase = true;
		}
	}
	void SpawnObjectLoot()
	{
		RaycastHit hitDown;
		if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hitDown) && Loot.Length != 0)
		{
			Instantiate(Loot[Random.Range(0,Loot.Length)],hitDown.point + new Vector3(0,0.1f,0),Quaternion.FromToRotation(Vector3.down, hitDown.normal));
		}
	}
}
