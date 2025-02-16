using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SavesOptionsUI : MonoBehaviour
{
	[Header("Saves")]

	public SaveItemUI saveSlotTemplate;
	public Transform saveParent;

	private List<SaveItemUI> saveItems = new();

	private SaveManager saveManager;

	private void Start()
	{
		saveManager = Manager.Instance.GetComponent<SaveManager>();

		Vector2 initialPosition = new(224 / 2 + 105, -105);
		Vector2 margin = new(224 + 12, -(96 + 12));

		int col = 0;
		int row = 0;

		foreach (SaveFile save in saveManager.CurrentUser.SaveSlots)
		{
			SaveItemUI c = Instantiate(saveSlotTemplate);

			c.transform.SetParent(saveParent);
			c.transform.localScale = new(1, 1, 1);
			c.transform.localPosition = initialPosition + (margin * new Vector2(row, col));

			row++;
			row %= 5;

			if (row == 0) col++;

			c.saveBtn.onClick.AddListener(() =>
			{
				saveManager.SaveFileSlot(save.Id);
				UpdateSaveMenu();
			});

			c.loadBtn.onClick.AddListener(() =>
			{
				saveManager.LoadFileSlot(save.Id);
				UpdateSaveMenu();
			});

			saveItems.Add(c);
		}

		UpdateSaveMenu();
	}

	public void UpdateSaveMenu()
	{
		foreach (SaveFile save in saveManager.CurrentUser.SaveSlots)
		{
			SaveItemUI c = saveItems[save.Id];

			if (!save.Empty)
			{
				c.GetComponentInChildren<TMP_Text>().text = save.SaveDate.ToString("yyyy-MM-dd HH:mm:ss");
				c.loadBtn.interactable = true;
			}
			else
			{
				c.GetComponentInChildren<TMP_Text>().text = "Empty Slot";
				c.loadBtn.interactable = false;
			}

			if (save.Id == saveManager.LastSaveId)
			{
				c.GetComponent<Image>().color = new Color32(102, 102, 108, 255);
				c.GetComponentInChildren<TMP_Text>().color = new Color32(255, 255, 255, 255);
			}
			else
			{
				c.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
				c.GetComponentInChildren<TMP_Text>().color = new Color32(10, 10, 8, 255);
			}
		}
	}
}
