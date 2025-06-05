using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialog Character", menuName = "Dialog Character")]
public class DialogCharacter : ScriptableObject
{
    public string characterName;

    public List<Sprite> characterPortraits;
    public Sprite currentPortrait;
}
