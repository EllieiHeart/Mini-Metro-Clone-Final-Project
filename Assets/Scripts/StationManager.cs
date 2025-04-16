using System.Collections.Generic;
using UnityEngine;

public class StationManager : MonoBehaviour
{
    [Header("Station Prefabs and Sprites")]
    public GameObject stationPrefab; // Assign your station GameObject prefab in the Inspector
    public Sprite[] stationSprites; // Array of sprites for different shapes

    [Header("Station Settings")]
    public float stationSize = 1f; // Size of the station GameObjects
    public float stationClickRadius = 0.5f; // Radius around stations for click detection
    // Add any other settings you need (e.g., max passengers, spawn rate)

    private List<Stations_Object> stations = new List<Stations_Object>(); // List to store all Stations_Object data

    void Start()
    {
        InitializeStations(); // Create the initial set of stations
    }

    void Update()
    {
        HandleStationInput(); // Check for player input (e.g., clicks on stations)
        UpdateStationBehavior(); // Update station behavior (passengers, overcrowding)
    }

    /// <summary>
    ///     Creates the initial stations in the scene.
    /// </summary>
    void InitializeStations()
    {
        // For this example, we'll create a few stations at fixed positions, but now let the method
        // determine the shape based on the frequencies.
        CreateStation(new Vector2(0, 0));
        CreateStation(new Vector2(3, 2));
        CreateStation(new Vector2(-2, -1));
    }

    /// <summary>
    ///     Creates a new station in the scene, considering station shape frequencies.
    /// </summary>
    /// <param name="position">The position where the station should be created.</param>
    void CreateStation(Vector2 position) // Removed shape parameter
    {
        // 1. Determine the station shape based on frequency
        StationShape shape = GetRandomStationShape();

        // 2. Create the GameObject
        // Instantiate the stationPrefab at the given position with no rotation (Quaternion.identity)
        GameObject stationGO = Instantiate(stationPrefab, position, Quaternion.identity);
        stationGO.transform.localScale = Vector3.one * stationSize; // Set the station's size

        // 3. Get the SpriteRenderer and set the sprite
        // Get the SpriteRenderer component attached to the station GameObject
        SpriteRenderer sr = stationGO.GetComponent<SpriteRenderer>();
        // If we found a SpriteRenderer and the shape index is valid
        if (sr != null && (int)shape < stationSprites.Length)
        {
            sr.sprite = stationSprites[(int)shape]; // Set the station's sprite
        }

        // 4. Create a Stations_Object data object
        // Create a new Stations_Object object to store station-related data
        Stations_Object newStation = new Stations_Object(stationGO, shape);
        stations.Add(newStation); // Add the new station to the list of stations
    }

    /// <summary>
    ///     Gets a random station shape based on the specified frequencies.
    /// </summary>
    /// <returns>A StationShape enum value.</returns>
    StationShape GetRandomStationShape()
    {
        float randomValue = Random.value; // Get a random number between 0 and 1

        // Define frequency thresholds (these should add up to 1)
        float circleThreshold = 0.5f; // 50% chance for Circle
        float triangleThreshold = 0.8f; // 30% chance for Triangle (50% + 30% = 80%)
        float squareThreshold = 0.9f; // 10% chance for Square (80% + 10% = 90%)
        float diamondThreshold = 0.95f; // 5% chance for Diamond (90% + 5% = 95%)

        if (randomValue < circleThreshold)
        {
            return StationShape.CIRCLE;
        }
        else if (randomValue < triangleThreshold)
        {
            return StationShape.TRIANGLE;
        }
        else if (randomValue < squareThreshold)
        {
            return StationShape.SQUARE;
        }
        else if (randomValue < diamondThreshold)
        {
            return StationShape.DIAMOND;
        }
        else
        {
            // 5% chance for a unique shape
            int uniqueShapeCount = (int)StationShape.UNIQUE_END - (int)StationShape.UNIQUE_START;
            int randomIndex = Random.Range(0, uniqueShapeCount);
            return (StationShape)((int)StationShape.UNIQUE_START + randomIndex);
        }
    }

