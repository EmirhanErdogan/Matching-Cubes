using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using Emir;

public class PlayerView : MonoBehaviour
{
    #region Serializable Fields
    [SerializeField] private float verticalSpeed;
    [SerializeField] private float horizontalSpeed;
    [SerializeField] private float playerBorder = 5.5f;
    
    #endregion
    
    private Vector3 Pos
    {
        get => transform.position;
        set => transform.position = value;
    }

    private Vector3 LastPos;

    private Vector3 movedPos;

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


    
   
    
    #region Movement

    /// <summary>
    /// This Funciton Helper For Vertical Movement.
    /// </summary>
    private void PlayerVerticalMovement()
    {
        transform.Translate(new Vector3(0, 0, verticalSpeed * Time.deltaTime));
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
            movedPos = new Vector3(Pos.x + DeltaPos * (Time.fixedDeltaTime * horizontalSpeed), Pos.y,
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
}