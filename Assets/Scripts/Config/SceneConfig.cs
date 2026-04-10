using System;
using System.Collections.Generic;
using UnityEngine;

namespace BlackAle.Config
{
    [Serializable]
    public class SceneConfig
    {
        public string sceneId;
        public string sceneName;
        public string description;
        public string background;
        public string music;
        public int gridWidth;
        public int gridHeight;
        public GridPosition playerStart;
        public List<ItemConfig> items;
        public List<ExitConfig> exits;
        public string script;
    }

    [Serializable]
    public class ItemConfig
    {
        public string itemId;
        public string name;
        public string description;
        public GridPosition position;
        public ColorConfig color;
        public bool takeable;
        public bool usable;
        public bool combinable;
        public string sprite;
        public string requiredAbility;
    }

    [Serializable]
    public class ExitConfig
    {
        public GridPosition position;
        public string targetScene;
        public GridPosition targetPosition;
        public string description;
    }

    [Serializable]
    public class GridPosition
    {
        public int x;
        public int y;

        public GridPosition() { }

        public GridPosition(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public Vector2Int ToVector2Int() => new Vector2Int(x, y);

        public override bool Equals(object obj)
        {
            if (obj is GridPosition other)
                return x == other.x && y == other.y;
            return false;
        }

        public override int GetHashCode() => HashCode.Combine(x, y);

        public override string ToString() => $"({x}, {y})";
    }

    [Serializable]
    public class ColorConfig
    {
        public float r;
        public float g;
        public float b;

        public Color ToColor() => new Color(r, g, b, 1f);
    }
}
