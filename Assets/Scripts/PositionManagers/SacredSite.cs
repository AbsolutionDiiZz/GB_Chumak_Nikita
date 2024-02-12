using System;
using UnityEngine;


public class SacredSite : MonoBehaviour // ������������ ��������� ����� � ����, ������� ����� ���� ������� � ������������ �������� �� ������� ����.
{
    [SerializeField]
    public BoardPosition linkedPosition; // ��������� ������� �� ������� ����.

    // ���������� ������� ��� ����������� � �������� ��� �������� ���������� �����.
    public static event Action<SacredSite> OnSacredSiteAdded;
    public static event Action<SacredSite> OnSacredSiteRemoved;

    private void Awake()
    {
        OnSacredSiteAdded?.Invoke(this); // ���������� � ���������� ���������� �����.
    }

    private void OnDestroy()
    {
        OnSacredSiteRemoved?.Invoke(this); // ���������� �� �������� ���������� �����.
    }

    private void OnMouseDown() // ����� ����� ���������� �����.
    {
        SacredSiteManager.SelectedPosition = linkedPosition;
    }
}