namespace Rothwell.State
{
    public class StateCredits : IGameState
    {
        public IGameState DoState(StateSearch state)
        {


            return state.CreditsState;
        }
    }
}