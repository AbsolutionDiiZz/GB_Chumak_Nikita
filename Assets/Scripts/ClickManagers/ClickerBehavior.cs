using UnityEngine;

public class ClickerBehavior : MonoBehaviour
{
    public void ToggleActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
}