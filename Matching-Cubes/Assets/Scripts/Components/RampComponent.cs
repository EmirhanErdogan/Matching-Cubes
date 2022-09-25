using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RampComponent : MonoBehaviour
{
    [SerializeField] private Transform m_targetTransform;



    public Transform GetTargetPos()
    {
        return m_targetTransform;
    }
}
