using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinManager : MonoBehaviour
{
    public SkinnedMeshRenderer mesh;
    public List<GameObject> skins;
    public List<GameObject> characterLocks; // List of character lock GameObjects
    public List<GameObject> scriptableSkinLocks; // List of scriptable skin lock GameObjects
    public GameObject prefab;
    private int skinCount, matCount, totalGem;
    public List<SkinOrMat> chars;
    public List<SkinOrMat> scriptableSkins;
    public Text charName;
    public MenuScript script;
    private const string MAT_KEY = "Mat";
    private const string SKIN_KEY = "Skin";
    private const string SCORE_KEY = "Score";
    private const string NAME_KEY = "Name";

    private void Start()
    {
        // Load saved data
        matCount = PlayerPrefs.GetInt(MAT_KEY, 0);
        skinCount = PlayerPrefs.GetInt(SKIN_KEY, 0);
        totalGem = PlayerPrefs.GetInt(SCORE_KEY, 0);
        charName.text = PlayerPrefs.GetString(NAME_KEY, chars[0].Charname);

        // Set character material
        mesh.material = chars[matCount].mat;

        // Activate selected skin
        skins[skinCount].SetActive(true);

        // Load purchased states
        LoadPurchasedStates(chars, characterLocks, "Character_");
        LoadPurchasedStates(scriptableSkins, scriptableSkinLocks, "ScriptableSkin_");
    }

    private void LoadPurchasedStates(List<SkinOrMat> items, List<GameObject> locks, string keyPrefix)
    {
        for (int i = 0; i < items.Count; i++)
        {
            bool purchased = PlayerPrefs.GetInt(keyPrefix + i, 0) == 1;
            items[i].purchased = purchased;

            // Deactivate lock if purchased
            if (purchased && locks.Count > i)
            {
                locks[i-1].SetActive(false);
            }
        }
        chars[0].purchased = true;
        scriptableSkins[0].purchased = true;
    }

    public void ButtonClick(int ButtonNo)
    {
        if (ButtonNo <= 19)
        {
            HandleCharacterButtonClick(ButtonNo);
        }
        else
        {
            HandleScriptableSkinButtonClick(ButtonNo);
        }

        // Save game state
        SaveGameState();
    }

    private void HandleCharacterButtonClick(int ButtonNo)
    {
        if (chars[ButtonNo].purchased || totalGem >= chars[ButtonNo].price)
        {
            mesh.material = chars[ButtonNo].mat;
            charName.text = chars[ButtonNo].Charname;
            matCount = ButtonNo;
            PlayerPrefs.SetInt(MAT_KEY, matCount);
            PlayerPrefs.SetString(NAME_KEY, charName.text);

            if (!chars[ButtonNo].purchased)
            {
                totalGem -= chars[ButtonNo].price;
                chars[ButtonNo].purchased = true;
                PlayerPrefs.SetInt("Character_" + ButtonNo, 1);
                UpdateGem();

                // Deactivate lock
                if (characterLocks.Count > ButtonNo)
                {
                    characterLocks[ButtonNo].SetActive(false);
                }
            }
        }
    }

    private void HandleScriptableSkinButtonClick(int ButtonNo)
    {
        if (scriptableSkins[ButtonNo - 19].purchased || totalGem > scriptableSkins[ButtonNo - 19].price)
        {
            skinCount = ButtonNo - 19;
            PlayerPrefs.SetInt(SKIN_KEY, skinCount);
            DeactivateSkins();
            skins[skinCount].SetActive(true);

            if (!scriptableSkins[ButtonNo - 19].purchased)
            {
                totalGem -= scriptableSkins[ButtonNo - 19].price;
                scriptableSkins[ButtonNo - 19].purchased = true;
                PlayerPrefs.SetInt("ScriptableSkin_" + (ButtonNo - 19), 1);
                UpdateGem();

                // Deactivate lock
                if (scriptableSkinLocks.Count > ButtonNo - 19)
                {
                    scriptableSkinLocks[ButtonNo - 20].SetActive(false);
                }
            }
        }
    }

    private void SaveGameState()
    {
        PlayerPrefs.SetInt(SCORE_KEY, totalGem);
        PlayerPrefs.Save();
    }

    private void UpdateGem()
    {
        script.diamond.text = totalGem.ToString();
        SaveGameState();
    }

    private void DeactivateSkins()
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
