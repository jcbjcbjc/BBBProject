using System;
using System.Collections.Generic;
using UnityEngine;

public class BasePanel : MonoBehaviour 
{
	protected virtual void Start()
	{
	}

	protected virtual void Update()
	{
		if (_isShowing == true) {
			if (GetComponent<CanvasGroup>() != null) {
				var canvasGroup = GetComponent<CanvasGroup> ();
				canvasGroup.blocksRaycasts = true;
				canvasGroup.interactable = true;
				canvasGroup.ignoreParentGroups = true;
				canvasGroup.alpha = 1;
			}
		} 
		else {
			if (GetComponent<CanvasGroup>() != null) {
				var canvasGroup = GetComponent<CanvasGroup> ();
				canvasGroup.blocksRaycasts = false;
				canvasGroup.interactable = false;
				canvasGroup.ignoreParentGroups = false;
				canvasGroup.alpha = 0;
			}
		}
	}

	bool _isShowing = false;
	bool _isFreezing=false;
	public virtual void OnEnter()
	{
		_isShowing = true;
	}

	public virtual void OnExit()
	{
		_isShowing = false;
	}
}
