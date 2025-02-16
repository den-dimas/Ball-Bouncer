using MessagePack;

[MessagePackObject]
public class Stat
{
	[Key("totalEnemiesKilled")]
	public int TotalEnemiesKilled;

	[Key("totalBounces")]
	public int TotalBounces;

	public Stat(int totalEnemiesKilled, int totalBounces)
	{
		TotalEnemiesKilled = totalEnemiesKilled;
		TotalBounces = totalBounces;
	}
}