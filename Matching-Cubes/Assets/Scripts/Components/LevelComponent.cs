using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Emir;

public class LevelComponent : Singleton<LevelComponent>
{
    [SerializeField] private List<CubeComponent> m_cubes = new List<CubeComponent>();



    public List<CubeComponent> GetCubes()
    {
        return m_cubes;
    }
}
