using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DatabaseData : MonoBehaviour
{
	[SerializeField] private QuestGiver[] _questGivers;

	private int _questIndex;
	private float _questTimer;
	private float _totalTimeTimer;
	private bool _countQuestTime;
	private bool _countTotalTime;
	private TimeSpan[] _questTimes;
	private TimeSpan _totalTimePlayed;

	public bool CompletedGame { get; private set; }
	public int TimesSpokenWithNPCs { get; private set; }
	public string TotalTimePlayed { get; private set; }
	public string[] QuestTimes { get; private set; }

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
		_questTimes = new TimeSpan[_questGivers.Length];
		QuestTimes = new string[_questGivers.Length];
		_questIndex = 1;
	}

	// Update is called once per frame
	void Update()
	{
		if (_questGivers.Length != 0) CountQuestTime();
		if (SceneManager.GetActiveScene().name == "Day15") IsGameCompleted();
		CountTotalTime();
	}

	public void IsCountingTotalTime(bool counting)
	{
		if (!counting)
		{
			_countTotalTime = false;
			_totalTimeTimer = 0;
			return;
		}
		else
			_countTotalTime = true;
	}

	private void CountTotalTime()
	{
		if (_countTotalTime)
		{
			_totalTimeTimer += Time.deltaTime;
			_totalTimePlayed = TimeSpan.FromSeconds(_totalTimeTimer);
		}
	}

	private void CountQuestTime()
	{
		if (IsCountingQuestTime(_questGivers[_questIndex]))
		{
			_questTimer += Time.deltaTime;
			_questTimes[_questIndex] = TimeSpan.FromSeconds(_questTimer);
		}
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
		_countQuestTime = false;
		_questTimer = 0;
		if (_questIndex < _questGivers.Length - 1) _questIndex++;
	}

	private void IsGameCompleted()
	{
		if (_questGivers[0].CompletedQuest)
			CompletedGame = true;
	}

	public void IncrementToTimesSpokenWithNPCs() => TimesSpokenWithNPCs++;

	public string[] FormatQuestTimes()
	{
		for (int i = 0; i < _questTimes.Length; i++)
			QuestTimes[i] = _questTimes[i].ToString(@"hh\:mm\:ss");

		return QuestTimes;
	}

	public void FormatTotalTime() =>
		TotalTimePlayed = _totalTimePlayed.ToString(@"hh\:mm\:ss");
}
