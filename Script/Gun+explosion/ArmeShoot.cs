using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ImpactTire
{
    public string tagName;
    public Transform ImpactSpawn;
    public bool SendDegat;
}

[System.Serializable]
public class AudioArme
{
    public AudioClip SondTire;
    public AudioClip SondRecharge;
    public AudioClip SondRechargeOutofAmmo;
    public AudioClip SondNoAmmoTire;
}

public class ArmeShoot : MonoBehaviour
{
    [Header("Arme")]
    [SerializeField] private int munitionMax = 0;
    [SerializeField] private int munition = 0;
    [SerializeField] private int munitionChargeur = 0;
    [SerializeField] private float cadence = 0.1f;
    [SerializeField] private float damage = 0f;
    private float cadenceVar = 0;
    [Header("RayCast")]
    [SerializeField] private LayerMask layerAuthoriser = 0;
    [SerializeField] private float hitForceTire = 0;
    [SerializeField] private bool RayCast = true;
    [SerializeField] private bool CoupParCoup = false;
    private RaycastHit hit;
    private Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    [Header("Spawn")]
    [SerializeField] private Transform SpawnProjectile = null;
    [SerializeField] private GameObject pointDapparitionProjectile = null;
    [Header("Effet")]
    [SerializeField] private AudioArme AA = new AudioArme();
    [SerializeField] private ImpactTire[] IT = new ImpactTire[11];
    [SerializeField] private ParticleSystem TireParticle = null;
    private Transform Reasigneparent;
    [Header("Rechargement")]
    [SerializeField] private float tempsDeRechargement = 0.5f;
    [SerializeField] private bool RechargementState = false;
    [Header("Autre")]
    [SerializeField] private Transform castingBullet = null;
    [SerializeField] private GameObject pointDapparitionBullet = null;
    [SerializeField] private float forceCasting = 200f;
    [SerializeField] private Vector3 angleCasting = new Vector3(90f,0f,0f);
    private Transform casting = null;

    private int i = 0;

    void Update()
    {
        if (Input.GetButton("Fire1") && !RechargementState && munition > 0 && cadenceVar < 0)
        {
            if (Input.GetButtonDown("Fire1") || !CoupParCoup) /*CoupParCoup*/
            {
                if (RayCast)
                {
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.rigidbody != null)
                        {
                            hit.rigidbody.AddForce(ray.direction * hitForceTire);
                        }
                        for (i = 0; i < IT.Length; i++)
                        {
                            if (hit.transform.tag == IT[i].tagName)
                            {
                                Reasigneparent = Instantiate(IT[i].ImpactSpawn, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
                                Reasigneparent.parent = hit.transform.parent;
                                Reasigneparent.localScale = new Vector3(1, 1, 1);
                                if (IT[i].SendDegat)
                                {
                                    hit.collider.gameObject.SendMessage("ReceiveDamagePlayer", damage);
                                }
                                i = IT.Length;
                            }
                        }
                    }
                }
                else
                {
                    Instantiate(SpawnProjectile, pointDapparitionProjectile.transform.position, Quaternion.identity);
                }
                if (castingBullet != null)
                {
                    casting = Instantiate(castingBullet, pointDapparitionBullet.transform.position, Quaternion.Euler(angleCasting));
                    casting.transform.eulerAngles = pointDapparitionBullet.transform.eulerAngles; 
                    casting.GetComponent<Rigidbody>().AddForce(-pointDapparitionBullet.transform.forward * forceCasting);
                }
                munition--;
                if(TireParticle != null){TireParticle.Play();}
                cadenceVar = cadence;
            }
        }

        if(cadenceVar >= 0){cadenceVar -= Time.deltaTime;}

        if (Input.GetButtonDown("Recharger"))
        {
            if (munition < munitionMax && !RechargementState)
            {
                StartCoroutine(Rechargement());
            }
        }
    }

    IEnumerator Rechargement()
    {
        RechargementState = true;
        yield return new WaitForSeconds(tempsDeRechargement);
        if (munitionChargeur >= Mathf.Abs(munition - munitionMax))
        {
            munitionChargeur += (munition - munitionMax);
            munition = munitionMax;
        }
        else
        {
            munition = munition + munitionChargeur;
        }
        RechargementState = false;
    }
}
