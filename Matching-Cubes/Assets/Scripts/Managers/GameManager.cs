// using UnityEngine;
//
// namespace Emir
// {
//     public class GameManager : Singleton<GameManager>
//     {
//         #region Serializable Fields
//
//         [Header("Controllers")] 
//         [SerializeField] private PlayerController m_playerController;
//         
//         #endregion
//         #region Private Fields
//
//         private EGameState gameState = EGameState.NONE;
//     
//         #endregion
//
//         /// <summary>
//         /// Start.
//         /// </summary>
//         private void Start()
//         {
//             Application.targetFrameRate = CommonTypes.DEFAULT_FPS;
//             
//             InitializeWorld();
//         }
//         
//         /// <summary>
//         /// This function helper for initialize world.
//         /// </summary>
//         private void InitializeWorld()
//         {
//             Level currentLevel = LevelService.GetCurrentLevel();
//             
//             ThemeManager.Instance.Initialize(currentLevel.Theme);
//             
//             ChangeGameState(EGameState.STAND_BY);
//         }
//
//         /// <summary>
//         /// This function helper for start game.
//         /// </summary>
//         public void StartGame()
//         {
//             ChangeGameState(EGameState.STARTED);
//         }
//
//         /// <summary>
//         /// This function helper for change current game state.
//         /// </summary>
//         /// <param name="gameState"></param>
//         public void ChangeGameState(EGameState gameState)
//         {
//             if(this.gameState == EGameState.WIN)
//                 return;
//             
//             if(this.gameState == EGameState.LOSE)
//                 return;
//             
//             if(this.gameState == EGameState.STAND_BY && (gameState == EGameState.WIN || gameState == EGameState.LOSE))
//                 return;
//             
//             this.gameState = gameState;
//
//             GameStateChangedEvent gameStateChangedEvent = new GameStateChangedEvent()
//             {
//                 GameState = GetGameState()
//             };
//             
//             EventService.DispatchEvent(gameStateChangedEvent);
//         }
//
//         /// <summary>
//         /// This function returns related game state.
//         /// </summary>
//         /// <returns></returns>
//         public EGameState GetGameState()
//         {
//             return gameState;
//         }
//         
//         /// <summary>
//         /// This function returns related player view component.
//         /// </summary>
//         /// <returns></returns>
//         public PlayerView GetPlayerView()
//         {
//             return GetPlayerController().GetView<PlayerView>();
//         }
//
//         /// <summary>
//         /// This function returns related player controller component.
//         /// </summary>
//         /// <returns></returns>
//         public PlayerController GetPlayerController()
//         {
//             return m_playerController;
//         }
//     
//         #region EDITOR
//
//         #if UNITY_EDITOR
//         
//         public void Update()
//         {
//             if(Input.GetKeyDown(KeyCode.N))
//                 LevelService.NextLevel();
//         
//             if(Input.GetKeyDown(KeyCode.R))
//                 LevelService.RetryLevel();
//             
//             if(Input.GetKeyDown(KeyCode.M))
//                 GetPlayerController().IncreaseCurrency(100);
//             
//             if(Input.GetKeyDown(KeyCode.M))
//                 InterfaceManager.Instance.FlyCurrencyFromWorld(Vector3.zero);
//             
//             if(Input.GetKeyDown(KeyCode.O))
//                 ChangeGameState(EGameState.WIN);
//             
//             if(Input.GetKeyDown(KeyCode.P))
//                 ChangeGameState(EGameState.LOSE);
//         }
//         #endif
//
//         #endregion
//     }
// }