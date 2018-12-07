using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest : MonoBehaviour 
{
	[Header("Quest Dialogue")]
	[TextArea(3, 10)]
	public Dialogue[] questDialogue1;
	[TextArea(3, 10)]
	public Dialogue[] questDialogue2;
	[TextArea(3, 10)]
	public Dialogue[] questDialogue3;
	[TextArea(3, 10)]
	public Dialogue[] questDialogue4;

	[Header("Quest Consequences")]
	public GameObject[] destroyAfterQuest;
	public GameObject[] enableAfterQuest;

	public QuestState questState { get; set; }
}
