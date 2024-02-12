using System.Collections.Generic;
using UnityEngine;


public class SacredSiteManager : MonoBehaviour // ��������� ����� ���������� ������� � ����, �������� �������������� � ������� ��.
{
    private static List<SacredSite> sacredSites = new List<SacredSite>(); // ������ ���� ��������� ���� � ����.

    public static BoardPosition SelectedPosition { get; set; } // ������� ��������� ������� �� �����.

    private void OnEnable() // ��� ��������� ���������� ������������� �� ������� ���������� � �������� ��������� ����.
    {
        SacredSite.OnSacredSiteAdded += AddSacredSite;
        SacredSite.OnSacredSiteRemoved += RemoveSacredSite;
    }

    private void OnDisable() // ��� ����������� ���������� ������������ �� ������� ���������� � �������� ��������� ����.
    {
        SacredSite.OnSacredSiteAdded -= AddSacredSite;
        SacredSite.OnSacredSiteRemoved -= RemoveSacredSite;
    }

    private static void AddSacredSite(SacredSite site)  // ��������� ��������� ����� � ������, ���� ��� ��� �� ���� ���������.
    {
        if (!sacredSites.Contains(site))
        {
            sacredSites.Add(site);
        }
    }
    private static void RemoveSacredSite(SacredSite site) // ������� ��������� ����� �� ������.
    {
        sacredSites.Remove(site);
    }
    public static List<SacredSite> GetAllSacredSites() // ���������� ������ ���� ������������������ ��������� ����.
    {
        return sacredSites;
    }
}