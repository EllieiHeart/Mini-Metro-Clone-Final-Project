using UnityEngine;

/// <summary>
/// This MonoBehaviour script is attached to the Station prefab in Unity.
/// It acts as the bridge between the Unity GameObject and the logical `Station` class.
/// The Manager is responsible for assigning the Station data using AssignStationData().
/// </summary>
public class StationBehavior : MonoBehaviour
{
    // Holds the backend Station logic class instance.
    private Station stationData;

    /// <summary>
    /// Called by the Manager to link this StationBehavior to its corresponding Station logic object.
    /// This should happen right after instantiating the prefab.
    /// </summary>
    /// <param name="station">The Station data class to associate with this behavior.</param>
    public void AssignStationData(Station station)
    {
        stationData = station;

        // If debug mode is enabled on the station, draw its access points for visualization.
        if (stationData.DebugMode)
        {
            DrawAccessPoints();
        }
    }

    /// <summary>
    /// Draws debug lines from the station center to its access points (entry/exit positions).
    /// Only visible in the Scene view during play mode for development purposes.
    /// </summary>
    private void DrawAccessPoints()
    {
        // Loop through all 8 angle-based directions (every 45 degrees)
        for (int i = 0; i < 8; i++)
        {
            // Each direction has 3 access positions
            for (int j = 0; j < 3; j++)
            {
                // Get the world position of this access connection
                Vector2 accessPos = stationData.GetAccessConnection(i, j);

                // Draw a line from the station center (GameObject position) to this access point
                Debug.DrawLine(transform.position, accessPos, Color.cyan, 5f);
            }
        }
    }

    /// <summary>
    /// Returns the Station data associated with this behavior.
    /// Useful for other systems (e.g. LineManager) to access the backend logic.
    /// </summary>
    public Station GetData() => stationData;

    /// <summary>
    /// Checks whether a world-space position lies within this station's collider (defined in Station logic).
    /// </summary>
    /// <param name="pos">World position to test.</param>
    /// <returns>True if the position is inside the station's bounds.</returns>
    public bool IsPositionInside(Vector2 pos)
    {
        return stationData.PositionInStationCollider(pos);
    }
}
