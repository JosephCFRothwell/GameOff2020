using UnityEngine;
using Rothwell.Managers;

namespace Rothwell.State
{
    public class StatePlayLander : IGameState
    {
        public IGameState DoState(StateSearch state)
        {
            // can return out to cutscene, playPlatformer or pause menu
            
            return state.PlayLanderState;
        }
    }
}