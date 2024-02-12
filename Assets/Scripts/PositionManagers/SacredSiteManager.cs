using System.Collections.Generic;
using UnityEngine;


public class SacredSiteManager : MonoBehaviour // Управляет всеми священными местами в игре, позволяя регистрировать и удалять их.
{
    private static List<SacredSite> sacredSites = new List<SacredSite>(); // Список всех священных мест в игре.

    public static BoardPosition SelectedPosition { get; set; } // Текущая выбранная позиция на доске.

    private void OnEnable() // При активации компонента подписываемся на события добавления и удаления священных мест.
    {
        SacredSite.OnSacredSiteAdded += AddSacredSite;
        SacredSite.OnSacredSiteRemoved += RemoveSacredSite;
    }

    private void OnDisable() // При деактивации компонента отписываемся от событий добавления и удаления священных мест.
    {
        SacredSite.OnSacredSiteAdded -= AddSacredSite;
        SacredSite.OnSacredSiteRemoved -= RemoveSacredSite;
    }

    private static void AddSacredSite(SacredSite site)  // Добавляет священное место в список, если оно ещё не было добавлено.
    {
        if (!sacredSites.Contains(site))
        {
            sacredSites.Add(site);
        }
    }
    private static void RemoveSacredSite(SacredSite site) // Удаляет священное место из списка.
    {
        sacredSites.Remove(site);
    }
    public static List<SacredSite> GetAllSacredSites() // Возвращает список всех зарегистрированных священных мест.
    {
        return sacredSites;
    }
}