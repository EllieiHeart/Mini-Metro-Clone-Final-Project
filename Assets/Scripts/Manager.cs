using System.Collections.Generic;
using TMPro.SpriteAssetUtilities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

/// <summary>
///     Every station shape variant present.
/// </summary>
public enum STATION_SHAPE
{
    SQUARE,
    CIRCLE,
    TRIANGLE,
    DIAMOND,
    PLUS,
    STAR,
    LENGTH // Used for math, not a station shape.
}

#region Basic Classes
public class CameraData
{
    #region FIELDS

    private Camera camera;
    private bool debugMode;
    private float sizeStart;
    private float sizeEnd;
    private float width;
    private float height;
    private float left;
    private float top;
    private float right;
    private float bottom;
    private float maxWidth;
    private float maxHeight;
    private float maxLeft;
    private float maxTop;
    private float maxRight;
    private float maxBottom;
    private float currentPercent;

    #endregion

    #region PROPERTIES
    /// <summary>
    ///     Whether or not the camera is currently in debug mode.
    /// </summary>
    public bool DebugMode
    {
        get { return debugMode; }
        set
        {
            debugMode = value;
            UpdateCameraSize(currentPercent);
        }
    }

    /// <summary>
    ///     The actual Camera object.
    /// </summary>
    public Camera Accessor
    {
        get { return camera; }
    }

    /// <summary>
    ///     The current x position of the Camera.
    /// </summary>
    public float PositionX
    {
        get { return camera.transform.position.x; }
    }

    /// <summary>
    ///     The current y position of the Camera.
    /// </summary>
    public float PositionY
    {
        get { return camera.transform.position.y; }
    }

    /// <summary>
    ///     The current width of the Camera's view.
    /// </summary>
    public float Width
    {
        get { return width; }
    }

    /// <summary>
    ///     The current height of the Camera's view.
    /// </summary>
    public float Height
    {
        get { return height; }
    }

    /// <summary>
    ///     The current position in the scene of the Camera's left edge.
    /// </summary>
    public float Left
    {
        get { return left; }
    }

    /// <summary>
    ///     The current position in the scene of the Camera's top edge.
    /// </summary>
    public float Top
    {
        get { return top; }
    }

    /// <summary>
    ///     The current position in the scene of the Camera's right edge.
    /// </summary>
    public float Right
    {
        get { return right; }
    }

    /// <summary>
    ///     The current position in the scene of the Camera's bottom edge.
    /// </summary>
    public float Bottom
    {
        get { return bottom; }
    }

    /// <summary>
    ///     The maximum width of the Camera's view.
    /// </summary>
    public float MaxWidth
    {
        get { return maxWidth; }
    }

    /// <summary>
    ///     The maximum height of the Camera's view.
    /// </summary>
    public float MaxHeight
    {
        get { return maxHeight; }
    }

    /// <summary>
    ///     The maximum position in the scene of the Camera's left edge.
    /// </summary>
    public float MaxLeft
    {
        get { return maxLeft; }
    }

    /// <summary>
    ///     The maximum position in the scene of the Camera's top edge.
    /// </summary>
    public float MaxTop
    {
        get { return maxTop; }
    }

    /// <summary>
    ///     The maximum position in the scene of the Camera's right edge.
    /// </summary>
    public float MaxRight
    {
        get { return maxRight; }
    }

    /// <summary>
    ///     The maximum position in the scene of the Camera's bottom edge.
    /// </summary>
    public float MaxBottom
    {
        get { return maxBottom; }
    }
    #endregion

    #region CONSTRUCTORS

    /// <summary>
    ///     Creates a storage system for the Camera along with allowing easier access to its information.
    /// </summary>
    /// <param name="_camera">The Camera object in the scene.</param>
    /// <param name="_sizeStart">The starting size to use for the Camera.</param>
    /// <param name="_sizeEnd">The maximum size that the Camera can reach.</param>
    public CameraData(Camera _camera, float _sizeStart, float _sizeEnd)
    {
        // STEP 1:
        // Insert data into generic fields.
        camera = _camera;
        sizeStart = _sizeStart;
        sizeEnd = _sizeEnd;
        UpdateCameraSize(0.0f);
        debugMode = false;

        // STEP 2:
        // Set Max Camera view values.
        maxHeight = sizeEnd * 2;
        maxWidth = maxHeight * camera.aspect;
        maxLeft = camera.transform.position.x - maxWidth * 0.5f;
        maxRight = camera.transform.position.x + maxWidth * 0.5f;
        maxTop = camera.transform.position.y + maxHeight * 0.5f;
        maxBottom = camera.transform.position.y - maxHeight * 0.5f;
    }

    #endregion

    #region METHODS

    /// <summary>
    ///     Changes the Camera's' dimensions using a percentage value between 0 and 1.
    /// </summary>
    /// <param name="_size">The percentage for the new size for the Camera between its minimum and maximums.</param>
    public void UpdateCameraSize(float _percentSize)
    {
        currentPercent = _percentSize; // Corrected: Use _percentSize
        if (debugMode)
        {
            camera.orthographicSize = sizeEnd;
            height = (sizeStart + (sizeEnd - sizeStart) * Mathf.Clamp(_percentSize, 0.0f, 1.0f)) * 2; // Corrected: Use _percentSize
            width = height * camera.aspect;
            left = camera.transform.position.x - width * 0.5f;
            right = camera.transform.position.x + width * 0.5f;
            top = camera.transform.position.y + height * 0.5f;
            bottom = camera.transform.position.y - height * 0.5f;
        }
        else
        {
            camera.orthographicSize = sizeStart + (sizeEnd - sizeStart) * Mathf.Clamp(_percentSize, 0.0f, 1.0f); // Corrected: Use _percentSize
            height = camera.orthographicSize * 2;
            width = height * camera.aspect;
            left = camera.transform.position.x - width * 0.5f;
            right = camera.transform.position.x + width * 0.5f;
            top = camera.transform.position.y + height * 0.5f;
            bottom = camera.transform.position.y - height * 0.5f;
        }
    }

    

    #endregion
}

public class Timer
{
    #region FIELDS

    private float time;
    private float length;
    private bool trigger;
    private bool runsOnce;

    #endregion

    #region PROPERTIES

    /// <summary>
    ///     Set to true when timer completes.
    /// </summary>
    public bool Trigger
    {
        get { return trigger; }
    }

    /// <summary>
    ///     Returns the percentage of time elpased until the camera is at its maximum size.
    /// </summary>
    public float TimerPercentage
    {
        get { return time / length; }
    }

    #endregion

    #region CONSTRUCTORS

    /// <summary>
    ///     Creates a timer that can be incremented, with a trigger to check when it is completed.
    /// </summary>
    /// <param name="_length">The length of the timer, tied to 'Time.deltaTime' (60 = 1 second).</param>
    /// <param name="_runsOnce"> If true, timer will not repeat</param>
    public Timer(float _length, bool _runsOnce)
    {
        runsOnce = _runsOnce;
        length = _length;
        time = 0.0f;
        trigger = false;
    }

