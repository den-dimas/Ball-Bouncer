using System;
using MessagePack;

[MessagePackObject]
public class Achievement
{
	[Key("id")]
	public int Id;
	[Key("title")]
	public string Title;
	[Key("description")]
	public string Description;
	[Key("unlocked")]
	public bool Unlocked;

	[IgnoreMember]
	public Predicate<object> Requirement;

	[SerializationConstructor]
	public Achievement(int id, string title, string description, bool unlocked)
	{
		Id = id;
		Title = title;
		Description = description;
		Unlocked = unlocked;
	}

	public Achievement(int id, string title, string description, Predicate<object> requirement)
	{
		Id = id;
		Title = title;
		Description = description;
		Requirement = requirement;
	}

	public void Unlock()
	{
		if (Unlocked) return;

		if (IsRequirementMet()) Unlocked = true;
	}

	public bool IsRequirementMet()
	{
		return Requirement.Invoke(null);
	}
}