using UnityEngine;

public class MenuUI : MonoBehaviour
{
	public void StartGame()
	{
		EventBus.Publish<SceneEvent>(new("MainGame"));
		EventBus.Publish<GameStateEvent>(new(GameState.END));
	}

	public void QuitGame()
	{
		Application.Quit();
	}

}
