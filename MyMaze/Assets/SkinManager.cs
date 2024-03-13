
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SkinManager : MonoBehaviour
{
    public List<Material> mats;
    public SkinnedMeshRenderer mesh;
    public List<GameObject> skins;
    public GameObject prefab;
    private int skinCount, matCount;
    public bool skin2Purchased, skin3Purchased, mat1Purchased;
    public List<bool> purchasedMatsAndSkins;


    private void Start()
    {
        skinCount = PlayerPrefs.GetInt("Skin");
        skins[skinCount].SetActive(true);
        matCount = PlayerPrefs.GetInt("Mat");
        mesh.material = mats[matCount];
    }
    public void ButtonClick(int ButtonNo)
    {

        if (purchasedMatsAndSkins[ButtonNo])
        {
            if (ButtonNo <= 19)
            {
                PlayerPrefs.SetInt("Mat", ButtonNo);
                mesh.material = mats[ButtonNo];

            }
            else
            {
                PlayerPrefs.SetInt("Skin", ButtonNo - 19);
                DeactivateSkins();
                skins[ButtonNo - 19].SetActive(true);
            }
            PlayerPrefs.Save();
        }

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
