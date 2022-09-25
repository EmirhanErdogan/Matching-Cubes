using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using Emir;
using Unity.VisualScripting;
using UnityEngine.Serialization;

public class PlayerView : MonoBehaviour
{
    #region Serializable Fields

    [Header("Movement Values")] [SerializeField]
    private float m_speed;

    [SerializeField] private float m_horizontalSpeed;
    [SerializeField] private float playerBorder = 5.5f;

    [Header("Transforms")] [SerializeField]
    private Transform m_character;

    [SerializeField] private Transform m_cubeRoot;
    [SerializeField] private List<CubeComponent> m_cubes = new List<CubeComponent>();

    #endregion

    #region Private Fields

    private Vector3 Pos
    {
        get => transform.position;
        set => transform.position = value;
    }

    private Vector3 LastPos;
    private Vector3 movedPos;
    private List<CubeComponent> RefCubes = new List<CubeComponent>();

    #endregion


    private void Update()
    {
        CheckInput();
    }

    private void CheckInput()
    {
        if (GameManager.Instance.GetGameState() != EGameState.STARTED) return;

        PlayerVerticalMovement();
        PlayerHorizantalMovement();
        SetBorder();
    }


    #region Cubes

    private void OrderCubes()
    {
        
            int Index = 0;
            GetCubes().Clear();
            Debug.Log(RefCubes.Count);
            CubeComponent[] CubeColorOne = RefCubes.FindAll(x => x.GetColor() == EColorType.COLOR1).ToArray();
            Debug.Log(CubeColorOne.Length);
            if (CubeColorOne.Length > 0)
            {
                for (int i = 0; i < CubeColorOne.Length; i++)
                {
                    CubeColorOne[i].SetCubeIndex(Index);
                    AddCubes(CubeColorOne[i],false);
                    Index++;
                }
            }
            CubeComponent[] CubeColorTwo = RefCubes.FindAll(x => x.GetColor() == EColorType.COLOR2).ToArray();
            Debug.Log(CubeColorTwo.Length);
            if (CubeColorTwo.Length>0)
            {
                for (int i = 0; i < CubeColorTwo.Length; i++)
                {
                    CubeColorTwo[i].SetCubeIndex(Index);
                    AddCubes(CubeColorTwo[i],false);
                    Index++;
                }
            }
            CubeComponent[] CubeColorThree = RefCubes.FindAll(x => x.GetColor() == EColorType.COLOR3).ToArray();
            Debug.Log(CubeColorThree.Length);
            if (CubeColorThree.Length>0)
            {
                for (int i = 0; i < CubeColorThree.Length; i++)
                {
                    CubeColorThree[i].SetCubeIndex(Index);
                    AddCubes(CubeColorThree[i],false);
                    Index++;
                }
            }
            MatchCubesControl();
    }

    private void RandomCubes()
    {
        
    }

    private void SortCubeIndex()
    {
        if (GetCubes().Count < 1) return;
        int index = 0;
        for (int i = GetCubes().Count - 1; i >= 0; i--)
        {
            GetCubes()[i].SetCubeIndex(index);
            index++;
        }
    }

    private void SortCubePos(float Duration = 0f)
    {
        Vector3 targetPos = Vector3.zero;
        float JumpPower = 1;
        float multiply = GameManager.Instance.GetGameSettings().PunchPositionMultiply;
        for (int i = GetCubes().Count - 1; i >= 0; i--)
        {
            targetPos = Vector3.up * GetCubes()[i].GetCubeIndex() * GetCubes()[i].transform.localPosition.y * 2;
            JumpPower += multiply;
            GetCubes()[i].SetCubePos(targetPos, JumpPower,
                GameManager.Instance.GetGameSettings().PunchPositionDuration, Duration);
        }

        if (GetCubes().Count > 0)
        {
            targetPos += Vector3.up * GetCubes()[0].transform.localPosition.y * 2;
        }
        else
        {
            targetPos = Vector3.zero;
        }

        JumpPower += multiply;
        GetCharacter().DOLocalMove(targetPos, Duration).OnComplete(() =>
        {
            GetCharacter().DOPunchPosition(Vector3.up * JumpPower,
                GameManager.Instance.GetGameSettings().PunchPositionDuration,
                0, 0);
        });
    }

    public void AddCubes(CubeComponent Cube, bool Value=true)
    {
        GetCubes().Add(Cube);
        if (Value is false) return;
        RefCubes.Add(Cube);
    }

    public void RemoveCubes(CubeComponent Cube)
    {
        GetCubes().Remove(Cube);
    }

    private void OpenLastCubeTrail()
    {
        foreach (CubeComponent Cube in GetCubes())
        {
            Cube.GetTrail().CloseTrail();
        }

        GetCubes().LastOrDefault().GetTrail().OpenTrail();
    }

