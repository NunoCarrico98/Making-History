using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(QuestGoal))]
public class QuestGoalInspector : PropertyDrawer
{
	[SerializeField] private bool _folded;
	[SerializeField] private int _selected;

	private const float _xOffset = 90;
	private const float _width = 293;
	private const float _spacing = 2;

	// Draw the property inside the given rect
	public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
	{
		SerializedProperty type = prop.FindPropertyRelative("_goalType");
		SerializedProperty itemID = prop.FindPropertyRelative("_itemID");
		SerializedProperty reqAmmount = prop.FindPropertyRelative("_requiredAmmount");

		pos.height = EditorGUIUtility.singleLineHeight;
		EditorGUI.BeginProperty(pos, label, prop);


		Rect elementRect = 
			new Rect(pos.x, pos.y, pos.width, pos.height);
		Rect typeRect = 
			new Rect(pos.x + _xOffset, pos.y + 16, _width, pos.height);
		Rect itemIDRect = 
			new Rect(pos.x + _xOffset, pos.y + 16 * 2, _width, pos.height - _spacing);
		Rect reqAmmountRect = 
			new Rect(pos.x + _xOffset + 50, pos.y + 16 * 3, _width, pos.height - _spacing);

		_folded = EditorGUI.PropertyField(elementRect, prop);

		if (_folded)
		{
			EditorGUI.LabelField(
				new Rect(pos.x + 20, typeRect.y, pos.width, pos.height), 
				"Goal Type");
			_selected = EditorGUI.Popup(typeRect, _selected, QuestGoal.GetEnumValuesAsStrings());

			if (_selected == (int)QuestGoal.GoalType.Collect)
			{
				EditorGUI.LabelField(
					new Rect(pos.x + 20, itemIDRect.y, pos.width, pos.height),
					"Item ID");
				EditorGUI.PropertyField(itemIDRect, itemID, GUIContent.none);

				EditorGUI.LabelField(
					new Rect(pos.x + 20, reqAmmountRect.y, pos.width, pos.height),
					"Required Ammount");
				EditorGUI.PropertyField(reqAmmountRect, reqAmmount, GUIContent.none);
			}
		}

		EditorGUI.EndProperty();
	}

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		int lineCount = 3;
		return EditorGUIUtility.singleLineHeight * lineCount +
			EditorGUIUtility.standardVerticalSpacing * (lineCount +2);
	}
}
