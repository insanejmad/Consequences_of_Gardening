using System;
using System.Text.RegularExpressions;
using System.Collections;
using System.Globalization;
using UnityEngine;
using UnityEngine.Events;

namespace TMPro {

    [System.Serializable] public class SentenceEvent : UnityEvent {}

    public class TMP_Animated : TextMeshProUGUI
    {
        public SentenceEvent OnSentenceFinished;
        public SentenceEvent OnCharacterRevealed;

        [SerializeField] float Speed = 20;
        [SerializeField] float PauseTime = .5f;
        bool Animated = true;

        public void Clear() {
            text = string.Empty;
            maxVisibleCharacters = 0;
        }

        public void AvoidAnimation() {
            Animated = false;
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

                        OnCharacterRevealed.Invoke();

                        if (Animated)
                            yield return new WaitForSeconds(1f / Speed);
                    }
                
                    if (counter % 2 == 1 && Animated)
                        yield return new WaitForSeconds(PauseTime);

                    visibleCounter = 0;
                }

                yield return null;

                Animated = true;
                OnSentenceFinished.Invoke();
            }
        }
    }
}