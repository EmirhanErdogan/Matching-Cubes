using UnityEngine;

namespace Emir
{
    public class GameManager : Singleton<GameManager>
    {
        #region Serializable Fields

        [Header("Controllers")] 
        [SerializeField] private GameSettings m_gameSettings;

        [SerializeField] private PlayerView m_playerView;
        
        #endregion
        #region Private Fields

        private EGameState gameState = EGameState.NONE;
    
        #endregion

        /// <summary>
        /// Start.
        /// </summary>
        private void Start()
        {
            Application.targetFrameRate = CommonTypes.DEFAULT_FPS;
            
            InitializeWorld();
        }
        
        /// <summary>
        /// This function helper for initialize world.
        /// </summary>
        private void InitializeWorld()
        {
            Level currentLevel = LevelService.GetCurrentLevel();
            
            ThemeManager.Instance.Initialize(currentLevel.Theme);
            
            ChangeGameState(EGameState.STAND_BY);
        }

        /// <summary>
        /// This function helper for start game.
        /// </summary>
        public void StartGame()
        {
            ChangeGameState(EGameState.STARTED);
        }

        /// <summary>
        /// This function helper for change current game state.
        /// </summary>
        /// <param name="gameState"></param>
        public void ChangeGameState(EGameState gameState)
        {
            if(this.gameState == EGameState.WIN)
                return;
            
            if(this.gameState == EGameState.LOSE)
                return;
            
            if(this.gameState == EGameState.STAND_BY && (gameState == EGameState.WIN || gameState == EGameState.LOSE))
                return;
            
            this.gameState = gameState;
        }

        /// <summary>
        /// This function returns related game state.
        /// </summary>
        /// <returns></returns>
        public EGameState GetGameState()
        {
            return gameState;
        }
        
        /// <summary>
        /// This function returns related player view component.
        /// </summary>
        /// <returns></returns>
        public PlayerView GetPlayerView()
        {
            return m_playerView;
        }

        /// <summary>
        /// This function returns related game settings.
        /// </summary>
        /// <returns></returns>
        public GameSettings GetGameSettings()
        {
            return m_gameSettings;
        }
    
        
    }
}