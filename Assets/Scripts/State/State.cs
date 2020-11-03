

namespace Rothwell.State
{
    public interface IGameState
    {
        IGameState DoState(StateSearch state);
    }
    
}