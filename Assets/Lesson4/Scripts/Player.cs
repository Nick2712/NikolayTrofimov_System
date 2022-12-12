using Unity.Netcode;
using UnityEngine;


namespace System_Programming.Lesson4
{
    public class Player : NetworkBehaviour
    {
        [SerializeField] private GameObject playerPrefab;
        private GameObject playerCharacter;


        private void Start()
        {
            SpawnCharacter();
        }

        private void SpawnCharacter()
        {
            if (!IsServer)
            {
                return;
            }
            playerCharacter = Instantiate(playerPrefab);
            NetworkManager.AddNetworkPrefab(playerCharacter);
            //NetworkServer.SpawnWithClientAuthority(playerCharacter,
            //connectionToClient);
        }
    }
}