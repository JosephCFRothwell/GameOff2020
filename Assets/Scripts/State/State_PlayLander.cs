namespace Rothwell.State
{
    public class StatePlayLander : IGameState
    {
        public IGameState DoState(StateSearch state)
        {


            return state.PlayLanderState;
        }
    }
}