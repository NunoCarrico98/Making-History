using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(QuestGoal))]
public class QuestGoalInspector : PropertyDrawer
{
	[SerializeField] private bool _folded = false;

	private const float _xOffset = 90;
	private const float _width = 320;
	private const float _spacing = 2;

	// Draw the property inside the given rect
	public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
	{
		// Serialize variables to be able to save them
		SerializedProperty itemID = prop.FindPropertyRelative("_itemID");
		SerializedProperty reqAmmount = prop.FindPropertyRelative("_requiredAmmount");
		SerializedProperty goalType = prop.FindPropertyRelative("_goalType");
		//SerializedProperty completed = prop.FindPropertyRelative("completed");

		// Height of rectangles will be 1 line
		pos.height = EditorGUIUtility.singleLineHeight;

		// Begin Property
		EditorGUI.BeginProperty(pos, label, prop);

		// Set rectangles and positions for all properties
		Rect elementRect = 
			new Rect(pos.x, pos.y, pos.width, pos.height);
		Rect typeRect = 
			new Rect(pos.x + 15, pos.y + 16, _width, pos.height);
		Rect itemIDRect = 
			new Rect(pos.x, pos.y + 16 * 2, _width, pos.height - _spacing);
		Rect reqAmmountRect = 
			new Rect(pos.x + 75, pos.y + 16 * 3, _width, pos.height);
        //Rect completedRect =
            //new Rect(pos.x + _xOffset + 50, pos.y + 16 * 4, _width, pos.height - _spacing);

        // Write on inspector the quest goal element
        _folded = EditorGUI.PropertyField(elementRect, prop);

		if (_folded)
		{
			// Change the indentation of the GUI content
			EditorGUI.indentLevel = 3;
			// Add a popup with the enum types
			goalType.intValue = EditorGUI.Popup(typeRect, "Goal Type", 
				goalType.intValue, QuestGoal.GetEnumValuesAsStrings());

			// If chosen type is Collect
			if (goalType.intValue == (int)QuestGoal.GoalType.Collect 
				|| goalType.intValue == (int)QuestGoal.GoalType.Use)
			{
				// Change the indentation of the GUI content
				EditorGUI.indentLevel = 4;
				// Add a field to input the desired item ID
				EditorGUI.PropertyField(itemIDRect, itemID, new GUIContent("Item ID"));

				// Change the indentation of the GUI content
				EditorGUI.indentLevel = -1;
				// Add a field to input the desired required ammount
				EditorGUI.PropertyField(reqAmmountRect, reqAmmount, new GUIContent("Required Ammount"));

				// Write on inspector the Required Ammount Text
				//EditorGUI.LabelField(
				//new Rect(pos.x + 20, completedRect.y, pos.width, pos.height),
				//"Completed");
				// Add a field to input the desired required ammount
				//EditorGUI.PropertyField(completedRect, completed, GUIContent.none);
			}
		}

		// End Property
		EditorGUI.EndProperty();
	}

	public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
	{
		int rows = 3;
		return base.GetPropertyHeight(prop, label) * (prop.isExpanded ? rows * 1.25f : 1);
	}
}
