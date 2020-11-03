namespace Rothwell.State
{
    public class StateCredits : IGameState
    {
        public IGameState DoState(StateSearch state)
        {
            //when credits are done, return state.MainMenuState;

            return state.CreditsState;
        }
    }
}