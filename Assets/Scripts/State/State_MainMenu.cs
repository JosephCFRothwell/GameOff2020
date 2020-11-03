namespace Rothwell.State
{
    public class StateMainMenu : IGameState
    {
        public IGameState DoState(StateSearch state)
        {
            //can return out to state.Shutdown if quitting the game
            
            //can return out to playPlatformer, playLander and Cutscene

            return state.MainMenuState;
        }
    }
}