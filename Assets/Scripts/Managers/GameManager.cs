using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public GameState State = GameState.END;

    private void OnEnable()
    {
        EventBus.Subscribe<GameStateEvent>(UpdateGameState);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<GameStateEvent>(UpdateGameState);
    }

    public void UpdateGameState(GameStateEvent stateEvent)
    {
        State = stateEvent.state;
    }
}
