using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
	private static T _instance;

	public static T Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = FindFirstObjectByType<T>();

				if (_instance == null)
				{
					GameObject obj = new()
					{
						name = typeof(T).Name
					};
					_instance = obj.AddComponent<T>();
				}
			}

			return _instance;
		}
	}

	protected virtual void Start() => Initialize();

	protected virtual void Initialize()
	{
		_instance = this as T;
	}
}