using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class Board : MonoBehaviour {
    public GameObject squarePrefab;
    public GameObject gridCellPrefab;
    public GameObject squareGhostPrefab;
    public GameObject goalMarkerPrefab;
    public GameObject deleteGhostPrefab;
    public GameObject goalGhostPrefab;

    public List<GameObject> goalMarkers;
    public static Dictionary<string, Square> placedSquares;
    


    private GameObject goalMarkerGhost;
    private GameObject squareGhost;
    private GameObject deleteGhost;

    Square startingSquare;
    public static Square[,] squaresOnBoard;
    Hand hand;

    private static Game game;


    public void SetUp()
    {
        goalMarkers = new List<GameObject>();
        squaresOnBoard = new Square[Game.BoardSize, Game.BoardSize];
        hand = GameObject.Find("Hand").GetComponent<Hand>();
        game = GameObject.Find("Game").GetComponent<Game>();
    }

    public void SetUpPlay() {
        Destroy(goalMarkerGhost);
        Destroy(squareGhost);
        Destroy(deleteGhost);
    }

   
    public void SetUpEdit()
    {
        
        goalMarkerGhost = Instantiate(goalGhostPrefab, transform.position, Quaternion.identity);
        goalMarkerGhost.transform.localScale = Game.Scale2D();

        squareGhost = Instantiate(squareGhostPrefab, transform.position, Quaternion.identity);
        squareGhost.transform.localScale = Game.Scale2D()/2;

        deleteGhost = Instantiate(deleteGhostPrefab, transform.position, Quaternion.identity);
        deleteGhost.transform.localScale = Game.Scale2D()/2;
    }

    public IntPair GoalMarkerCoords(int index)
    {
        int[] coords = GetGridCoordinates(goalMarkers[index].transform.position);
        return new IntPair(coords[0], coords[1]);
    }

    //Converts a square's position into two-dimensional array coordinates, centered at [Game.BoardSize/2, Game.BoardSize/2]
    public static int[] GetGridCoordinates(Vector2 position)
    {
        int[] coords = new int[2];
      
        //Round to the nearest int based on the ratio of position to grid square size
        int a = Mathf.RoundToInt(position.x / (Game.Scale*2));
        int b = Mathf.RoundToInt(position.y / (Game.Scale*2));

        a += Game.BoardSize / 2;
        b += Game.BoardSize / 2;

        //Clamp a and b vals to prevent index out of bounds exceptions.
        //Binds the grid coordinates within the SquaresOnBoard array.
        coords[0] = Clamp(a);
        coords[1] = Clamp(b);
        
        return coords;
    }

    //Converts the grid coordinates back to a grid position, centered on where the grid cell would be. 
    //This enables the "Snap to Grid" functionality in edit and play mode.
    public static Vector3 GetGridPosition(int[] coords)
    {
        return new Vector3((coords[0] - Game.BoardSize / 2) * Game.Scale * 2,
            (coords[1] - Game.BoardSize / 2) * Game.Scale * 2, 0);
    }


    //Forces a value to stay within the bounds of the SquaresOnBoard array.
    private static int Clamp(int val)
    {
        if (val >= Game.BoardSize)
        {
            val = Game.BoardSize - 1;
        }

        if (val < 0)
        {
            val = 0;
        }
        return val;
    }

   

    public void PlaceOnBoard()
    {
        int[] touchCoords = GetGridCoordinates(Game.worldTouchPosition);
        FindSquarePlacement(touchCoords);

    }


   

    public void ClearBoard()
    {
        if (placedSquares != null)
        {
            foreach (KeyValuePair<string, Square> pair in placedSquares)
            {
                
                Square s = pair.Value;
                if (s != null)
                {
                    if (squaresOnBoard != null)
                    {
                        squaresOnBoard[s.coords[0], s.coords[1]] = null;
                    }

                    Destroy(s.gameObject);
                }
            }

            placedSquares.Clear();
        }
        else
        {
            placedSquares = new Dictionary<string, Square>();
        }


        for(int i = 0; i < goalMarkers.Count; i++)
        {
            Destroy(goalMarkers[i]);
        }

        

        goalMarkers.Clear();
    }


    public void UpdateBoardData(Board_Data data)
    {
        ClearBoard();


        for(int i = 0; i < data.goalSquareCoords.Count; i++)
        {
            int[] coords = {data.goalSquareCoords[i].x, data.goalSquareCoords[i].y};
            GameObject goalMarker = Instantiate(goalMarkerPrefab, GetGridPosition(coords), Quaternion.identity);
            goalMarker.transform.localScale = new Vector3(Game.Scale, Game.Scale, 1);
            goalMarker.transform.SetParent(transform);
            goalMarkers.Add(goalMarker);
        }

        for(int i = 0; i < data.boardSquares.Count; i++)
        {
            Square_Data squareData = data.boardSquares[i];

            //Adjust for new scale if the scale has changed
            Vector3 squarePosition = GetGridPosition(squareData.coords);
            squarePosition.z = squareData.position.z;


            Square s = Instantiate(squarePrefab, squarePosition, Quaternion.identity).GetComponent<Square>();
            s.SetUpCells();
            
            s.UpdateData(squareData);
            s.transform.SetParent(transform);
            s.PlaceSquareOnBoard += PlaceSquare;
            AddSquareToBoard(s, false);

        }
    }



    public void EditLevel()
    {
        int[] coords = GetGridCoordinates(Game.worldTouchPosition);
        Vector3 placement = GetGridPosition(coords);
        if (Input.GetKey(KeyCode.G))
        {
                goalMarkerGhost.SetActive(true);
                goalMarkerGhost.transform.position = placement;
          

            if (Input.GetMouseButtonDown(0))
            {
                Vector3 goalSpawnPos = placement;
                goalSpawnPos.z += 0.2f;
                GameObject goalMarker = Instantiate(goalMarkerPrefab, goalSpawnPos, Quaternion.identity);
                goalMarker.transform.localScale = Game.Scale2D();
                goalMarker.transform.parent = transform;
                goalMarkers.Add(goalMarker);
            }
        }else if (Input.GetKey(KeyCode.S))
        {
                squareGhost.SetActive(true);
                squareGhost.transform.position = placement;
           

            if (Input.GetMouseButtonDown(0))
            {
                if (!placedSquares.ContainsKey(Game.CoordsToString(coords)))
                {
                    Square square = Instantiate(squarePrefab, placement, Quaternion.identity).GetComponent<Square>();
                    square.transform.SetParent(transform);
                    square.coords = coords;
                    AddSquareToBoard(square, false);
                }
            }
        }else if (Input.GetKey(KeyCode.D))
        {
            deleteGhost.SetActive(true);
            deleteGhost.transform.position = placement;

            if (Input.GetMouseButtonDown(0))
            {
                Delete_Mouseover();
            }

        }


        if (!Input.GetKey(KeyCode.D)){
            deleteGhost.SetActive(false);
        }

        if (!Input.GetKey(KeyCode.S))
        {
            squareGhost.SetActive(false);
        }

        if(!Input.GetKey(KeyCode.G))
        {
            goalMarkerGhost.SetActive(false);
        }
    }



    private void Delete_Mouseover()
    {
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //Translate ray behind the camera to make sure we hit the thing
        worldPoint.z -= 10;
        Ray ray = new Ray(worldPoint, Vector3.forward);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit))
        {
            if(hit.collider.tag == "Square")
            {
                Square s = hit.collider.gameObject.GetComponent<Square>();
                
                placedSquares.Remove(Game.CoordsToString(s.coords));
                Destroy(hit.collider.gameObject);
            }
            else if(hit.collider.tag == "Goal")
            {
                
                goalMarkers.Remove(hit.collider.gameObject);
                Destroy(hit.collider.gameObject);
            }

            
        }

    }


    public void Cleanup()
    {
        foreach(KeyValuePair<string, Square> pair in placedSquares)
        {
            pair.Value.highlighted = false;
        }
    }


    public static List<Square> GetNeighborSquares(int[] coords)
    {
        int right = coords[0] + 1;
        right = Clamp(right);

        int up = coords[1] + 1;
        up = Clamp(up);

        int left = coords[0] - 1;
        left = Clamp(left);

        int down = coords[1] - 1;
        down = Clamp(down);


        Square rightSquare = squaresOnBoard[right, coords[1]];
        Square upSquare = squaresOnBoard[coords[0], up];
        Square leftSquare = squaresOnBoard[left, coords[1]];
        Square downSquare = squaresOnBoard[coords[0], down];

        return new List<Square>
         {
                rightSquare,
                upSquare,
                leftSquare,
                downSquare

         };
    }

    private void FindSquarePlacement(int[] coords)
    {

        Color c = Color.magenta;
        Color c2 = Color.white;

        if (!Game.SelectedSquare.rotating) {

            Cleanup();
        }
       
  
        //The zero square is the position on the board we are currently at. If this is null, we are not overlapping any square.
        Square zeroSquare = squaresOnBoard[coords[0], coords[1]];

        if (zeroSquare != null) {
            //Debug.Log("Distance: " + di)
            if (Vector3.Distance(Game.SelectedSquare.transform.position, zeroSquare.transform.position) > Game.Scale/2)
            {
                zeroSquare = null;
            }
            c2 = Color.red;
        }

        Debug.DrawLine(Game.SelectedSquare.transform.position, GetGridPosition(coords), c2);
        if (zeroSquare == null)
        {

            List<Square> neighbors = GetNeighborSquares(coords);
           

            Game.SelectedSquare.highlighted = false;
            bool filter = true;
            Vector2 placementVector = Vector2.zero;
            Square matchingNeighbor = null;

            if(neighbors[0] == null && neighbors[1] == null && neighbors[2] == null && neighbors[3] == null)
            {
                c = Color.black;
                filter = false;
            }

            for(int i = 0; i < neighbors.Count; i++)
            {
               
                if(neighbors[i] != null)
                {
                    
                    if (!Game.SelectedSquare.Matches(neighbors[i], (MatchDirection)i))
                    {
                        c = Color.green;
                        filter = false;
                    }
                    else
                    {
                        placementVector = Game.MatchOffset((MatchDirection)i);
                        matchingNeighbor = neighbors[i];

                    }
                }
            }
           

            if (filter)
            {
                c = Color.blue;
                
                if (!Game.SelectedSquare.placed && !Game.SelectedSquare.inTray)
                {
                    
                    float dist = Vector2.Distance(Game.SelectedSquare.flatPosition, matchingNeighbor.flatPosition + placementVector);
                   
                    if (dist <= Game.Scale * 1.5f)
                    {
                        c = Color.red;
                        Square s = Game.SelectedSquare;
                        Cleanup();
                        SetHighlight(neighbors, s, true);


                        if (!Game.touching)
                        {
                           
                           
                            hand.RemoveSquare(s);
                            s.placed = true;
                            s.transform.position = matchingNeighbor.flatPosition + placementVector;
                          
                            s.transform.parent = transform;
                            s.Deselect();
                            s.highlighted = false;
                            s.PlaceSquareOnBoard += PlaceSquare;
                            PlaceSquare(s);

                            Cleanup();
                            
                        }
                        
                    }
                   
                }
                
                Debug.DrawLine(Game.SelectedSquare.transform.position, matchingNeighbor.transform.position, c);
            }
            
        }

    }

    public void PlaceSquare(Square s)
    {
        AddSquareToBoard(s, true);

        CheckWin();
    }


    private void AddSquareToBoard(Square s, bool checkAbilities)
    {
        placedSquares.Add(Game.CoordsToString(s.coords), s);
        squaresOnBoard[s.coords[0], s.coords[1]] = s;
        s.placed = true;

        if (checkAbilities) {
            s.CheckNeighborAbilities();

            if (!s.shifting)
            {
                s.AffectNeighborsWithAbility();
            }

        }
    }

    public static void DislodgeSquare(Square s)
    {
        placedSquares.Remove(Game.CoordsToString(s.coords));
        squaresOnBoard[s.coords[0], s.coords[1]] = null;
    }

  
   

    public void SetHighlight(List<Square> neighbors, Square s, bool highlight)
    {
        for (int i = 0; i < neighbors.Count; i++)
        {
            if (neighbors[i] != null)
            {
                
                    neighbors[i].highlighted = highlight;
            }
        }
        s.highlighted = highlight;
    }

    void CheckWin()
    {

        for(int i = 0; i < goalMarkers.Count; i++)
        {
            int[] goalCoords = GetGridCoordinates(goalMarkers[i].transform.position);

            if(squaresOnBoard[goalCoords[0], goalCoords[1]] == null)
            {
                return;
            }
        }

        
        game.WinLevel();
    }
}
