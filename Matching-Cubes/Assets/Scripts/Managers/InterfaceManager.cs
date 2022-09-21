// using TMPro;
// using System;
// using DG.Tweening;
// using UnityEngine;
// using MoreMountains.NiceVibrations;
//
// namespace Emir
// {
//     public class InterfaceManager : Singleton<InterfaceManager>
//     {
//         #region Serialzable Fields
//
//         [Header("Transforms")]
//         [SerializeField] private RectTransform m_canvas;
//         [SerializeField] private RectTransform m_currencySlot;
//         
//         [Header("Panels")] 
//         [SerializeField] private UIWinPanel m_winPanel;
//         [SerializeField] private UILosePanel m_losePanel;
//
//         [Header("Texts")] 
//         [SerializeField] private TMP_Text m_levelText;
//         [SerializeField] private TMP_Text m_currencyText;
//         
//         [Header("Canvas Groups")] 
//         [SerializeField] private CanvasGroup m_menuCanvasGroup;
//         [SerializeField] private CanvasGroup m_gameCanvasGroup;
//         [SerializeField] private CanvasGroup m_commonCanvasGroup;
//         [SerializeField] private CanvasGroup m_settingsCanvasGroup;
//
//         [Header("Prefabs")] 
//         [SerializeField] private RectTransform m_currencyPrefab;
//         
//         #endregion
//         
//         /// <summary>
//         /// Awake.
//         /// </summary>
//         protected override void Awake()
//         {
//             EventService.AddListener<GameStateChangedEvent>(OnGameStateChanged);
//             EventService.AddListener<CurrencyUpdatedEvent>(OnPlayerCurrencyUpdated);
//             
//             base.Awake();
//         }
//
//         /// <summary>
//         /// This function helper for change settings panel state.
//         /// </summary>
//         /// <param name="state"></param>
//         public void ChangeSettingsPanelState(bool state)
//         {
//             if(DOTween.IsTweening(m_settingsCanvasGroup.GetInstanceID()))
//                 return;
//             
//             Sequence sequence = DOTween.Sequence();
//
//             sequence.Join(m_settingsCanvasGroup.DOFade(state ? 1 : 0, 0.25F));
//             sequence.OnComplete(() =>
//             {
//                 m_settingsCanvasGroup.blocksRaycasts = state;
//             });
//
//             sequence.SetId(m_settingsCanvasGroup.GetInstanceID());
//             sequence.Play();
//         }
//
//         /// <summary>
//         /// This function helper for fly currency animation to target currency icon.
//         /// </summary>
//         /// <param name="worldPosition"></param>
//         public void FlyCurrencyFromWorld(Vector3 worldPosition)
//         {
//             Camera targetCamera = CameraManager.Instance.GetCamera();
//             Vector3 screenPosition = GameUtils.WorldToCanvasPosition(m_canvas, targetCamera, worldPosition);
//             Vector3 targetScreenPosition = m_canvas.InverseTransformPoint(m_currencySlot.position);
//                 
//             RectTransform createdCurrency = Instantiate(m_currencyPrefab, m_canvas);
//             createdCurrency.anchoredPosition = screenPosition;
//             
//             Sequence sequence = DOTween.Sequence();
//
//             sequence.Join(createdCurrency.transform.DOLocalMove(targetScreenPosition, 0.5F));
//
//             sequence.OnComplete(() =>
//             {
//                 Destroy(createdCurrency.gameObject);
//             });
//
//             sequence.Play();
//             
//             SoundManager.Instance.Play(CommonTypes.SFX_CURRENCY_FLY);
//             VibrationManager.Instance.Haptic(HapticTypes.Success);
//         }
//         
//         /// <summary>
//         /// This function helper for fly currency animation to target currency icon.
//         /// </summary>
//         /// <param name="screenPosition"></param>
//         public void FlyCurrencyFromScreen(Vector3 screenPosition)
//         {
//             Vector3 targetScreenPosition = m_canvas.InverseTransformPoint(m_currencySlot.position);
//                 
//             RectTransform createdCurrency = Instantiate(m_currencyPrefab, m_canvas);
//             createdCurrency.position = screenPosition;
//             
//             Sequence sequence = DOTween.Sequence();
//
//             sequence.Join(createdCurrency.transform.DOLocalMove(targetScreenPosition, 0.5F));
//
//             sequence.OnComplete(() =>
//             {
//                 Destroy(createdCurrency.gameObject);
//             });
//
//             sequence.Play();
//             
//             SoundManager.Instance.Play(CommonTypes.SFX_CURRENCY_FLY);
//             VibrationManager.Instance.Haptic(HapticTypes.Success);
//         }
//
//         /// <summary>
//         /// This function called when game state changed.
//         /// </summary>
//         /// <param name="e"></param>
//         private void OnGameStateChanged(GameStateChangedEvent e)
//         {
//             switch (e.GameState)
//             {
//                 case EGameState.STAND_BY:
//
//                     m_levelText.text = $"Level {LevelService.GetCachedLevel(1)}";
//                     
//                     break;
//                 case EGameState.STARTED:
//                     
//                     GameUtils.SwitchCanvasGroup(m_menuCanvasGroup, m_gameCanvasGroup);
//                     
//                     break;
//                 case EGameState.WIN:
//
//                     m_winPanel.Initialize();
//
//                     break;
//                 case EGameState.LOSE:
//                     
//                     m_losePanel.Initialize();
//                     
//                     break;
//             }
//         }
//         
//         /// <summary>
//         /// This function called when player currency updated.
//         /// </summary>
//         /// <param name="e"></param>
//         private void OnPlayerCurrencyUpdated(CurrencyUpdatedEvent e)
//         {
//             string currencyText = m_currencyText.text;
//
//             currencyText = currencyText.Replace(".", String.Empty);
//             currencyText = currencyText.Replace(",", String.Empty);
//             
//             int cachedCurrency = int.Parse(currencyText);
//
//             float delay = e.UIDelay;
//             float duration = e.UIDuration == -1 ? CommonTypes.UI_DEFAULT_FLY_CURRENCY_DURATION : e.UIDuration;
//             
//             Sequence sequence = DOTween.Sequence();
//
//             sequence.Join(DOTween.To(() => cachedCurrency, x => cachedCurrency = x, e.Currency, duration).SetDelay(delay));
//
//             sequence.OnUpdate(() =>
//             {
//                 m_currencyText.text = $"{cachedCurrency.ToString("N0").Replace(",",String.Empty)}";
//             });
//
//             sequence.SetId(m_currencyText.GetInstanceID());
//             sequence.Play();
//         }
//
//         /// <summary>
//         /// This function called when this component destroyed.
//         /// </summary>
//         protected override void OnDestroy()
//         {
//             EventService.RemoveListener<GameStateChangedEvent>(OnGameStateChanged);
//             EventService.RemoveListener<CurrencyUpdatedEvent>(OnPlayerCurrencyUpdated);
//
//             base.OnDestroy();
//         }
//     }  
// }
//
