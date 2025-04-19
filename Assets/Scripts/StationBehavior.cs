using TMPro;
using UnityEngine;

public class StationBehavior : MonoBehaviour
{
    private Station stationRef;
    [SerializeField] private TextMeshPro passengerCountText;

    public void AssignStation(Station station)
    {
        stationRef = station;
    }

    private void Update()
    {
        if (stationRef != null && passengerCountText != null)
        {
            passengerCountText.text = stationRef.GetPassengers().Count.ToString();
        }
    }
}
