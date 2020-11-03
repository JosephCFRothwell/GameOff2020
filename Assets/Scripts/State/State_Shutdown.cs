using UnityEngine;
using Rothwell.Managers;

namespace Rothwell.State
{
    public class StateShutdown : IGameState
    {
        public IGameState DoState(StateSearch state)
        {
            //This state should handle properly shutting down
            //certain processes safely, then quitting the game

            Application.Quit();
            
            return state.ShutdownState;
        }
    }
}