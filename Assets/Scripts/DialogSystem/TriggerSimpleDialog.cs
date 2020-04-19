using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameObjectBehavior;

namespace DialogSystem
{
    [RequireComponent(typeof(Interactable))]
    public class TriggerSimpleDialog : MonoBehaviour
    {
        public Dialog Dialog;

        Interactable Interactable;

        void Awake()
        {
            Interactable = GetComponent<Interactable>();
        }

        void OnEnable()
        {
            Interactable.OnInteracted.AddListener(TriggerDialog);
        }

        void OnDisable()
        {
            Interactable.OnInteracted.RemoveListener(TriggerDialog);
        }

        void TriggerDialog()
        {
            if (UIDialogManager.Instance.InDialog)
                return;

            UIDialogManager.Instance.Dialog = Dialog;
        }
    }
}
