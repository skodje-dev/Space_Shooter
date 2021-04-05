using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Player _player;
    
    private static GameManager _instance;
    public static GameManager Instance
    {
        get => _instance;
        set => _instance = value;
    }

    private void Awake()
    {
        if (!_instance) _instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        _player = FindObjectOfType<Player>();
    }

    public Transform GetPlayerTransform()
    {
        return _player.transform;
    }
}
