using System;
using System.Collections.Generic;
using Loadson;
using LoadsonAPI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RetryKey
{
    public class Main : Mod
    {
        public override void OnEnable()
        {
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        }

        private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            _currentLevel = arg0.buildIndex;
        }

        public override void Update(float _)
        {
            if (_currentLevel > 1 && Input.GetKeyDown(KeyCode.R))
                Game.Instance.RestartGame();
        }

        private int _currentLevel;
    }
}
