using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]

public class Item : ScriptableObject
{
    public string ItemName;

    public string Description;

    public string InventoryDescription;

    public Sprite Sprite;
    public Sprite InventorySprite;

    public Dialog InspectDialog;
}
