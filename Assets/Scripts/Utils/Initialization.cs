using System;
using System.IO;
using UnityEngine;

public class Initialization : MonoBehaviour
{
	public GameObject manager;

	private void Start()
	{
		manager = FindFirstObjectByType<Manager>().gameObject;

		SaveManager saveManager = manager.GetComponent<SaveManager>();
		LevelManager levelManager = manager.GetComponent<LevelManager>();

		// Try to load first
		saveManager.Load(out SaveStatus status);

		// If status is FILE_ERROR, that means saved file is not created.
		if (status == SaveStatus.FAILED || status == SaveStatus.FILE_ERROR)
		{
			// Make sure directory is available
			Directory.CreateDirectory(Path.GetDirectoryName(saveManager.SAVE_PATH));

			SaveStatus create = saveManager.CreateUser(Environment.UserName);

			if (create != SaveStatus.SUCCESS)
			{
				Debug.Log("Failed to create default user.");
			}

			// If create success
			if (create == SaveStatus.SUCCESS)
			{
				// Load newly created user
				saveManager.LoadUser(0);
			}

			// Load Main Menu
			levelManager.LoadScene("MainMenu");
		}

		// If there is saved file
		if (status == SaveStatus.SUCCESS)
		{
			// Load last save slot
			saveManager.LoadFileSlot(saveManager.CurrentUser.LastSaveId);
		}

		manager.GetComponent<AchievementManager>().InitializeAchievements();

		Debug.Log("Succeed");

		// Load Main Menu
		levelManager.LoadScene("MainMenu");
	}
}