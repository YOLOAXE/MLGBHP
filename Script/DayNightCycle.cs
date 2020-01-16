using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine;

namespace UnityEngine.PostProcessing
{

public class DayNightCycle : MonoBehaviour
{

	public Light sun;
	public Light night;
	public float secondInFullDay = 900f; //durée d'un jour en secondes
	[Range(0,1)]
	public float currentTimeofDay = 0.5f; //l'heure actuelle sur une échelle de 0 à 1
	public AudioClip m_VilleJour;           
    public AudioClip m_VilleNuit;
	public AudioSource m_AudioSource;
	public Color[] gradiantNight,gradiantDay;
	public bool OnJour;
	public float exposure = 1f;
	public PostProcessingProfile profileJour;
	public PostProcessingProfile profileNuit;
	public PostProcessingBehaviour shader;
	private float AbianceVolume = 0.5f;
	[HideInInspector]
	public float timeMultiplier = 1f; //variable permettant de faire avancer le temps
	[HideInInspector]
	public bool AntiSpamSond = true;
	float sunInitialIntensity;
	public GameObject lampeEteint,lampeAllumer,FonduAuNoire;
	private Animator AnimationFondu;
	public GameObject[] SendInfo;
	

    void Start()
    {
		sunInitialIntensity = sun.intensity;
		//night.intensity = 
		if(currentTimeofDay <= 0.25f || currentTimeofDay >= 0.75f){
			AntiSpamSond = false;
			lampeEteint.SetActive(false);
			lampeAllumer.SetActive(true);
			RenderSettings.ambientSkyColor = gradiantNight[0];RenderSettings.ambientEquatorColor = gradiantNight[1];RenderSettings.ambientGroundColor = gradiantNight[2];
		}else{
			AntiSpamSond = true;
			lampeEteint.SetActive(true);
			lampeAllumer.SetActive(false);	
			RenderSettings.ambientSkyColor = gradiantDay[0];RenderSettings.ambientEquatorColor = gradiantDay[1];RenderSettings.ambientGroundColor = gradiantDay[2];
		}
		FonduAuNoire = GameObject.FindWithTag("FonduAuNoire");
        if (FonduAuNoire != null) { AnimationFondu = FonduAuNoire.GetComponent<Animator>(); }
		SendInfo = GameObject.FindGameObjectsWithTag("PNJ") ;
    }


    void Update()
    {
        UpdateSun();
		for(int i = 0;i<SendInfo.Length;i++){
			SendInfo[i].SendMessage("ReceiveCycle",currentTimeofDay);
		}
		
		currentTimeofDay += (Time.deltaTime / secondInFullDay) * timeMultiplier; //avancement du temps
		if(currentTimeofDay >= 1){currentTimeofDay=0;}
		
		if(currentTimeofDay >= 0.246f && currentTimeofDay <= 0.247f || currentTimeofDay >= 0.747f && currentTimeofDay <= 0.748f){
			AnimationFondu.Play("FonduTransparentVersNoire", 0, 0.25f);
		//	Debug.Log("Debut");
		}
		if(currentTimeofDay >= 0.251f && currentTimeofDay <= 0.252f || currentTimeofDay >= 0.752f && currentTimeofDay <= 0.753f){
			AnimationFondu.Play("FonduNoireVersTrasnparent", 0, 0.25f);
		//	Debug.Log("Fin");
		}
		
		if(currentTimeofDay >= 0.248f && currentTimeofDay <= 0.252f || currentTimeofDay >= 0.748f && currentTimeofDay <= 0.753f){
			if(AbianceVolume > 0.05f){AbianceVolume -= Time.deltaTime * 0.3f;}else{AbianceVolume = 0f;}
			
		
		}else{
			if(AbianceVolume < 0.5f){AbianceVolume += Time.deltaTime * 0.3f;}else{AbianceVolume = 0.5f;}
		}
		m_AudioSource.volume = AbianceVolume;
			if(currentTimeofDay <= 0.25f || currentTimeofDay >= 0.75f){
			lampeEteint.SetActive(false);
			lampeAllumer.SetActive(true);
			RenderSettings.ambientSkyColor = gradiantNight[0];RenderSettings.ambientEquatorColor = gradiantNight[1];RenderSettings.ambientGroundColor = gradiantNight[2];
				if(exposure > 0.05f){exposure -= Time.deltaTime * 0.1f;	}else{exposure = 0f;}
				OnJour = false;
			}else{
			lampeEteint.SetActive(true);
			lampeAllumer.SetActive(false);	
			RenderSettings.ambientSkyColor = gradiantDay[0];RenderSettings.ambientEquatorColor = gradiantDay[1];RenderSettings.ambientGroundColor = gradiantDay[2];
				if(exposure < 1f){exposure += Time.deltaTime * 0.1f;	}else{exposure = 1f;}
				OnJour = true;
			}
			if(OnJour){
				if(AntiSpamSond){
					shader.profile = profileJour;
					m_AudioSource.clip = m_VilleJour;
					m_AudioSource.Play();	
				}
				AntiSpamSond = false;
			}else{
				if(!AntiSpamSond){
					shader.profile = profileNuit;
					m_AudioSource.clip = m_VilleNuit;
					m_AudioSource.Play();	
				}
				AntiSpamSond = true;
			}
		RenderSettings.skybox.SetFloat("_Exposure", exposure);	
    }
	
	void UpdateSun(){
		sun.transform.localRotation = Quaternion.Euler((currentTimeofDay*360f) -90, 170, 0); //rotation du soleil autour de la scène
		float intensityMultiplier = 1;
		if(currentTimeofDay <= 0.23f || currentTimeofDay >= 0.75){intensityMultiplier=0f;} //intensité entre 0h et 7h et entre 19h et 0h
		else if(currentTimeofDay <= 0.25f){intensityMultiplier = Mathf.Clamp01((currentTimeofDay -0.23f) * (1 / 0.02f));} //apparition du soleil le matin
		else if(currentTimeofDay >= 0.73f){intensityMultiplier = Mathf.Clamp01((1 - (currentTimeofDay -0.73f) * (1 / 0.02f)));} //disparition du soleil le soir
		sun.intensity = sunInitialIntensity * intensityMultiplier;
	}
}
}