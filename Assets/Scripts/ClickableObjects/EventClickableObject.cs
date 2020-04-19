using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventClickableObject : AClickableObject
{
    public UnityEvent onClickOn = null;
    public UnityEvent onHoverOn = null;
    public UnityEvent onHoverOff = null;

    protected override void ClickOn()
    {
        if (onClickOn != null)
            onClickOn.Invoke();
    }

    protected override void HoverOn()
    {
        if (onHoverOn != null)
            onHoverOn.Invoke();
    }

    protected override void HoverOff()
    {
        if (onHoverOff != null)
            onHoverOff.Invoke();
    }
}