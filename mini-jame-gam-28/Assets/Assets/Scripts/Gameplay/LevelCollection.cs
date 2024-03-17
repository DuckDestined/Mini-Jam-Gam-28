using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Gameplay
{
  public class LevelCollection : MonoBehaviour
  {
    [SerializeField] private int _currentLevelIndex = -1;
    [SerializeField] private List<GameObject> levels; 
  
  
    public Level NextLevel()
    {
      if (_currentLevelIndex > -1){

        levels[_currentLevelIndex].SetActive(false);
      }
      _currentLevelIndex++;
      levels[_currentLevelIndex].SetActive(true);
      return levels[_currentLevelIndex].GetComponent<Level>();
    }
  
  }
}
