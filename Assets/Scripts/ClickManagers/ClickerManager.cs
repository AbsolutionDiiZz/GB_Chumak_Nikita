using UnityEngine;
using System;

public class ClickerManager : MonoBehaviour
{
    public static event Action OnToggleAllClickers;

    [SerializeField]
    private ClickerBehavior[] clickers;

    public static void ToggleAllClickers()
    {
        OnToggleAllClickers?.Invoke();
    }

    private void Awake()
    {
        OnToggleAllClickers += HandleToggleAllClickers;
    }

    private void OnDestroy()
    {
        OnToggleAllClickers -= HandleToggleAllClickers;
    }

    private void HandleToggleAllClickers()
    {
        foreach (var clicker in clickers)
        {
            // Инвертируем текущее состояние активности для каждого кликера
            bool newState = !clicker.gameObject.activeSelf;
            clicker.ToggleActive(newState);
        }
    }
}