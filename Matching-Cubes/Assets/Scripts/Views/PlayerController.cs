using System;
using UnityEngine;

namespace Emir
{
    public class PlayerController : Singleton<PlayerController>
    {
        [SerializeField] private Animator m_animator;
        private int Currency;

        private void Awake()
        {
            Initialize();
        }

        public void Initialize()
        {
            Currency = GameManager.Instance.GetCurreny();
        }

        public void Update()
        {
            CheckFinish();
        }

        /// <summary>
        /// This function helper for increase currency.
        /// </summary>
        /// <param name="amount"></param>
        public void IncreaseCurrency(int amount)
        {
            Currency += amount;
        }

        /// <summary>
        /// This function helper for decrease currency.
        /// </summary>
        /// <param name="amount"></param>
        public void DecreaseCurrency(int amount)
        {
            Currency -= amount;
        }

        /// <summary>
        /// This function helper for check currency.
        /// </summary>
        /// <param name="targetAmount"></param>
        /// <returns></returns>
        public bool CheckCurrency(float targetAmount)
        {
            return Currency >= targetAmount;
        }

        private void CheckFinish()
        {
            LevelComponent levelComponent = LevelComponent.Instance;

            if (!GameUtils.IsInFrontOfObject(transform, levelComponent.GetFinish().GetRoot()))
            {
                if (GameManager.Instance.GetPlayerView().GetCubes().Count > 0)
                {
                    GameManager.Instance.ChangeGameState(EGameState.WIN);
                    GetAnimator().SetBool(CommonTypes.DANCE_KEY, true);
                }
                else if (GameManager.Instance.GetPlayerView().GetCubes().Count < 1)
                {
                    GameManager.Instance.ChangeGameState(EGameState.LOSE);
                    GetAnimator().SetBool(CommonTypes.DEFEAT_KEY, true);
                }

                InterfaceManager.Instance.OnGameStateChanged(GameManager.Instance.GetGameState());
            }
        }

        #region Animation

        public void PlayAnimation(string key)
        {
            GetAnimator().SetBool(key, true);
        }

        public void StopAnimation(string key)
        {
            GetAnimator().SetBool(key, false);
        }

        #endregion

        #region Getters

        public Animator GetAnimator()
        {
            return m_animator;
        }

        #endregion
    }
}