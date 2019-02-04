using System;
using System.Data.SqlClient;
using UnityEngine;

public class DatabaseInterface : MonoBehaviour
{
	public DatabaseConnectionParams _connectionParams;
	public DatabaseData _dbData;

	SqlConnection _connection;

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
}
