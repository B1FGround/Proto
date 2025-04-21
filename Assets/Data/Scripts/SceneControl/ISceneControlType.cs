using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface ISceneControlType
{
    void OnAction(string sceneName, Vector3 playerPos, Image sceneChangeBG, List<GameObject> characters);
}