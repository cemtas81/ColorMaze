
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinManager : MonoBehaviour
{
   
    public SkinnedMeshRenderer mesh;
    public List<GameObject> skins;
    public GameObject prefab;
    private int skinCount, matCount, totalGem;
    //public bool skin2Purchased, skin3Purchased, mat1Purchased;
    //public List<bool> purchasedMatsAndSkins;
    public List<SkinOrMat> chars; 
    public List<SkinOrMat> scriptableSkins;
    public Text charName;
    public MenuScript script;
    private void Start()
    {
        skinCount = PlayerPrefs.GetInt("Skin");
        skins[skinCount].SetActive(true);
        matCount = PlayerPrefs.GetInt("Mat");
        mesh.material = chars[matCount].mat;
        totalGem = PlayerPrefs.GetInt("Score");
        if (PlayerPrefs.GetString("Name") == "")

            charName.text = chars[0].Charname;

        else
            charName.text = PlayerPrefs.GetString("Name");
    }
    public void ButtonClick(int ButtonNo)
    {

        if (ButtonNo <= 19)
        {

            if (chars[ButtonNo].purchased)
            {
                mesh.material = chars[ButtonNo].mat;
                charName.text = chars[ButtonNo].Charname;
                PlayerPrefs.SetInt("Mat", ButtonNo);
                PlayerPrefs.SetString("Name", charName.text);
            }
            else
            {
                if (totalGem >= chars[ButtonNo].price)
                {
                    mesh.material = chars[ButtonNo].mat;
                    charName.text = chars[ButtonNo].Charname;
                    PlayerPrefs.SetInt("Mat", ButtonNo);
                    PlayerPrefs.SetString("Name", charName.text);
                    totalGem -= chars[ButtonNo].price;                
                    chars[ButtonNo].purchased = true;
                    UpdateGem();
                }
            }
        }
        else
        {
            if (scriptableSkins[ButtonNo - 19].purchased)
            {
                PlayerPrefs.SetInt("Skin", ButtonNo - 19);
                DeactivateSkins();
                skins[ButtonNo - 19].SetActive(true);
            }
            else if (totalGem > scriptableSkins[ButtonNo - 19].price)
            {
                PlayerPrefs.SetInt("Skin", ButtonNo - 19);
                DeactivateSkins();
                skins[ButtonNo - 19].SetActive(true);
                totalGem -= scriptableSkins[ButtonNo-19].price;
                scriptableSkins[ButtonNo-19].purchased = true;
                UpdateGem();
            }
           
        }
        PlayerPrefs.Save();
    }
    void UpdateGem()
    {
        script.diamond.text = totalGem.ToString();
        PlayerPrefs.SetInt("Score", totalGem);
        PlayerPrefs.Save();
    }
    void DeactivateSkins()
    {
        foreach (GameObject skin in skins)
        {
            skin.SetActive(false);
        }
    }
    public void Bowin()
    {
        PlayerPrefs.SetInt("Skin", 0);
        DeactivateSkins();
    }

}
