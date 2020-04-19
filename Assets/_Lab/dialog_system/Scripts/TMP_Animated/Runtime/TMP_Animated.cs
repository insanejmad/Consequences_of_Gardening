using System;
using System.Text.RegularExpressions;
using System.Collections;
using System.Globalization;
using UnityEngine;
using UnityEngine.Events;

namespace TMPro {

    [System.Serializable] public class DialogEvent : UnityEvent {}

    public class TMP_Animated : TextMeshProUGUI
    {
        [SerializeField] float Speed = 20;
        [SerializeField] float PauseTime = .5f;

        public DialogEvent OnDialogFinish;

        public void Clear() {
            text = string.Empty;
            maxVisibleCharacters = 0;
        }

        public void Read(string textToRead) {
            Clear();

            text = textToRead;
            ForceMeshUpdate();

            // Remove TextMeshPro built-in tags and split with pause punctuation marks
            string[] subTexts = Regex.Split(Regex.Replace(textToRead, "<[^>]*>", ""), @"(,|\.{3})"); // Yes, I'm guilty... o/

            StartCoroutine(DisplayText());

            IEnumerator DisplayText() {
                int total_counter = 0;
                int visibleCounter = 0;

                for (int counter = 0; counter < subTexts.Length; counter++) {

                    while (visibleCounter < subTexts[counter].Length)
                    {
                        visibleCounter++;
                        maxVisibleCharacters++;
                        total_counter++;
                        yield return new WaitForSeconds(1f / Speed);
                    }
                
                    if (counter % 2 == 1)
                        yield return new WaitForSeconds(PauseTime);

                    visibleCounter = 0;
                }

                yield return null;

                OnDialogFinish.Invoke();
            }
        }
    }
}