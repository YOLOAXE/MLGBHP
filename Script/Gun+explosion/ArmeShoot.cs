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
    public AudioSource m_AudioSource;
    public AudioClip SondTire;
    public AudioClip SondRecharge;
    public AudioClip SondRechargeOutofAmmo;
    public AudioClip SondNoAmmoTire;
}

public class ArmeShoot : MonoBehaviour
{
    [Header("Arme")]
    public int munitionMax = 0;
    public int munition = 0;
    public int munitionChargeur = 0;
    public float ConsumeMana = 0f;
    public float cadence = 0.1f;
    public float damage = 0f;
    public bool useMana = false;
    public bool NoAmmo = false;
    private float cadenceVar = 0;
    [Header("RayCast")]
    [SerializeField] private LayerMask layerAuthoriser = 0;
    [SerializeField] private float hitForceTire = 0;
    [SerializeField] private bool RayCast = true;
    [SerializeField] private bool CoupParCoup = false;
    private RaycastHit hit;
    private Ray ray;
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
    [SerializeField] private Vector3 angleCasting = new Vector3(90f, 0f, 0f);
    [SerializeField] private PlayerStat statPlayer = null;
    [SerializeField] private Animator Arm_Animator = null;
    private Transform casting = null;

    private int i = 0;

    void Update()
    {
        if (Input.GetButton("Fire1") && !RechargementState && cadenceVar < 0)
        {
            if ((munition > 0 || useMana) && (statPlayer.ManaJoueur >= ConsumeMana || !useMana))
            {
                if (Input.GetButtonDown("Fire1") || !CoupParCoup) /*CoupParCoup*/
                {
                    if (RayCast)
                    {
                        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerAuthoriser))
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

                    if (!useMana){munition--;}else{statPlayer.ReceiveTakeMana(ConsumeMana);}
                    if (TireParticle != null) { TireParticle.Play(); }

                    PlaySound(AA.SondTire);
                    Arm_Animator.SetBool("Shoot", true);
                    cadenceVar = cadence;
                }
            }
            else
            {
                PlaySound(AA.SondNoAmmoTire);
                if (cadenceVar < 0) { Arm_Animator.SetBool("Shoot", false); }
            }
        }

        if (cadenceVar >= 0) { cadenceVar -= Time.deltaTime; }

        if (Input.GetButtonDown("Recharger") && !useMana && munitionChargeur > 0)
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
        if(munition > 0)
        {
            PlaySound(AA.SondRecharge);
            Arm_Animator.SetBool("Rechargement", true);
        }
        else
        {
            PlaySound(AA.SondRechargeOutofAmmo);
            Arm_Animator.SetBool("RechargementOutOfAmmo", true);
        }
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

    void PlaySound(AudioClip sound)
    {
        if (sound != null)
        {
            AA.m_AudioSource.clip = sound;
            AA.m_AudioSource.PlayOneShot(AA.m_AudioSource.clip);
        }
    }

}
