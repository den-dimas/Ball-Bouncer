using UnityEngine;

public class Manager : MonoBehaviour
{
	private static GameObject _instance;
	public static GameObject Instance
	{
		get
		{
			return _instance;
		}
	}

	private void Start()
	{
		_instance = GameObject.Find("ManagerSingleton");

		if (_instance != null && _instance != this)
		{
			Destroy(gameObject);
		}
		else
		{
			_instance = gameObject;
			_instance.name = "ManagerSingleton";

			DontDestroyOnLoad(this);
		}
	}
}
