using UnityEngine;

[CreateAssetMenu]
public class Item : ScriptableObject
{
    /// <summary>
    /// helmet,hpPotion,weapon,mpPotion
    /// /// </summary>
    public enum itemType
    {
        helmet, hpPotion, weapon, mpPotion
    }
    public itemType type;
    public string itemName;
    public string itemInfo;
    public Sprite itemImage;
}