using UnityEngine;


public class BoardPosition : MonoBehaviour // ”правл€ет позицией на доске, отслежива€, зан€та ли она и построен ли в ней храм.
{
    [SerializeField]
    public bool isTempleBuilt = false; // ќтслеживание, построен ли храм в этой позиции.

    private Card cardInPosition; //  арта, наход€ща€с€ в данной позиции.
  
    public bool IsOccupied { get; private set; } = false; // ѕроверка, зан€та ли позици€.

    public void OccupyPosition(Card card) // «анимает позицию указанной картой.
    {
        IsOccupied = true;
        cardInPosition = card;
    }
 
    public void VacatePosition() // ќсвобождает позицию, удал€€ из нее карту.
    {
        IsOccupied = false;
        cardInPosition = null;
    }
   
    public Card GetCard() // ¬озвращает карту, наход€щуюс€ в позиции.
    {
        return cardInPosition;
    }

    public bool IsTempleBuilt // ѕроверка, построен ли в позиции храм.
    {
        get => isTempleBuilt;
        set => isTempleBuilt = value;
    }
}