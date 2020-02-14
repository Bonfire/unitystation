﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIActionManager : MonoBehaviour
{
	public GameObject Panel;

	private static UIActionManager uIActionManager;
	public static UIActionManager Instance
	{
		get
		{
			if (!uIActionManager)
			{
				uIActionManager = FindObjectOfType<UIActionManager>();
			}

			return uIActionManager;
		}
	}


	public UIAction UIAction;
	public List<UIAction> PooledUIAction = new List<UIAction>();

	public Dictionary<IActionGUI, UIAction> DicIActionGUI = new Dictionary<IActionGUI, UIAction>();


	public static void Toggle(IActionGUI iActionGUI, bool Add, GameObject recipient = null)
	{

		if (Add)
		{
			if (Instance.DicIActionGUI.ContainsKey(iActionGUI))
			{
				Logger.Log("iActionGUI Already added", Category.UI);
				return;
			}
			Show(iActionGUI);
		}
		else {
			Hide(iActionGUI);
		}
	}


	public static void SetSprite(IActionGUI iActionGUI, Sprite sprite)
	{
		if (Instance.DicIActionGUI.ContainsKey(iActionGUI))
		{
			var _UIAction = Instance.DicIActionGUI[iActionGUI];
			_UIAction.IconFront.SetSprite(sprite);
		}
		else {
			Logger.Log("iActionGUI Not present", Category.UI);
		}
	}



	public static void SetSprite(IActionGUI iActionGUI, int Location)
	{
		if (Instance.DicIActionGUI.ContainsKey(iActionGUI))
		{
			var _UIAction = Instance.DicIActionGUI[iActionGUI];
			_UIAction.IconFront.ChangeSpriteVariant(Location);
		}
		else {

			Logger.Log("iActionGUI Not present", Category.UI);
		}
	}

	public static void SetBackground(IActionGUI iActionGUI, int Location)
	{
		if (Instance.DicIActionGUI.ContainsKey(iActionGUI))
		{
			var _UIAction = Instance.DicIActionGUI[iActionGUI];
			_UIAction.IconBackground.ChangeSpriteVariant(Location);
		}
		else {

			Logger.Log("iActionGUI Not present", Category.UI);
		}
	}


	public static void SetBackground(IActionGUI iActionGUI, Sprite sprite)
	{
		if (Instance.DicIActionGUI.ContainsKey(iActionGUI))
		{
			var _UIAction = Instance.DicIActionGUI[iActionGUI];
			_UIAction.IconBackground.SetSprite(sprite);
		}
		else {
			Logger.Log("iActionGUI Not present", Category.UI);
		}
	}

	public static void Show(IActionGUI iActionGUI)
	{
		UIAction _UIAction;
		if (Instance.PooledUIAction.Count > 0)
		{
			_UIAction = Instance.PooledUIAction[0];
			Instance.PooledUIAction.RemoveAt(0);
		}
		else {
			_UIAction = Instantiate(Instance.UIAction);
			_UIAction.transform.SetParent(Instance.Panel.transform, false);
		}
		Instance.DicIActionGUI[iActionGUI] = _UIAction;
		_UIAction.SetUp(iActionGUI);

	}

	public static void Hide(IActionGUI iActionGUI)
	{
		if (Instance.DicIActionGUI.ContainsKey(iActionGUI))
		{
			var _UIAction = Instance.DicIActionGUI[iActionGUI];
			_UIAction.Pool();
			Instance.PooledUIAction.Add(_UIAction);
			Instance.DicIActionGUI.Remove(iActionGUI);
		}
		else {
			Logger.Log("iActionGUI Not present", Category.UI);
		}
	}
 
	public void OnRoundEnd()
	{
		foreach (var _Action in DicIActionGUI) { 
			_Action.Value.Pool();
		}
		DicIActionGUI = new Dictionary<IActionGUI, UIAction>();
	}
}
