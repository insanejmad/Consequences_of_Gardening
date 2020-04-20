using System.Collections.Generic;
using UnityEngine;

namespace Lib.Struct
{
    [System.Serializable]
    public struct CharacterDialog
    {
        public Character Character;
        public AudioClip Clip;
        public bool ClipOverrideAmbiance;
        [TextArea] public List<string> Sentences;
    }
}