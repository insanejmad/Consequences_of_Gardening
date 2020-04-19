using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogSystem;

public abstract class AClickableObject : MonoBehaviour
{
    protected bool _isHovered = false;

    public bool IsHovered
    {
        get => _isHovered;
    }

    protected virtual void Awake()
    {
        CheckCollider();
    }

    private void OnMouseOver()
    {
        if ((UIDialogManager.Instance && UIDialogManager.Instance.InDialog) ||
        (ClickableObjectManager.instance.CursorOnChoicePanel)) {
            _isHovered = false;
            HoverOff();
            return;
        }
        if (_isHovered)
            return;
        _isHovered = true;
        HoverOn();
    }

    private void OnMouseExit()
    {
        _isHovered = false;
        HoverOff();
    }

    private void OnMouseDown()
    {
        if (_isHovered)
            ClickOn();
    }

    protected bool CheckCollider()
    {
        if (!GetComponentInParent<Canvas>() && !GetComponent<Collider2D>()) {
            Debug.LogError("A clickable need a collider");
            return (false);
        }
        return (true);
    }

    protected abstract void HoverOn();
    protected abstract void HoverOff();
    protected abstract void ClickOn();
}