    #endregion

    #region METHODS

    /// <summary>
    ///     Increments the timer. If Runsonce is true, the timer will stop incrementing after time > length.
    /// </summary>
    /// <param name="_deltaTime"> Increments the timer. </param>
    public void IncrementTimer(float _deltaTime)
    {
        if (runsOnce)
        {
            time = Mathf.Min(time + _deltaTime, length);
            trigger = time >= length;
        }
        else
        {
            time += _deltaTime;
            trigger = time >= length;
            if (trigger)
            {
                time -= length;
            }
        }
    }

    #endregion
}

public class MouseData
{
    #region FIELDS
    private Vector2 position;
    private bool leftPressed;
    private bool leftReleased;
    #endregion

    #region PARAMETERS
    public float X { get { return position.x; } }
    public float Y { get { return position.y; } }
    public Vector2 Position { get { return position; } }
    public bool LeftPressed { get { return leftPressed; } }
    public bool LeftReleased { get { return leftReleased; } }
    #endregion

    #region CONSTRUCTORS
    public MouseData ()
    {
        position = Vector2.zero;
        leftPressed = false;
        leftReleased = false;
    }
    #endregion

    #region METHODS
    public void SetData ()
    {
        // Handle Mouse Position
        position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Buttons pressed.
        if (leftPressed) leftPressed = false;
        if (leftReleased) leftReleased = false;
        if (Input.GetMouseButtonDown(0)) leftPressed = true;
        if (Input.GetMouseButtonUp(0)) leftReleased = true;
    }
    #endregion
}
#endregion

public class RectCollider
{
    #region FIELDS
    private float left;
    private float right;
    private float top;
    private float bottom;
    Vector2 topLeft;
    Vector2 bottomRight;
    #endregion

    #region PROPERTIES
    /// <summary>
    /// The left x position of the collider
    /// </summary>
    public float Left { get { return left; } }
    /// <summary>
    /// The right x position of the collider
    /// </summary>
    public float Right { get { return right; } }
    /// <summary>
    /// The top y position of the collider
    /// </summary>
    public float Top { get { return top; } }
    /// <summary>
    /// The bottom y position of the collider
    /// </summary>
    public float Bottom { get { return bottom; } }
    public Vector2 TopLeft { get { return topLeft; } }
    public Vector2 BottomRight { get { return bottomRight; } }
    #endregion

    #region CONSTRUCTOR
    public RectCollider(float _left, float _right, float _top, float _bottom)
    {
        left = _left;
        right = _right;
        top = _top;
        bottom = _bottom;
        topLeft = new Vector2(_left, _top);
        bottomRight = new Vector2(_right, _bottom);
    }
    #endregion

    #region METHODS
    public bool PositionInCollider(Vector2 _position)
    {
        return (_position.x >= left && _position.x <= right && _position.y >= bottom && _position.y <= top);
    }
    #endregion
}

public class Station
{
    #region FIELDS

    private bool debugMode;
    private GameObject accessor;
    private STATION_SHAPE shape;
    private Vector2 position;

    private Point[,] pointConnections;
    private Vector2[,] accessConnections;
    private List<Point> totalPoints;

    private RectCollider collider;
    

    #endregion

    #region PROPERTIES

    public bool DebugMode { get { return debugMode; } set { debugMode = value; } }

    public Vector2[,] AccessConnections { get { return accessConnections; } }

    public GameObject Accessor
    {
        get { return accessor; }
    }

    public STATION_SHAPE Shape
    {
        get { return shape; }
    }

    public Vector2 StationTruePosition { get { return position; } }

    public float Scale
    {
        get { return accessor.transform.localScale.x; }
        set { accessor.transform.localScale.Set(value, value, 1); }
    }

    #endregion

    #region CONSTRUCTORS

    public Station(Vector2 _position, int _shape)
    {
        position = _position;
        shape = (STATION_SHAPE)_shape;
        pointConnections = new Point[8, 3];
        accessConnections = new Vector2[8, 3];
        totalPoints = new List<Point>();

        float _length = 0.1f;
        float _offsetLength = 0.05f;
        for (int _i = 0; _i < 8; ++_i)
        {
            accessConnections[_i, 0] = new Vector2(
                position.x + _length * Mathf.Cos((Mathf.PI / 4) * _i),
                position.y + _length * Mathf.Sin((Mathf.PI / 4) * _i));
            accessConnections[_i, 1] = new Vector2(
                accessConnections[_i, 0].x + _offsetLength * Mathf.Cos((Mathf.PI / 2) * _i + (Mathf.PI / 2)),
                accessConnections[_i, 0].y + _offsetLength * Mathf.Sin((Mathf.PI / 2) * _i + (Mathf.PI / 2)));
            accessConnections[_i, 2] = new Vector2(
                accessConnections[_i, 1].x + _offsetLength * Mathf.Cos((Mathf.PI / 2) * _i - (Mathf.PI / 2)),
                accessConnections[_i, 1].y + _offsetLength * Mathf.Sin((Mathf.PI / 2) * _i - (Mathf.PI / 2)));
        }
        debugMode = true;

        collider = new RectCollider(_position.x - 0.4f, _position.x + 0.4f, _position.y + 0.4f, _position.y - 0.4f);
    }

    #endregion

    #region METHODS

    public void InjectStationObject(GameObject _accessor, Sprite _sprite)
    {
        accessor = _accessor;
        
        // Assuming the accessor (the station object) has a SpriteRenderer component.
        SpriteRenderer spriteRenderer = accessor.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = _sprite; // Set the sprite on the station object
        }
    }

    public bool PositionInStationCollider(Vector2 _position)
    {
        return collider.PositionInCollider(_position);
    }

    public Vector2 GetAccessConnection(int _accessAngleIndex, int _accessSlot)
    {
        return accessConnections[_accessAngleIndex, _accessSlot];
    }

    public Point GetPointConnection(int _accessAngleIndex, int _accessSlot)
    {
        return pointConnections[_accessAngleIndex, _accessSlot];
    }

    public void SetPointConnection(int _accessAngleIndex, int _accessSlot, Point _point)
    {
        pointConnections[_accessAngleIndex, _accessSlot] = _point;
    }

    public bool CheckOvercrowding()
    {
        // For example:
        return totalPoints.Count > 100; // Or some custom threshold
    }

    public void UpdatePassengers()
    {
        // For example:
        Debug.Log("Updating passengers...");
    }

    #endregion
}


public class Passenger
{
    // Data for individual passengers (e.g., destination station, waiting time)
}

// ... (rest of Manager.cs code)



public class Point
{
    #region FIELDS
    private List<Point> line;
    private Station tiedStation;
    private int accessEntryAngleIndex;
    private int accessEntrySlot;
    private int accessExitAngleIndex;
    private int accessExitSlot;
    private bool heldByMouse;
    #endregion

    #region PROPERTIES
    /// <summary>
    /// The line this point falls within.
    /// </summary>
    public List<Point> Line { get { return line; } }
    /// <summary>
    /// The station this line is tied to.
    /// </summary>
    public Station TiedStation { get { return tiedStation; } }

