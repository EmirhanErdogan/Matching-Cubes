using System;
using System.Collections;
using System.Collections.Generic;
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
            Debug.Log(other.gameObject.GetComponent<CubeComponent>().GetRoot().gameObject.name);
            SetIsActive(false);
            other.gameObject.transform.SetParent(null);
            player.RemoveCubes(other.gameObject.GetComponent<CubeComponent>());
            player.RefCubes.Remove(other.gameObject.GetComponent<CubeComponent>());
            player.SortCubeIndex();
            player.SortCubePos(0.5f);

        }
    }
}