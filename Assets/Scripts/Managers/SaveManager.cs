using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using MessagePack;
using UnityEngine;

public class SaveManager : Singleton<SaveManager>
{
	public string SAVE_PATH;
	public string BACKUP_PATH;
	public static readonly string REGEX_PATHNAME = @"[<>:""/\\|?*\s]+";

	/* ------------------------------------------------------------------------- */

	public Stat Stats;

	public int LastUserId;
	public int LastSaveId;

	public List<User> Users = new();
	public User CurrentUser;

	private void Awake()
	{
		SAVE_PATH = Path.Combine(Application.persistentDataPath, "saves/saved.map");
		BACKUP_PATH = Path.Combine(Application.persistentDataPath, "saves/backup-saved.map");
	}

	override protected void Start()
	{
		Initialize();
	}

	/* ------------------------------------------------------------------------- */

	public void Save(out SaveStatus status)
	{
		using BinaryWriter writer = new(File.Open(SAVE_PATH, FileMode.Create));

		var b = MessagePackSerializer.Serialize(new Saved(LastUserId, Users));

		if (new DriveInfo(SAVE_PATH).AvailableFreeSpace > b.LongLength)
		{
			writer.Write(b);
			status = SaveStatus.SUCCESS;
			return;
		}

		status = SaveStatus.FAILED;
		return;
	}

	public void Load(out SaveStatus status)
	{
		try
		{
			using BinaryReader reader = new(File.Open(SAVE_PATH, FileMode.Open));

			Saved save = MessagePackSerializer.Deserialize<Saved>(reader.BaseStream);

			if (save != null)
			{
				Users = save.Users;
				LastUserId = save.LastUserId;

				CurrentUser = save.Users.Find((User u) => u.Id == LastUserId);
				LastSaveId = CurrentUser.LastSaveId;
				Stats = CurrentUser.SaveSlots[LastSaveId].PlayerStats;

				status = SaveStatus.SUCCESS;
				return;
			}
		}
		catch (Exception err)
		{
			if (err is FileNotFoundException)
			{
				status = SaveStatus.FILE_ERROR;
				return;
			}
		}

		status = SaveStatus.FAILED;
		return;
	}

	public SaveStatus SaveAndLoad()
	{
		Save(out SaveStatus save);

		if (save == SaveStatus.SUCCESS)
		{
			Load(out SaveStatus load);

			return load;
		}

		return save;
	}

	/* ------------------------------------------------------------------------- */

	public SaveStatus CreateUser(string userName)
	{
		int index = Users != null ? Users.Count : 0;
		string name = Regex.Replace(userName, REGEX_PATHNAME, "");

		User found = Users.Find((User u) => u.Id == index);
		if (found != null)
		{
			index = Users[^1].Id + 1;
		}

		SaveFile newSaveSlot = new(
			0,
			DateTime.Now,
			new(0, 0)
		);
		newSaveSlot.Empty = false;
		User newUser = new(index, name, new());

		newUser.SaveSlots[0] = newSaveSlot;

		Users.Add(newUser);

		return SaveAndLoad();
	}

	public SaveStatus LoadUser(int userId)
	{
		if (userId == CurrentUser.Id) return SaveStatus.SUCCESS;

		User found = Users.Find((User u) => u.Id == userId);

		if (found != null)
		{
			LastUserId = found.Id;

			SaveStatus save = SaveAndLoad();

			EventBus.Publish<ChangeUserEvent>(new(userId, found.LastSaveId));

			return save;
		}

		return SaveStatus.FAILED;
	}

	public SaveStatus DeleteUser(int userId)
	{
		if (userId == 0) return SaveStatus.TRY_DELETE_DEFAULT_USER;

		User found = Users.Find((User u) => u.Id == userId);

		if (found == null) return SaveStatus.FAILED;

		Users.Remove(found);

		return LoadUser(0);
	}

	/* ------------------------------------------------------------------------- */

	public SaveStatus SaveFileSlot(int index)
	{
		CurrentUser.SaveSlots[index] = new(
			index,
			DateTime.Now,
			CurrentUser.SaveSlots[LastSaveId].PlayerStats
		);
		CurrentUser.SaveSlots[index].Empty = false;

		Save(out SaveStatus status);

		return status;
	}

	public SaveStatus LoadFileSlot(int index)
	{
		CurrentUser.LastSaveId = index;
		LastSaveId = CurrentUser.LastSaveId;

		return SaveAndLoad();
	}

	/* ------------------------------------------------------------------------- */

	private void OnEnable()
	{
		EventBus.Subscribe<PlayerBounceEvent>(OnPlayerBounce);
		EventBus.Subscribe<EnemyKilledEvent>(OnEnemyKilled);
		EventBus.Subscribe<SceneEvent>(OnSceneChanged);
	}

	private void OnDisable()
	{
		EventBus.Unsubscribe<PlayerBounceEvent>(OnPlayerBounce);
		EventBus.Unsubscribe<EnemyKilledEvent>(OnEnemyKilled);
		EventBus.Unsubscribe<SceneEvent>(OnSceneChanged);
	}

	private void OnPlayerBounce(PlayerBounceEvent _)
	{
		Stats.TotalBounces++;
		CurrentUser.SaveSlots[LastSaveId].PlayerStats = Stats;

		EventBus.Publish<StatEvent>(new(Stats.TotalEnemiesKilled, Stats.TotalBounces));
	}

	private void OnEnemyKilled(EnemyKilledEvent _)
	{
		Stats.TotalEnemiesKilled++;
		CurrentUser.SaveSlots[LastSaveId].PlayerStats = Stats;

		EventBus.Publish<StatEvent>(new(Stats.TotalEnemiesKilled, Stats.TotalBounces));
	}

	private void OnSceneChanged(SceneEvent sceneEvent)
	{
		if (sceneEvent.sceneName == "MainMenu")
		{
			SaveAndLoad();
		}
	}

}