    /// <summary>
    ///     Handles player input related to stations (e.g., clicking on them).
    /// </summary>
    void HandleStationInput()
    {
        // Check for left mouse button click
        if (Input.GetMouseButtonDown(0)) // 0 = left mouse button
        {
            // Get the mouse position in world coordinates
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Iterate through all stations to check if they were clicked
            foreach (Stations_Object station in stations)
            {
                // Check if the mouse position is within the station's click radius
                if (IsMouseOverStation(mousePos, station))
                {
                    Debug.Log("Station clicked: " + station.shape);
                    // Handle station selection here (e.g., select it for line drawing)
                }
            }
        }
    }

    /// <summary>
    ///     Checks if the given mouse position is over the station.
    /// </summary>
    /// <param name="mousePos">The mouse position in world coordinates.</param>
    /// <param name="station">The station to check.</param>
    /// <returns>True if the mouse is over the station, false otherwise.</returns>
    bool IsMouseOverStation(Vector2 mousePos, Stations_Object station)
    {
        // Simple distance check: if the distance between the mouse and the station's position
        // is less than or equal to the station's click radius, consider it a click.
        return Vector2.Distance(mousePos, station.gameObject.transform.position) <= stationClickRadius;
    }

    /// <summary>
    ///     Updates the behavior of all stations (e.g., passenger generation, overcrowding).
    /// </summary>
    void UpdateStationBehavior()
    {
        // Iterate through all stations and update their behavior
        foreach (Stations_Object station in stations)
        {
            station.UpdatePassengers(); // Update passenger-related logic
            station.CheckOvercrowding(); // Check for overcrowding conditions
        }
    }

    // Public methods for other scripts to access stations (if needed)

    /// <summary>
    ///     Gets a station at a specific index.
    /// </summary>
    /// <param name="index">The index of the station to get.</param>
    /// <returns>The Stations_Object object at the given index.</returns>
    public Stations_Object GetStation(int index)
    {
        return stations[index];
    }

    /// <summary>
    ///     Gets a list of all stations.
    /// </summary>
    /// <returns>A List containing all Stations_Object objects.</returns>
    public List<Stations_Object> GetAllStations()
    {
        return stations;
    }
}

/// <summary>
///     Enum representing the different shapes of stations, including frequency and unique types.
/// </summary>
public enum StationShape
{
    CIRCLE,     // Most frequent
    TRIANGLE,   // Second most frequent
    SQUARE,     // Least frequent
    DIAMOND,    // Another common shape
    // Unique shapes (less frequent)
    STAR,
    WEDGE,
    FOOTBALL,
    CROSS,
    // Add more unique shapes as needed
    UNIQUE_START, // Marker for the start of unique shapes (internal use)
    UNIQUE_END,   // Marker for the end of unique shapes (internal use)
    LENGTH       // Used for calculations, should be the last entry
}

/// <summary>
///     Class representing a station in the game.
///     This class holds data about a station, separate from the GameObject itself.
/// </summary>
public class Stations_Object // Renamed from Station to Stations_Object
{
    public GameObject gameObject; // The station's GameObject in the scene
    public StationShape shape; // The shape of the station
    public List<Passenger> passengers = new List<Passenger>(); // List of passengers currently at the station
    public float timeSinceLastTrain = 0f; // Time since the last train arrived
    public float overcrowdingThreshold = 10f; // Time in seconds before overcrowding occurs

    /// <summary>
    ///     Constructor for the Stations_Object class.
    /// </summary>
    /// <param name="go">The station's GameObject.</param>
    /// <param name="_shape">The shape of the station.</param>
    public Stations_Object(GameObject go, StationShape _shape) // Updated constructor name
    {
        gameObject = go;
        shape = _shape;
    }

    /// <summary>
    ///     Updates passenger-related logic for the station.
    ///     This is where you'd handle passenger generation and behavior.
    /// </summary>
    public void UpdatePassengers()
    {
        // Logic to generate new passengers at this station
        // Logic to update existing passengers (e.g., waiting, boarding trains)
    }

    /// <summary>
    ///     Checks if the station is overcrowded.
    /// </summary>
    public void CheckOvercrowding()
    {
        timeSinceLastTrain += Time.deltaTime; // Increment the time since the last train

        // If the time since the last train exceeds the threshold, the station is overcrowded
        if (timeSinceLastTrain > overcrowdingThreshold)
        {
            Debug.Log("Station is overcrowded!");
            // Implement consequences of overcrowding (e.g., game over, delays)
        }
    }
}

/// <summary>
///     Class representing a passenger.
///     This is a placeholder for more detailed passenger data.
/// </summary>
public class Passenger
{
    // Data for individual passengers (e.g., destination station, waiting time)
}