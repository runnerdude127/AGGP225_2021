using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Gamemode
{
    public string name;
    public Sprite icon;
    public bool FFA;
    public bool twoTeam;
    public bool threeTeam;
    public bool fourTeam;
    public bool fixedTeams;

    public List<Team> presetTeams;

    public Gamemode(string newName, Sprite newIcon, bool newTwo, bool newThree, bool newFour, bool newFixedTeams, List<Team> newPresetTeams)
    {
        name = newName;
        icon = newIcon;
        twoTeam = newTwo;
        threeTeam = newThree;
        fourTeam = newFour;
        fixedTeams = newFixedTeams;
        presetTeams = newPresetTeams;     
    }
}
