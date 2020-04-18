using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "TEMPItem")]

public class Item : ScriptableObject
{
    public string ItemName;

    public string Description;

    public Sprite Sprite;
}
