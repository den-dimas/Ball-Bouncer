using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementsOptionsUI : MonoBehaviour
{
	[Header("Saves")]

	public AchievementItemUI achievementItemTemplate;
	public Transform parent;
	public RectTransform content;

	private List<AchievementItemUI> achievementsUI = new();

	private SaveManager saveManager;

	private void Start()
	{
		saveManager = Manager.Instance.GetComponent<SaveManager>();

		Rect size = achievementItemTemplate.GetComponent<RectTransform>().rect;

		Vector2 initialPosition = new(size.width, -size.height);
		Vector2 margin = new(0, -(size.height + 12));
		int index = 0;

		content.sizeDelta = new(0, (size.height * 2) + (size.height * saveManager.CurrentUser.Achievements.Count));

		foreach (Achievement achievement in saveManager.CurrentUser.Achievements)
		{
			AchievementItemUI c = Instantiate(achievementItemTemplate);

			c.title.text = achievement.Title;
			c.description.text = achievement.Description;

			c.transform.SetParent(parent);
			c.transform.localScale = new(1, 1, 1);
			c.transform.localPosition = initialPosition + (margin * new Vector2(1, index));

			achievementsUI.Add(c);

			index++;
		}

		UpdateAchievementsUI();
	}

	public void UpdateAchievementsUI()
	{
		foreach (Achievement achievement in saveManager.CurrentUser.Achievements)
		{
			AchievementItemUI c = achievementsUI[achievement.Id];

			if (achievement.Unlocked)
			{
				c.GetComponentInChildren<Image>().color = new Color32(136, 168, 135, 255);
				c.title.color = new Color32(255, 255, 255, 255);
				c.description.color = new Color32(255, 255, 255, 255);
			}
			else
			{
				c.GetComponentInChildren<Image>().color = new Color32(229, 226, 237, 255);
				c.title.color = new Color32(15, 15, 16, 255);
				c.description.color = new Color32(15, 15, 16, 255);
			}
		}
	}
}