    #region ENTRY & EXITS
    /// <summary>
    /// The angle this station should enter at into a station.
    /// </summary>
    public int AccessEntryAngleIndex { get { return accessEntryAngleIndex; } set { accessEntryAngleIndex = value; } }
    /// <summary>
    /// Within one of the 8 directions lines can link up to a station, this point enters into the given slot of a direction.
    /// </summary>
    public int AccessEntrySlot { get { return accessEntrySlot; } set { accessEntrySlot = value; } }
    /// <summary>
    /// The angle this station should exit at from a station.
    /// </summary>
    public int AccessExitAngleIndex { get { return accessExitAngleIndex; } set { accessExitAngleIndex = value; } }
    /// <summary>
    /// Within one of the 8 directions lines can link up to a station, this point exits from the given slot of a direction.
    /// </summary>
    public int AccessExitSlot { get { return accessExitSlot; } set { accessExitSlot = value; } }
    #endregion
    #endregion

    #region CONSTRUCTORS
    /// <summary>
    /// Creates a point and links it up with a station and associated line.
    /// </summary>
    /// <param name="_line">The line this point is within.</param>
    /// <param name="_tiedStation">The station to link up with.</param>
    public Point (List<Point> _line, Station _tiedStation)
    {
        line = _line;
        tiedStation = _tiedStation;
        accessEntryAngleIndex = -1;
        accessEntrySlot = -1;
        accessExitAngleIndex = -1;
        accessExitSlot = -1;
        //tiedStation.SetPoint(accessEntryAngleIndex, this, true);
    }
    /// <summary>
    /// When creating a point at a station with both a known entry and exit.
    /// </summary>
    /// <param name="_line">The line this point is within.</param>
    /// <param name="_tiedStation">The station to link up with.</param>
    /// <param name="_accessEntryAngle">The starting angle to use for this point.</param>
    /// <param name="_accessExitAngle">The angle to leave at for this point.</param>
    public Point (List<Point> _line, Station _tiedStation, int _accessEntryAngle, int _accessExitAngle)
    {
        line = _line;
        tiedStation = _tiedStation;
        accessEntryAngleIndex = _accessEntryAngle;
        accessExitAngleIndex = _accessExitAngle;
        
    }
    #endregion

    #region METHODS
    #endregion
}

public class LineManager
{
    #region FIELDS
    private MouseData mouseData;
    private List<Station> stations;
    private LineRenderer[] lineRenderers;
    private List<Point>[] linePoints;
    private List<Vector2>[] bendPositions;

    private Point playerConnectedPoint;
    private int playerConnectedLineIndex;
    private int playerConnectedPointIndex;
    private int currentNewLine;

    private float playerLineAngle;
    private float oldPlayerLineAngle;
    private float fixedLineStartAngle;
    private float fixedLineEndAngle;
    private bool bendRight;
    #endregion

    #region PROPERTIES

    #endregion

    #region CONSTRUCTORS
    public LineManager (MouseData _mouseData, List<Station> _stations, LineRenderer[] _lineRenderers)
    {
        mouseData = _mouseData;
        stations = _stations;
        lineRenderers = _lineRenderers;
        linePoints = new List<Point>[lineRenderers.Length];
        for (int _i = 0; _i < lineRenderers.Length; ++_i)
        {
            linePoints[_i] = new List<Point> ();
        }
        bendPositions = new List<Vector2>[lineRenderers.Length];
        for (int _i = 0; _i < lineRenderers.Length; ++_i)
        {
            bendPositions[_i] = new List<Vector2>();
        }
        playerConnectedPoint = null;
        playerConnectedLineIndex = 0;
        playerConnectedPointIndex = 0;
        currentNewLine = 0;

        playerLineAngle = 0.0f;
        oldPlayerLineAngle = 0.0f;
        fixedLineStartAngle = 0;
        fixedLineEndAngle = 0;
        bendRight = false;
    }
    #endregion

    #region METHODS
    public void PlayerInput ()
    {
        oldPlayerLineAngle = playerLineAngle;

        if (playerConnectedPoint == null)
        {
            // Player can click stations.
            for (int _i = 0; _i < stations.Count; ++_i)
            {
                if (stations[_i].PositionInStationCollider(mouseData.Position))
                {
                    if (mouseData.LeftPressed)
                    {
                        // Create new line from scratch.
                        playerConnectedPoint = new Point(linePoints[currentNewLine], stations[_i]);
                        linePoints[currentNewLine].Add(playerConnectedPoint);
                        linePoints[currentNewLine].Add(null);
                        playerConnectedLineIndex = currentNewLine;
                        playerConnectedPointIndex = 0;
                        bendPositions[currentNewLine].Add(Vector2.zero);
                        currentNewLine++;

                        UpdateBend(
                                playerConnectedLineIndex, playerConnectedPointIndex,
                                playerConnectedPoint.TiedStation.StationTruePosition.x,
                                playerConnectedPoint.TiedStation.StationTruePosition.y,
                                mouseData.X, mouseData.Y);
                    }
                }
            }
        }
        else
        {
            // Update the position of the currently held line's bends.
            UpdateBend(
                    playerConnectedLineIndex, playerConnectedPointIndex,
                    playerConnectedPoint.TiedStation.StationTruePosition.x, 
                    playerConnectedPoint.TiedStation.StationTruePosition.y,
                    mouseData.X, mouseData.Y);

            // Player can hover over stations.
            for (int _i = 0; _i < stations.Count; ++_i)
            {
                if (stations[_i].PositionInStationCollider(mouseData.Position))
                {
                    bool _stationExists = false;
                    for (int _j = 1; _j < linePoints[playerConnectedLineIndex].Count; ++_j)
                    {
                        if (linePoints[playerConnectedLineIndex][_j] == null) continue;
                        if (linePoints[playerConnectedLineIndex][_j].TiedStation == stations[_i])
                        {
                            _stationExists = true;
                            break;
                        }
                    }

                    // Check for 0 index to allow for looping.
                    bool _completeLoop = false;                    
                    if (linePoints[playerConnectedLineIndex][0].TiedStation == stations[_i])
                    {
                        if (linePoints[playerConnectedLineIndex].Count <= 3) _stationExists = true;
                        else _completeLoop = true;
                    }

                    if (!_stationExists)
                    {
                        // Create new line from scratch.
                        playerConnectedPoint = new Point(linePoints[playerConnectedLineIndex], stations[_i]);
                        linePoints[playerConnectedLineIndex].Insert(
                                linePoints[playerConnectedLineIndex].Count - 1, playerConnectedPoint);
                        bendPositions[playerConnectedLineIndex].Add(Vector2.zero);
                        playerConnectedPointIndex++;
                        UpdateBend(
                                playerConnectedLineIndex, playerConnectedPointIndex,
                                playerConnectedPoint.TiedStation.StationTruePosition.x,
                                playerConnectedPoint.TiedStation.StationTruePosition.y,
                                mouseData.X, mouseData.Y);

                        if (_completeLoop) MouseDropLine();
                    }
                }
            }

            if (mouseData.LeftReleased)
            {
                MouseDropLine();
            }
        }

        UpdateRenderer();
    }

