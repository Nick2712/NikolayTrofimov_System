using Unity.Collections;
using UnityEngine;
using UnityEngine.Jobs;


namespace System_Programming.Lesson2
{
    public class Lesson2_3 : MonoBehaviour
    {
        private const float MAX_DISTANCE = 5.0f;
        [SerializeField] private GameObject _rotationObject;
        [SerializeField] private int _objectsCount;
        [SerializeField] private float _rotationSpeed;

        private TransformAccessArray _transformAccessArray;

        
        private void Start()
        {
            Transform[] transforms = new Transform[_objectsCount];
            transforms[0] = _rotationObject.transform;
            PlaceObject(transforms[0]);
            for (int i = 1; i < _objectsCount; ++i)
            {
                transforms[i] = Instantiate(_rotationObject).transform;
                PlaceObject(transforms[i]);
            }

            _transformAccessArray = new TransformAccessArray(transforms);
        }

        private void PlaceObject(Transform obj)
        {
            obj.position = Random.insideUnitSphere * MAX_DISTANCE;
        }

        private void FixedUpdate()
        {
            var jobForTransform = new JobForTransform()
            {
                RotationSpeed = _rotationSpeed,
                FixedDeltaTime = Time.fixedDeltaTime
            };
            jobForTransform.Schedule(_transformAccessArray).Complete();
        }

        private void OnDestroy()
        {
            _transformAccessArray.Dispose();
        }
    }

    public struct JobForTransform : IJobParallelForTransform
    {
        [ReadOnly] public float RotationSpeed;
        [ReadOnly] public float FixedDeltaTime;


        public void Execute(int index, TransformAccess transform)
        {
            var rotation = Quaternion.Euler(FixedDeltaTime * RotationSpeed * Vector3.right);
            transform.rotation *= rotation;
        }
    }
}