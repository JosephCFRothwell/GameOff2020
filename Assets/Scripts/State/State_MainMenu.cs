namespace Rothwell.State
{
    public class StateMainMenu : IGameState
    {
        public IGameState DoState(StateSearch state)
        {


            return state.MainMenuState;
        }
    }
}