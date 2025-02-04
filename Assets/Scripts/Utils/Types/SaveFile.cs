using System;
using System.Collections.Generic;
using MessagePack;

[MessagePackObject]
public class SaveFile
{
	[Key("id")]
	public int Id;
	[Key("saveDate")]
	public DateTime SaveDate;

	[Key("playerStats")]
	public Stat PlayerStats;

	[Key("empty")]
	public bool Empty;

	[SerializationConstructor]
	public SaveFile(int id, DateTime saveDate, Stat playerStats)
	{
		Id = id;
		SaveDate = saveDate;
		PlayerStats = playerStats;
		Empty = true;
	}

	public SaveFile(int id, Stat playerStats)
	{
		Id = id;
		PlayerStats = playerStats;
		Empty = true;
	}

	public SaveFile() { }
}