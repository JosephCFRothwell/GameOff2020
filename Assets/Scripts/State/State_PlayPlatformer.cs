

namespace Rothwell.State
{
    public class StatePlayPlatformer : IGameState
    {
        public IGameState DoState(StateSearch state)
        {
            // can return out to cutscene, playLander or pause menu
            
            return state.PlayPlatformerState;
        }
    }
}