    private void MatchCubesControl()
    {
        if (GetCubes().Count < 3) return;
        for (int i = GetCubes().Count - 1; i >= 0; i--)
        {
            if (i < 2) break;
            EColorType CurrentColor = GetCubes()[i].GetColor();
            EColorType NextColor = GetCubes()[i - 1].GetColor();
            EColorType TwoNextColor = GetCubes()[i - 2].GetColor();
            if (CurrentColor == NextColor && CurrentColor == TwoNextColor)
            {
                CubeComponent[] matchcubes = { GetCubes()[i], GetCubes()[i - 1], GetCubes()[i - 2] };
                Match(matchcubes);
            }
            else
            {
                continue;
            }
        }
    }

    private void Match(CubeComponent[] Cubes)
    {
        foreach (CubeComponent Cube in Cubes)
        {
            Cube.MatchCube();
        }

        DOVirtual.DelayedCall(0.5f, () =>
        {
            foreach (CubeComponent Cube in Cubes)
            {
                Cube.GetRoot().DOScale(0.01f, 0.375f);
                RemoveCubes(Cube);
                Cube.GetRoot().SetParent(null);
                Cube.GetRoot().transform.position = Vector3.zero;
            }

            SortCubeIndex();
            SortCubePos(0.75f);
            // SpeedUp();
        });
    }

    #endregion

    #region Getters

    public Transform GetCubeRoot()
    {
        return m_cubeRoot;
    }

    private Transform GetCharacter()
    {
        return m_character;
    }

    private List<CubeComponent> GetCubes()
    {
        return m_cubes;
    }

    #endregion

    #region Movement

    public void SpeedUp()
    {
        float BaseSpeed = m_speed;
        m_speed *= GameManager.Instance.GetGameSettings().SpeedUpMultiply;
        DOVirtual.DelayedCall(GameManager.Instance.GetGameSettings().SpeedUpDuration, () => { m_speed = BaseSpeed; });
    }

    public void Jump(Transform TargetTransform)
    {
        transform.DOJump(TargetTransform.position, GameManager.Instance.GetGameSettings().JumpPower, 1,
            GameManager.Instance.GetGameSettings().JumpDuration);
    }

    /// <summary>
    /// This Funciton Helper For Vertical Movement.
    /// </summary>
    private void PlayerVerticalMovement()
    {
        transform.Translate(new Vector3(0, 0, m_speed * Time.deltaTime));
    }

    /// <summary>
    /// This Functon Helper For Horizontal Movement.
    /// </summary>
    private void PlayerHorizantalMovement()
    {
        if (Input.GetMouseButtonDown(0))
        {
            LastPos = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            float DeltaPos = Input.mousePosition.x - LastPos.x;
            movedPos = new Vector3(Pos.x + DeltaPos * (Time.fixedDeltaTime * m_horizontalSpeed), Pos.y,
                Pos.z);
            Pos = movedPos;
            LastPos = Input.mousePosition;
        }
    }

    /// <summary>
    /// This Function Helper For Player Horizontal Border.
    /// </summary>
    private void SetBorder()
    {
        if (Pos.x < -playerBorder)
        {
            Pos = new Vector3(-playerBorder, Pos.y, Pos.z);
        }

        if (Pos.x > playerBorder)
        {
            Pos = new Vector3(playerBorder, Pos.y, Pos.z);
        }
    }

    #endregion

    #region Collision

    private void OnCollisionEnter(Collision Target)
    {
        CollectCube(Target);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(CommonTypes.JUMP_TAG))
        {
            Jump(other.gameObject.GetComponent<RampComponent>().GetTargetPos());
        }
        else if (other.gameObject.CompareTag(CommonTypes.SPEED_UP_TAG))
        {
            SpeedUp();
        }
        else if (other.gameObject.CompareTag(CommonTypes.RANDOM_GATE))
        {
            RandomCubes();
            other.gameObject.GetComponent<Collider>().enabled = false;
        }
        else if (other.gameObject.CompareTag(CommonTypes.ORDER_GATE))
        {
            OrderCubes();
            other.gameObject.GetComponent<Collider>().enabled = false;
        }
    }

    private void CollectCube(Collision Target)
    {
        if (Target.gameObject.CompareTag(CommonTypes.COLLECTABLE_TAG))
        {
            CubeComponent TargetCube = Target.gameObject.GetComponent<CubeComponent>();
            TargetCube.CollectCube();
            AddCubes(TargetCube);
            SortCubeIndex();
            SortCubePos();
            OpenLastCubeTrail();
            MatchCubesControl();
        }
    }

    #endregion
}