using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GSceneManager : MonoBehaviour
{
    [SerializeField] private GameObject joueur = null;
    [SerializeField] private GameObject objJoueur = null;
    [SerializeField] private Vector3 lastApparitionPoint = new Vector3(0f, 0f, 0f);
    [SerializeField] private AudioSource bAudio = null;
    [SerializeField] private AudioClip CloseDoor = null;

    void Awake()
    {
        joueur = GameObject.FindWithTag("player");
        if (!joueur)
        {
            DontDestroyOnLoad(Instantiate(objJoueur, lastApparitionPoint, Quaternion.Euler(0f, 0f, 0f)));
        }
        else
        {
            joueur.SendMessage("chercheElment");
            joueur.SendMessage("CE");
            joueur.transform.GetChild(0).SendMessage("ChercheScope");
            joueur.SendMessage("ChercheElement");
            joueur.SendMessage("ReceiveDialogState", false);
        }
        if (PlayerPrefs.GetInt("CloseSound", 0) == 1)
        {
            PlayerPrefs.SetInt("CloseSound", 0);
            bAudio.clip = CloseDoor;
            bAudio.PlayOneShot(bAudio.clip);
        }
    }

    Vector3 GetPPLastPosition()
    {
        return new Vector3(PlayerPrefs.GetFloat("LAP_x"), PlayerPrefs.GetFloat("LAP_y"), PlayerPrefs.GetFloat("LAP_z"));
    }
}
