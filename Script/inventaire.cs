using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class ClassInfoItem
{
    public string Name;
    [TextArea(3, 10)]
    public string Description;
    public Texture TableauImageID;
    public bool Use, Move, Drop;
    public Behaviour ScriptUse;
    public Transform ObjectSpawn;
}
public class inventaire : MonoBehaviour
{
    [HideInInspector] public int[] EmplacementID, EmplacementNombreObject, EmplacementIDMaterial, EmplacementNombreObjectMaterial, EmplacementNombreObjectSpecial1, EmplacementNombreObjectSpecial2;
    [SerializeField] private ClassInfoItem[] InfoItem = null;

    private GameObject TexteAddItem1, TexteAddItem2, RawImageAddItem, AnimationAddItem;
    public int IdLoot, VarNombreObject;
    public bool ObjectMax, Onloot, OnDrop, OpenInventaire;
    public Behaviour[] DisableComponent;
    private int EtapeLoot;
    private GameObject AntiAsiqgneSpam;
    public bool OnDialog;
    private GameObject Inventaire = null;
    public AudioSource m_Audio;
    public AudioClip m_Open, Close, TakeObject, Drop_sound, Use_sound, Erreur_sound;
    private bool itemSpecial, Material;
    public LightColorSpawn ScriptLumierre;
    public GameObject DropGameObject;
    //UI
    private GameObject[] UIInventaireEquipement, UIInventaireResource, TexteObjectNombre, ImageObjectNombre, ImageFavorie;
    private GameObject allInfoUITexteObject, allTO_image, allTO_text_name, allTO_text_Description, selected_Image;
    private bool selectedID;
    private int lastIDSelected, NewIDSelected;
    private int MenuEquipement, MenuResource;
    private int CursorEmplacementID;
    private Canvas myCanvas;
    private Vector2 pos;

    public void ChercheElement()
    {
        selected_Image = GameObject.Find("selecte_Image_bsdg");
        Inventaire = GameObject.Find("Inventaire_qdrgio");
        allInfoUITexteObject = GameObject.Find("AITO_qsdgf");// sert a afficher les infos des item dans l'inventaire
        allTO_image = GameObject.Find("AITO_Image_qsdf");
        allTO_text_name = GameObject.Find("qmlkdfjq_AITO_Name");
        allTO_text_Description = GameObject.Find("qmlkdfjq_AITO_Description");
        myCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        allInfoUITexteObject.SetActive(false);
        selected_Image.SetActive(false);
        for (int i = 0; i < ImageFavorie.Length; i++)
        {
            ImageFavorie[i] = GameObject.Find("F" + (i + 1).ToString() + "_Image_bsdg");
        }
        for (int i = 0; i < TexteObjectNombre.Length; i++)
        {
            TexteObjectNombre[i] = GameObject.Find("TexteID_fhgjfh_" + i.ToString());
            TexteObjectNombre[i].SetActive(i == 41 || i == 42);
        }
        for (int i = 0; i < ImageObjectNombre.Length; i++)
        {
            ImageObjectNombre[i] = GameObject.Find("Image_sdgfh_" + i.ToString());
        }
        for (int i = 0; i < UIInventaireEquipement.Length; i++)
        {
            UIInventaireEquipement[i] = GameObject.Find("Equipement_qsdsq_" + (i + 1).ToString());
            UIInventaireEquipement[i].SetActive(false);
        }
        for (int i = 0; i < UIInventaireResource.Length; i++)
        {
            UIInventaireResource[i] = GameObject.Find("Resource_qsdsq_" + (i + 1).ToString());
            UIInventaireResource[i].SetActive(false);
        }
        TexteAddItem1 = GameObject.Find("Text _azsdk_1");
        TexteAddItem2 = GameObject.Find("Text _azsdk_2");
        RawImageAddItem = GameObject.Find("Image_azsdk");
        AnimationAddItem = GameObject.Find("AnimeInventaireAdd_azsdk");
        Inventaire.SetActive(false);
        ActiveScriptItemID();
    }

    void Start()
    {
        EtapeLoot = 0;
        EmplacementID = new int[12];
        EmplacementNombreObject = new int[12];
        EmplacementIDMaterial = new int[15];
        EmplacementNombreObjectMaterial = new int[15];
        EmplacementNombreObjectSpecial1 = new int[14];
        EmplacementNombreObjectSpecial2 = new int[5];
        UIInventaireEquipement = new GameObject[3];
        UIInventaireResource = new GameObject[2];
        TexteObjectNombre = new GameObject[46];
        ImageObjectNombre = new GameObject[41];
        ImageFavorie = new GameObject[2];
        ChercheElement();
    }

