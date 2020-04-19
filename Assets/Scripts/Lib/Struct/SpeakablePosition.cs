using System.Collections.Generic;
using UnityEngine;
using DialogSystem;

namespace Lib.Struct
{
    [System.Serializable]
    public struct SpeakablePosition
    {
        public Transform Point;
        public Dialog Dialog;
    }
}