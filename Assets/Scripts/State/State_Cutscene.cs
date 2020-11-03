namespace Rothwell.State
{
    public class StateCutscene : IGameState
    {
        public IGameState DoState(StateSearch state)
        {
            //If a cutscene is triggered, play the correct one
            //When cutscene is complete, return to the previous state
            //This specifically won't include going to another scene
            
            //However, if it is the last cutscene, return out to credits instead.
            
            //Most of the time, can return out to playPlatformer, playLander and PauseMenu

            return state.CutsceneState;
        }
    }
}