using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace BlackAle.Core
{
    public class GridSystem : MonoBehaviour
    {
        [Header("Grid Settings")]
        [SerializeField] private int gridWidth = 16;
        [SerializeField] private int gridHeight = 10;
        [SerializeField] private float cellSize = 56f;
        [SerializeField] private float dotSize = 4f;

        [Header("References")]
        [SerializeField] private RectTransform gridContainer;
        [SerializeField] private GameObject dotPrefab;

        private bool[,] _occupied;
        private Dictionary<Vector2Int, GameObject> _dotObjects = new Dictionary<Vector2Int, GameObject>();

        public int Width => gridWidth;
        public int Height => gridHeight;
        public float CellSize => cellSize;

        public void Initialize(int width, int height)
        {
            gridWidth = width;
            gridHeight = height;
            _occupied = new bool[gridWidth, gridHeight];
            CreateDotMatrix();
        }

        private void CreateDotMatrix()
        {
            // Clear existing dots
            foreach (var dot in _dotObjects.Values)
            {
                if (dot != null) Destroy(dot);
            }
            _dotObjects.Clear();

            for (int y = 0; y < gridHeight; y++)
            {
                for (int x = 0; x < gridWidth; x++)
                {
                    GameObject dot;
                    if (dotPrefab != null)
                    {
                        dot = Instantiate(dotPrefab, gridContainer);
                    }
                    else
                    {
                        dot = CreateDot();
                    }

                    dot.name = $"Dot_{x}_{y}";
                    RectTransform rt = dot.GetComponent<RectTransform>();
                    rt.anchoredPosition = GridToLocalPosition(x, y);
                    rt.sizeDelta = new Vector2(dotSize, dotSize);

                    _dotObjects[new Vector2Int(x, y)] = dot;
                }
            }
        }

        private GameObject CreateDot()
        {
            GameObject dot = new GameObject("Dot", typeof(RectTransform), typeof(Image));
            dot.transform.SetParent(gridContainer, false);
            Image img = dot.GetComponent<Image>();
            img.color = new Color(0.2f, 0.2f, 0.2f, 1f); // Dark grey
            img.raycastTarget = false;
            return dot;
        }

        public Vector2 GridToLocalPosition(int x, int y)
        {
            float startX = -(gridWidth - 1) * cellSize * 0.5f;
            float startY = (gridHeight - 1) * cellSize * 0.5f;
            return new Vector2(startX + x * cellSize, startY - y * cellSize);
        }

        public Vector2Int ScreenToGrid(Vector2 screenPos)
        {
            Vector2 localPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                gridContainer, screenPos, null, out localPos);

            float startX = -(gridWidth - 1) * cellSize * 0.5f;
            float startY = (gridHeight - 1) * cellSize * 0.5f;

            int gx = Mathf.RoundToInt((localPos.x - startX) / cellSize);
            int gy = Mathf.RoundToInt((startY - localPos.y) / cellSize);

            gx = Mathf.Clamp(gx, 0, gridWidth - 1);
            gy = Mathf.Clamp(gy, 0, gridHeight - 1);

            return new Vector2Int(gx, gy);
        }

        public bool IsOccupied(int x, int y)
        {
            if (x < 0 || x >= gridWidth || y < 0 || y >= gridHeight) return true;
            return _occupied[x, y];
        }

        public bool IsOccupied(Vector2Int pos) => IsOccupied(pos.x, pos.y);

        public void SetOccupied(int x, int y, bool occupied)
        {
            if (x >= 0 && x < gridWidth && y >= 0 && y < gridHeight)
                _occupied[x, y] = occupied;
        }

        public void SetOccupied(Vector2Int pos, bool occupied) => SetOccupied(pos.x, pos.y, occupied);

        public bool IsInBounds(int x, int y)
        {
            return x >= 0 && x < gridWidth && y >= 0 && y < gridHeight;
        }

        public bool IsInBounds(Vector2Int pos) => IsInBounds(pos.x, pos.y);

        public bool IsAdjacent(Vector2Int a, Vector2Int b)
        {
            int dx = Mathf.Abs(a.x - b.x);
            int dy = Mathf.Abs(a.y - b.y);
            return dx <= 1 && dy <= 1 && !(dx == 0 && dy == 0);
        }

        // A* Pathfinding
        public List<Vector2Int> FindPath(Vector2Int start, Vector2Int end)
        {
            if (!IsInBounds(end) || IsOccupied(end))
                return null;

            var openSet = new List<PathNode>();
            var closedSet = new HashSet<Vector2Int>();
            var cameFrom = new Dictionary<Vector2Int, Vector2Int>();
            var gScore = new Dictionary<Vector2Int, float>();
            var fScore = new Dictionary<Vector2Int, float>();

            gScore[start] = 0;
            fScore[start] = Heuristic(start, end);
            openSet.Add(new PathNode(start, fScore[start]));

            while (openSet.Count > 0)
            {
                // Find node with lowest fScore
                openSet.Sort((a, b) => a.fScore.CompareTo(b.fScore));
                PathNode current = openSet[0];
                openSet.RemoveAt(0);

                if (current.position == end)
                    return ReconstructPath(cameFrom, end);

                closedSet.Add(current.position);

                // Check all 8 neighbors (including diagonals)
                for (int dx = -1; dx <= 1; dx++)
                {
                    for (int dy = -1; dy <= 1; dy++)
                    {
                        if (dx == 0 && dy == 0) continue;

                        Vector2Int neighbor = new Vector2Int(current.position.x + dx, current.position.y + dy);

                        if (!IsInBounds(neighbor) || closedSet.Contains(neighbor) || IsOccupied(neighbor))
                            continue;

                        float tentativeG = gScore[current.position] + (dx != 0 && dy != 0 ? 1.414f : 1f);

                        if (!gScore.ContainsKey(neighbor) || tentativeG < gScore[neighbor])
                        {
                            cameFrom[neighbor] = current.position;
                            gScore[neighbor] = tentativeG;
                            fScore[neighbor] = tentativeG + Heuristic(neighbor, end);

                            if (!openSet.Exists(n => n.position == neighbor))
                                openSet.Add(new PathNode(neighbor, fScore[neighbor]));
                        }
                    }
                }
            }

            return null; // No path found
        }

        private float Heuristic(Vector2Int a, Vector2Int b)
        {
            // Chebyshev distance for 8-directional movement
            return Mathf.Max(Mathf.Abs(a.x - b.x), Mathf.Abs(a.y - b.y));
        }

        private List<Vector2Int> ReconstructPath(Dictionary<Vector2Int, Vector2Int> cameFrom, Vector2Int current)
        {
            var path = new List<Vector2Int> { current };
            while (cameFrom.ContainsKey(current))
            {
                current = cameFrom[current];
                path.Insert(0, current);
            }
            // Remove start position from path
            if (path.Count > 0)
                path.RemoveAt(0);
            return path;
        }

        private struct PathNode
        {
            public Vector2Int position;
            public float fScore;

            public PathNode(Vector2Int pos, float f)
            {
                position = pos;
                fScore = f;
            }
        }
    }
}
