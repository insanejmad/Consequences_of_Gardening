using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New DialogMeta", menuName = "DialogMeta")]
public class DialogMeta : ScriptableObject
{
    public Sprite Background;
    public Color FontColor;
    public AudioSource[] SpeakSoundList;
}
