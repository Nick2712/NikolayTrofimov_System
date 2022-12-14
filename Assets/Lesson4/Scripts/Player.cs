using Unity.Netcode;
using UnityEngine;


namespace System_Programming.Lesson4
{
    public class Player : NetworkBehaviour
    {
        [SerializeField] private GameObject playerPrefab;
        //private GameObject playerCharacter;


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
            
            
            Instantiate(playerPrefab).GetComponent<NetworkObject>().SpawnWithOwnership(OwnerClientId);
            //playerCharacter = Instantiate(playerPrefab);
            //NetworkServer.SpawnWithClientAuthority(playerCharacter, connectionToClient);
        }
    }
}