namespace Rothwell.State
{
    public class StatePauseMenu : IGameState
    {
        public IGameState DoState(StateSearch state)
        {


            return state.PauseMenuState;
        }
    }
}