using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using Emir;
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

    private void SortCubeIndex()
    {
        int index = 0;
        for (int i = GetCubes().Count - 1; i >= 0; i--)
        {
            GetCubes()[i].SetCubeIndex(index);
            index++;
        }
    }

    private void SortCubePos()
    {
        Vector3 targetPos = Vector3.zero;
        float JumpPower = 1;
        float multiply = GameManager.Instance.GetGameSettings().PunchPositionMultiply;
        for (int i = GetCubes().Count - 1; i >= 0; i--)
        {
            targetPos = Vector3.up * GetCubes()[i].GetCubeIndex() * GetCubes()[i].transform.localPosition.y * 2;
            JumpPower += multiply;
            GetCubes()[i].SetCubePos(targetPos, JumpPower,
                GameManager.Instance.GetGameSettings().PunchPositionDuration);
        }

        targetPos += Vector3.up * GetCubes()[0].transform.localPosition.y * 2;
        JumpPower += multiply;
        GetCharacter().localPosition = targetPos;
        GetCharacter().DOPunchPosition(Vector3.up * JumpPower,
            GameManager.Instance.GetGameSettings().PunchPositionDuration,
            0, 0);
    }

    public void AddCubes(CubeComponent Cube)
    {
        GetCubes().Add(Cube);
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
            if (CurrentColor == NextColor && CurrentColor==TwoNextColor)
            {
                Match();
            }
            else
            {
                return;
            }
        }
    }

    private void Match()
    {
        
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