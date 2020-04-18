using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TMPro {
    public class TMP_Animated : TextMeshProUGUI
    {
        [SerializeField] public float speed = 10;

        public void Clear() {
            text = string.Empty;
            maxVisibleCharacters = 0;
        }

        public void Read(string textToRead) {
            Clear();
            text = textToRead;

            StartCoroutine(DisplayText());

            IEnumerator DisplayText() {
                int counter = 0;
                while (counter < text.Length) {
                    counter++;
                    maxVisibleCharacters++;
                    yield return new WaitForSeconds(1f / speed);
                }
            }
        }
    }
}