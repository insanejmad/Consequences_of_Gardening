using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lib.Struct;
using DialogSystem;
using DG.Tweening;

namespace GameObjectBehavior
{
    public class Walkable : MonoBehaviour
    {
        public float Speed = 1f;
        public List<SpeakablePosition> PointList = new List<SpeakablePosition>();
        public bool IsFinished { get; private set; } = false;

        int PointIndex = 0;
        float DeltaSpeed { get => Speed * Time.deltaTime; }
        bool Walking = false;
        bool IsTargetReached { get => Vector2.Distance(transform.position, CurrentTarget) == 0f; }
        Animator Animator;
        Vector2 CurrentTarget {
            get {
                Vector2 target = PointList[PointIndex].Point.position;
                target.y = transform.position.y;
                return target;
            }
        }
        Dialog CurrentDialog { get => PointList[PointIndex].Dialog; }

        void Awake()
        {
            Animator = GetComponent<Animator>();
        }

        void Update() {

            if (PointList.Count == 0 || IsFinished)
                return;

            if (IsTargetReached) {
                Walking = false;

                if (null != CurrentDialog)
                    SetDialog();

                if (PointIndex == PointList.Count - 1)
                {
                    if (null != CurrentDialog) {
                        UIDialogManager.Instance.OnDialogFinished.AddListener(Finish);
                    } else
                        Finish();
                } else
                    PointIndex++;
            } else
                GoToPosition(CurrentTarget);

            UpdateAnimator();
        }

        void SetDialog()
        {
            UIDialogManager.Instance.Dialog = CurrentDialog;
        }

        void Finish()
        {
            UIDialogManager.Instance.OnDialogFinished.RemoveListener(Finish);
            IsFinished = true;
        }

        void GoToPosition(Vector2 target) {
            Walking = true;
            transform.position = Vector2.MoveTowards(transform.position, target, DeltaSpeed);
        }

        void UpdateAnimator()
        {
            if (null == Animator)
                return;

            Animator.SetBool("isWalking", Walking);
        }
    }
}