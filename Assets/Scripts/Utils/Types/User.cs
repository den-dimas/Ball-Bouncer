using System;
using System.Collections.Generic;
using MessagePack;

[MessagePackObject]
public class User
{
	[IgnoreMember]
	public static readonly int SAVE_SLOTS_COUNT = 15;

	[Key("id")]
	public int Id;

	[Key("name")]
	public string Name;

	[Key("achievements")]
	public List<Achievement> Achievements;

	[Key("saveSlots")]
	public List<SaveFile> SaveSlots;

	[Key("lastSaveId")]
	public int LastSaveId;

	[Key("createdAt")]
	public DateTime CreatedAt;

	[SerializationConstructor]
	public User(int Id, string Name, List<Achievement> Achievements)
	{
		this.Id = Id;
		this.Name = Name;
		this.Achievements = Achievements;

		CreatedAt = DateTime.Now;
		SaveSlots = new List<SaveFile>(SAVE_SLOTS_COUNT);

		for (int i = 0; i < SAVE_SLOTS_COUNT; i++)
		{
			SaveSlots.Add(new(i, new(0, 0)));
		}
	}
}