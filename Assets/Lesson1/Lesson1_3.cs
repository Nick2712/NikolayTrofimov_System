using System.Threading;
using System.Threading.Tasks;
using UnityEngine;


namespace System_Programming.Lesson1
{
    public class Lesson1_3
    {
        public async Task<bool> WhatTaskFasterAsync(CancellationToken cancellationToken, Task task1, Task task2)
        {
            await Task.WhenAny(task1, task2);
            if (cancellationToken.IsCancellationRequested)
            {
                Debug.Log("сработал cancellation token");
                return false;
            }
            if (task1.IsCompleted)
            {
                Debug.Log("первым завершился таск 1");
                return true;
            }
            else
            {
                Debug.Log("первым завершился таск 2");
                return false;
            }
        }
    }
}