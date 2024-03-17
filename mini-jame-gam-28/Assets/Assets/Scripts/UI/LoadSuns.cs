using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using TMPro;
using UnityEngine;

public class LoadSuns : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private int _suns;
    private void Start()
    {
        _text = GetComponent<TMP_Text>();
        TileManager.OnCollectedSun += OnCollectedSun;
    }

    private void OnCollectedSun()
    {
        ++_suns ;
        _text.SetText( "" + _suns);
    }
}
