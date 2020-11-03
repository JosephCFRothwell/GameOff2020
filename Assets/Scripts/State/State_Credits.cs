using Rothwell.Managers;

namespace Rothwell.State
{
    public class StateCredits : IGameState
    {
        public IGameState DoState(StateSearch state)
        {
            //when credits are done, return state.MainMenuState;

            if (ManagerGameState.GSMI.creditsHaveFinished != true) 
            { return state.CreditsState; }
            
            return state.MainMenuState;

        }
    }
}