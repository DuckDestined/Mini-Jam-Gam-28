using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using TMPro;
using UnityEngine;

public class LoadLevelName : MonoBehaviour
{
    [SerializeField] private TMP_Text text;

    private void Awake()
    {
        Level.OnLevelLoaded += LevelOnLevelLoaded;
    }

    private void LevelOnLevelLoaded(int id,Vector3 _ )
    {
        text.SetText("Level " + id);
    }
}
