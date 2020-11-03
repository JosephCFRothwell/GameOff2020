namespace Rothwell.State
{
    public class StatePauseMenu : IGameState
    {
        public IGameState DoState(StateSearch state)
        {
            //can return out to state.Shutdown if quitting the game
            //can return out to state.MainMenu if quitting to menu
            
            //can return out to playPlatformer, playLander and Cutscene

            return state.PauseMenuState;
        }
    }
}