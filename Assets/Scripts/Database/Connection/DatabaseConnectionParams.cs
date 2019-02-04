using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "Database Params")]
public class DatabaseConnectionParams : ScriptableObject
{
    public string dataSource;
    public string databaseName;
    public string username;
    public string password;
}


#if UNITY_EDITOR
[CustomEditor(typeof(DatabaseConnectionParams))]
class DatabaseInterfaceEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DatabaseConnectionParams connectionParams = (DatabaseConnectionParams)target;

        EditorGUI.BeginChangeCheck();
        connectionParams.dataSource = EditorGUILayout.TextField("Data Source", connectionParams.dataSource);
        connectionParams.databaseName = EditorGUILayout.TextField("Database", connectionParams.databaseName);
        connectionParams.username = EditorGUILayout.TextField("Username", connectionParams.username);
        connectionParams.password = EditorGUILayout.PasswordField("Password", connectionParams.password);
        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(connectionParams);
        }
    }
}
#endif

