using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CouleurImpact : MonoBehaviour
{
   //public Texture textureParent;
   public Color couleurParent;
   private GameObject target;
   public ParticleSystem Mainparticle,Smoke,Debris;
   
    void Start()
    {
        if(transform.parent != null){
			target = transform.parent.gameObject;
			if(target.GetComponent<Renderer>().material.color != null){
				//textureParent = target.GetComponent<Renderer>().material.mainTexture;
				couleurParent = target.GetComponent<Renderer>().material.color;
				var main = Mainparticle.main;
				main.startColor = new Color(couleurParent.r,couleurParent.g,couleurParent.b,0.31f);
				
				var mainSmoke = Smoke.main;
				mainSmoke.startColor = new Color(couleurParent.r,couleurParent.g,couleurParent.b,0.78f);;
				
				var mainDebris = Debris.main;
				mainDebris.startColor = couleurParent;
			}
		}
		
    }
}
