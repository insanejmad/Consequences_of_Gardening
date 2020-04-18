using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

namespace DialogSystem
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UIDialogManager : MonoBehaviour
    {
        public static UIDialogManager instance;

        public TMP_Animated text;

        CanvasGroup canvasGroup;

        void Awake() {
            canvasGroup = GetComponent<CanvasGroup>();
            FadeUI(false, 0, 0);
        }

        void Update() {
            if (Input.GetMouseButtonDown(0)) {
                FadeUI(true, .2f, .65f);
            }
        }

        public void FadeUI(bool show, float time, float delay)
        {
            Sequence s = DOTween.Sequence();
            s.AppendInterval(delay);
            s.Append(canvasGroup.DOFade(show ? 1 : 0, time));
            if (show)
            {
                text.Clear();
                s.Join(canvasGroup.transform.DOScale(0, time * 2).From().SetEase(Ease.OutBack));
                s.AppendCallback(() => text.Read("Ceci est un text à lire, avec des pauses... C'est un peu compliqué !"));
            }
        }

    }
}
