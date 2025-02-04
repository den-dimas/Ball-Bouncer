using UnityEngine;

public class TutorialUI : MonoBehaviour
{
	private void OnEnable()
	{
		EventBus.Subscribe<GameStateEvent>(OnGameStateChanged);
	}

	private void OnDisable()
	{
		EventBus.Unsubscribe<GameStateEvent>(OnGameStateChanged);
	}

	private void OnGameStateChanged(GameStateEvent game)
	{
		if (game.state == GameState.SPAWNING_ENEMIES) gameObject.SetActive(false);
		if (game.state == GameState.STARTED) gameObject.SetActive(false);
		if (game.state == GameState.FINISHED) gameObject.SetActive(false);
		if (game.state == GameState.END) gameObject.SetActive(true);
	}
}
