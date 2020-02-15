using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    [SerializeField] private string targetNameScene = "";
    [SerializeField] private Vector3 posTP = new Vector3(0f, 0f, 0f);
    [SerializeField] private Vector3 angleTP = new Vector3(0f, 0f, 0f);
    [SerializeField] private AudioSource bAudio = null;
    [SerializeField] private AudioClip OpenDoor = null;
    private bool loadScene = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "player" && !loadScene)
        {
            StartCoroutine(NextScene(other.gameObject));
        }
    }

    public IEnumerator NextScene(GameObject player)
    {
        if (!loadScene)
        {
            if (bAudio != null)
            {
                bAudio.clip = OpenDoor;
                bAudio.PlayOneShot(bAudio.clip);
                PlayerPrefs.SetInt("CloseSound",1);
            }
            loadScene = true;
            StartCoroutine(GameObject.Find("Titre_lieux").GetComponent<Titre_Lieux>().ExitS());
            yield return new WaitForSeconds(2f);
            player.SendMessage("ReceiveDialogState", true);
            player.transform.position = posTP;
            player.transform.GetChild(0).transform.eulerAngles = angleTP;
            SceneManager.LoadScene(targetNameScene);
        }
    }
}
