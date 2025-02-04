using System.Collections.Generic;
using MessagePack;

[MessagePackObject]
public class Saved
{
	[Key("LastUser")]
	public int LastUserId;

	[Key("saved")]
	public List<User> Users;

	public Saved(int LastUserId, List<User> Users)
	{
		this.LastUserId = LastUserId;
		this.Users = Users;
	}
}