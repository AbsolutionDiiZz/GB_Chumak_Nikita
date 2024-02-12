using UnityEngine;


public class BoardPosition : MonoBehaviour // ��������� �������� �� �����, ����������, ������ �� ��� � �������� �� � ��� ����.
{
    [SerializeField]
    public bool isTempleBuilt = false; // ������������, �������� �� ���� � ���� �������.

    private Card cardInPosition; // �����, ����������� � ������ �������.
  
    public bool IsOccupied { get; private set; } = false; // ��������, ������ �� �������.

    public void OccupyPosition(Card card) // �������� ������� ��������� ������.
    {
        IsOccupied = true;
        cardInPosition = card;
    }
 
    public void VacatePosition() // ����������� �������, ������ �� ��� �����.
    {
        IsOccupied = false;
        cardInPosition = null;
    }
   
    public Card GetCard() // ���������� �����, ����������� � �������.
    {
        return cardInPosition;
    }

    public bool IsTempleBuilt // ��������, �������� �� � ������� ����.
    {
        get => isTempleBuilt;
        set => isTempleBuilt = value;
    }
}