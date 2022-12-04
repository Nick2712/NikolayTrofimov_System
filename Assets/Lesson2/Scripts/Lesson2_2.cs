using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;


namespace System_Programming.Lesson2
{
    public class Lesson2_2 : MonoBehaviour
    {
        private void Start()
        {
            var positions = new NativeArray<Vector3>(new Vector3[]
            {
                new Vector3(1,2,3),
                new Vector3(4,5,6),
                new Vector3(7,8,9)
            }, Allocator.TempJob);
            var velocities = new NativeArray<Vector3>(new Vector3[]
            {
                new Vector3(1,2,3),
                new Vector3(4,5,6),
                new Vector3(7,8,9)
            }, Allocator.TempJob);
            var finalPositions = new NativeArray<Vector3>(new Vector3[3], Allocator.TempJob);

            var jobParalelFor = new JobParalelFor()
            {
                Positions = positions,
                Velocities = velocities,
                FinalPositions = finalPositions
            };
            jobParalelFor.Schedule(positions.Length, 0).Complete();

            for (int i = 0; i < finalPositions.Length; i++) Debug.Log(finalPositions[i]);
            positions.Dispose();
            velocities.Dispose();
            finalPositions.Dispose();
        }
    }

    public struct JobParalelFor : IJobParallelFor
    {
        [ReadOnly] public NativeArray<Vector3> Positions;
        [ReadOnly] public NativeArray<Vector3> Velocities;
        [WriteOnly] public NativeArray<Vector3> FinalPositions;

        public void Execute(int index)
        {
            FinalPositions[index] = Positions[index] + Velocities[index];
        }
    }
}