using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableObjectManager : MonoBehaviour
{
    public static ClickableObjectManager instance = null;
    [SerializeField] RectTransform ChoicePanel = null;

    [Tooltip("End point for take animation")] [SerializeField]
    private Transform itemTarget = null;

    private bool _isTargetOnUI = false;
    private ClickableObject _target;

    private void Awake()
    {
	instance = this;
	if (itemTarget == null)
	    Debug.LogWarning(name + " need an itemTarget");
	else
	    _isTargetOnUI = itemTarget.GetComponent<RectTransform>() != null;
    }

    private void Start()
    {
	ChoicePanel.gameObject.SetActive(false);
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
	ChoicePanel.position = Input.mousePosition;
	ChoicePanel.gameObject.SetActive(true);
	_target = target;
    }

    public void Take()
    {
	ChoicePanel.gameObject.SetActive(false);
	_target.Take();
	_target = null;
    }

    public void Inspect()
    {
	ChoicePanel.gameObject.SetActive(false);
	_target.Inspect();
	_target = null;
    }
}
