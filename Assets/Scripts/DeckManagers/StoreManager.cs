using UnityEngine;

public class StoreManager : MonoBehaviour
{
    public Transform[] storePositions; // Эти позиции теперь будут использовать BoardPosition

    public bool AddCardToFirstEmptyStore(Card card)
    {
        foreach (var position in storePositions)
        {
            var boardPosition = position.GetComponent<BoardPosition>();
            if (!boardPosition.IsOccupied)
            {
                card.transform.position = position.position;
                boardPosition.OccupyPosition(card);
                return true;
            }
        }
        return false;
    }

    public Card GetCardFromStore(int positionIndex)
    {
        if (positionIndex >= 0 && positionIndex < storePositions.Length)
        {
            var boardPosition = storePositions[positionIndex].GetComponent<BoardPosition>();
            if (boardPosition.IsOccupied)
            {
                return boardPosition.GetCard();
            }
        }
        return null;
    }
}