using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace System_Programming.Lesson1
{
    public class Unit : MonoBehaviour
    {
        [SerializeField] private Button _healButton;
        [SerializeField] private TMP_Text _textMeshPro;

        private int _health;
        private Coroutine _coroutine;


        private void Start()
        {
            _healButton.onClick.AddListener(ReciveHealing);
            ShowText();
        }

        private void ReciveHealing()
        {
            _coroutine ??= StartCoroutine(HealCoroutine());
        }

        private IEnumerator HealCoroutine()
        {
            for (int i = 0; i < 6; i++)
            {
                if (_health < 100)
                {
                    _health += 5;
                    if (_health > 100)
                    {
                        _health = 100;
                        ShowText();
                        yield break;
                    }
                }
                else
                {
                    yield break;
                }
                ShowText();
                yield return new WaitForSeconds(0.5f);
            }
            _coroutine = null;
        }

        private void ShowText()
        {
            _textMeshPro.text = _health.ToString();
        }

        private void OnDestroy()
        {
            _healButton.onClick.RemoveAllListeners();
        }
    }
}