using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.Gameplay;
using TMPro;
using UnityEngine;

public class LoadPlayerMoves : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
        PlayerController.OnPlayerMove += OnPlayerMove;
    }

    private void OnPlayerMove(int remainingMmoves)
    {
        _text.SetText("X"+ remainingMmoves);
    }
}
