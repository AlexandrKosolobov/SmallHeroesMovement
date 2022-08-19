using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private string _name;
    [SerializeField] private int _id;

    public string Name { get => _name; }
    public int Id { get => _id; }
}
