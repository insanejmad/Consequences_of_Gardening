using System;
using System.Text.RegularExpressions;
using System.Collections;
using System.Globalization;
using UnityEngine;

namespace TMPro {
    public class TMP_Animated : TextMeshProUGUI
    {
        [SerializeField] float speed = 10;
        [SerializeField] float pauseTime = .2f;

        public void Clear() {
            text = string.Empty;
            maxVisibleCharacters = 0;
        }

        public void Read(string textToRead) {
            Clear();

            string[] subTexts = Regex.Split(textToRead, @"(,|\.{3})"); // Yes, guilty... o/
            text = textToRead;

            StartCoroutine(DisplayText());

            IEnumerator DisplayText() {
                int counter = 0;
                int visibleCounter = 0;

                while (counter < subTexts.Length) {

                    while (visibleCounter < subTexts[counter].Length)
                    {
                        visibleCounter++;
                        maxVisibleCharacters++;
                        yield return new WaitForSeconds(1f / speed);
                    }

                    if (counter % 2 == 1) // If pause punctuation
                        yield return new WaitForSeconds(pauseTime);

                    visibleCounter = 0;
                    counter++;
                }
                yield return null;
            }
        }
    }
}