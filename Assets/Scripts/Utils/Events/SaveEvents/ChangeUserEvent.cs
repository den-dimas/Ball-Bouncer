public class ChangeUserEvent : EventData
{
	public int userId;
	public int saveId;

	public ChangeUserEvent(int userId, int saveId)
	{
		this.userId = userId;
		this.saveId = saveId;
	}
}