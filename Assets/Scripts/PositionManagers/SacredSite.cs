using System;
using UnityEngine;


public class SacredSite : MonoBehaviour // Представляет священное место в игре, которое может быть связано с определенной позицией на игровом поле.
{
    [SerializeField]
    public BoardPosition linkedPosition; // Связанная позиция на игровом поле.

    // Используем событие для уведомления о создании или удалении священного места.
    public static event Action<SacredSite> OnSacredSiteAdded;
    public static event Action<SacredSite> OnSacredSiteRemoved;

    private void Awake()
    {
        OnSacredSiteAdded?.Invoke(this); // Уведомляем о добавлении священного места.
    }

    private void OnDestroy()
    {
        OnSacredSiteRemoved?.Invoke(this); // Уведомляем об удалении священного места.
    }

    private void OnMouseDown() // Выбор этого священного места.
    {
        SacredSiteManager.SelectedPosition = linkedPosition;
    }
}