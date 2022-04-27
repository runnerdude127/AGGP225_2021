using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mirror;

public class RainbowNetwork : NetworkManager
{
    /// <summary>
    /// current game version
    /// </summary>
    string gameVersion = "indev 1.0";

    public string myUsername = "Default";
    //public CharacterClass myClass;

    public bool canConnect;

    public List<Gamemode> gamemodeList;
    public List<CharClass> classList;
    public int classID = 0;
    public List<Weapon> weaponList;
    public int weaponID = 0;
    public List<Bullet> bulletList;

    //string gameplayLevel = "InGame";
    public int timer;

    //List<RoomInfo> roomsAware;

    public const string GAMEMODE = "GM";
    public const string TIMELIMIT = "TL";
    public const string MAP = "MP";

    public static RainbowNetwork instance { get; private set; }
    public override void Awake()
    {
        base.Awake();
        if (instance)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }

        //PhotonNetwork.AutomaticallySyncScene = true;
        //roomsAware = new List<RoomInfo>();
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        // add player at correct spawn position
        int playerTeam = GameManager.instance.GetPlayerTeam();
        Transform spawnPoint = GameManager.instance.GetSpawn(playerTeam);
        GameObject newPlayer = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        NetworkServer.AddPlayerForConnection(conn, newPlayer);
        GameManager.instance.teamID = playerTeam;
    }

    public int GetPlayerClass()
    {
        return classID;
    }

    public Bullet GetBullet(int ID)
    {
        return bulletList[ID];
    }
}
