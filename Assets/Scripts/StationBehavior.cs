
using UnityEngine;
using TMPro; // If using TextMeshPro

public class StationBehavior : MonoBehaviour
{
    [SerializeField] private TextMeshPro passengerCountText; // Drag in inspector

    private Station station;

    public void AssignStation(Station _station)
    {
        station = _station;
    }

    public void UpdatePassengerCountDisplay(int count)
    {
        if (passengerCountText == null) return;

        passengerCountText.text = count.ToString();
        passengerCountText.gameObject.SetActive(count > 0);
    }
}
