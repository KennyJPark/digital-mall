using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IViewable
{
    Vector3 Position { get; }
    void View(GameObject viewer);
}
