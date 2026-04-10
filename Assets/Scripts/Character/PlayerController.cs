using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BlackAle.Core;

namespace BlackAle.Character
{
    public class PlayerController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private RectTransform characterRect;
        [SerializeField] private TextMeshProUGUI characterLabel;
        [SerializeField] private Image characterImage;
        [SerializeField] private GridSystem gridSystem;

        [Header("Movement")]
        [SerializeField] private float moveSpeed = 8f;
        [SerializeField] private float characterSize = 40f;

        private Vector2Int _gridPosition;
        private List<Vector2Int> _currentPath;
        private int _pathIndex;
        private bool _isMoving;
        private Vector2 _targetLocalPos;

        public Vector2Int GridPosition => _gridPosition;
        public bool IsMoving => _isMoving;

        public void Initialize(GridSystem grid, int startX, int startY)
        {
            gridSystem = grid;
            _gridPosition = new Vector2Int(startX, startY);

            if (characterRect == null)
                characterRect = GetComponent<RectTransform>();

            characterRect.sizeDelta = new Vector2(characterSize, characterSize);
            characterRect.anchoredPosition = gridSystem.GridToLocalPosition(startX, startY);

            // Set magenta color with "C"
            if (characterImage != null)
                characterImage.color = Color.magenta;
            if (characterLabel != null)
            {
                characterLabel.text = "C";
                characterLabel.color = Color.white;
                characterLabel.alignment = TextAlignmentOptions.Center;
            }
        }

        private void Update()
        {
            if (GameManager.Instance == null) return;
            if (GameManager.Instance.CurrentState != GameState.Playing) return;

            if (_isMoving)
            {
                AnimateMovement();
            }
            else
            {
                HandleArrowKeys();
                HandleClickToMove();
            }
        }

        private void HandleArrowKeys()
        {
            Vector2Int direction = Vector2Int.zero;

            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
                direction = new Vector2Int(0, -1);
            else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
                direction = new Vector2Int(0, 1);
            else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
                direction = new Vector2Int(-1, 0);
            else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
                direction = new Vector2Int(1, 0);

            if (direction != Vector2Int.zero)
            {
                Vector2Int newPos = _gridPosition + direction;
                TryMoveTo(newPos);
            }
        }

        private void HandleClickToMove()
        {
            if (Input.GetMouseButtonDown(0) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                Vector2Int clickedGrid = gridSystem.ScreenToGrid(Input.mousePosition);

                if (clickedGrid != _gridPosition && !gridSystem.IsOccupied(clickedGrid))
                {
                    _currentPath = gridSystem.FindPath(_gridPosition, clickedGrid);
                    if (_currentPath != null && _currentPath.Count > 0)
                    {
                        _pathIndex = 0;
                        StartMoveToNext();
                    }
                }
            }
        }

        private bool TryMoveTo(Vector2Int newPos)
        {
            if (!gridSystem.IsInBounds(newPos) || gridSystem.IsOccupied(newPos))
                return false;

            _currentPath = null;
            _gridPosition = newPos;
            _targetLocalPos = gridSystem.GridToLocalPosition(newPos.x, newPos.y);
            _isMoving = true;
            return true;
        }

        private void StartMoveToNext()
        {
            if (_currentPath == null || _pathIndex >= _currentPath.Count)
            {
                _currentPath = null;
                _isMoving = false;
                return;
            }

            Vector2Int nextPos = _currentPath[_pathIndex];

            // Verify the next position is still valid
            if (gridSystem.IsOccupied(nextPos))
            {
                _currentPath = null;
                _isMoving = false;
                return;
            }

            _gridPosition = nextPos;
            _targetLocalPos = gridSystem.GridToLocalPosition(nextPos.x, nextPos.y);
            _isMoving = true;
            _pathIndex++;
        }

        private void AnimateMovement()
        {
            characterRect.anchoredPosition = Vector2.MoveTowards(
                characterRect.anchoredPosition, _targetLocalPos, moveSpeed * cellUnitsPerSecond * Time.deltaTime);

            if (Vector2.Distance(characterRect.anchoredPosition, _targetLocalPos) < 0.5f)
            {
                characterRect.anchoredPosition = _targetLocalPos;
                _isMoving = false;

                // Continue path if exists
                if (_currentPath != null && _pathIndex < _currentPath.Count)
                {
                    StartMoveToNext();
                }
            }
        }

        private float cellUnitsPerSecond => gridSystem.CellSize;
    }
}
