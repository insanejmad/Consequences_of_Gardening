using System.Collections.Generic;
using UnityEngine;

namespace Lib.Struct
{
    [System.Serializable]
    public struct CharacterDialog
    {
        public Character Character;
        [TextArea] public List<string> Sentences;
    }
}