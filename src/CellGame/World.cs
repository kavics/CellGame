using System.Data.SqlTypes;
using System.Diagnostics;

namespace CellGame;

public class World
{
    private readonly Random _random;
    private int[,] _cells;
    private int[,] _newCells;

    public int[,] Cells => _cells;
    public int Width { get; }
    public int Height { get; }
    public bool Infinite { get; set; } = true;
    public int Generation { get; set; }
    public int HistoryLength { get; set; } = 256;

    public World(int width, int height)
    {
        Width = width;
        Height = height;
        _random = new Random();
        _cells = new int[width, height];
        _newCells = new int[width, height];
    }

    public void Clear()
    {
        for (var y = 0; y < Height; y++)
            for (var x = 0; x < Width; x++)
                _cells[x, y] = 0;
        Generation = 0;
    }
    public void InitializeRandom()
    {
        for (var y = 0; y < Height; y++)
            for (var x = 0; x < Width; x++)
                _cells[x, y] = _random.NextDouble() > 0.7d ? Alive() : 0;
        Generation = 0;
    }
    public void GenerateRandomBullet()
    {
        /*
#..
#.#
##.
         */

        var sprite = new int[3,3];
        sprite[0, 0] = 1; sprite[1, 0] = 0; sprite[2, 0] = 0;
        sprite[0, 1] = 1; sprite[1, 1] = 0; sprite[2, 1] = 1;
        sprite[0, 2] = 1; sprite[1, 2] = 1; sprite[2, 2] = 0;
        var size = sprite.GetLength(0);
        // world position of the sprite's top left corner
        var wx = _random.Next(Width - size);
        var wy = _random.Next(Height - size);

        // algorithm id is a random number 0-7
        var id = _random.Next(8);

        var sx = 0;
        var sy = 0;
        for (var y = 0; y < size; y++)
        {
            for (var x = 0; x < size; x++)
            {
                switch (id)
                {
                    case 0: sx =     x; sy =     y; break;
                    case 1: sx = 2 - x; sy =     y; break;
                    case 2: sx =     x; sy = 2 - y; break;
                    case 3: sx = 2 - x; sy = 2 - y; break;
                    case 4: sx =     y; sy =     x; break;
                    case 5: sx = 2 - y; sy =     x; break;
                    case 6: sx =     y; sy = 2 - x; break;
                    case 7: sx = 2 - y; sy = 2 - x; break;
                }
                _cells[wx + x, wy + y] = sprite[sx, sy];
            }
        }
    }


    public void GenerateNext()
    {
        Generation++;

        for (var y = 0; y < Height; y++)
            for (var x = 0; x < Width; x++)
                _newCells[x, y] = GenerateCell(x, y, _cells[x, y]);
        
        // swap buffers
        var temp = _cells;
        _cells = _newCells;
        _newCells = temp;
    }

    private int GenerateCell(int x, int y, int value)
    {
        var neighbors = Infinite ? CountNeighborsInfinite(x, y) : CountNeighbors(x, y);
        if (IsAlive(value))
            return neighbors == 2 || neighbors == 3 ? Alive() : Dead(value);
        else
            return neighbors == 3 ? Alive() : Dead(value);
    }

    private bool IsAlive(int value) => value == 1;
    private int Alive() => 1;
    private int Dead(int value) => value == 0 ? 0 : ++value >= HistoryLength ? 0 : value;

    private int CountNeighbors(int x, int y)
    {
        var count = 0;
        for (var dy = -1; dy <= 1; dy++)
        for (var dx = -1; dx <= 1; dx++)
        {
            if (dx == 0 && dy == 0)
                continue;
            var nx = x + dx;
            var ny = y + dy;
            if (nx < 0 || nx >= Width || ny < 0 || ny >= Height)
                continue;
            if (IsAlive(_cells[nx, ny]))
                count++;
        }
        return count;
    }

    private int CountNeighborsInfinite(int x, int y)
    {
        var count = 0;
        for (var dy = -1; dy <= 1; dy++)
        for (var dx = -1; dx <= 1; dx++)
        {
            if (dx == 0 && dy == 0)
                continue;
            var nx = x + dx;
            var ny = y + dy;
            if (nx < 0)
                nx = Width - 1;
            if (nx >= Width)
                nx = 0;
            if (ny < 0)
                ny = Height - 1;
            if (ny >= Height)
                ny = 0;
            if (IsAlive(_cells[nx, ny]))
                count++;
        }

        return count;
    }
}