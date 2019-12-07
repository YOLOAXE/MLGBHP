using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.PostProcessing
{
public class interieurGradiantLight : MonoBehaviour
{
	public Color[] gradiantNight,gradiantDay;
	public PostProcessingProfile profileInterieur;
	public PostProcessingBehaviour shader;
	private GameObject Camera;
	public bool OnJour;
	
    void Start()
    {
		Camera = GameObject.FindWithTag("MainCamera");
		shader = Camera.GetComponent<PostProcessingBehaviour>();
		if(OnJour){
			RenderSettings.ambientSkyColor = gradiantDay[0];RenderSettings.ambientEquatorColor = gradiantDay[1];RenderSettings.ambientGroundColor = gradiantDay[2];
		}else{
			RenderSettings.ambientSkyColor = gradiantNight[0];RenderSettings.ambientEquatorColor = gradiantNight[1];RenderSettings.ambientGroundColor = gradiantNight[2];
		}
		shader.profile = profileInterieur;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
}