    /// <summary>
    /// Used to cause whatever line is currently being held to be dropped.
    /// </summary>
    private void MouseDropLine()
    {
        playerConnectedPoint = null;
        linePoints[playerConnectedLineIndex].Remove(null);
        bendPositions[playerConnectedLineIndex].RemoveAt(playerConnectedPointIndex);
        if (linePoints[playerConnectedLineIndex].Count == 1)
        {
            // remove point here.
            linePoints[playerConnectedLineIndex].Clear();
            currentNewLine--;
        }
    }

    /// <summary>
    /// Sets the position of a bend point for a line.
    /// </summary>
    /// <param name="_bendLine">The line which the desired points are within.</param>
    /// <param name="_bendIndex">The index in bendPositions this bend is at.</param>
    /// <param name="_startX">The starting x position that the bend will be placed after.</param>
    /// <param name="_startY">The starting y position that the bend will be placed after.</param>
    /// <param name="_endX">The ending x position that the bend will be placed after.</param>
    /// <param name="_endY">The ending y position that the bend will be placed after.</param>
    private void UpdateBend(int _bendLine, int _bendIndex, float _startX, float _startY, float _endX, float _endY)
    {
        // STEP 1:
        // Calculate the angles to start and end the bend with.
        
        playerLineAngle =
                Mathf.Repeat(
                    Mathf.Atan2(
                        _endY - _startY,
                        _endX - _startX) 
                    * Mathf.Rad2Deg,
                    360.0f);
        fixedLineStartAngle =
                (int)Mathf.Repeat(
                    Mathf.Floor(
                        (playerLineAngle + 25.5f) / 45.0f)
                    * 45.0f, 360);
        if (fixedLineStartAngle == 0)
        {
            fixedLineEndAngle = (playerLineAngle > 270) ? 315 : 45;
        }
        else
        {
            fixedLineEndAngle =
                    (playerLineAngle <= fixedLineStartAngle)
                    ? (int)Mathf.Repeat(fixedLineStartAngle - 45.0f, 360)
                    : (int)Mathf.Repeat(fixedLineStartAngle + 45.0f, 360);
        }

        // STEP 2:
        // Set bend position proprer, accounting for direction of the bend.
        if (bendRight)
        {
            if (fixedLineStartAngle == 0 && fixedLineEndAngle == 315)
            {
                bendPositions[_bendLine][_bendIndex] = new Vector2(_endX - Mathf.Abs(_startY - _endY), _startY);
            }
            else if (fixedLineStartAngle == 0 && fixedLineEndAngle == 45)
            {
                bendPositions[_bendLine][_bendIndex] = new Vector2(_startX + Mathf.Abs(_endY - _startY), _endY);
            }
            else if (fixedLineStartAngle == 45 && fixedLineEndAngle == 0)
            {
                bendPositions[_bendLine][_bendIndex] = new Vector2(_startX + Mathf.Abs(_endY - _startY), _endY);
            }
            else if (fixedLineStartAngle == 45 && fixedLineEndAngle == 90)
            {
                bendPositions[_bendLine][_bendIndex] = new Vector2(_startX, _endY - Mathf.Abs(_startX - _endX));
            }
            else if (fixedLineStartAngle == 90 && fixedLineEndAngle == 45)
            {
                bendPositions[_bendLine][_bendIndex] = new Vector2(_startX, _endY - Mathf.Abs(_startX - _endX));
            }
            else if (fixedLineStartAngle == 90 && fixedLineEndAngle == 135)
            {
                bendPositions[_bendLine][_bendIndex] = new Vector2(_endX, _startY + Mathf.Abs(_endX - _startX));
            }
            else if (fixedLineStartAngle == 135 && fixedLineEndAngle == 90)
            {
                bendPositions[_bendLine][_bendIndex] = new Vector2(_endX, _startY + Mathf.Abs(_endX - _startX));
            }
            else if (fixedLineStartAngle == 135 && fixedLineEndAngle == 180)
            {
                bendPositions[_bendLine][_bendIndex] = new Vector2(_endX + Mathf.Abs(_startY - _endY), _startY);
            }
            else if (fixedLineStartAngle == 180 && fixedLineEndAngle == 135)
            {
                bendPositions[_bendLine][_bendIndex] = new Vector2(_endX + Mathf.Abs(_startY - _endY), _startY);
            }
            else if (fixedLineStartAngle == 180 && fixedLineEndAngle == 225)
            {
                bendPositions[_bendLine][_bendIndex] = new Vector2(_startX - Mathf.Abs(_endY - _startY), _endY);
            }
            else if (fixedLineStartAngle == 225 && fixedLineEndAngle == 180)
            {
                bendPositions[_bendLine][_bendIndex] = new Vector2(_startX - Mathf.Abs(_endY - _startY), _endY);
            }
            else if (fixedLineStartAngle == 225 && fixedLineEndAngle == 270)
            {
                bendPositions[_bendLine][_bendIndex] = new Vector2(_startX, _endY + Mathf.Abs(_startX - _endX));
            }
            else if (fixedLineStartAngle == 270 && fixedLineEndAngle == 225)
            {
                bendPositions[_bendLine][_bendIndex] = new Vector2(_startX, _endY + Mathf.Abs(_startX - _endX));
            }
            else if (fixedLineStartAngle == 270 && fixedLineEndAngle == 315)
            {
                bendPositions[_bendLine][_bendIndex] = new Vector2(_endX, _startY - Mathf.Abs(_endX - _startX));
            }
            else if (fixedLineStartAngle == 315 && fixedLineEndAngle == 270)
            {
                bendPositions[_bendLine][_bendIndex] = new Vector2(_endX, _startY - Mathf.Abs(_endX - _startX));
            }
            else if (fixedLineStartAngle == 315 && fixedLineEndAngle == 0)
            {
                bendPositions[_bendLine][_bendIndex] = new Vector2(_endX - Mathf.Abs(_startY - _endY), _startY);
            }

            if (Mathf.Floor(oldPlayerLineAngle / 45.0f) < Mathf.Floor(playerLineAngle / 45.0f) &&
                !(Mathf.Floor(oldPlayerLineAngle / 45.0f) == 0 && Mathf.Floor(playerLineAngle / 45.0f) == 7))
            {
                bendRight = false;
            }
        }
        else
        {
            if (fixedLineStartAngle == 0 && fixedLineEndAngle == 315)
            {
                bendPositions[_bendLine][_bendIndex] = new Vector2(_endX - Mathf.Abs(_startY - _endY), _startY);
            }
            else if (fixedLineStartAngle == 0 && fixedLineEndAngle == 45)
            {
                bendPositions[_bendLine][_bendIndex] = new Vector2(_endX - Mathf.Abs(_startY - _endY), _startY);
            }
            else if (fixedLineStartAngle == 45 && fixedLineEndAngle == 0)
            {
                bendPositions[_bendLine][_bendIndex] = new Vector2(_endX - Mathf.Abs(_startY - _endY), _startY);
            }
            else if (fixedLineStartAngle == 45 && fixedLineEndAngle == 90)
            {
                bendPositions[_bendLine][_bendIndex] = new Vector2(_endX, _startY + Mathf.Abs(_endX - _startX));
            }
            else if (fixedLineStartAngle == 90 && fixedLineEndAngle == 45)
            {
                bendPositions[_bendLine][_bendIndex] = new Vector2(_endX, _startY + Mathf.Abs(_endX - _startX));
            }
            else if (fixedLineStartAngle == 90 && fixedLineEndAngle == 135)
            {
                bendPositions[_bendLine][_bendIndex] = new Vector2(_startX, _endY - Mathf.Abs(_startX - _endX));
            }
            else if (fixedLineStartAngle == 135 && fixedLineEndAngle == 90)
            {
                bendPositions[_bendLine][_bendIndex] = new Vector2(_startX, _endY - Mathf.Abs(_startX - _endX));
            }
            else if (fixedLineStartAngle == 135 && fixedLineEndAngle == 180)
            {
                bendPositions[_bendLine][_bendIndex] = new Vector2(_startX - Mathf.Abs(_endY - _startY), _endY);
            }
            else if (fixedLineStartAngle == 180 && fixedLineEndAngle == 135)
            {
                bendPositions[_bendLine][_bendIndex] = new Vector2(_startX - Mathf.Abs(_endY - _startY), _endY);
            }
            else if (fixedLineStartAngle == 180 && fixedLineEndAngle == 225)
            {
                bendPositions[_bendLine][_bendIndex] = new Vector2(_endX + Mathf.Abs(_startY - _endY), _startY);
            }
            else if (fixedLineStartAngle == 225 && fixedLineEndAngle == 180)
            {
                bendPositions[_bendLine][_bendIndex] = new Vector2(_endX + Mathf.Abs(_startY - _endY), _startY);
            }
            else if (fixedLineStartAngle == 225 && fixedLineEndAngle == 270)
            {
                bendPositions[_bendLine][_bendIndex] = new Vector2(_endX, _startY - Mathf.Abs(_endX - _startX));
            }
            else if (fixedLineStartAngle == 270 && fixedLineEndAngle == 225)
            {
                bendPositions[_bendLine][_bendIndex] = new Vector2(_endX, _startY - Mathf.Abs(_endX - _startX));
            }
            else if (fixedLineStartAngle == 270 && fixedLineEndAngle == 315)
            {
                bendPositions[_bendLine][_bendIndex] = new Vector2(_startX, _endY + Mathf.Abs(_startX - _endX));
            }
            else if (fixedLineStartAngle == 315 && fixedLineEndAngle == 270)
            {
                bendPositions[_bendLine][_bendIndex] = new Vector2(_startX, _endY + Mathf.Abs(_startX - _endX));
            }
            else if (fixedLineStartAngle == 315 && fixedLineEndAngle == 0)
            {
                bendPositions[_bendLine][_bendIndex] = new Vector2(_endX - Mathf.Abs(_startY - _endY), _startY);
            }

            if (Mathf.Floor(oldPlayerLineAngle / 45.0f) > Mathf.Floor(playerLineAngle / 45.0f) &&
                !(Mathf.Floor(oldPlayerLineAngle / 45.0f) == 7 && Mathf.Floor(playerLineAngle / 45.0f) == 0))
            {
                bendRight = true;
            }
        }
    }

