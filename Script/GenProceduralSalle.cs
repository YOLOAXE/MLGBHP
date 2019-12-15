using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenProceduralSalle : MonoBehaviour
{
    public int SalleMax, idThemeSalle, Palier;
    public Transform[] salleLambdaObject_HGDB;
    public Transform[] salleLambdaObject_B;
    public Transform[] salleLambdaObject_G;
    public Transform[] salleLambdaObject_GD;
    public Transform[] salleLambdaObject_D;
    public Transform[] salleLambdaObject_BD;
    public Transform[] salleLambdaObject_BG;
    public Transform[] salleLambdaObject_H;
    public Transform[] salleLambdaObject_HB;
    public Transform[] salleLambdaObject_HG;
    public Transform[] salleLambdaObject_HD;
    public Transform[] salleLambdaObject_HDG;
    public Transform[] salleLambdaObject_HDB;
    public Transform[] salleLambdaObject_HGB;
    public Transform[] salleLambdaObject_DGB;
    public Transform[] salleBossObject;
    private int SpawnRandom, RoomRandom, ErreurHuit;
    public GameObject[] SpawnPoint;
    private bool HBool, GBool, DBool, BBool, Hblock, Gblock, Dblock, Bblock;
    private Vector3 SpawnPointPlayerPos;
    [SerializeField]
    private GameObject LightControl = null;

    void Start()
    {
        SalleMax += Palier;
        StartCoroutine(SpawnDonjon());
    }
    IEnumerator SpawnDonjon()
    {
        Transform ChangeInfoSpawn = Instantiate(salleLambdaObject_HGDB[idThemeSalle], gameObject.transform.position, Quaternion.identity);
        ChangeInfoSpawn.GetComponent<RoomManager>().SpawnState = 1;
        SpawnPointPlayerPos = GameObject.FindWithTag("RoomPoint").transform.position;
        GameObject.FindWithTag("player").GetComponent<Rigidbody>().useGravity = false;
        yield return new WaitForSeconds(0.1f);
        while (SalleMax > 0)
        {
            yield return new WaitForSeconds(0.15f);
            GameObject.FindWithTag("player").transform.position = SpawnPointPlayerPos;
            SpawnPoint = GameObject.FindGameObjectsWithTag("SpawnPoint");
            SpawnRandom = Random.Range(0, SpawnPoint.Length);
            HBool = SpawnPoint[SpawnRandom].GetComponent<SpawnSalle>().Haut;
            BBool = SpawnPoint[SpawnRandom].GetComponent<SpawnSalle>().Bas;
            GBool = SpawnPoint[SpawnRandom].GetComponent<SpawnSalle>().Gauche;
            DBool = SpawnPoint[SpawnRandom].GetComponent<SpawnSalle>().Droite;
            Hblock = SpawnPoint[SpawnRandom].GetComponent<SpawnSalle>().BlockeHaut;
            Bblock = SpawnPoint[SpawnRandom].GetComponent<SpawnSalle>().BlockeBas;
            Gblock = SpawnPoint[SpawnRandom].GetComponent<SpawnSalle>().BlockeGauche;
            Dblock = SpawnPoint[SpawnRandom].GetComponent<SpawnSalle>().BlockeDroite;
            ErreurHuit++;

            if (HBool && BBool && GBool && DBool)
            {
                Instantiate(salleLambdaObject_HGDB[idThemeSalle], SpawnPoint[SpawnRandom].transform.position, Quaternion.identity);
            }
            else if (!HBool && BBool && !GBool && DBool)
            {
                Instantiate(salleLambdaObject_HG[idThemeSalle], SpawnPoint[SpawnRandom].transform.position, Quaternion.identity);
            }
            else if (!HBool && BBool && GBool && !DBool)
            {
                Instantiate(salleLambdaObject_HD[idThemeSalle], SpawnPoint[SpawnRandom].transform.position, Quaternion.identity);
            }
            else if (HBool && BBool && !GBool && !DBool)
            {
                Instantiate(salleLambdaObject_HB[idThemeSalle], SpawnPoint[SpawnRandom].transform.position, Quaternion.identity);
            }
            else if (HBool && !BBool && GBool && !DBool)
            {
                Instantiate(salleLambdaObject_BD[idThemeSalle], SpawnPoint[SpawnRandom].transform.position, Quaternion.identity);
            }
            else if (HBool && !BBool && !GBool && DBool)
            {
                Instantiate(salleLambdaObject_BG[idThemeSalle], SpawnPoint[SpawnRandom].transform.position, Quaternion.identity);
            }
            else if (!HBool && !BBool && GBool && DBool)
            {
                Instantiate(salleLambdaObject_GD[idThemeSalle], SpawnPoint[SpawnRandom].transform.position, Quaternion.identity);
            }
            else if (HBool && !BBool && !GBool && !DBool)
            {
                while (RoomRandom != 99)
                {
                    RoomRandom = Random.Range(0, 4);
                    if (RoomRandom == 0 && !Hblock && Bblock && !Gblock && !Dblock) { Instantiate(salleLambdaObject_HGDB[idThemeSalle], SpawnPoint[SpawnRandom].transform.position, Quaternion.identity); RoomRandom = 99; }
                    if (RoomRandom == 1 && Bblock && !Dblock && ErreurHuit > 3) { Instantiate(salleLambdaObject_BD[idThemeSalle], SpawnPoint[SpawnRandom].transform.position, Quaternion.identity); RoomRandom = 99; }
                    if (RoomRandom == 2 && Bblock && !Gblock) { Instantiate(salleLambdaObject_BG[idThemeSalle], SpawnPoint[SpawnRandom].transform.position, Quaternion.identity); RoomRandom = 99; }
                    if (RoomRandom == 3 && Bblock && !Hblock && ErreurHuit > 3) { Instantiate(salleLambdaObject_HB[idThemeSalle], SpawnPoint[SpawnRandom].transform.position, Quaternion.identity); RoomRandom = 99; }
                    if (Hblock && Bblock && Gblock && Dblock) { RoomRandom = 99; }
                }
            }
            else if (!HBool && BBool && !GBool && !DBool)
            {
                while (RoomRandom != 98)
                {
                    RoomRandom = Random.Range(0, 4);
                    if (RoomRandom == 0 && Hblock && !Bblock && !Gblock && !Dblock) { Instantiate(salleLambdaObject_HGDB[idThemeSalle], SpawnPoint[SpawnRandom].transform.position, Quaternion.identity); RoomRandom = 98; }
                    if (RoomRandom == 1 && Hblock && !Bblock) { Instantiate(salleLambdaObject_HB[idThemeSalle], SpawnPoint[SpawnRandom].transform.position, Quaternion.identity); RoomRandom = 98; }
                    if (RoomRandom == 2 && Hblock && !Gblock && ErreurHuit > 3) { Instantiate(salleLambdaObject_HG[idThemeSalle], SpawnPoint[SpawnRandom].transform.position, Quaternion.identity); RoomRandom = 98; }
                    if (RoomRandom == 3 && Hblock && !Dblock && ErreurHuit > 3) { Instantiate(salleLambdaObject_HD[idThemeSalle], SpawnPoint[SpawnRandom].transform.position, Quaternion.identity); RoomRandom = 98; }
                    if (Hblock && Bblock && Gblock && Dblock) { RoomRandom = 98; }
                }
            }
            else if (!HBool && !BBool && GBool && !DBool)
            {
                while (RoomRandom != 97)
                {
                    RoomRandom = Random.Range(0, 4);
                    if (RoomRandom == 0 && !Hblock && !Bblock && !Gblock && Dblock) { Instantiate(salleLambdaObject_HGDB[idThemeSalle], SpawnPoint[SpawnRandom].transform.position, Quaternion.identity); RoomRandom = 97; }
                    if (RoomRandom == 1 && Dblock && !Hblock && ErreurHuit > 3) { Instantiate(salleLambdaObject_HD[idThemeSalle], SpawnPoint[SpawnRandom].transform.position, Quaternion.identity); RoomRandom = 97; }
                    if (RoomRandom == 2 && Dblock && !Bblock && ErreurHuit > 3) { Instantiate(salleLambdaObject_BD[idThemeSalle], SpawnPoint[SpawnRandom].transform.position, Quaternion.identity); RoomRandom = 97; }
                    if (RoomRandom == 3 && Dblock && !Gblock) { Instantiate(salleLambdaObject_GD[idThemeSalle], SpawnPoint[SpawnRandom].transform.position, Quaternion.identity); RoomRandom = 97; }
                    if (Hblock && Bblock && Gblock && Dblock) { RoomRandom = 97; }
                }
            }
            else if (!HBool && !BBool && !GBool && DBool)
            {
                while (RoomRandom != 96)
                {
                    RoomRandom = Random.Range(0, 4);
                    if (RoomRandom == 0 && !Hblock && !Bblock && Gblock && !Dblock) { Instantiate(salleLambdaObject_HGDB[idThemeSalle], SpawnPoint[SpawnRandom].transform.position, Quaternion.identity); RoomRandom = 96; }
                    if (RoomRandom == 1 && Gblock && !Dblock) { Instantiate(salleLambdaObject_GD[idThemeSalle], SpawnPoint[SpawnRandom].transform.position, Quaternion.identity); RoomRandom = 96; }
                    if (RoomRandom == 2 && Gblock && !Bblock && ErreurHuit > 3) { Instantiate(salleLambdaObject_BG[idThemeSalle], SpawnPoint[SpawnRandom].transform.position, Quaternion.identity); RoomRandom = 96; }
                    if (RoomRandom == 3 && Gblock && !Hblock && ErreurHuit > 3) { Instantiate(salleLambdaObject_HG[idThemeSalle], SpawnPoint[SpawnRandom].transform.position, Quaternion.identity); RoomRandom = 96; }
                    if (Hblock && Bblock && Gblock && Dblock) { RoomRandom = 96; }
                }
            }
            SalleMax--;
        }
        StartCoroutine(PlayerDown());
        yield return new WaitForSeconds(0.1f);
        SpawnPoint = GameObject.FindGameObjectsWithTag("SpawnPoint");
        Transform ChangeInfoSortie = null;
        for (int i = 0; i < SpawnPoint.Length; i++)
        {
            if (SpawnPoint[i] != null)
            {
                HBool = SpawnPoint[i].GetComponent<SpawnSalle>().Haut;
                BBool = SpawnPoint[i].GetComponent<SpawnSalle>().Bas;
                GBool = SpawnPoint[i].GetComponent<SpawnSalle>().Gauche;
                DBool = SpawnPoint[i].GetComponent<SpawnSalle>().Droite;
                Hblock = SpawnPoint[i].GetComponent<SpawnSalle>().BlockeHaut;
                Bblock = SpawnPoint[i].GetComponent<SpawnSalle>().BlockeBas;
                Gblock = SpawnPoint[i].GetComponent<SpawnSalle>().BlockeGauche;
                Dblock = SpawnPoint[i].GetComponent<SpawnSalle>().BlockeDroite;
                if (HBool && BBool && GBool && DBool)
                {
                    ChangeInfoSortie = Instantiate(salleLambdaObject_HGDB[idThemeSalle], SpawnPoint[i].transform.position, Quaternion.identity);
                }
                else if (!HBool && BBool && !GBool && DBool)
                {
                    ChangeInfoSortie = Instantiate(salleLambdaObject_HG[idThemeSalle], SpawnPoint[i].transform.position, Quaternion.identity);
                }
                else if (!HBool && BBool && GBool && !DBool)
                {
                    ChangeInfoSortie = Instantiate(salleLambdaObject_HD[idThemeSalle], SpawnPoint[i].transform.position, Quaternion.identity);
                }
                else if (HBool && BBool && !GBool && !DBool)
                {
                    ChangeInfoSortie = Instantiate(salleLambdaObject_HB[idThemeSalle], SpawnPoint[i].transform.position, Quaternion.identity);
                }
                else if (HBool && !BBool && GBool && !DBool)
                {
                    ChangeInfoSortie = Instantiate(salleLambdaObject_BD[idThemeSalle], SpawnPoint[i].transform.position, Quaternion.identity);
                }
                else if (HBool && !BBool && !GBool && DBool)
                {
                    ChangeInfoSortie = Instantiate(salleLambdaObject_BG[idThemeSalle], SpawnPoint[i].transform.position, Quaternion.identity);
                }
                else if (!HBool && !BBool && GBool && DBool)
                {
                    ChangeInfoSortie = Instantiate(salleLambdaObject_GD[idThemeSalle], SpawnPoint[i].transform.position, Quaternion.identity);
                }
                else if (HBool && !BBool && !GBool && !DBool)
                {
                    ChangeInfoSortie = Instantiate(salleLambdaObject_B[idThemeSalle], SpawnPoint[i].transform.position, Quaternion.identity);
                }
                else if (!HBool && BBool && !GBool && !DBool)
                {
                    ChangeInfoSortie = Instantiate(salleLambdaObject_H[idThemeSalle], SpawnPoint[i].transform.position, Quaternion.identity);
                }
                else if (!HBool && !BBool && GBool && !DBool)
                {
                    ChangeInfoSortie = Instantiate(salleLambdaObject_D[idThemeSalle], SpawnPoint[i].transform.position, Quaternion.identity);
                }
                else if (!HBool && !BBool && !GBool && DBool)
                {
                    ChangeInfoSortie = Instantiate(salleLambdaObject_G[idThemeSalle], SpawnPoint[i].transform.position, Quaternion.identity);
                }
                else if (!HBool && BBool && GBool && DBool)
                {
                    ChangeInfoSortie = Instantiate(salleLambdaObject_HDG[idThemeSalle], SpawnPoint[i].transform.position, Quaternion.identity);
                }
                else if (HBool && !BBool && GBool && DBool)
                {
                    ChangeInfoSortie = Instantiate(salleLambdaObject_DGB[idThemeSalle], SpawnPoint[i].transform.position, Quaternion.identity);
                }
                else if (HBool && BBool && !GBool && DBool)
                {
                    ChangeInfoSortie = Instantiate(salleLambdaObject_HGB[idThemeSalle], SpawnPoint[i].transform.position, Quaternion.identity);
                }
                else if (HBool && BBool && GBool && !DBool)
                {
                    ChangeInfoSortie = Instantiate(salleLambdaObject_HDB[idThemeSalle], SpawnPoint[i].transform.position, Quaternion.identity);
                }
                yield return new WaitForSeconds(0.15f);
            }
        }
        if (Palier == 15)
        {
            ChangeInfoSortie.GetComponent<RoomManager>().spawnExitBoss = salleBossObject[idThemeSalle];
            ChangeInfoSortie.GetComponent<RoomManager>().SpawnState = 3;
        }
        else
        {
            ChangeInfoSortie.GetComponent<RoomManager>().SpawnState = 2;
        }
    }
    IEnumerator PlayerDown()
    {
        LightControl.GetComponent<RenderLightPlayer>().Generate();
        GameObject player = GameObject.FindWithTag("player");
        float Ypos = SpawnPointPlayerPos.y - 5;
        while (Ypos >= -13)
        {
            Ypos -= 0.1f;
            yield return new WaitForSeconds(0.03f);
            player.transform.position = new Vector3(player.transform.position.x, Ypos, player.transform.position.z);
            player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
        player.GetComponent<Rigidbody>().useGravity = true;
    }
}
