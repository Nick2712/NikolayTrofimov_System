using UnityEngine;


namespace System_Programming.Lesson4
{
    public class PlayerSpawner
    {
        private readonly Transform[] _spawnPoints;
        private int _index = 0;


        public PlayerSpawner(Transform[] spawnPoints)
        {
            _spawnPoints = spawnPoints;
        }

        public void SetRandomPlayerPosition(GameObject player)
        {
            var randomPoint = _spawnPoints[Random.Range(0, _spawnPoints.Length)];
            player.transform.SetPositionAndRotation(randomPoint.position, randomPoint.rotation);
        }

        public void SetNextPlayerPosition(GameObject player)
        {
            player.transform.SetPositionAndRotation(_spawnPoints[_index].position, _spawnPoints[_index].rotation);
            _index++;
            if (_index >= _spawnPoints.Length) _index = 0;
        }
    }
}