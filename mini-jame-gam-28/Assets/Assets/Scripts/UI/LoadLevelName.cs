using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using TMPro;
using UnityEngine;

public class LoadLevelName : MonoBehaviour
{
    [SerializeField] private TMP_Text text;

    private void Start()
    {
        Level.OnLevelLoaded += LevelOnLevelLoaded;
    }

    private void LevelOnLevelLoaded(int id)
    {
        text.SetText("Level " + id);
    }
}
