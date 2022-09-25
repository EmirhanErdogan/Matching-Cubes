using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using Emir;
using Unity.VisualScripting;
using UnityEngine;

public class CubeComponent : MonoBehaviour
{
    [SerializeField] private int Index;
    [SerializeField] private Transform m_root;
    [SerializeField] private TrailRenderer m_trail;
    [SerializeField] private EColorType m_type;
    [SerializeField] private Material m_trailMat;
    [SerializeField] private Material m_material;

    private GameSettings gameSettings => GameManager.Instance.GetGameSettings();
    private PlayerView player => GameManager.Instance.GetPlayerView();

    public void ColorInitialize(Color color)
    {
        m_material.color = color;
        m_trailMat.color = color;
    }
    public void CollectCube()
    {
        GetRoot().position = player.GetCubeRoot().position;
        GetRoot().SetParent(player.GetCubeRoot());
        GetRoot().DOPunchScale(Vector3.one * gameSettings.PunchScaleMultiply, gameSettings.PunchScaleDuration, 0, 0);
        gameObject.tag = CommonTypes.COLLECT_TAG;
    }
    public void SetCubePos(Vector3 Pos,float JumpPower,float Duration,float MoveDuration)
    {
        GetRoot().DOLocalMove(Pos, MoveDuration).OnComplete(() =>
        {
            GetRoot().DOPunchPosition(Vector3.up * JumpPower, Duration,
                0, 0);
        });
        
    }
    public void SetCubeIndex(int Value )
    {
        Index = Value;
    }

    public void CubeIndexDecrease(int Value = 1)
    {
        Index -= Value;
    }
    public int GetCubeIndex()
    {
        return Index;
    }
    public Transform GetRoot()
    {
        return m_root;
    }

    public EColorType GetColor()
    {
        return m_type;
    }

    public TrailRenderer GetTrail()
    {
        return m_trail;
    }

    public void MatchCube()
    {
        Color maincolor = GetComponent<MeshRenderer>().material.color;
        Color targetcolor = GameManager.Instance.GetGameSettings().MatchColor;
        GameManager.Instance.GetGameSettings().MatchMaterial.color = maincolor;
        GetComponent<MeshRenderer>().material = GameManager.Instance.GetGameSettings().MatchMaterial;
        GameManager.Instance.GetGameSettings().MatchMaterial.DOColor(GameManager.Instance.GetGameSettings().MatchColor, 0.5f);

    }
    
}