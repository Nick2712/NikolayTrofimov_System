using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;


namespace System_Programming.Lesson4
{
    public abstract class FireAction : IDisposable
    {
        public string BulletCount => _countBullet;

        protected readonly GameObject _player;
        protected readonly GameObject _bulletPrefab;
        protected readonly int _startAmmunition;
        protected string _countBullet = string.Empty;
        protected Queue<GameObject> _bullets = new Queue<GameObject>();
        protected Queue<GameObject> _ammunition = new Queue<GameObject>();
        protected bool _reloading = false;
        private CancellationTokenSource _cancellationTokenSource;
        private int _reloadVisualizationIndex;
        private readonly string[] _reloadVisualization = new string[] { " | ", @" \ ", " - ", " / " };


        public FireAction(GameObject bulletPrefab, GameObject player, int startAmunition)
        {
            _bulletPrefab = bulletPrefab;
            _player = player;
            _startAmmunition = startAmunition;
            _cancellationTokenSource = new CancellationTokenSource();
            Start();
        }

        protected virtual void Start()
        {
            for (var i = 0; i < _startAmmunition; i++)
            {
                GameObject bullet;
                if (_bulletPrefab == null)
                {
                    bullet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    bullet.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                }
                else
                {
                    bullet = UnityEngine.Object.Instantiate(_bulletPrefab);
                }
                bullet.SetActive(false);
                _ammunition.Enqueue(bullet);
            }
        }

        public virtual void Update() { }

        public virtual async void Reloading()
        {
            _bullets = await Reload(_cancellationTokenSource.Token);
        }

        protected virtual void Shooting()
        {
            if (_bullets.Count == 0)
            {
                Reloading();
            }
        }

        private async Task<Queue<GameObject>> Reload(CancellationToken cancellationToken)
        {
            if (!_reloading)
            {
                _reloading = true;
                await ReloadingAnim(cancellationToken);
                return await Task.Run(async delegate
                {
                    var cage = 10;
                    if (_bullets.Count < cage)
                    {
                        await Task.Delay(3000);
                        if (cancellationToken.IsCancellationRequested) return null;
                        var bullets = _bullets;
                        while (bullets.Count > 0)
                        {
                            _ammunition.Enqueue(bullets.Dequeue());
                        }
                        cage = Mathf.Min(cage, _ammunition.Count);
                        if (cage > 0)
                        {
                            for (var i = 0; i < cage; i++)
                            {
                                var sphere = _ammunition.Dequeue();
                                bullets.Enqueue(sphere);
                            }
                        }
                    }
                    _reloading = false;
                    return _bullets;
                });
            }
            else
            {
                return _bullets;
            }
        }

        private async Task ReloadingAnim(CancellationToken cancellationToken)
        {
            while (_reloading)
            {
                _countBullet = ReloadVisualization();
                if (cancellationToken.IsCancellationRequested) return;
                await Task.Delay(10);
            }
            _countBullet = _bullets.Count.ToString();
        }

        private string ReloadVisualization()
        {
            _reloadVisualizationIndex++;
            if (_reloadVisualizationIndex >= _reloadVisualization.Length)
                _reloadVisualizationIndex = 0;
            return _reloadVisualization[_reloadVisualizationIndex];
        }

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
        }
    }
}