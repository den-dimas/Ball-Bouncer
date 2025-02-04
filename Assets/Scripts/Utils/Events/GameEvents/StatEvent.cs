public class StatEvent : EventData
{
	public Stat stats;

	public StatEvent(int totalEnemiesKilled, int totalBounces)
	{
		stats = new(totalEnemiesKilled, totalBounces);
	}
}