    /// <summary>
    /// Causes all line renderers to have their data updated.
    /// </summary>
    public void UpdateRenderer()
    {
        List<Vector3> _allPointPositions = new List<Vector3>();
        for (int _i = 0; _i < linePoints.Length; ++_i)
        {
            for (int _j = 0; _j < linePoints[_i].Count; ++_j)
            {
                if (linePoints[_i][_j] == null)
                {
                    // Null found, use mouse position instead.
                    _allPointPositions.Add(mouseData.Position);
                }
                else _allPointPositions.Add(linePoints[_i][_j].TiedStation.StationTruePosition);

                if (_j < linePoints[_i].Count - 1) _allPointPositions.Add(bendPositions[_i][_j]);
            }
            lineRenderers[_i].positionCount = _allPointPositions.Count;

            lineRenderers[_i].SetPositions(_allPointPositions.ToArray());
            _allPointPositions.Clear();
        }
    }

    /*public Point AddPoint(int _lineIndex, Station _tiedStation)
    {

    }*/
    #endregion
}

#region Station Spawning
public class StationGridReference
{
    #region FIELDS

    private bool used;
    private Vector2 truePosition;
    private Vector2Int gridPosition;
    private Vector2Int removalPosition;

    #endregion

    #region PROPERTIES

    /// <summary>
    ///     The x position in the scene.
    /// </summary>
    public float TruePositionX
    {
        get { return truePosition.x; }
    }

    /// <summary>
    ///     The y position in the scene.
    /// </summary>
    public float TruePositionY
    {
        get { return truePosition.y; }
    }

    /// <summary>
    ///     The x position in the 'trueGrid' of the StationGrid.
    /// </summary>
    public int GridPositionX
    {
        get { return gridPosition.x; }
    }

    /// <summary>
    ///     The y position in the 'trueGrid' of the StationGrid.
    /// </summary>
    public int GridPositionY
    {
        get { return gridPosition.y; }
    }

    /// <summary>
    ///     The x position in the 'removalList' of the StationGrid.
    /// </summary>
    public int RemovalPositionX
    {
        get { return removalPosition.x; }
        set { removalPosition.x = value; }
    }

    /// <summary>
    ///     The y position in the 'removalList' of the StationGrid.
    /// </summary>
    public int RemovalPositionY
    {
        get { return removalPosition.y; }
        set { removalPosition.y = value; }
    }

    /// <summary>
    ///     Whether or not this station slot can still be used.
    /// </summary>
    public bool Used
    {
        get { return used; }
        set { used = value; }
    }

    #endregion

    #region CONSTRUCTORS

    /// <summary>
    ///     Creates an instance specifically to store information on where stations should be placed.
    /// </summary>
    /// <param name="_position">The actual location this station should be placed at.</param>
    /// <param name="_truePosition">The position in the 'trueGrid'.</param>
    /// <param name="_removalPosition">The position in the 'removalGrid'.</param>
    public StationGridReference(Vector2 _truePosition, Vector2Int _gridPosition, Vector2Int _removalPosition)
    {
        truePosition = _truePosition;
        gridPosition = _gridPosition;
        removalPosition = _removalPosition;
    }

    #endregion

    #region METHODS

    // Currently empty.

    #endregion
}

public class StationGrid
{
    #region FIELDS
    private bool debugMode;
    private CameraData cameraData;
    private float distanceBetweenGridIndices;
    private float stationPlacementScreenPercent;
    private float stationBoundaryRadius;

