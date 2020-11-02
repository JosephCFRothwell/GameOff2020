namespace Rothwell.State
{
    public class StateCutscene : IGameState
    {
        public IGameState DoState(StateSearch state)
        {


            return state.CutsceneState;
        }
    }
}