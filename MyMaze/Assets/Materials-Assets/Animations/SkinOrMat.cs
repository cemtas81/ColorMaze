
using UnityEngine;
[CreateAssetMenu(fileName = "Skins&Mats", menuName = "Scriptables/Shop", order = 1)]
public class SkinOrMat : ScriptableObject
{
    public string Charname;
    public int price;
    public Material mat;
    public bool purchased;
}
