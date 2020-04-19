using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lib.Struct;

[CreateAssetMenu(fileName = "New Dialog", menuName = "Dialog")]
public class Dialog : ScriptableObject
{
    public List<CharacterDialog> List = new List<CharacterDialog>();
}
