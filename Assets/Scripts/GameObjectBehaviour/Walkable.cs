using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lib.Struct;
using DialogSystem;

namespace GameObjectBehavior
{
    public class Walkable : MonoBehaviour
    {
        public float Speed = 1f;
        public List<SpeakablePosition> PointList = new List<SpeakablePosition>();

        int PointIndex = 0;
        float DeltaSpeed { get => Speed * Time.deltaTime; }
        bool WalkFinished = false;
        bool Walking = false;
        bool Finished { get => PointIndex == PointList.Count; }
        Animator Animator;
        Vector2 CurrentTarget { get => PointList[PointIndex].Point.position; }
        Dialog CurrentDialog { get => PointList[PointIndex].Dialog; }

        void Awake()
        {
            Animator = GetComponent<Animator>();
        }

        void Update() {

            if (Finished || UIDialogManager.Instance.InDialog)
                return;

            if (Vector2.Distance(transform.position, CurrentTarget) == 0f) {
                Walking = false;

                if (null != CurrentDialog)
                    UIDialogManager.Instance.Dialog = CurrentDialog;

                PointIndex++;
            } else
                GoToPosition(CurrentTarget);

            UpdateAnimator();
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