using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Titre_Lieux : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI Texte_Titre = null;
    [SerializeField] private RawImage BackGround = null;
	public Slider Chargement;
	public string Titre;
	private float fade_a_BG,fade_a_txt;
	private GameObject DonjonManager;
	public string[] Nom_DeDongeon;
	
    void Start()
    {
		DonjonManager = GameObject.Find("DonjonManager");
		if(DonjonManager != null)
		{
			StartCoroutine(EnterDonjon());
			Titre = "[ " + Nom_DeDongeon[Random.Range(0,Nom_DeDongeon.Length)] + " ] Etage " + DonjonManager.GetComponent<GenProceduralSalle>().Palier.ToString();
			Texte_Titre.text = Titre;
		}
		else
		{
			Chargement.enabled = false;
			StartCoroutine(EnterS());
			Texte_Titre.text = Titre;
		}
    }

	IEnumerator EnterS()
	{
		fade_a_BG = 1;
		fade_a_txt = 1;
		BackGround.color = new Color(BackGround.color.r,BackGround.color.g,BackGround.color.b,fade_a_BG);
		Texte_Titre.color = new Color(Texte_Titre.color.r,Texte_Titre.color.g,Texte_Titre.color.b,fade_a_txt);
		while(fade_a_BG >= 0)
		{
			fade_a_BG -= 0.01f;
			fade_a_txt -= 0.025f;
			yield return new WaitForSeconds(0.02f);
			BackGround.color = new Color(BackGround.color.r,BackGround.color.g,BackGround.color.b,fade_a_BG);
			Texte_Titre.color = new Color(Texte_Titre.color.r,Texte_Titre.color.g,Texte_Titre.color.b,fade_a_txt);
		}
		BackGround.enabled = false;
		Texte_Titre.enabled = false;
    }

	IEnumerator EnterDonjon()
	{
		fade_a_BG = 1;
		fade_a_txt = 1;
		BackGround.color = new Color(BackGround.color.r,BackGround.color.g,BackGround.color.b,fade_a_BG);
		Texte_Titre.color = new Color(Texte_Titre.color.r,Texte_Titre.color.g,Texte_Titre.color.b,fade_a_txt);
		while(DonjonManager.GetComponent<GenProceduralSalle>().SalleMax != 0)
		{
			yield return new WaitForSeconds(0.2f);
		}
		yield return new WaitForSeconds(1.2f);
		while(fade_a_BG >= 0)
		{
			fade_a_BG -= 0.01f;
			fade_a_txt -= 0.025f;
			yield return new WaitForSeconds(0.01f);
			BackGround.color = new Color(BackGround.color.r,BackGround.color.g,BackGround.color.b,fade_a_BG);
			Texte_Titre.color = new Color(Texte_Titre.color.r,Texte_Titre.color.g,Texte_Titre.color.b,fade_a_txt);
		}
		BackGround.enabled = false;
		Texte_Titre.enabled = false;
	}
	
	public IEnumerator ExitS()
	{
        fade_a_BG = 0;
        BackGround.enabled = true;
		Texte_Titre.enabled = false;
		BackGround.color = new Color(BackGround.color.r,BackGround.color.g,BackGround.color.b,fade_a_BG);
        while (fade_a_BG <= 1)
		{
			fade_a_BG += 0.02f;
			yield return new WaitForSeconds(0.01f);
			BackGround.color = new Color(BackGround.color.r,BackGround.color.g,BackGround.color.b,fade_a_BG);
        }

        while(true)
        {
            yield return new WaitForSeconds(0.01f);
            BackGround.color = new Color(BackGround.color.r, BackGround.color.g, BackGround.color.b, 1);
        }
    }

	
}
