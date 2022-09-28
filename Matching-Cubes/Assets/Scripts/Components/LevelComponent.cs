using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Emir;

public class LevelComponent : Singleton<LevelComponent>
{
    [SerializeField] private List<CubeComponent> m_cubes = new List<CubeComponent>();
    [SerializeField] private FinishComponent m_finish;


    public List<CubeComponent> GetCubes()
    {
        return m_cubes;
    }

    public FinishComponent GetFinish()
    {
        return m_finish;
    }
}