    private StationGridReference[,] trueGrid;
    private Vector2Int trueGridDimensions;
    private List<List<StationGridReference>> removalGrid;
    private List<int> removalGridRemainingXPositions;

    private int startingStationTotal;
    private int stationMaximumTotal;
    private List<Station> stations;
    private List<Station>[] shapeStations;
    #endregion

    #region PROPERTIES

    /// <summary>
    ///     Whether or not the StationGrid is in debug mode for visualizing station spawning.
    /// </summary>
    public bool DebugMode { get { return debugMode; } set { debugMode = value; } }

    /// <summary>
    ///     Returns the dimensions of the 'trueGrid'.
    /// </summary>
    public Vector2Int TrueGridDimensions { get { return trueGridDimensions; } }

    #region STATION SPAWN AREA WITHIN CAMERA
    /// <summary>
    ///     The left edge of the current camera view where stations can spawn.
    /// </summary>
    public float StationSpawnLeft
    {
        get
        {
            return cameraData.Left + cameraData.Width * (1.0f - stationPlacementScreenPercent) * 0.5f;
        }
    }

    /// <summary>
    ///     The right edge of the current camera view where stations can spawn.
    /// </summary>
    public float StationSpawnRight
    {
        get
        {
            return cameraData.Right - cameraData.Width * (1.0f - stationPlacementScreenPercent) * 0.5f;
        }
    }

    /// <summary>
    ///     The top edge of the current camera view where stations can spawn.
    /// </summary>
    public float StationSpawnTop
    {
        get
        {
            return cameraData.Top - cameraData.Height * (1.0f - stationPlacementScreenPercent) * 0.5f;
        }
    }

    /// <summary>
    ///     The bottom edge of the current camera view where stations can spawn.
    /// </summary>
    public float StationSpawnBottom
    {
        get
        {
            return cameraData.Bottom + cameraData.Height * (1.0f - stationPlacementScreenPercent) * 0.5f;
        }
    }
    #endregion
    #endregion

    #region CONSTRUCTORS

    /// <summary>
    ///     A submanager which handles stations and their placement within the current camera.
    /// </summary>
    /// <param name="_cameraData">The camera information manager.</param>
    /// <param name="_distanceBetweenGridIndices">How much space in scene should be between each index in the grid.</param>
    /// <param name="_startingStationTotal">How many stations should there be at game start.</param>
    /// <param name="_stationMaximumTotal">The maximum number of stations that can exist.</param>
    /// <param name="_stationPlacementScreenPercent">The percentage of padding around camera's edges to prevent station spawning.</param>
    /// <param name="_stationBoundaryRadius">The radius around a station to block other stations from spawning in.</param>
    /// <param name="_stationPrefab">The basic station prefab to pull from.</param>
    /// <param name="_stationSprites">Array of sprites for stations.</param>
    /// <param name="_stations">A list that contains all statons, is shared in main.</param>
    public StationGrid (
        CameraData _cameraData, float _distanceBetweenGridIndices,
        int _startingStationTotal, int _stationMaximumTotal,
        float _stationPlacementScreenPercent, float _stationBoundaryRadius,
        List<Station> _stations)
    {
        // STEP 1:
        // Insert data into generic fields.
        debugMode = false;
        cameraData = _cameraData;
        distanceBetweenGridIndices = _distanceBetweenGridIndices;
        startingStationTotal = _startingStationTotal;
        stationMaximumTotal = _stationMaximumTotal;
        stationPlacementScreenPercent = _stationPlacementScreenPercent;
        stationBoundaryRadius = _stationBoundaryRadius;
        shapeStations = new List<Station>[(int)STATION_SHAPE.LENGTH];
        for (int _i = 0; _i < shapeStations.Length; ++_i)
        {
            shapeStations[_i] = new List<Station>();
        }
        stations = _stations;

        // STEP 2:
        // Get the grid dimensions.
        trueGridDimensions = new Vector2Int(
            (int)(cameraData.MaxWidth * _stationPlacementScreenPercent / distanceBetweenGridIndices),
            (int)(cameraData.MaxHeight * _stationPlacementScreenPercent / distanceBetweenGridIndices));

        // STEP 3:
        // Create the two grids the fill them piecemail.
        trueGrid = new StationGridReference[trueGridDimensions.x, trueGridDimensions.y];
        removalGrid = new List<List<StationGridReference>>();
        for (int _x = 0; _x < trueGridDimensions.x; ++_x)
        {
            // Add slot to 'removalGrid'
            removalGrid.Add(new List<StationGridReference>());

            for (int _y = 0; _y < trueGridDimensions.y; ++_y)
            {
                // Create StationGridReference
                StationGridReference _station = new StationGridReference(
                    new Vector2(
                        cameraData.MaxLeft
                        + cameraData.MaxWidth * (1.0f - stationPlacementScreenPercent) * 0.5f
                        + cameraData.MaxWidth * _stationPlacementScreenPercent
                        * ((_x + 0.5f) / (float)(trueGridDimensions.x)),
                        cameraData.MaxBottom
                        + cameraData.MaxHeight * (1.0f - stationPlacementScreenPercent) * 0.5f
                        + cameraData.MaxHeight * _stationPlacementScreenPercent
                        * ((_y + 0.5f) / (float)(trueGridDimensions.y))),
                    new Vector2Int(_x, _y),
                    new Vector2Int(0, 0)); // Fixes itself whenever GridRemoveUsed() runs.

                // Assign to 'trueGrid'.
                trueGrid[_x, _y] = _station;

                // Check to see if positions overlap with river lines.
                // TODO - finish rivers first, just set them to Used, don't remove that is done below.

                // Assign to 'removalGrid'.
                removalGrid[_x].Add(_station);
            }
        }

        // STEP 4:
        // Remove all used slots & update positions.
        GridRemoveUsed();
    }

    #endregion

    #region METHODS
    /// <summary>
    ///     Returns a StationGridReference at a given position in the 'trueGrid'.
    /// </summary>
    /// <param name="_x">X position in true grid to pull from.</param>
    /// <param name="_y">Y position in true grid to pull from.</param>
    /// <returns></returns>
    public StationGridReference GetStationGridReference(int _x, int _y)
    {
        return trueGrid[_x, _y];
    }

    /// <summary>
    ///     Removes used indices and updates positions on those after.
    /// </summary>
    private void GridRemoveUsed()
    {
        for (int _x = 0; _x < removalGrid.Count; ++_x)
        {
            for (int _y = 0; _y < removalGrid[_x].Count; ++_y)
            {
                if (removalGrid[_x][_y].Used)
                {
                    // Remove from the grid.
                    removalGrid[_x].RemoveAt(_y);
                    --_y;
                }
                else
                {
                    // Update its position
                    removalGrid[_x][_y].RemovalPositionX = _x;
                    removalGrid[_x][_y].RemovalPositionY = _y;
                }
            }

            if (removalGrid[_x].Count == 0)
            {
                removalGrid.RemoveAt(_x);
                --_x;
            }
        }
    }

