﻿using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using Lib.Struct;

namespace DialogSystem
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UIDialogManager : MonoBehaviour
    {
        public static UIDialogManager Instance;

        public float DialogOpenDelay = .65f;
        public float DialogOpenSpeed = .3f;
        public TMP_Animated Text;
        public Dialog Dialog;
        public bool InDialog { get; private set; } = false;
        public bool HasNextDialog { get; private set; } = false;
        public bool HasNextSentence { get; private set; } = false;
        public bool TextWasRead { get; private set; } = false;
        public bool CanStop { get; private set; } = false;

        int DialogIndex = 0;
        int SentenceIndex = 0;
        CharacterDialog CurrentDialog { get => Dialog.List[DialogIndex]; }
        string CurrentSentence { get => CurrentDialog.Sentences[SentenceIndex]; }
        CanvasGroup CanvasGroup;
        Character Character;

        void OnEnable()
        {
            Text.OnDialogFinish.AddListener(OnFinishDialog);
        }

        void OnDisable()
        {
            Text.OnDialogFinish.RemoveListener(OnFinishDialog);
        }

        void Awake() {
            CanvasGroup = GetComponent<CanvasGroup>();
            DisplayDialogUI(false, 0, 0);
        }

        void Update() {
            if (InDialog && Input.GetMouseButtonDown(0))
                HandlePlayerAction();

            if (!InDialog && null != Dialog)
                StartDialog();
        }

        void UpdateUI() {
            // TODO Implement
            Text.Clear();
        }

        void HandlePlayerAction() {
            if (!TextWasRead) {
                Text.AvoidAnimation();
                return;
            }

            TextWasRead = false;

            if (CanStop)
                StopDialog();
            else if (HasNextSentence)
                Text.Read(CurrentSentence);
            else if (HasNextDialog)
                SwitchCharacter();

        }

        void StartDialog() {
            InDialog = true;
            Character = CurrentDialog.Character;
            UpdateUI();
            DisplayDialogUI(true, DialogOpenSpeed, DialogOpenDelay);
        }

        void StopDialog() {
            Dialog = null;
            Character = null;
            DialogIndex = 0;
            SentenceIndex = 0;
            InDialog = false;
            HasNextDialog = false;
            HasNextSentence = false;
            CanStop = false;
            DisplayDialogUI(false, DialogOpenSpeed, 0);
        }

        void SwitchCharacter() {
            Character NextCharacter = CurrentDialog.Character;

            if (NextCharacter.Name == Character.Name) { // If it's the same character, just continue reading as usual
                Text.Read(CurrentSentence);
                return;
            }

            Character = NextCharacter;

            DisplayDialogUI(false, DialogOpenSpeed, 0).AppendCallback(() => {
                UpdateUI();
                DisplayDialogUI(true, DialogOpenSpeed, DialogOpenDelay);
            });
        }

        Sequence DisplayDialogUI(bool show, float time, float delay)
        {
            Sequence s = DOTween.Sequence();
            s.AppendInterval(delay);
            s.Append(CanvasGroup.transform.DOScale(show ? 1 : 0, time).SetEase(show ? Ease.OutBack : Ease.InBack));

            if (show)
                s.AppendCallback(() => Text.Read(CurrentSentence));

            return s;
        }

        public void OnFinishDialog()
        {
            TextWasRead = true;
            HasNextDialog = false;
            HasNextSentence = false;

            if (SentenceIndex < CurrentDialog.Sentences.Count - 1) {
                SentenceIndex++;
                HasNextSentence = true;
            } else if (DialogIndex < Dialog.List.Count - 1) {
                DialogIndex++;
                SentenceIndex = 0;
                HasNextDialog = true;
            } else
                CanStop = true;
        }
    }
}
