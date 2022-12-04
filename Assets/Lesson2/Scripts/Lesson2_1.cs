using Unity.Collections;
using Unity.Jobs;
using UnityEngine;


namespace System_Programming.Lesson2
{
    public class Lesson2_1 : MonoBehaviour
    {
        private void Start()
        {
            NativeArray<int> array = new(new int[] { 1, 2, 3, 4, 11, 12, 12, 13 }, Allocator.TempJob);
            Job job = new()
            {
                Data = array
            };
            job.Schedule().Complete();

            for (int i = 0; i < array.Length; i++) Debug.Log(array[i]);
            array.Dispose();
        }
    }

    public struct Job : IJob
    {
        public NativeArray<int> Data;

        public void Execute()
        {
            for (int i = 0; i < Data.Length; ++i)
                if (Data[i] > 10) Data[i] = 0;
        }
    }
}