using System.Collections.Generic;
using UnityEngine;

public class StationManager : MonoBehaviour
{
    [Header("Station Prefabs and Sprites")]
    public GameObject stationPrefab;
    public Sprite[] stationSprites = new Sprite[(int)STATION_SHAPE.LENGTH];

    [Header("Station Settings")]
    
    public float stationSize = 1f;
    public float stationClickRadius = 0.5f;

    [Header("Grid Settings")]
    public float distanceBetweenGridIndices = 1.0f; 
    public float stationPlacementScreenPercent = 0.9f;
    public float stationBoundaryRadius = 3.0f;

    private List<Station> stations = new List<Station>();
    private StationGrid stationGrid;
    private CameraData cameraData;

    private Camera mainCamera;

    public void Initialize(Camera _camera, int _startingStationTotal, int _stationMaximumTotal)
    {
        mainCamera = _camera;
        cameraData = new CameraData(mainCamera, 5.0f, 10.0f); 
        InitializeStationGrid(_startingStationTotal, _stationMaximumTotal);
        CreateStartingStations(_startingStationTotal);
    }

    void Start() { }

    void Update()
    {
        HandleStationInput();
        UpdateStationBehavior();
    }

    private void InitializeStationGrid(int _startingStationTotal, int _stationMaximumTotal)
    {
        stationGrid = new StationGrid(cameraData, distanceBetweenGridIndices, 
                                      _startingStationTotal, _stationMaximumTotal,
                                      stationPlacementScreenPercent, stationBoundaryRadius, stations);
        stationGrid.DebugMode = true;
    }

    private void CreateStartingStations(int startingStationTotal)
    {
        for (int i = 0; i < startingStationTotal; i++)
        {
            CreateStation();
        }
    }

    public void CreateStation()
    {
        Station _station = stationGrid.CreateStation();
        if (_station != null)
        {
            _station.InjectStationObject(
                Instantiate(stationPrefab, Vector3.zero, Quaternion.identity),
                stationSprites[(int)_station.Shape]);

            stations.Add(_station);  // Add the newly created station to the list
        }
    }

    void HandleStationInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);

            foreach (Station station in stations)
            {
                if (IsMouseOverStation(mousePos, station))
                {
                    Debug.Log("Station clicked: " + station.Shape);
                    // Handle station selection here
                }
            }
        }
    }

    bool IsMouseOverStation(Vector2 mousePos, Station station)
    {
        return Vector2.Distance(mousePos, station.Accessor.transform.position) <= stationClickRadius;
    }

    void UpdateStationBehavior()
    {
        foreach (Station station in stations)
        {
            station.UpdatePassengers();
            if (station.CheckOvercrowding())
            {
                // Handle overcrowding behavior
            }
        }
    }

    public Station GetStation(int index)
    {
        return stations[index];
    }

    public List<Station> GetAllStations()
    {
        return stations;
    }
}
