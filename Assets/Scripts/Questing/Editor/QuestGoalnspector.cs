using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(QuestGoal))]
public class QuestGoalInspector : PropertyDrawer
{
	[SerializeField] private bool _folded = false;
	[SerializeField] private int _selected;

	private const float _xOffset = 90;
	private const float _width = 293;
	private const float _spacing = 2;

	// Draw the property inside the given rect
	public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
	{
		// Serialize variables to be able to save them
		SerializedProperty itemID = prop.FindPropertyRelative("_itemID");
		SerializedProperty reqAmmount = prop.FindPropertyRelative("_requiredAmmount");
		SerializedProperty completed = prop.FindPropertyRelative("completed");

		// Height of rectangles will be 1 line
		pos.height = EditorGUIUtility.singleLineHeight;

		// Begin Property
		EditorGUI.BeginProperty(pos, label, prop);

		// Set rectangles and positions for all properties
		Rect elementRect = 
			new Rect(pos.x, pos.y, pos.width, pos.height);
		Rect typeRect = 
			new Rect(pos.x + _xOffset, pos.y + 16, _width, pos.height);
		Rect itemIDRect = 
			new Rect(pos.x + _xOffset, pos.y + 16 * 2, _width, pos.height - _spacing);
		Rect reqAmmountRect = 
			new Rect(pos.x + _xOffset + 50, pos.y + 16 * 3, _width, pos.height - _spacing);
        Rect completedRect =
            new Rect(pos.x + _xOffset + 50, pos.y + 16 * 4, _width, pos.height - _spacing);

        // Write on inspector the quest goal element
        _folded = EditorGUI.PropertyField(elementRect, prop);

		if (_folded)
		{
			// Write on inspector the Goal Type Text
			EditorGUI.LabelField(
				new Rect(pos.x + 20, typeRect.y, pos.width, pos.height), 
				"Goal Type");
			// Set a dropdownbox to appear and be able to choose type of objective
			_selected = EditorGUI.Popup(typeRect, _selected, QuestGoal.GetEnumValuesAsStrings());

			// If chosen type is Collect
			if (_selected == (int)QuestGoal.GoalType.Collect)
			{
				// Write on inspector the Item ID Text
				EditorGUI.LabelField(
					new Rect(pos.x + 20, itemIDRect.y, pos.width, pos.height),
					"Item ID");
				// Add a field to input the desired item ID
				EditorGUI.PropertyField(itemIDRect, itemID, GUIContent.none);

				// Write on inspector the Required Ammount Text
				EditorGUI.LabelField(
					new Rect(pos.x + 20, reqAmmountRect.y, pos.width, pos.height),
					"Required Ammount");
				// Add a field to input the desired required ammount
				EditorGUI.PropertyField(reqAmmountRect, reqAmmount, GUIContent.none);

                // Write on inspector the Required Ammount Text
                EditorGUI.LabelField(
                    new Rect(pos.x + 20, completedRect.y, pos.width, pos.height),
                    "Completed");
                // Add a field to input the desired required ammount
                EditorGUI.PropertyField(completedRect, completed, GUIContent.none);
            }
		}

		// End Property
		EditorGUI.EndProperty();
	}

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		if (_folded)
		{
			// Set Space available to write the property variables
			int lineCount = 4;
			return EditorGUIUtility.singleLineHeight * lineCount +
				EditorGUIUtility.standardVerticalSpacing * (lineCount + 2);
		}
		else return 5;
	}
}
