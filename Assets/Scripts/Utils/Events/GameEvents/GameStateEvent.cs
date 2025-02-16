public class GameStateEvent : EventData
{
    public GameState state;

    public GameStateEvent(GameState state)
    {
        this.state = state;
    }
}