    /// <summary>
    ///     Creates a station and places it somewhere on the currently visible screen.
    /// </summary>
    public Station CreateStation()
    {
        // STEP 1:
        // Make sure a station can still be added.
        if (removalGrid.Count == 0 || stations.Count == stationMaximumTotal) return null;

        // STEP 2:
        // Get all slots that are within current camera view.
        List<StationGridReference> _available = new List<StationGridReference>();
        for (int _x = 0; _x < removalGrid.Count; ++_x)
        {
            for (int _y = 0; _y < removalGrid[_x].Count; ++_y)
            {
                if (removalGrid[_x][_y].TruePositionX
                    > StationSpawnLeft &&
                    removalGrid[_x][_y].TruePositionX
                    < StationSpawnRight &&
                    removalGrid[_x][_y].TruePositionY
                    > StationSpawnBottom &&
                    removalGrid[_x][_y].TruePositionY
                    < StationSpawnTop)
                {
                    _available.Add(removalGrid[_x][_y]);
                }
            }
        }

        // STEP 3:
        // If none are available cancel here.
        if (_available.Count == 0) return null;

        // STEP 4:
        // Pick and create random station.
        StationGridReference _station = _available[Random.Range(0, _available.Count)];
        _station.Used = true;
        int _stationShape = Random.Range(0, (int)STATION_SHAPE.LENGTH);
        Station _stationObject = new Station(new Vector2(_station.TruePositionX, _station.TruePositionY), _stationShape);
        stations.Add(_stationObject);
        shapeStations[_stationShape].Add(_stationObject);

        // STEP 5:
        // Calculate edges of relevance to check within.
        int _leftIndex = -1;
        int _rightIndex = trueGridDimensions.x;
        for (int _x = 0; _x < trueGridDimensions.x; ++_x)
        {
            if (_leftIndex == -1 &&
                trueGrid[_x, 0].TruePositionX > _station.TruePositionX - stationBoundaryRadius)
            {
                _leftIndex = _x;
            }

            if (trueGrid[_x, 0].TruePositionX > _station.TruePositionX + stationBoundaryRadius)
            {
                _rightIndex = _x;
                break;
            }
        }

        int _bottomIndex = -1;
        int _topIndex = trueGridDimensions.y;
        for (int _y = 0; _y < trueGridDimensions.y; ++_y)
        {
            if (_bottomIndex == -1 &&
                trueGrid[0, _y].TruePositionY > _station.TruePositionY - stationBoundaryRadius)
            {
                _bottomIndex = _y;
            }

            if (trueGrid[0, _y].TruePositionY > _station.TruePositionY + stationBoundaryRadius)
            {
                _topIndex = _y;
                break;
            }
        }

        // STEP 6:
        // Make all possible station positions within radius used.
        for (int _x = _leftIndex; _x < _rightIndex; ++_x)
        {
            for (int _y = _bottomIndex; _y < _topIndex; ++_y)
            {
                if (!trueGrid[_x, _y].Used &&
                    Mathf.Pow(trueGrid[_x, _y].TruePositionX - _station.TruePositionX, 2)
                    + Mathf.Pow(trueGrid[_x, _y].TruePositionY - _station.TruePositionY, 2)
                    < Mathf.Pow(stationBoundaryRadius, 2))
                {
                    // Station position within boundary radius and should be made unusable.
                    trueGrid[_x, _y].Used = true;
                }
            }
        }

        // STEP 7:
        // Remove all used slots.
        GridRemoveUsed();

        // STEP 8:
        // Return the station so its game object can be made in the main class.
        return _stationObject;
    }

    /// <summary>
    ///     Returns an existing Station class instance.
    /// </summary>
    /// <param name="_index">The index of the Station to be returned.</param>
    /// <returns></returns>
    public Station GetStation(int _index)
    {
        return stations[_index];
    }
    #endregion
}
#endregion

public class Manager : MonoBehaviour
{
    #region STATIC INSTANCE

    public static Manager Instance { get; private set; }

