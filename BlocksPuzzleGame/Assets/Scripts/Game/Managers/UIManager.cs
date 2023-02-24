using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Managers
{
    public class UIManager : MonoBehaviour, IProvidable
    {
        [SerializeField] private GameObject gameMenu;
        [SerializeField] private GameObject winMenu;
        private void Awake()
        {
            ServiceProvider.Register(this);
        }
        public void OpenWinMenu()
        {
            gameMenu.SetActive(false);
            winMenu.SetActive(true);
        }
        private void OnEnable()
        {
            LevelManager.levelSuccesedEvent += OpenWinMenu;
        }
        private void OnDisable()
        {
            LevelManager.levelSuccesedEvent -= OpenWinMenu;
        }
    }
}
