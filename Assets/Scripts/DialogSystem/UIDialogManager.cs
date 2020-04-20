using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;
using TMPro;
using Lib.Struct;
using AmbianceSystem;

namespace DialogSystem
{
    [System.Serializable] public class DialogEvent : UnityEvent {}
    public enum PositionType { LEFT, RIGHT };

    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(AudioSource))]
    public class UIDialogManager : MonoBehaviour
    {
        public static UIDialogManager Instance;

        public float DialogOpenDelay = .65f;
        public float DialogOpenSpeed = .3f;
        public TMP_Animated Text;
        public Image LeftAvatar;
        public Image RightAvatar;
        public Image Cursor;
        public Dialog Dialog;
        public AudioSource VoiceAudio;
        public AudioClip OpenClip;
        public AudioClip CloseClip;
        public DialogEvent OnDialogFinished;
        public bool InDialog { get; private set; } = false;
        public bool HasNextDialog { get; private set; } = false;
        public bool HasNextSentence { get; private set; } = false;
        public bool TextWasRead { get; private set; } = false;
        public bool CanStop { get; private set; } = false;

        int DialogIndex = 0;
        int SentenceIndex = 0;
        string CurrentSentence { get => CurrentDialog.Sentences[SentenceIndex]; }
        bool AmbianceOverrided = false;
        Image Background;
        Image CurrentAvatar { get => PositionToDisplay == PositionType.LEFT ? LeftAvatar : RightAvatar; }
        CharacterDialog CurrentDialog { get => Dialog.List[DialogIndex]; }
        CanvasGroup CanvasGroup;
        AudioSource Audio;
        Character Character;
        PositionType PositionToDisplay = PositionType.LEFT;

        void OnEnable()
        {
            Text.OnSentenceFinished.AddListener(OnFinishDialog);
            Text.OnCharacterRevealed.AddListener(ReproduceSound);
        }

        void OnDisable()
        {
            Text.OnSentenceFinished.RemoveListener(OnFinishDialog);
            Text.OnCharacterRevealed.RemoveListener(ReproduceSound);
        }

        void Awake() {
            Instance = this;
            Background = GetComponent<Image>();
            CanvasGroup = GetComponent<CanvasGroup>();
            Audio = GetComponent<AudioSource>();

            DisplayDialogUI(false, 0, 0);
        }

        void Update() {
            if (InDialog && Input.GetMouseButtonDown(0))
                HandlePlayerAction();

            if (!InDialog && null != Dialog)
                StartDialog();

            Cursor.gameObject.SetActive(TextWasRead);
        }

        void UpdateUI()
        {
            Text.Clear();

            LeftAvatar.gameObject.SetActive(PositionToDisplay == PositionType.LEFT);
            RightAvatar.gameObject.SetActive(PositionToDisplay == PositionType.RIGHT);

            if (null != Character.Avatar)
                CurrentAvatar.sprite = Character.Avatar;

            if (null != Character.DialogMeta.Background)
                Background.sprite = Character.DialogMeta.Background;

            if (null != Character.DialogMeta.FontColor)
            {
                Text.color = Character.DialogMeta.FontColor;
                Cursor.color = Character.DialogMeta.FontColor;
            }
        }

        void UpdateAudio()
        {
            if (null == CurrentDialog.Clip) return;

            if (CurrentDialog.ClipOverrideAmbiance)
            {
                AmbianceOverrided = true;

                if (null != AmbianceManager.Switch)
                    AmbianceManager.Switch(CurrentDialog.Clip);

                return;
            }

            Audio.clip = CurrentDialog.Clip;
            Audio.Play();
        }

        void ReproduceSound()
        {
            if (null == VoiceAudio || VoiceAudio.isPlaying || Character.VoiceClipList.Length == 0)
                return;

            VoiceAudio.clip = Character.VoiceClipList[Random.Range(0, Character.VoiceClipList.Length)];
            VoiceAudio.Play();
        }

        void ResetAudio()
        {
            Audio.Stop();
            Audio.clip = null;
        }

        void HandlePlayerAction()
        {
            if (!TextWasRead) {
                Text.AvoidAnimation();
                return;
            }

            if (CanStop)
                StopDialog();
            else if (HasNextSentence)
                Text.Read(CurrentSentence);
            else if (HasNextDialog)
                SwitchCharacter();

            TextWasRead = false;
        }

        void StartDialog()
        {
            InDialog = true;
            Character = CurrentDialog.Character;
            UpdateUI();
            DisplayDialogUI(true, DialogOpenSpeed, DialogOpenDelay);
        }

        void SwitchCharacter()
        {
            Character NextCharacter = CurrentDialog.Character;

            if (null == NextCharacter || NextCharacter.Name == Character.Name) { // If it's the same character, just continue reading as usual
                Text.Read(CurrentSentence);
                return;
            }

            Character = NextCharacter;
            PositionToDisplay = PositionToDisplay == PositionType.LEFT ? PositionType.RIGHT : PositionType.LEFT;

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
            {
                Audio.clip = OpenClip;
                Audio.PlayDelayed(delay / 2);

                s.AppendCallback(() => {
                    Text.Read(CurrentSentence);
                    UpdateAudio();
                });
            } else if (time != 0) {
                Audio.clip = CloseClip;
                Audio.Play();
            }


            return s;
        }

        public Sequence StopDialog()
        {
            if (!TextWasRead)
                Text.AvoidAnimation();

            if (AmbianceOverrided && null != AmbianceManager.Reset)
                AmbianceManager.Reset();
            else
                ResetAudio();

            return DisplayDialogUI(false, DialogOpenSpeed, 0).AppendCallback(() => {
                InDialog = false;
                Dialog = null;
                Character = null;
                DialogIndex = 0;
                SentenceIndex = 0;
                HasNextDialog = false;
                HasNextSentence = false;
                CanStop = false;
                PositionToDisplay = PositionType.LEFT;

                OnDialogFinished.Invoke();
            });
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
