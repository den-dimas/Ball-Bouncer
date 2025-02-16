public class SceneEvent : EventData
{
	public string sceneName;

	public SceneEvent(string sceneName)
	{
		this.sceneName = sceneName;
	}
}