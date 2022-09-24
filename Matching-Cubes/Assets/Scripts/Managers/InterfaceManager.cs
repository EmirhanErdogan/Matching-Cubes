using TMPro;
using System;
using DG.Tweening;
using UnityEngine;

namespace Emir
{
    public class InterfaceManager : Singleton<InterfaceManager>
    {
        #region Serialzable Fields

        [Header("Transforms")]
        [SerializeField] private RectTransform m_canvas;
        [SerializeField] private RectTransform m_currencySlot;
        
        [Header("Panels")] 
        // [SerializeField] private UIWinPanel m_winPanel;
        // [SerializeField] private UILosePanel m_losePanel;

        [Header("Texts")] 
        [SerializeField] private TMP_Text m_levelText;
        [SerializeField] private TMP_Text m_currencyText;
        
        [Header("Canvas Groups")] 
        [SerializeField] private CanvasGroup m_menuCanvasGroup;
        [SerializeField] private CanvasGroup m_gameCanvasGroup;
        [SerializeField] private CanvasGroup m_commonCanvasGroup;

        [Header("Prefabs")] 
        [SerializeField] private RectTransform m_currencyPrefab;
        
        #endregion
        
        /// <summary>
        /// Awake.
        /// </summary>
        protected override void Awake()
        {
            OnGameStateChanged(GameManager.Instance.GetGameState());
            OnPlayerCurrencyUpdated();
            base.Awake();
        }

        /// <summary>
        /// This function helper for fly currency animation to target currency icon.
        /// </summary>
        /// <param name="worldPosition"></param>
        public void FlyCurrencyFromWorld(Vector3 worldPosition)
        {
            Camera targetCamera = CameraManager.Instance.GetCamera();
            Vector3 screenPosition = GameUtils.WorldToCanvasPosition(m_canvas, targetCamera, worldPosition);
            Vector3 targetScreenPosition = m_canvas.InverseTransformPoint(m_currencySlot.position);
                
            RectTransform createdCurrency = Instantiate(m_currencyPrefab, m_canvas);
            createdCurrency.anchoredPosition = screenPosition;
            
            Sequence sequence = DOTween.Sequence();

            sequence.Join(createdCurrency.transform.DOLocalMove(targetScreenPosition, 0.5F));

            sequence.OnComplete(() =>
            {
                Destroy(createdCurrency.gameObject);
            });

            sequence.Play();
        }
        
        /// <summary>
        /// This function helper for fly currency animation to target currency icon.
        /// </summary>
        /// <param name="screenPosition"></param>
        public void FlyCurrencyFromScreen(Vector3 screenPosition)
        {
            Vector3 targetScreenPosition = m_canvas.InverseTransformPoint(m_currencySlot.position);
                
            RectTransform createdCurrency = Instantiate(m_currencyPrefab, m_canvas);
            createdCurrency.position = screenPosition;
            
            Sequence sequence = DOTween.Sequence();

            sequence.Join(createdCurrency.transform.DOLocalMove(targetScreenPosition, 0.5F));

            sequence.OnComplete(() =>
            {
                Destroy(createdCurrency.gameObject);
            });

            sequence.Play();
        }
    
    /// <summary>
    /// This function called when game state changed.
    /// </summary>
    /// <param name="e"></param>
    public void OnGameStateChanged(EGameState GameState)
    {
        switch (GameState)
        {
            case EGameState.STAND_BY:
    
                m_levelText.text = $"Level {LevelService.GetCachedLevel(1)}";
                
                break;
            case EGameState.STARTED:
                
                GameUtils.SwitchCanvasGroup(m_menuCanvasGroup, m_gameCanvasGroup);
                
                break;
            case EGameState.WIN:
    
                // m_winPanel.Initialize();
    
                break;
            case EGameState.LOSE:
                
                // m_losePanel.Initialize();
                
                break;
        }
    }
    
    /// <summary>
    /// This function called when player currency updated.
    /// </summary>
    /// <param name="e"></param>
    private void OnPlayerCurrencyUpdated()
    {
        string currencyText = m_currencyText.text;
    
        currencyText = currencyText.Replace(".", String.Empty);
        currencyText = currencyText.Replace(",", String.Empty);
        
        int cachedCurrency = int.Parse(currencyText);

        Sequence sequence = DOTween.Sequence();
    
        sequence.Join(DOTween.To(() => cachedCurrency, x => cachedCurrency = x, GameManager.Instance.GetCurreny(), CommonTypes.UI_DEFAULT_FLY_CURRENCY_DURATION));
    
        sequence.OnUpdate(() =>
        {
            m_currencyText.text = $"{cachedCurrency.ToString("N0").Replace(",",String.Empty)}";
        });
    
        sequence.SetId(m_currencyText.GetInstanceID());
        sequence.Play();
    }
    
    }  
}

