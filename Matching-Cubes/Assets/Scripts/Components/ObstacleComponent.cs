using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Emir;
using UnityEngine;

public class ObstacleComponent : MonoBehaviour
{
    private bool IsActive = true;
    private PlayerView player => GameManager.Instance.GetPlayerView();

    public bool GetIsActive()
    {
        return IsActive;
    }

    public void SetIsActive(bool value)
    {
        IsActive = value;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(CommonTypes.COLLECT_TAG))
        {
            if (GetIsActive() is false) return;
            CubeComponent TargetCube = other.gameObject.GetComponent<CubeComponent>();
            SetIsActive(false);
            TargetCube.GetRoot().SetParent(null);
            other.gameObject.transform.SetParent(null);
            player.RemoveCubes(TargetCube);
            player.RefCubes.Remove(TargetCube);
            player.SortCubeIndex();

            DOVirtual.DelayedCall(0.375f, () => { player.SortCubePos(); });
        }
    }
}