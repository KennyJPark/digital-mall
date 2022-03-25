using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
public enum PatronType
{
    Man,
    Woman,
    Child,
}
*/

public enum BudgetType
{

}

// Preference for specific type of NPC such as Rich Man, Poor Child, Spoiled Child, Frugal Woman, Specific/Special NPC etc
[CreateAssetMenu(fileName = "New NPC Preference", menuName = "Scripts/ScriptableObjects/NPCPreference")]
public class NPCPreference : ScriptableObject
{
    //public int patronID;
    int preferenceID;

    string preferenceName;

    public PatronType patronType;
    public BudgetType budgetType;



    // Needs relative preferences based on item categories available for sale
    // Some customers may have a specific item preferences

    // Base preferences assuming every category of item is for sale
    public struct GlobalPreferences
    {

    }

    // Preferences based on what the player has for sale
    public struct RelativePreferences
    {

    }

    /*
    public struct Preferences
    {
        public readonly DrinkItem item;
        public readonly float weight;

        public Preference(DrinkItem item, float weight)
        {
            this.item = item;
            this.weight = weight;
        }
    }
    */
}
