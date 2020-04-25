using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]

public class Item : ScriptableObject
{
    [Header("Basic Informations")]
    public string ItemName;
    public string Description;
    public string InventoryDescription;
    [Header("Sprites")]
    public Sprite Sprite;
    public Sprite InventorySprite;
    [Header("Conditions")]
    public List<Item> ItemsNeeded;
    public List<string> PnjDeathNeeded;
    [Header("Dialog")]
    public Dialog InspectDialog;
    public Dialog FailedConditionsDialog;
}
