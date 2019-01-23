using UnityEngine;

/// <summary>
/// Class to manage object that won't be destroyed.
/// </summary>
public class DontDestroy : MonoBehaviour
{
	/// <summary>
	/// Unity Awake Method.
	/// </summary>
	private void Awake()
	{
		// Make current gameobject is not destroyed.
		DontDestroyOnLoad(gameObject);
	}
}
