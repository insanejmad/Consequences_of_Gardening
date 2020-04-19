using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableObjectManager : MonoBehaviour
{
    public static ClickableObjectManager instance = null;
    [SerializeField] ChoicePanel choicePanel = null;
    [Tooltip("End point for take animation")]
    [SerializeField]
    private Transform itemTarget = null;
    private bool _isTargetOnUI = false;

    private ClickableObject[] _clickableObjectsList = null;

    public static bool HaveInstance
    {
        get => (instance != null);
    }

    private void Awake()
    {
        instance = this;
        if (itemTarget == null)
            Debug.LogWarning(name + " need an itemTarget =^=", this);
        else
            _isTargetOnUI = itemTarget.GetComponentInParent<Canvas>() != null;
        if (choicePanel == null)
            choicePanel = GameObject.FindObjectOfType<ChoicePanel>();
        if (choicePanel == null)
            Debug.LogError(name + " need a ChoicePanel instance ;-;", this);
        _clickableObjectsList = GameObject.FindObjectsOfType<ClickableObject>();
    }

    public Vector2 GetItemTargetPos
    {
        get
        {
            if (itemTarget == null)
                return Vector2.zero;
            if (_isTargetOnUI)
                return Camera.main.ScreenToWorldPoint(itemTarget.position);
            return itemTarget.position;
        }
    }

    #region CHOICE_PANEL
    public void OpenChoicePanel(ClickableObject target)
    {
        if (choicePanel != null)
            choicePanel.Setup(target);
        else
            Debug.Log("Choice Panel not found :/", this);
    }

    public bool IsChoicePanelOpen
    {
        get {
            if (!choicePanel)
                return false;
            return (choicePanel.gameObject.activeSelf);
        }
    }

    public bool CursorOnChoicePanel
    {
        get {
            if (!choicePanel)
                return false;
            return choicePanel.HaveCursorOn;
        }
    }
    #endregion

    private void OnDestroy()
    {
        instance = null;
    }
}