    void Update()
    {
        if (Input.GetButtonDown("Favori1"))
        {
            if (EmplacementID[EmplacementNombreObjectSpecial2[3]] > 0)
            {
                InfoItem[EmplacementID[EmplacementNombreObjectSpecial2[3]]].ScriptUse.SendMessage("UseItem" + EmplacementID[EmplacementNombreObjectSpecial2[3]].ToString());
                EmplacementNombreObject[EmplacementNombreObjectSpecial2[3]]--;
                RemoveAnimationF(EmplacementID[EmplacementNombreObjectSpecial2[3]], EmplacementNombreObject[EmplacementNombreObjectSpecial2[3]], 1);
                if (EmplacementNombreObject[EmplacementNombreObjectSpecial2[3]] == 0)
                {
                    EmplacementID[EmplacementNombreObjectSpecial2[3]] = 0;
                }
                PlayOneShotSound(Use_sound);
            }
            else
            {
                PlayOneShotSound(Erreur_sound);
            }
            NomberImageSet();
        }
        if (Input.GetButtonDown("Favori2"))
        {
            if (EmplacementID[EmplacementNombreObjectSpecial2[4]] > 0)
            {
                InfoItem[EmplacementID[EmplacementNombreObjectSpecial2[4]]].ScriptUse.SendMessage("UseItem" + EmplacementID[EmplacementNombreObjectSpecial2[4]].ToString());
                EmplacementNombreObject[EmplacementNombreObjectSpecial2[4]]--;
                RemoveAnimationF(EmplacementID[EmplacementNombreObjectSpecial2[4]], EmplacementNombreObject[EmplacementNombreObjectSpecial2[4]], 1);
                if (EmplacementNombreObject[EmplacementNombreObjectSpecial2[4]] == 0)
                {
                    EmplacementID[EmplacementNombreObjectSpecial2[4]] = 0;
                }
                PlayOneShotSound(Use_sound);
            }
            else
            {
                PlayOneShotSound(Erreur_sound);
            }
            NomberImageSet();
        }
        if (Input.GetButtonDown("Interagire") && OpenInventaire && lastIDSelected != -1 && !selectedID)
        {
            if (lastIDSelected >= 0 && lastIDSelected <= 12 && InfoItem[EmplacementID[lastIDSelected]].Use)
            {
                //Debug.Log("UseItem" + EmplacementID[lastIDSelected]);
                InfoItem[EmplacementID[lastIDSelected]].ScriptUse.SendMessage("UseItem" + EmplacementID[lastIDSelected].ToString());
                EmplacementNombreObject[lastIDSelected]--;
                if (EmplacementNombreObject[lastIDSelected] == 0)
                {
                    EmplacementID[lastIDSelected] = 0;
                }
                PlayOneShotSound(Use_sound);
                allInfoUITexteObject.SetActive(false);
            }
            else if (lastIDSelected >= 26 && lastIDSelected <= 40 && InfoItem[EmplacementIDMaterial[lastIDSelected - 26]].Use)
            {
                InfoItem[EmplacementIDMaterial[lastIDSelected]].ScriptUse.SendMessage("UseItem" + EmplacementIDMaterial[lastIDSelected].ToString());
                EmplacementNombreObjectMaterial[lastIDSelected - 26]--;
                if (EmplacementNombreObjectMaterial[lastIDSelected - 26] == 0)
                {
                    EmplacementIDMaterial[lastIDSelected - 26] = 0;
                }
                PlayOneShotSound(Use_sound);
                allInfoUITexteObject.SetActive(false);
            }
            else if (lastIDSelected >= 44 && InfoItem[EmplacementID[EmplacementNombreObjectSpecial2[lastIDSelected - 41]]].Use)
            {
                InfoItem[EmplacementID[EmplacementNombreObjectSpecial2[lastIDSelected - 41]]].ScriptUse.SendMessage("UseItem" + EmplacementID[EmplacementNombreObjectSpecial2[lastIDSelected - 41]].ToString());
                EmplacementNombreObject[EmplacementNombreObjectSpecial2[lastIDSelected - 41]]--;
                if (EmplacementNombreObject[EmplacementNombreObjectSpecial2[lastIDSelected - 41]] == 0)
                {
                    EmplacementID[EmplacementNombreObjectSpecial2[lastIDSelected - 41]] = 0;
                }
                PlayOneShotSound(Use_sound);
                allInfoUITexteObject.SetActive(false);
            }
            else
            {
                PlayOneShotSound(Erreur_sound);
            }
            NomberImageSet();
        }
        if (Input.GetButtonDown("Drop") && OpenInventaire && lastIDSelected != -1 && !selectedID)
        {
            Transform SpawnLootDrop = null;
            if (lastIDSelected >= 0 && lastIDSelected <= 12 && InfoItem[EmplacementID[lastIDSelected]].Drop)
            {
                SpawnLootDrop = Instantiate(InfoItem[EmplacementID[lastIDSelected]].ObjectSpawn, DropGameObject.transform.position, Quaternion.identity);
                SpawnLootDrop.SendMessage("ModificationNombre", EmplacementNombreObject[lastIDSelected]);
                EmplacementNombreObject[lastIDSelected] = 0;
                EmplacementID[lastIDSelected] = 0;
                allInfoUITexteObject.SetActive(false);
                PlayOneShotSound(Drop_sound);
            }
            else if (lastIDSelected >= 26 && lastIDSelected <= 40 && InfoItem[EmplacementIDMaterial[lastIDSelected - 26]].Drop)
            {
                SpawnLootDrop = Instantiate(InfoItem[EmplacementIDMaterial[lastIDSelected - 26]].ObjectSpawn, DropGameObject.transform.position, Quaternion.identity);
                SpawnLootDrop.SendMessage("ModificationNombre", EmplacementNombreObjectMaterial[lastIDSelected - 26]);
                EmplacementNombreObjectMaterial[lastIDSelected - 26] = 0;
                EmplacementIDMaterial[lastIDSelected - 26] = 0;
                allInfoUITexteObject.SetActive(false);
                PlayOneShotSound(Drop_sound);
            }
            else
            {
                PlayOneShotSound(Erreur_sound);
            }
            NomberImageSet();
        }
        if (Input.GetButtonDown("Inventaire") && (!OnDialog || OpenInventaire))
        {
            OpenInventaire = !OpenInventaire;
            selected_Image.SetActive(false);
            if (OpenInventaire) { PlayOneShotSound(m_Open); } else { PlayOneShotSound(Close); }
            gameObject.SendMessage("ReceiveInventaireState", OpenInventaire);
            for (int i = 0; i < DisableComponent.Length; i++)
            {
                DisableComponent[i].enabled = !OpenInventaire;
            }
            MenuChange();
            NomberImageSet();
            Inventaire.SetActive(OpenInventaire);
        }
        if (OpenInventaire)
        {
            if (EventSystem.current.IsPointerOverGameObject() && !Input.GetMouseButton(0))
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(myCanvas.transform as RectTransform, Input.mousePosition, myCanvas.worldCamera, out pos);
                allInfoUITexteObject.transform.position = myCanvas.transform.TransformPoint(pos);
            }
            else
            {
                allInfoUITexteObject.SetActive(false);
            }
            if (!selectedID && !EventSystem.current.IsPointerOverGameObject()) { lastIDSelected = -1; }
            if (Input.GetMouseButtonDown(0) && lastIDSelected != -1 && !(lastIDSelected >= 41))
            {
                selectedID = true;
                NewIDSelected = lastIDSelected;
                if (NewIDSelected >= 26)
                {
                    selected_Image.GetComponent<UnityEngine.UI.RawImage>().texture = InfoItem[EmplacementIDMaterial[NewIDSelected - 26]].TableauImageID;
                }
                else
                {
                    selected_Image.GetComponent<UnityEngine.UI.RawImage>().texture = InfoItem[EmplacementID[NewIDSelected]].TableauImageID;
                }
                selected_Image.SetActive(true);
            }
            if (selectedID && Input.GetMouseButton(0))
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(myCanvas.transform as RectTransform, Input.mousePosition, myCanvas.worldCamera, out pos);
                selected_Image.transform.position = myCanvas.transform.TransformPoint(pos);
            }
            if (Input.GetMouseButtonUp(0) && lastIDSelected != NewIDSelected && lastIDSelected != -1)
            {
                PermutePostitionObject(lastIDSelected, NewIDSelected);
                NomberImageSet();
                selectedID = false;
                selected_Image.SetActive(false);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                NewIDSelected = -1;
                lastIDSelected = -1;
                selected_Image.SetActive(false);
                selectedID = false;
            }
        }
        if (Onloot)
        {
            if (EtapeLoot == 3)
            {
                VerificationInventaire();
                if (!itemSpecial)
                {
                    if (!Material)
                    {
                        if (ObjectMax)
                        {
                            for (int i = 0; i < EmplacementID.Length; i++)
                            {
                                if (EmplacementID[i] == IdLoot)
                                {
                                    EmplacementNombreObject[i] += VarNombreObject;
                                    Destroy(AntiAsiqgneSpam);
                                    PlayOneShotSound(TakeObject);
                                    AddAnimationF(IdLoot, EmplacementNombreObject[i], VarNombreObject);
                                    break;
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < EmplacementID.Length; i++)
                            {
                                if (EmplacementID[i] == 0)
                                {
                                    EmplacementID[i] = IdLoot;
                                    EmplacementNombreObject[i] = VarNombreObject;
                                    Destroy(AntiAsiqgneSpam);
                                    PlayOneShotSound(TakeObject);
                                    AddAnimationF(IdLoot, EmplacementNombreObject[i], VarNombreObject);
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (ObjectMax)
                        {
                            for (int i = 0; i < EmplacementIDMaterial.Length; i++)
                            {
                                if (EmplacementIDMaterial[i] == IdLoot)
                                {
                                    EmplacementNombreObjectMaterial[i] += VarNombreObject;
                                    Destroy(AntiAsiqgneSpam);
                                    PlayOneShotSound(TakeObject);
                                    AddAnimationF(IdLoot, EmplacementNombreObjectMaterial[i], VarNombreObject);
                                    break;
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < EmplacementIDMaterial.Length; i++)
                            {
                                if (EmplacementIDMaterial[i] == 0)
                                {
                                    EmplacementIDMaterial[i] = IdLoot;
                                    EmplacementNombreObjectMaterial[i] = VarNombreObject;
                                    Destroy(AntiAsiqgneSpam);
                                    PlayOneShotSound(TakeObject);
                                    AddAnimationF(IdLoot, EmplacementNombreObjectMaterial[i], VarNombreObject);
                                    break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (IdLoot < 40)
                    {
                        for (int i = 0; i < EmplacementNombreObjectSpecial1.Length; i++)
                        {
                            if ((i + 12) == IdLoot)
                            {
                                EmplacementNombreObjectSpecial1[i] += VarNombreObject;
                                Destroy(AntiAsiqgneSpam);
                                PlayOneShotSound(TakeObject);
                                AddAnimationF(IdLoot, EmplacementNombreObjectSpecial1[i], VarNombreObject);
                                break;
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < EmplacementNombreObjectSpecial2.Length; i++)
                        {
                            if ((i + 41) == IdLoot)
                            {
                                EmplacementNombreObjectSpecial2[i] += VarNombreObject;
                                Destroy(AntiAsiqgneSpam);
                                PlayOneShotSound(TakeObject);
                                AddAnimationF(IdLoot, EmplacementNombreObjectSpecial2[i], VarNombreObject);
                                break;
                            }
                        }
                    }
                }
                IdLoot = 0;
                VarNombreObject = 0;
                EtapeLoot = 0;
                Onloot = false;
                NomberImageSet();
                ActiveScriptItemID();
            }
        }
    }

    public bool UseItemIdExtern(int id)
    {
        for (int i = 0; i < EmplacementID.Length; i++)
        {
            if (EmplacementID[i] == id)
            {
                EmplacementNombreObject[i]--;
                if (EmplacementNombreObject[i] == 0)
                {
                    EmplacementID[i] = 0;
                }
                RemoveAnimationF(EmplacementID[i], EmplacementNombreObject[i], 1);
                return true;
            }
        }
        for (int i = 0; i < EmplacementIDMaterial.Length; i++)
        {
            if (EmplacementIDMaterial[i] == id)
            {
                EmplacementNombreObjectMaterial[i]--;
                if (EmplacementNombreObjectMaterial[i] == 0)
                {
                    EmplacementIDMaterial[i] = 0;
                }
                RemoveAnimationF(EmplacementIDMaterial[i], EmplacementNombreObjectMaterial[i], 1);
                return true;
            }
        }
        if (id >= 12 && id <= 25)
        {
            if (EmplacementNombreObjectSpecial1[id - 12] > 0)
            {
                EmplacementNombreObjectSpecial1[id - 12]--;
                RemoveAnimationF(id, EmplacementNombreObjectSpecial1[id - 12], 1);
                return true;
            }
        }
        if (id >= 41)
        {
            if (EmplacementNombreObjectSpecial2[id - 41] > 0)
            {
                EmplacementNombreObjectSpecial2[id - 41]--;
                RemoveAnimationF(id, EmplacementNombreObjectSpecial2[id - 41], 1);
                return true;
            }
        }
        return false;
    }

    void VerificationInventaire()
    {
        if (!Material)
        {
            for (int i = 0; i < EmplacementID.Length; i++)
            {
                if (EmplacementID[i] == 0)
                {
                    ObjectMax = false;
                    break;
                }
                else
                {
                    ObjectMax = true;
                }
                if (EmplacementID[i] == IdLoot)
                {
                    ObjectMax = true;
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < EmplacementIDMaterial.Length; i++)
            {
                if (EmplacementIDMaterial[i] == 0)
                {
                    ObjectMax = false;
                    break;
                }
                else
                {
                    ObjectMax = true;
                }
                if (EmplacementIDMaterial[i] == IdLoot)
                {
                    ObjectMax = true;
                    break;
                }
            }
        }
    }

    void Drop()
    {
        if (!Onloot)
        {
            OnDrop = true;
            AntiAsiqgneSpam = null;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "ObjectLoot" && (AntiAsiqgneSpam != other.gameObject) && !OnDrop && !Onloot)
        {
            AntiAsiqgneSpam = other.gameObject;
            Onloot = true;
            EtapeLoot = 1;
            other.gameObject.SendMessage("SendInfoObjectLoot");
        }
    }

    public void ReceiveSendMessageInfoisSpecial(bool S) { itemSpecial = S; }
    public void ReceiveSendMessageInfoisMaterial(bool M) { Material = M; }
    public void ReceiveSendMessageInfoAudioClip(AudioClip Sound) { TakeObject = Sound; }
    public void ReceiveSendMessageInfoID(int nombre) { IdLoot = nombre; EtapeLoot += 1; }
    public void ReceiveSendMessageInfoNombre(int nombre) { VarNombreObject = nombre; EtapeLoot += 1; }
    public void ReceiveDialogState(bool Dialog) { OnDialog = Dialog; }

    void MenuChange()
    {
        for (int i = 0; i < UIInventaireEquipement.Length; i++)
        {
            UIInventaireEquipement[i].SetActive(i == MenuEquipement);
        }
        for (int i = 0; i < UIInventaireResource.Length; i++)
        {
            UIInventaireResource[i].SetActive(i == MenuResource);
        }
    }

    void NomberImageSet()
    {
        for (int i = 0; i < EmplacementID.Length; i++)
        {
            TexteObjectNombre[i].GetComponent<TextMeshProUGUI>().text = "X" + EmplacementNombreObject[i].ToString();
            ImageObjectNombre[i].GetComponent<UnityEngine.UI.RawImage>().texture = InfoItem[EmplacementID[i]].TableauImageID;
            TexteObjectNombre[i].SetActive(EmplacementNombreObject[i] > 0);
        }
        for (int i = 0; i < EmplacementIDMaterial.Length; i++)
        {
            TexteObjectNombre[i + 26].GetComponent<TextMeshProUGUI>().text = "X" + EmplacementNombreObjectMaterial[i].ToString();
            ImageObjectNombre[i + 26].GetComponent<UnityEngine.UI.RawImage>().texture = InfoItem[EmplacementIDMaterial[i]].TableauImageID;
            TexteObjectNombre[i + 26].SetActive(EmplacementNombreObjectMaterial[i] > 0);
        }
        for (int i = 0; i < EmplacementNombreObjectSpecial1.Length; i++)
        {
            TexteObjectNombre[i + 12].GetComponent<TextMeshProUGUI>().text = "X" + EmplacementNombreObjectSpecial1[i].ToString();
            TexteObjectNombre[i + 12].SetActive(EmplacementNombreObjectSpecial1[i] > 0);
            if (EmplacementNombreObjectSpecial1[i] > 0)
            {
                ImageObjectNombre[i + 12].GetComponent<UnityEngine.UI.RawImage>().texture = InfoItem[i + 12].TableauImageID;
            }
            else
            {
                ImageObjectNombre[i + 12].GetComponent<UnityEngine.UI.RawImage>().texture = InfoItem[0].TableauImageID;
            }
        }
        for (int i = 0; i < 3; i++)
        {
            TexteObjectNombre[i + 41].GetComponent<TextMeshProUGUI>().text = "X" + EmplacementNombreObjectSpecial2[i].ToString();
        }
        for (int i = 0; i < 2; i++)
        {
            TexteObjectNombre[i + 44].GetComponent<TextMeshProUGUI>().text = "X" + EmplacementNombreObject[EmplacementNombreObjectSpecial2[i + 3]].ToString();
            TexteObjectNombre[i + 44].SetActive(EmplacementID[EmplacementNombreObjectSpecial2[i + 3]] > 0);
            ImageFavorie[i].GetComponent<UnityEngine.UI.RawImage>().texture = InfoItem[EmplacementID[EmplacementNombreObjectSpecial2[i + 3]]].TableauImageID;
        }
    }

    void AddAnimationF(int Id, int NB, int ajout)
    {
        if (Id == 43)
        {
            TexteAddItem1.GetComponent<TextMeshProUGUI>().text = "x1";
            TexteAddItem2.GetComponent<TextMeshProUGUI>().text = "";
        }
        else
        {
            TexteAddItem1.GetComponent<TextMeshProUGUI>().text = "x" + NB.ToString();
            TexteAddItem2.GetComponent<TextMeshProUGUI>().text = "+" + ajout.ToString();
        }
        RawImageAddItem.GetComponent<UnityEngine.UI.RawImage>().texture = InfoItem[Id].TableauImageID;
        AnimationAddItem.GetComponent<Animator>().Play("AddNewItem", 0, 0.25f);
    }

    void RemoveAnimationF(int Id, int NB, int ajout)
    {
        if (Id == 43)
        {
            TexteAddItem1.GetComponent<TextMeshProUGUI>().text = "x1";
            TexteAddItem2.GetComponent<TextMeshProUGUI>().text = "";
        }
        else
        {
            TexteAddItem1.GetComponent<TextMeshProUGUI>().text = "x" + NB.ToString();
            TexteAddItem2.GetComponent<TextMeshProUGUI>().text = "-" + ajout.ToString();
        }
        RawImageAddItem.GetComponent<UnityEngine.UI.RawImage>().texture = InfoItem[Id].TableauImageID;
        AnimationAddItem.GetComponent<Animator>().Play("AddNewItem", 0, 0.25f);
    }

    public void EquipementNumReceive(int ME) { MenuEquipement = ME; MenuChange(); }
    public void ResourceNumReceive(int MR) { MenuResource = MR; MenuChange(); }

    void PlayOneShotSound(AudioClip audioSound)
    {
        m_Audio.clip = audioSound; m_Audio.PlayOneShot(m_Audio.clip);
    }

    void ActiveScriptItemID()
    {
        ScriptLumierre.enabled = (EmplacementNombreObjectSpecial2[2] > 0);
    }

    public void EnterUI_ID(int IDE)
    {
        if (EmplacementNombreObject[IDE] > 0)
        {
            allTO_text_name.GetComponent<TextMeshProUGUI>().text = InfoItem[EmplacementID[IDE]].Name;
            allTO_text_Description.GetComponent<TextMeshProUGUI>().text = InfoItem[EmplacementID[IDE]].Description;
            allTO_image.GetComponent<UnityEngine.UI.RawImage>().texture = InfoItem[EmplacementID[IDE]].TableauImageID;
            if (!Input.GetMouseButton(0))
            {
                allInfoUITexteObject.SetActive(true);
            }
        }
        lastIDSelected = IDE;
    }

    public void EnterUI_IDM(int IDE)
    {
        if (EmplacementNombreObjectMaterial[IDE - 26] > 0)
        {
            allTO_text_name.GetComponent<TextMeshProUGUI>().text = InfoItem[EmplacementIDMaterial[IDE - 26]].Name;
            allTO_text_Description.GetComponent<TextMeshProUGUI>().text = InfoItem[EmplacementIDMaterial[IDE - 26]].Description;
            allTO_image.GetComponent<UnityEngine.UI.RawImage>().texture = InfoItem[EmplacementIDMaterial[IDE - 26]].TableauImageID;
            if (!Input.GetMouseButton(0))
            {
                allInfoUITexteObject.SetActive(true);
            }
        }
        lastIDSelected = IDE;
    }

    public void EnterUI_IDS1(int IDE)
    {
        if (EmplacementNombreObjectSpecial1[IDE - 12] > 0)
        {
            allTO_text_name.GetComponent<TextMeshProUGUI>().text = InfoItem[IDE].Name;
            allTO_text_Description.GetComponent<TextMeshProUGUI>().text = InfoItem[IDE].Description;
            allTO_image.GetComponent<UnityEngine.UI.RawImage>().texture = InfoItem[IDE].TableauImageID;
            allInfoUITexteObject.SetActive(true);
        }
    }

    public void EnterUI_IDS2(int IDE)
    {
        if (EmplacementNombreObjectSpecial2[IDE - 41] > 0 && IDE < 44)
        {
            allTO_text_name.GetComponent<TextMeshProUGUI>().text = InfoItem[IDE].Name;
            allTO_text_Description.GetComponent<TextMeshProUGUI>().text = InfoItem[IDE].Description;
            allTO_image.GetComponent<UnityEngine.UI.RawImage>().texture = InfoItem[IDE].TableauImageID;
            if (!Input.GetMouseButton(0))
            {
                allInfoUITexteObject.SetActive(true);
            }
        }
        else if (IDE >= 44)
        {
            allTO_text_name.GetComponent<TextMeshProUGUI>().text = InfoItem[EmplacementID[EmplacementNombreObjectSpecial2[IDE - 41]]].Name;
            allTO_text_Description.GetComponent<TextMeshProUGUI>().text = InfoItem[EmplacementID[EmplacementNombreObjectSpecial2[IDE - 41]]].Description;
            allTO_image.GetComponent<UnityEngine.UI.RawImage>().texture = InfoItem[EmplacementID[EmplacementNombreObjectSpecial2[IDE - 41]]].TableauImageID;
            if (!Input.GetMouseButton(0))
            {
                allInfoUITexteObject.SetActive(true);
            }
        }
        lastIDSelected = IDE;
    }

    void PermutePostitionObject(int place1, int place2)
    {
        //Debug.Log(place1 + " to " + place2);
        int idTransition, nbTransition;
        if (place1 >= 26 && place1 <= 40 && place2 >= 26 && place2 <= 40)// interval des materiaux
        {
            if (InfoItem[EmplacementIDMaterial[place1 - 26]].Move && InfoItem[EmplacementIDMaterial[place2 - 26]].Move)
            {
                idTransition = EmplacementIDMaterial[place2 - 26];
                nbTransition = EmplacementNombreObjectMaterial[place2 - 26];
                EmplacementIDMaterial[place2 - 26] = EmplacementIDMaterial[place1 - 26];
                EmplacementNombreObjectMaterial[place2 - 26] = EmplacementNombreObjectMaterial[place1 - 26];
                EmplacementIDMaterial[place1 - 26] = idTransition;
                EmplacementNombreObjectMaterial[place1 - 26] = nbTransition;
            }
        }
        if (place1 >= 0 && place1 <= 12 && place2 >= 0 && place2 <= 12)
        {
            if (InfoItem[EmplacementID[place1]].Move && InfoItem[EmplacementID[place2]].Move)
            {
                idTransition = EmplacementID[place2];
                nbTransition = EmplacementNombreObject[place2];
                EmplacementID[place2] = EmplacementID[place1];
                EmplacementNombreObject[place2] = EmplacementNombreObject[place1];
                EmplacementID[place1] = idTransition;
                EmplacementNombreObject[place1] = nbTransition;
            }
        }
        if (place1 >= 44 && place2 >= 0 && place2 <= 12)
        {
            //Debug.Log("Favorie");//attention apres l'id 44 ce n'est plus un nombre d'object mais un pointeur sur l'emplacement.
            if (InfoItem[EmplacementID[place2]].Use)
            {
                EmplacementNombreObjectSpecial2[place1 - 41] = place2;
            }
        }
    }
}
