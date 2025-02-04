using System;
using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : Singleton<AchievementManager>
{
	public List<Achievement> Achievements = new();

	public void InitializeAchievements()
	{
		Achievements.Clear();

		/* Enemy-Based Achievements */
		AddAchievement(0, "The Brick Slayer", "Kill 1 brick.", (object o) => SaveManager.Instance.Stats.TotalEnemiesKilled == 1);
		AddAchievement(1, "The Legendary Brick Slayer", "Kill 3 brick.", (object o) => SaveManager.Instance.Stats.TotalEnemiesKilled == 3);
		AddAchievement(2, "The Myth", "Kill 5 brick.", (object o) => SaveManager.Instance.Stats.TotalEnemiesKilled == 5);
		AddAchievement(3, "Enough is not enough!", "Kill 7 brick.", (object o) => SaveManager.Instance.Stats.TotalEnemiesKilled == 7);
		AddAchievement(4, "Father of Bricks.", "Kill 10 brick.", (object o) => SaveManager.Instance.Stats.TotalEnemiesKilled == 10);

		/* Bounce-Based Achievements */
		AddAchievement(5, "The Bouncer", "Bounce 3 times.", (object o) => SaveManager.Instance.Stats.TotalBounces == 3);
		AddAchievement(6, "World Record Holder", "Bounce 8 times.", (object o) => SaveManager.Instance.Stats.TotalBounces == 8);
		AddAchievement(7, "Tung Tang Tung", "Bounce 15 times.", (object o) => SaveManager.Instance.Stats.TotalBounces == 15);

		foreach (Achievement a in SaveManager.Instance.CurrentUser.Achievements)
		{
			Achievements[a.Id].Unlocked = a.Unlocked;
		}
	}

	public void AddAchievement(int id, string title, string description, Predicate<object> predicate)
	{
		Achievements.Add(new(id, title, description, predicate));

		Achievement user = SaveManager.Instance.CurrentUser.Achievements.Find((Achievement a) => a.Id == id);

		if (user == null)
		{
			SaveManager.Instance.CurrentUser.Achievements.Add(new(id, title, description, predicate));
		}
	}

	public void UpdateAchievement()
	{
		foreach (Achievement achievement in Achievements)
		{
			achievement.Unlock();

			SaveManager.Instance.CurrentUser.Achievements[achievement.Id].Unlocked = achievement.Unlocked;
		}
	}

	/* ---------------------------------------------------------------------------------------------- */

	private void OnUpdateStatsEvent(StatEvent statEvent)
	{
		UpdateAchievement();
	}

	private void OnChangeUser(ChangeUserEvent user)
	{
		InitializeAchievements();
	}

	private void OnEnable()
	{
		EventBus.Subscribe<ChangeUserEvent>(OnChangeUser);
		EventBus.Subscribe<StatEvent>(OnUpdateStatsEvent);
	}

	private void OnDisable()
	{
		EventBus.Unsubscribe<ChangeUserEvent>(OnChangeUser);
		EventBus.Unsubscribe<StatEvent>(OnUpdateStatsEvent);
	}
}
