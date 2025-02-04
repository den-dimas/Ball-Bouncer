using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserOptionsUI : MonoBehaviour
{
	[Header("Stats")]
	[SerializeField] private TMP_Text userName;
	[SerializeField] private TMP_Text totalKilled;
	[SerializeField] private TMP_Text totalBounces;
	[SerializeField] private TMP_Text createdAt;

	[Header("Management")]
	[SerializeField] private TMP_Dropdown accountDropdown;
	[SerializeField] private TMP_InputField createUserInput;
	[SerializeField] private TMP_Text inputErrorText;
	[SerializeField] private Button createUserBtn;

	private SaveManager saveManager;

	private void Start()
	{
		saveManager = Manager.Instance.GetComponent<SaveManager>();

		UpdateUserAccount();

		UpdateDropdownAccount();

		accountDropdown.value = saveManager.Users.FindIndex((User u) => u.Id == saveManager.CurrentUser.Id);
		accountDropdown.onValueChanged.AddListener(OnChangeUserAccount);

		createUserBtn.onClick.AddListener(CreateUser);
	}

	private void OnChangeUserAccount(int selected)
	{
		User selectedUser = saveManager.Users[selected];
		SaveStatus load = saveManager.LoadUser(selectedUser.Id);

		if (load == SaveStatus.SUCCESS)
		{
			StartCoroutine(ShowErrorMessage("User loaded."));

			UpdateUserAccount();
			return;
		}

		if (load == SaveStatus.FAILED)
		{
			StartCoroutine(ShowErrorMessage("Failed to load user."));
			return;
		}
	}

	private void CreateUser()
	{
		if (createUserInput.text.Length < 3)
		{
			StartCoroutine(ShowErrorMessage("Username must have minimal length of 3."));
			return;
		}

		SaveStatus created = saveManager.CreateUser(createUserInput.text);

		if (created == SaveStatus.SUCCESS)
		{
			StartCoroutine(ShowErrorMessage("User created."));

			UpdateDropdownAccount();
			return;
		}

		if (created == SaveStatus.FAILED)
		{
			StartCoroutine(ShowErrorMessage("Failed to create new user."));
			return;
		}
	}

	public void DeleteUser()
	{
		if (saveManager.CurrentUser.Id == 0)
		{
			StartCoroutine(ShowErrorMessage("Cannot delete default user!"));
			return;
		}

		saveManager.DeleteUser(saveManager.CurrentUser.Id);

		UpdateUserAccount();
		UpdateDropdownAccount();
	}

	IEnumerator ShowErrorMessage(string message)
	{
		inputErrorText.text = message;
		inputErrorText.enabled = true;

		yield return new WaitForSeconds(5);

		inputErrorText.enabled = false;
	}

	private void UpdateDropdownAccount()
	{
		accountDropdown.ClearOptions();

		List<string> names = new();

		foreach (User user in saveManager.Users)
		{
			names.Add(user.Name);
		}

		accountDropdown.AddOptions(names);
	}

	public void UpdateUserAccount()
	{
		userName.text = saveManager.CurrentUser.Name;
		totalKilled.text = saveManager.CurrentUser.SaveSlots[saveManager.CurrentUser.LastSaveId].PlayerStats.TotalEnemiesKilled + " Bricks";
		totalBounces.text = saveManager.CurrentUser.SaveSlots[saveManager.CurrentUser.LastSaveId].PlayerStats.TotalBounces + " Bounces";
		createdAt.text = saveManager.CurrentUser.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss");
	}
}