    public StationManager stationManager; // Drag-and-drop this in the Inspector


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Multiple Manager instances found!");
            Destroy(gameObject);
        }
    }

    #endregion

    #region FIELDS

    // Prefabs.
    [SerializeField] private GameObject stationPrefab;
    [SerializeField] private GameObject lineRendererPrefab;
    [SerializeField] private Sprite[] stationSprites = new Sprite[(int)STATION_SHAPE.LENGTH];

    // Basic System Storage.
    private MouseData mouseData;
    private List<Timer> timers;
    private CameraData cameraData;
    [SerializeField] private float maxCameraTime = 600f;
    private Timer cameraZoomOutTime;

    //Station Manager
    [SerializeField] private int startingStationTotal = 3;
    [SerializeField] private int stationMaximumTotal = 20;
    [SerializeField] private float timeBetweenStationSpawns = 15f;
    private Timer stationSpawnTimer;
    private StationGrid stationGrid;

    // Station Handling
    private List<Station> stations;

    // Line Manger
    private LineRenderer[] lineRenderers;
    private LineManager lineManager;

    // Game Pausing.
    private bool isGameRunning = false;
    public bool isGamePaused = false; // Add this to track pause state

    // Train Resources
    public int availableTrains = 3; // Example starting value
    #endregion

    #region UNITY MONOBEHAVIOR
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
{
    // STEP 1:
    // Insert data into generic fields.
    isGameRunning = true;
    isGamePaused = false; // Initialize pause state
    mouseData = new MouseData();
    cameraData = new CameraData(Camera.main, 5.0f, 10.0f);
    cameraData.DebugMode = true;
    timers = new List<Timer>();
    stations = new List<Station>();
    stationGrid = new StationGrid(cameraData, 1.0f, startingStationTotal, stationMaximumTotal, 0.9f, 3.0f, stations);
    stationGrid.DebugMode = true;

    // STEP 2:
    // Assign stationManager from the scene (assuming it's attached to this GameObject or another GameObject)
    stationManager = GetComponent<StationManager>(); // or reference it if it's on another GameObject

    // Check if stationManager was found (to avoid NullReferenceException)
    if (stationManager == null)
    {
        Debug.LogError("StationManager is not assigned or found!");
        return; // Exit early if stationManager is missing
    }

    // STEP 3:
    // Create starting stations.
    for (int _i = 0; _i < startingStationTotal; ++_i)
    {
        stationManager.CreateStation(); // Call CreateStation on stationManager
    }

    // STEP 4:
    // Set variables for camera size change and station spawning.
    cameraZoomOutTime = CreateTimer(maxCameraTime, true);
    stationSpawnTimer = CreateTimer(timeBetweenStationSpawns, false);

    // STEP 5:
    // Create the line manager.
    lineRenderers = new LineRenderer[8];
    for (int _i = 0; _i < lineRenderers.Length; ++_i)
    {
        lineRenderers[_i] = Instantiate(lineRendererPrefab).GetComponent<LineRenderer>();
        switch (_i)
        {
            case 0:
                lineRenderers[_i].startColor = Color.yellow;
                lineRenderers[_i].endColor = Color.yellow;
                break;

            case 1:
                lineRenderers[_i].startColor = Color.red;
                lineRenderers[_i].endColor = Color.red;
                break;

            case 2:
                lineRenderers[_i].startColor = Color.blue;
                lineRenderers[_i].endColor = Color.blue;
                break;
        }
    }
    lineManager = new LineManager(mouseData, stations, lineRenderers);
}


    // Update is called once per frame
    private void Update()
{
    mouseData.SetData();

    if (!isGamePaused) // Only update game elements if not paused
    {
        // STEP 1:
        // Increment all existing Timer class instances.
        foreach (Timer timer in timers)
        {
            timer.IncrementTimer(Time.deltaTime);
        }

        // STEP 2:
        // TODO - Camera scaling and spawing of stations over time.
        cameraData.UpdateCameraSize(cameraZoomOutTime.TimerPercentage);
        if (stationSpawnTimer.Trigger)
        {
            stationManager.CreateStation(); // Call CreateStation on stationManager
        }
    }

    // STEP 3:
    // Handle player input for line manager
    lineManager.PlayerInput();
}


    #endregion

    #region METHODS


    /// <summary>
    ///     Creates a new Timer and adds it to the timers list
    /// </summary>
    /// <param name="_length"> Length of the timer. </param>
    /// <param name="_runsOnce"> Whether or not the timer will run once or will loop. </param>
    /// <returns> Returns the newly created Timer. </returns>
    private Timer CreateTimer(float _length, bool _runsOnce)
    {
        Timer _timer = new Timer(_length, _runsOnce);
        timers.Add(_timer);
        return _timer;
    }

    #region UI CONTENT

    // Methods to control game state from UI
    public void PauseGame()
    {
        isGamePaused = true;
        Time.timeScale = 0f; // Stop time for game elements
        Debug.Log("Game Paused");
    }

    public void ResumeGame()
    {
        isGamePaused = false;
        Time.timeScale = 1f; // Resume normal time
        Debug.Log("Game Resumed");
    }

    public void ExitGame()
    {
                Debug.Log("Exiting Game...");
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }

    public void SelectColor(int colorIndex)
    {
        Debug.Log("Selected color index: " + colorIndex);
        // Implement logic for selecting this color for line drawing
    }

    public void SelectTunnel()
    {
        Debug.Log("Selected Tunnel");
        // Implement logic for selecting the tunnel tool
    }

    public void SelectTrain()
    {
        Debug.Log("Selected Train");
        // Implement logic for selecting the train tool
    }

    /// <summary>
    ///     Sets the game speed.
    /// </summary>
    /// <param name="speed">The speed multiplier (1f = normal, 2f = double speed, etc.).</param>
    public void SetGameSpeed(float speed)
    {
        Time.timeScale = speed;
        Debug.Log("Game speed set to: " + speed);
    }

    // Method to get available train count
    public int GetAvailableTrains()
    {
        return availableTrains;
    }

    // Method to reduce available train count
    public void UseTrain()
    {
        availableTrains--;
        if (availableTrains < 0)
        {
            availableTrains = 0; // Prevent going below zero
        }

        Debug.Log("Train used. Available trains: " + availableTrains);
    }

    // Method to increase available train count (if needed)
    public void AddTrain(int amount)
    {
        availableTrains += amount;
        Debug.Log("Trains added. Available trains: " + availableTrains);
    }
    #endregion
    
    #region GIZMOS
    private void OnDrawGizmos()
    {
        if (!isGameRunning) return;

        // Show camera size.
        if (cameraData.DebugMode)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawLine(
                new Vector3(cameraData.Left, cameraData.Top, 0),
                new Vector3(cameraData.Left, cameraData.Bottom, 0));
            Gizmos.DrawLine(
                new Vector3(cameraData.Right, cameraData.Top, 0),
                new Vector3(cameraData.Right, cameraData.Bottom, 0));
            Gizmos.DrawLine(
                new Vector3(cameraData.Left, cameraData.Top, 0),
                new Vector3(cameraData.Right, cameraData.Top, 0));
            Gizmos.DrawLine(
                new Vector3(cameraData.Left, cameraData.Bottom, 0),
                new Vector3(cameraData.Right, cameraData.Bottom, 0));
        }

        if (stationGrid.DebugMode)
        {
            // Draw spawning screen space
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(
                new Vector3(stationGrid.StationSpawnLeft, stationGrid.StationSpawnTop, 0),
                new Vector3(stationGrid.StationSpawnLeft, stationGrid.StationSpawnBottom, 0));
            Gizmos.DrawLine(
                new Vector3(stationGrid.StationSpawnRight, stationGrid.StationSpawnTop, 0),
                new Vector3(stationGrid.StationSpawnRight, stationGrid.StationSpawnBottom, 0));
            Gizmos.DrawLine(
                new Vector3(stationGrid.StationSpawnLeft, stationGrid.StationSpawnTop, 0),
                new Vector3(stationGrid.StationSpawnRight, stationGrid.StationSpawnTop, 0));
            Gizmos.DrawLine(
                new Vector3(stationGrid.StationSpawnLeft, stationGrid.StationSpawnBottom, 0),
                new Vector3(stationGrid.StationSpawnRight, stationGrid.StationSpawnBottom, 0));

            // Show station spawn positions.
            for (int _x = 0; _x < stationGrid.TrueGridDimensions.x; ++_x)
            {
                for (int _y = 0; _y < stationGrid.TrueGridDimensions.y; ++_y)
                {
                    if (stationGrid.GetStationGridReference(_x, _y).Used)
                    {
                        Gizmos.color = Color.red;
                    }
                    else if (stationGrid.GetStationGridReference(_x, _y).TruePositionX
                             < stationGrid.StationSpawnLeft ||
                             stationGrid.GetStationGridReference(_x, _y).TruePositionX
                             > stationGrid.StationSpawnRight ||
                             stationGrid.GetStationGridReference(_x, _y).TruePositionY
                             < stationGrid.StationSpawnBottom ||
                             stationGrid.GetStationGridReference(_x, _y).TruePositionY
                             > stationGrid.StationSpawnTop)
                    {
                        Gizmos.color = Color.yellow;
                    }
                    else
                    {
                        Gizmos.color = Color.green;
                    }

                    Gizmos.DrawSphere(new Vector3(
                                             stationGrid.GetStationGridReference(_x, _y).TruePositionX,
                                             stationGrid.GetStationGridReference(_x, _y).TruePositionY,
                                             0), 0.15f);
                }
            }
        }

        Gizmos.color = Color.white;
        for (int _i = 0; _i < stations.Count; _i++)
        {
            if (stations[_i].DebugMode)
            {
                for (int _j = 0; _j < 8; ++_j)
                {
                    Gizmos.DrawSphere(stations[_i].AccessConnections[_j, 0], 0.05f);
                    Gizmos.DrawSphere(stations[_i].AccessConnections[_j, 1], 0.05f);
                    Gizmos.DrawSphere(stations[_i].AccessConnections[_j, 2], 0.05f);
                }
            }
        }
    }
    #endregion
    #endregion
}