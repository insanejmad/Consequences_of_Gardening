using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableObjectManager : MonoBehaviour
{
    public static ClickableObjectManager instance = null;
    [SerializeField] ChoicePanel choicePanel = null;
    [Tooltip("End point for take animation")] [SerializeField]
    private Transform itemTarget = null;
    private bool _isTargetOnUI = false;

    private void Awake()
    {
	instance = this;
	if (itemTarget == null)
	    Debug.LogWarning(name + " need an itemTarget", this);
	else
	    _isTargetOnUI = itemTarget.GetComponent<RectTransform>() != null;
	if (choicePanel == null)
	    choicePanel = GameObject.FindObjectOfType<ChoicePanel>();
	if (choicePanel == null)
	    Debug.LogError(name + " need a ChoicePanel instance", this);
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

    public void OpenPanel(ClickableObject target)
    {
	choicePanel.Setup(target);
    }
}
