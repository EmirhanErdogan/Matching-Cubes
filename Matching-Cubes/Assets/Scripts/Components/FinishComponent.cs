using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishComponent : MonoBehaviour
{
    
    [Header("Transforms")]
    [SerializeField] private Transform m_root;
    [SerializeField] private Transform m_center;
   
    public Transform GetRoot()
    {
        return m_root;
    }
    
    
    public Transform GetCenter()
    {
        return m_center;
    }
}
