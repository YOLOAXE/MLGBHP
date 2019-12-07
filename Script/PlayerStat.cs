using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
	public float VieJoueur; 
	public float VieJoueurMax; 
	public float ManaJoueur,ManaJoueurMax,CoeffRegeneMana;  
	public float CoeffRegene;
	public bool IsDead;
	public Slider VieSlider,ManaSlider;
	public TextMeshProUGUI VieTxt,ManaTxt;
	public AudioClip[] DamageSond,DamageMagiqueSond,DamageFireSond;
	private AudioSource m_AudioSource;
	
    void Start()
    {
		VieSlider = GameObject.Find("qmlkdfjq_VieSlider").GetComponent<Slider>();
		VieTxt = GameObject.Find("qmlkdfjq_VieTxt").GetComponent<TextMeshProUGUI>();
		ManaSlider = GameObject.Find("qmlkdfjq_ManaSlider").GetComponent<Slider>();
		ManaTxt = GameObject.Find("qmlkdfjq_ManaTxt").GetComponent<TextMeshProUGUI>();
        VieSlider.maxValue = VieJoueurMax;
		ManaSlider.maxValue = ManaJoueurMax;
		VieSlider.value = VieJoueur;
		ManaSlider.value = ManaJoueur;
		m_AudioSource = GetComponent<AudioSource>();
    }
	
    void Update()
    {
		if(VieJoueur >= VieJoueurMax){VieJoueur = VieJoueurMax;}
		if(VieJoueur < VieJoueurMax && !IsDead){VieJoueur += CoeffRegene * Time.deltaTime;}
		VieSlider.value = Mathf.Lerp(VieSlider.value,VieJoueur,Time.deltaTime * 10);
		VieTxt.text = "HP " + ((int)VieJoueur).ToString() + "/" + ((int)VieJoueurMax).ToString();

		if(ManaJoueur >= ManaJoueurMax){ManaJoueur = ManaJoueurMax;}
		if(ManaJoueur < ManaJoueurMax){ManaJoueur += CoeffRegeneMana * Time.deltaTime;}
		ManaSlider.value = Mathf.Lerp(ManaSlider.value,ManaJoueur,Time.deltaTime * 10);
		ManaTxt.text = "MP " + ((int)ManaJoueur).ToString() + "/" + ((int)ManaJoueurMax).ToString();
    }
	
	public void ReceiveDamage(float Damage)
	{
		DamageHelper(Damage,1f,DamageSond[Random.Range(0,DamageSond.Length)],true);
	}
	public void ReceiveTakeMana(float DamageMana)
	{
		DamageHelper(DamageMana,1f,null,false);
	}
	public void ReceiveDamageNosound(float Damage)
	{
		DamageHelper(Damage,1f,null,true);
	}
	public void ReceiveDamageMagic(float Damage)
	{
		DamageHelper(Damage,1f,DamageMagiqueSond[Random.Range(0,DamageMagiqueSond.Length)],true);
	}
	public void ReceiveDamageFire(float Damage)
	{
		DamageHelper(Damage,1f,DamageFireSond[Random.Range(0,DamageFireSond.Length)],true);
	}
	
	private void DamageHelper(float Damage,float Coeff,AudioClip Sound,bool Vie)
	{	
		if(Vie)
		{
			VieJoueur -= (Damage * Coeff);
		}
		else
		{
			ManaJoueur -= (Damage * Coeff);
		}
		if(Sound != null)
		{
			m_AudioSource.clip = Sound;
			m_AudioSource.PlayOneShot(m_AudioSource.clip);
		}
		if(VieJoueur <= 0){IsDead = true;VieJoueur = 0;}
		if(ManaJoueur <= 0){ManaJoueur = 0;}
	}
	
	/////////////////////////////////////////////////ITEM
	public void UseItem26 (){VieJoueur += 50;}
	public void UseItem27 (){ManaJoueur += 50;}
	public void UseItem28 (){VieJoueur += 150;}
	public void UseItem29 (){ManaJoueur += 150;}
	
}
