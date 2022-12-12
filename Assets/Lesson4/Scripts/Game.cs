using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;


namespace System_Programming.Lesson4
{
    public class Game
    {
        private PlayerSpawner _playerSpawner;


        public Game(PlayerSpawner playerSpawner)
        {
            _playerSpawner = playerSpawner;
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        }

        private void OnClientConnected(ulong data)
        {

        }
    }
}