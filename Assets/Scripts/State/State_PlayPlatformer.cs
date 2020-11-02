namespace Rothwell.State
{
    public class StatePlayPlatformer : IGameState
    {
        public IGameState DoState(StateSearch state)
        {


            return state.PlayPlatformerState;
        }
    }
}