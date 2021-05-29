﻿using UnityEngine;

public class GameTile : MonoBehaviour
{

    [SerializeField]
    private Transform _arrow;

    private GameTile _north, _east, _south, _west, _nextOnPath;

    private int _distance; //количество ячеек до пункта назначения

    public bool HasPath => _distance != int.MaxValue;
    public bool IsAlternative { get; set; }

    private Quaternion _northRotation = Quaternion.Euler(90f, 0f, 0f);
    private Quaternion _eastRotation = Quaternion.Euler(90f, 90f, 0f);
    private Quaternion _southRotation = Quaternion.Euler(90f, 180f, 0f);
    private Quaternion _westRotation = Quaternion.Euler(90f, 270f, 0f);

    public static void MakeEastWestNeighbors(GameTile east, GameTile west)
    {
        west._east = east;
        east._west = west;
    }

    public static void MakeNorthSouthNeighbors(GameTile north, GameTile south)
    {
        north._south = south;
        south._north = north;
    }

    public void ClearPath() //Перезагрузка состояния клетки
    {
        _distance = int.MaxValue;
        _nextOnPath = null;
    }

    public void BecomeDestination() //Клетка пункт назначения
    {
        _distance = 0;
        _nextOnPath = null;
    }

    private GameTile GrowPathTo(GameTile neighbor) //Вызывается только для тех клеток которые уже имеют свой дистанс
    {
        if (!HasPath || neighbor == null || neighbor.HasPath)
        {
            return null;
        }
        neighbor._distance = _distance + 1;
        neighbor._nextOnPath = this;
        return neighbor;
    }
    public GameTile GrowPathNorth() => GrowPathTo(_north);
    public GameTile GrowPathEast() => GrowPathTo(_east);
    public GameTile GrowPathSouth() => GrowPathTo(_south);
    public GameTile GrowPathWest() => GrowPathTo(_west);


    public void ShowPath()//Метод который будет правильно показывать стрелку
    {
        if (_distance == 0)
        {
            _arrow.gameObject.SetActive(false);
            return;
        }
        _arrow.gameObject.SetActive(true);
        _arrow.localRotation=
            _nextOnPath == _north ? _northRotation :
            _nextOnPath == _east ? _eastRotation :
            _nextOnPath == _south ? _southRotation :
            _westRotation;
    }
}