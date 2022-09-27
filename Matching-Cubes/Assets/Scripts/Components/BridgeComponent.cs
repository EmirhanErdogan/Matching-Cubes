using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Emir;
using UnityEngine;

public class BridgeComponent : MonoBehaviour
{
    [SerializeField] private List<Transform> m_points = new List<Transform>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(CommonTypes.PLAYER))
        {
            _ = Move();
        }
        
    }

    private async UniTask Move()
    {
        if (m_points.Count < 1) return;
        int TargetTransform = 0;
        GameManager.Instance.GetPlayerView().SetIsBridge(true);
        while (true)
        {
            if (DOTween.IsTweening(GetInstanceID()))
            {
                await UniTask.WaitForEndOfFrame();
                continue;
            }

            Transform targetRoad = m_points[TargetTransform];

            if (targetRoad == null)
            {
                break;
            }
            Sequence sequence = DOTween.Sequence();
            sequence.Join(GameManager.Instance.GetPlayerView().transform.DOMove(targetRoad.transform.position, 0.25F).SetEase(Ease.Linear));
            sequence.SetId(GetInstanceID());
            sequence.Play();
            TargetTransform++;
            if (TargetTransform >= m_points.Count)
            {
                break;
            }
        }
        GameManager.Instance.GetPlayerView().SetIsBridge(false);
    }
}