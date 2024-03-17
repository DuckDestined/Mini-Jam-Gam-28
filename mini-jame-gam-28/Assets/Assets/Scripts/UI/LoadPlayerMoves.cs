using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using TMPro;
using UnityEngine;

public class LoadPlayerMoves : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    private void Start()
    {
        _text = GetComponent<TMP_Text>();
        PlayerController.OnPlayerMove += OnPlayerMove;
    }

    private void OnPlayerMove(int remainingMmoves)
    {
        _text.SetText("X"+ remainingMmoves);
    }
}
