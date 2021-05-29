using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Отвечает за валидность клетки относительно пути
public class GameBoard : MonoBehaviour
{
    [SerializeField]
    private Transform _ground;

    [SerializeField]
    private GameTile _tilePrefab;

    private Vector2Int _size;

    private GameTile[] _tiles;

    private Queue<GameTile> _searchFrontier = new Queue<GameTile>(); //клетки должны обрабатываться в том же
                                                                     //порядке в котором добавлены границы,
                                                                     //поэтому используем очередь

    public void Initialize(Vector2Int size)
    {
        _size = size;
        _ground.localScale = new Vector3(size.x, size.y, 1f);

        Vector2 offset = new Vector2((size.x - 1) * 0.5f, (size.y - 1) * 0.5f);

        _tiles = new GameTile[size.x * size.y];
        for (int i = 0, y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++, i ++)
            {
                GameTile tile = _tiles[i] = Instantiate(_tilePrefab);
                tile.transform.SetParent(transform, false);
                tile.transform.localPosition = new Vector3(x - offset.x, 0f, y - offset.y);

                if (x > 0) // проверка что бы не выйти за пределы поля
                {
                    GameTile.MakeEastWestNeighbors(tile, _tiles[i - 1]);
                }
                if (y > 0)
                {
                    GameTile.MakeNorthSouthNeighbors(tile, _tiles[i - size.x]);
                }

                tile.IsAlternative = (x & 1) == 0; //Проверяем на четность и меняем альтернативный путь
                if ((y&1) == 0)
                {
                    tile.IsAlternative = !tile.IsAlternative;
                }
            }
        }
        FindPaths();
    }

    public void FindPaths() //метод поиска пути
    {
        foreach (var tile in _tiles) //Пройдёмся по всем клеткам и обнулим их
        {
            tile.ClearPath();
        }
        int destinationIndex = _tiles.Length / 2; //Считаем середину пунктом назначения
        _tiles[destinationIndex].BecomeDestination(); // добавляем в очередь середину
        _searchFrontier.Enqueue(_tiles[destinationIndex]);
        while (_searchFrontier.Count>0) //если в очереди не нулевые элементы
        {
            GameTile tile = _searchFrontier.Dequeue();
            if (tile != null) //если да то раширяемся по всем направлениям
            {
                if (tile.IsAlternative)
                {
                    _searchFrontier.Enqueue(tile.GrowPathNorth());
                    _searchFrontier.Enqueue(tile.GrowPathSouth());
                    _searchFrontier.Enqueue(tile.GrowPathEast());
                    _searchFrontier.Enqueue(tile.GrowPathWest());
                }
                else
                {
                    _searchFrontier.Enqueue(tile.GrowPathWest());
                    _searchFrontier.Enqueue(tile.GrowPathEast());
                    _searchFrontier.Enqueue(tile.GrowPathSouth());
                    _searchFrontier.Enqueue(tile.GrowPathNorth());

                }
            }
        }

        foreach (var t in _tiles)
        {
            t.ShowPath();
        }
    }
}
