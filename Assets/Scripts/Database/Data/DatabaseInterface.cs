using System;
using System.Data.SqlClient;
using UnityEngine;

public class DatabaseInterface : MonoBehaviour
{
	[SerializeField] private DatabaseConnectionParams _connectionParams;
	[SerializeField] private string _tablePlaythroughs;
	[SerializeField] private string _tableDay1;
	[SerializeField] private string _tableDay15;

	private DatabaseData _dbData;
	private SqlConnection _connection;
	private string[] _questTimesDay1;
	private string[] _questTimesDay15;

	public static DatabaseInterface Instance { get; private set; }

	private void Awake()
	{
		if (Instance != null)
		{
			Destroy(gameObject);
			return;
		}
		else Instance = this;
		DontDestroyOnLoad(gameObject);

		_dbData = GetComponent<DatabaseData>();
	}

	private void Start()
	{
		InitialiseDatabase();
	}

	private void InitialiseDatabase()
	{
		if (!Connect()) return;

		/* 
		 * FEITO APENAS UM VEZ PARA A CRIA��O DAS TABELAS
		 * ClearStructure apenas para apagar as tabelas para testar
		 */
		 //ClearStructure();
		 //if (!CreateStructure()) return;
	}

	private void ClearStructure()
	{
		RunQuery(string.Format("DROP TABLE {0}", _tablePlaythroughs));
		RunQuery(string.Format("DROP TABLE {0}", _tableDay1));
		RunQuery(string.Format("DROP TABLE {0}", _tableDay15));
	}

	private bool CreateStructure()
	{
		if (!CreateDay1Structure()) return false;
		if (!CreateDay15Structure()) return false;
		if (!CreatePlaythroughsStructure()) return false;

		return true;
	}

	private bool CreateDay1Structure()
	{
		// Create planet structure
		string createDay1Query = "";
		createDay1Query += $"CREATE TABLE {_tableDay1}(";
		createDay1Query += "  ID INT NOT NULL PRIMARY KEY IDENTITY(1,1),";
		createDay1Query += "  TimeQuest1 VARCHAR(16),";
		createDay1Query += "  TimeQuest2 VARCHAR(16),";
		createDay1Query += "  TimeQuest3 VARCHAR(16)";
		createDay1Query += ");";

		if (!RunQuery(createDay1Query)) return false;

		return true;
	}

	private bool CreateDay15Structure()
	{
		// Create planet structure
		string createDay15Query = "";
		createDay15Query += $"CREATE TABLE {_tableDay15}(";
		createDay15Query += "  ID INT NOT NULL PRIMARY KEY IDENTITY(1,1),";
		createDay15Query += "  TimeQuest1 VARCHAR(16),";
		createDay15Query += "  TimeQuest2 VARCHAR(16)";
		createDay15Query += ");";

		if (!RunQuery(createDay15Query)) return false;

		return true;
	}

	private bool CreatePlaythroughsStructure()
	{
		// Create planet structure
		string createPlaythroughsQuery = "";
		createPlaythroughsQuery += $"CREATE TABLE {_tablePlaythroughs}(";
		createPlaythroughsQuery += "  PlaythroughID INT NOT NULL PRIMARY KEY IDENTITY(1,1),";
		createPlaythroughsQuery += "  TotalTimePlayed VARCHAR(16),";
		createPlaythroughsQuery += "  CompletedGame INT NOT NULL DEFAULT 0,";
		createPlaythroughsQuery += "  TimesSpokenWithNPCs INT NOT NULL DEFAULT 0,";
		createPlaythroughsQuery += "  Day1ID INT REFERENCES Day1(ID),";
		createPlaythroughsQuery += "  Day15ID INT REFERENCES Day15(ID)";
		createPlaythroughsQuery += ");";

		if (!RunQuery(createPlaythroughsQuery)) return false;

		return true;
	}

	private bool InsertDay1QuestTimes()
	{
		string insertDay1QuestTimes = "";
		insertDay1QuestTimes += $"INSERT INTO {_tableDay1} VALUES (";
		insertDay1QuestTimes += $"  '{_questTimesDay1[1]}',";
		insertDay1QuestTimes += $"  '{_questTimesDay1[2]}',";
		insertDay1QuestTimes += $"  '{_questTimesDay1[3]}'";
		insertDay1QuestTimes += ");";

		if (!RunQuery(insertDay1QuestTimes)) return false;

		return true;
	}

	private bool InsertDay15QuestTimes()
	{
		string insertDay15QuestTimes = "";
		insertDay15QuestTimes += $"INSERT INTO {_tableDay15} VALUES (";
		insertDay15QuestTimes += $"  '{_questTimesDay15[1]}',";
		insertDay15QuestTimes += $"  '{_questTimesDay15[2]}'";
		insertDay15QuestTimes += ");";

		if (!RunQuery(insertDay15QuestTimes)) return false;

		return true;
	}

	private bool InsertPlaythroughData()
	{
		_dbData.FormatTotalTime();
		string insertPlaythroughData = "";
		insertPlaythroughData += $"INSERT INTO {_tablePlaythroughs} VALUES (";
		insertPlaythroughData += $"  '{_dbData.TotalTimePlayed}',";
		insertPlaythroughData += $"  {Convert.ToInt32(_dbData.CompletedGame)},";
		insertPlaythroughData += $"  {_dbData.TimesSpokenWithNPCs},";
		insertPlaythroughData += $"  IDENT_CURRENT('{_tableDay1}'),";
		insertPlaythroughData += $"  IDENT_CURRENT('{_tableDay15}')";
		insertPlaythroughData += ");";

		if (!RunQuery(insertPlaythroughData)) return false;

		return true;
	}

	public void UpdateDatabase(string scene)
	{
		if (scene == _tableDay1) _questTimesDay1 = _dbData.FormatQuestTimes();
		if (scene == _tableDay15)
		{
			_dbData.IsCountingTotalTime(false);
			_questTimesDay15 = _dbData.FormatQuestTimes();
			if (!InsertDay1QuestTimes()) return;
			if (!InsertDay15QuestTimes()) return;
			if (!InsertPlaythroughData()) return;
		}
	}

	private bool Connect()
	{
		if (_connection != null) return true;

		string connectionString = "";
		connectionString += $"Data Source={ _connectionParams.dataSource};";
		connectionString += $"Initial Catalog={ _connectionParams.databaseName};";
		connectionString += $"User ID={ _connectionParams.username};";
		connectionString += $"Password= {_connectionParams.password };";

		_connection = new SqlConnection(connectionString);
		try
		{
			Debug.Log("Connecting...");
			_connection.Open();
		}
		catch (Exception e)
		{
			Debug.LogError("Failed to connect: " + e.Message);
			return false;
		}

		Debug.Log("Connection established!");

		return true;
	}

	private bool RunQuery(string query)
	{
		SqlCommand sql = new SqlCommand(query, _connection);
		try
		{
			sql.ExecuteNonQuery();
		}
		catch (Exception e)
		{
			Debug.LogError("Failed to run query: " + query);
			Debug.LogError("Error: " + e.Message);
			return false;
		}

		Debug.Log(query);

		return true;
	}
}
