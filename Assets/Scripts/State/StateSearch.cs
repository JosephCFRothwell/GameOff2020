using Rothwell.Managers;
using UnityEngine;

namespace Rothwell.State
{
    public class StateSearch : MonoBehaviour
    {
        private IGameState _currentState;
        
        public readonly StateMainMenu MainMenuState = new StateMainMenu();
        public readonly StatePauseMenu PauseMenuState = new StatePauseMenu();
        public readonly StateCredits CreditsState = new StateCredits();
        public readonly StatePlayPlatformer PlayPlatformerState = new StatePlayPlatformer();
        public readonly StatePlayLander PlayLanderState = new StatePlayLander();
        public readonly StateCutscene CutsceneState = new StateCutscene();

        private void OnEnable()
        {
            ManagerGameState.GSMI.currentStateName = ManagerGameState.GSMI.defaultState;
        }

        private void Update()
        {
            _currentState = _currentState.DoState(this);
            ManagerGameState.GSMI.currentStateName = _currentState.ToString();
        }
    }
}