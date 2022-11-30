using System.Threading;
using System.Threading.Tasks;
using UnityEngine;


namespace System_Programming.Lesson1
{
    public class Lesson1_2 : MonoBehaviour
    {
        private Lesson1_3 _lesson1_3;
        private CancellationTokenSource _cancellationTokenSource;


        private async void Start()
        {
            _lesson1_3 = new Lesson1_3();

            _cancellationTokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = _cancellationTokenSource.Token;

            Task task1 = Task1(cancellationToken);
            Task task2 = Task2(cancellationToken);

            bool result = await _lesson1_3.WhatTaskFasterAsync(cancellationToken, task1, task2);
            _cancellationTokenSource.Cancel();

            Debug.Log(result);
        }

        private async Task Task1(CancellationToken cancellationToken)
        {
            await Task.Delay(3000);
            if (cancellationToken.IsCancellationRequested)
            {
                Debug.Log("выполнение таска 1 прервано");
                return;
            }
            Debug.Log("“аск 1 завершилс€");
        }

        private async Task Task2(CancellationToken cancellationToken)
        {
            for (int i = 0; i < 650; i++)
            {
                await Task.Yield();
                if (cancellationToken.IsCancellationRequested)
                {
                    Debug.Log("выполнение таска 2 прервано");
                    return;
                }
            }
            Debug.Log("“аск 2 завершилс€");
        }

        private void OnDestroy()
        {
            if (!_cancellationTokenSource.IsCancellationRequested)
            {
                _cancellationTokenSource.Cancel();
            }
        }
    }
}