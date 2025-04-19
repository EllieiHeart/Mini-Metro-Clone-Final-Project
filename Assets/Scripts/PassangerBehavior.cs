using UnityEngine;

public class PassengerBehavior : MonoBehaviour
{
    private Passenger passengerData;

    /// <summary>
    /// Initializes this passenger's data and visual.
    /// </summary>
    public void AssignPassenger(Passenger passenger, Sprite icon)
    {
        passengerData = passenger;
        GetComponent<SpriteRenderer>().sprite = icon;
    }

    public Passenger GetPassengerData() => passengerData;
}
