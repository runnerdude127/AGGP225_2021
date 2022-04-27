using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Team
{
    public string name;
    public Color teamColor;

    public bool swapOnRespawn;


    public List<PlayerMIRROR> members;
    public List<Spawnpoint> spawnPoints;
    public bool respawning;

    public Team(string newName, Color newTeamColor, List<PlayerMIRROR> newMembers, List<Spawnpoint> newSpawnPoints, bool newRespawning)
    {
        name = newName;
        teamColor = newTeamColor;
        members = newMembers;
        spawnPoints = newSpawnPoints;
        respawning = newRespawning;
    }
}
