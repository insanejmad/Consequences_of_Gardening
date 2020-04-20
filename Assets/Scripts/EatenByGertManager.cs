using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogSystem;

[RequireComponent(typeof(Animator))]
public class EatenByGertManager : MonoBehaviour
{
    public Dialog Dialog;

    Animator Animator;

    void OnEnable()
    {
        PNJ.OnDied += OnPNJDied;
    }

    void OnDisable()
    {
        PNJ.OnDied -= OnPNJDied;
    }

    void Awake()
    {
        Animator = GetComponent<Animator>();
        Animator.enabled = false;
    }

    void OnPNJDied(PNJ pnj)
    {
        Debug.Log("PNJ DIED!!!");
        Animator.enabled = true;
    }

    void OnAnimationFinished()
    {
        Animator.enabled = false;
        if (null != Dialog)
            UIDialogManager.Instance.Dialog = Dialog;
    }
}
