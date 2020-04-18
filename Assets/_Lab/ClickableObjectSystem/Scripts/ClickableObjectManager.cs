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

    private void Awake()
    {
        instance = this;
        if (itemTarget == null)
            Debug.LogWarning(name + " need an itemTarget =^=", this);
        else
            _isTargetOnUI = itemTarget.GetComponent<RectTransform>() != null;
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
        choicePanel.Setup(target);
    }

    public bool IsChoicePanelOpen
    {
        get => choicePanel.gameObject.activeSelf;
    }

    public bool CursorOnChoicePanel
    {
        get => choicePanel.HaveCursorOn;
    }
    #endregion

    private void OnDestroy()
    {
        instance = null;
    }
}
