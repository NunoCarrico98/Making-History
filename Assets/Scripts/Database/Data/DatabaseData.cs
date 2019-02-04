using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DatabaseData : MonoBehaviour
{
	[SerializeField] private QuestGiver _lastNPC;
	[SerializeField] private QuestGiver[] _questGivers;

	private int _questIndex;
	private bool _countQuestTime;

	public bool CompletedGame { get; private set; }
	public int TimesSpokenWithNPCs { get; private set; }
	public TimeSpan TotalTimePlayed { get; private set; }
	public TimeSpan[] QuestTimes { get; private set; }

	// Use this for initialization
	private void Start()
	{
		_questIndex = 1;
		_countQuestTime = false;
	}

	private void OnEnable()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	private void OnDisable()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		_questGivers = FindObjectsOfType<QuestGiver>();
		Array.Sort(_questGivers, (npc1, npc2) => npc1.ID.CompareTo(npc2.ID));
		QuestTimes = new TimeSpan[_questGivers.Length];
		_questIndex = 1;
	}

	// Update is called once per frame
	void Update()
	{
		if (_questGivers.Length != 0) CountQuestTime();
		if (SceneManager.GetActiveScene().name == "Day15") IsGameCompleted();
	}

	public void CountTotalTimePLayed(bool counting)
	{
		if (!counting)
		{
			Debug.Log("Total time:" + TotalTimePlayed);
			return;
		}

		if (counting)
			TotalTimePlayed = TimeSpan.FromSeconds(Time.timeSinceLevelLoad);
	}

	private void OnApplicationQuit()
	{
		CountTotalTimePLayed(false);
	}

	private void CountQuestTime()
	{
		if (IsCountingQuestTime(_questGivers[_questIndex]))
			QuestTimes[_questIndex] = TimeSpan.FromSeconds(Time.timeSinceLevelLoad);
	}

	private void IsGameCompleted()
	{
		if (_questGivers[0].CompletedQuest)
			CompletedGame = true;
	}

	private bool IsCountingQuestTime(QuestGiver npc)
	{
		if (npc.IsQuestAssigned && !npc.CompletedQuest)
			_countQuestTime = true;

		if (npc.CompletedQuest)
			AdvanceToNextQuestData();
		else if (npc.NPCQuest.Completed && !npc.NPCQuest.NeedsNPCToComplete)
			AdvanceToNextQuestData();

		return _countQuestTime;
	}

	private void AdvanceToNextQuestData()
	{
		Debug.Log(QuestTimes[_questIndex]);
		_countQuestTime = false;
		if (_questIndex < _questGivers.Length - 1) _questIndex++;
	}

	public void IncrementToTimesSpokenWithNPCs() => TimesSpokenWithNPCs++;
}
