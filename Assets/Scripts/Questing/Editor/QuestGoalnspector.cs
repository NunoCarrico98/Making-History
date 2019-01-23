using UnityEngine;
using UnityEditor;

/// <summary>
/// Class that overrides the Unity Editor for the QuestGoal class.
/// </summary>
[CustomPropertyDrawer(typeof(QuestGoal))]
public class QuestGoalInspector : PropertyDrawer
{
	/// <summary>
	/// Check if the quest goal is open or closed in inspetor.
	/// </summary>
	[SerializeField] private bool _folded = false;

	/// <summary>
	/// Width of the box to write all the necessary text of the property on inspector.
	/// </summary>
	private const float _width = 320;
	/// <summary>
	/// Spacing between the lines of each property.
	/// </summary>
	private const float _spacing = 2;

	/// <summary>
	/// Override the OnGUI method from Unity.
	/// </summary>
	/// <param name="pos">Position of the property on inspector.</param>
	/// <param name="prop">Property to be written on inspector.</param>
	/// <param name="label">Lable of the property on inspector.</param>
	public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
	{
		// Serialize variables to be able to save them
		SerializedProperty itemID = prop.FindPropertyRelative("_itemID");
		SerializedProperty reqAmmount = prop.FindPropertyRelative("_requiredAmmount");
		SerializedProperty goalType = prop.FindPropertyRelative("_goalType");
		//SerializedProperty npcID = prop.FindPropertyRelative("_npcID");

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

        // Write on inspector the quest goal element
        _folded = EditorGUI.PropertyField(elementRect, prop);

		if (_folded)
		{
			// Change the indentation of the GUI content
			EditorGUI.indentLevel = 3;
			// Add a popup with the enum types
			goalType.intValue = EditorGUI.Popup(typeRect, "Goal Type", 
				goalType.intValue, QuestGoal.GetEnumValuesAsStrings());

			// Write on the collection goal on inspector
			ShowCollectOptions(itemIDRect, reqAmmountRect, goalType, itemID, reqAmmount);
		}

		// End Property
		EditorGUI.EndProperty();
	}

	/// <summary>
	/// Override the GetPropertyHeight method from Unity.
	/// </summary>
	/// <param name="prop">Analysed property.</param>
	/// <param name="label">Label of the property.</param>
	/// <returns></returns>
	public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
	{
		// Row number
		int rows = 3;
		// Returns the height based on the number of rows and depending if 
		// it's folded or not.
		return base.GetPropertyHeight(prop, label) * (prop.isExpanded ? rows * 1.25f : 1);
	}

	/// <summary>
	/// Method to write the Collection Goal properties on the inspector.
	/// </summary>
	/// <param name="itemIDRect">Rectangle for the itemID property.</param>
	/// <param name="reqAmmountRect">Rectangle for the reqAmmount property.</param>
	/// <param name="goalType">Goal type property. Decides which goal type.</param>
	/// <param name="itemID">Item ID property. Decides which itemID is necessary.</param>
	/// <param name="reqAmmount">Required Ammount property. Decides how many 
	/// of the item is necessary.</param>
	private void ShowCollectOptions(Rect itemIDRect, Rect reqAmmountRect, 
		SerializedProperty goalType, SerializedProperty itemID, 
		SerializedProperty reqAmmount)
	{
		// If chosen type is Collect
		if (goalType.intValue == (int)GoalType.Collect)
		{
			// Change the indentation of the GUI content
			EditorGUI.indentLevel = 4;
			// Add a field to input the desired item ID
			EditorGUI.PropertyField(itemIDRect, itemID, new GUIContent("Item ID"));

			// Change the indentation of the GUI content
			EditorGUI.indentLevel = -1;
			// Add a field to input the desired required ammount
			EditorGUI.PropertyField(reqAmmountRect, reqAmmount, new GUIContent("Required Ammount"));
		}
	}
}
