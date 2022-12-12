using System.Threading.Tasks;
using UnityEngine;


namespace System_Programming.Lesson4
{
    public class RayShooter : FireAction
    {
        private Camera _camera;


        public RayShooter(GameObject bulletPrefab, GameObject player, int startAmunition) : base(bulletPrefab, player, startAmunition)
        {
        }

        protected override void Start()
        {
            base.Start();
            _camera = _player.GetComponentInChildren<Camera>();
        }

        public override void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Shooting();
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                Reloading();
            }
            if (Input.anyKey && !Input.GetKeyDown(KeyCode.Escape))
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        protected override void Shooting()
        {
            base.Shooting();
            if (_bullets.Count > 0)
            {
                Shoot();
            }
        }

        private async void Shoot()
        {
            if (_reloading)
            {
                return;
            }
            var point = new Vector3(Camera.main.pixelWidth / 2,
            Camera.main.pixelHeight / 2, 0);
            var ray = _camera.ScreenPointToRay(point);
            if (!Physics.Raycast(ray, out var hit))
            {
                return;
            }
            var shoot = _bullets.Dequeue();
            _countBullet = _bullets.Count.ToString();
            _ammunition.Enqueue(shoot);
            shoot.SetActive(true);
            shoot.transform.position = hit.point;
            shoot.transform.parent = hit.transform;
            await Task.Delay(2000);
            shoot.SetActive(false);
        }
    }
}