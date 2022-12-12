using Unity.Netcode;
using UnityEngine;


namespace System_Programming.Lesson4
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerCharacter : NetworkBehaviour
    {
        [Range(0, 100)][SerializeField] private int _health = 100;
        [Range(0.5f, 10.0f)][SerializeField] private float _movingSpeed = 8.0f;
        [SerializeField] private float _acceleration = 3.0f;
        [SerializeField] private GameObject _bulletPrefab;
        private const float gravity = -9.8f;
        private CharacterController _characterController;
        private MouseLook _mouseLook;
        private FireAction _fireAction;


        private void Start()
        {
            if (!IsOwner) return;
            Initiate();
        }

        protected void Initiate()
        {
            _fireAction = new RayShooter(_bulletPrefab, gameObject, 10);
            _fireAction.Reloading();
            _characterController = GetComponentInChildren<CharacterController>();
            _characterController ??= gameObject.AddComponent<CharacterController>();
            _mouseLook = GetComponentInChildren<MouseLook>();
            _mouseLook ??= gameObject.AddComponent<MouseLook>();
        }

        public void Movement()
        {
            //if (_mouseLook != null && _mouseLook.PlayerCamera != null)
            //{
            //    _mouseLook.PlayerCamera.enabled = IsClient;
            //}
            var moveX = Input.GetAxis("Horizontal") * _movingSpeed;
            var moveZ = Input.GetAxis("Vertical") * _movingSpeed;
            var movement = new Vector3(moveX, 0, moveZ);
            movement = Vector3.ClampMagnitude(movement, _movingSpeed);
            movement *= Time.deltaTime;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                movement *= _acceleration;
            }
            movement.y = gravity;
            movement = transform.TransformDirection(movement);
            _characterController.Move(movement);
        }

        private void Update()
        {
            if (!IsOwner) return;
            Movement();
            _fireAction.Update();
            _mouseLook.Rotation();
        }

        private void OnGUI()
        {
            if (!IsOwner) return;
            if (Camera.main == null)
            {
                return;
            }
            var info = $"Health: {_health}\nClip: {_fireAction.BulletCount}";
            var size = 12;
            var bulletCountSize = 50;
            var posX = Camera.main.pixelWidth / 2 - size / 4;
            var posY = Camera.main.pixelHeight / 2 - size / 2;
            var posXBul = Camera.main.pixelWidth - bulletCountSize * 2;
            var posYBul = Camera.main.pixelHeight - bulletCountSize;
            GUI.Label(new Rect(posX, posY, size, size), "+");
            GUI.Label(new Rect(posXBul, posYBul, bulletCountSize * 2,
            bulletCountSize * 2), info);
        }

        public override void OnDestroy()
        {
            _fireAction.Dispose();
            base.OnDestroy();
        }
    }
}