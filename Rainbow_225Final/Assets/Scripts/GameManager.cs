using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

using Mirror;
//using Photon.Pun;
//using Photon.Realtime;

public class GameManager : NetworkBehaviour
{
    public GameObject playerPrefab;
    public List<Spawnpoint> FFAspawnPoints = new List<Spawnpoint>();
    public List<Team> teams = new List<Team>();
    public int teamID;
    public GameObject timer;
    public CameraFocus mainCam;
    public LayerMask stageLayerMask;

    public GameObject weaponPickup;

    public string UNKNOWN = "something out of this world";
    public string LEVEL = "unfortunality";

    [SerializeField]
    TMP_Text timeText;

    public AudioSource source;
    public static GameManager instance { get; private set; } // SINGLETON INSTANCE

    void Awake()
    {
        if (instance)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

        source = gameObject.GetComponent<AudioSource>();
        mainCam = CameraFocus.instance;

        Spawnpoint[] foundPoints = FindObjectsOfType<Spawnpoint>();
        for (int count = 0; count < foundPoints.Length; count++)
        {
            if (foundPoints[count].teamSpawn == 0)
            {
                FFAspawnPoints.Add(foundPoints[count]);
            }
            else if (foundPoints[count].teamSpawn == 1 && teams[0] != null)
            {
                teams[0].spawnPoints.Add(foundPoints[count]);
            }
            else if (foundPoints[count].teamSpawn == 2 && teams[1] != null)
            {
                teams[1].spawnPoints.Add(foundPoints[count]);
            }
            else if (foundPoints[count].teamSpawn == 3 && teams[2] != null)
            {
                teams[2].spawnPoints.Add(foundPoints[count]);
            }
            else if (foundPoints[count].teamSpawn == 4 && teams[3] != null)
            {
                teams[3].spawnPoints.Add(foundPoints[count]);
            }
        }

    }

    public void PlayerEnter()
    {
        //RainbowNetwork.instance.OnServerAddPlayer();
    }

    public int GetPlayerTeam()
    {
        if (playerPrefab)   // if there's actually a player prefab
        {
            if (teams.Count > 0)    // if the teams are higher than 0 (not FFA)
            {
                if (teams.Count == 2)   // if there are two teams
                {
                    if (teams[0].members.Count > teams[1].members.Count)    // if the amount of people on team 1 are higher than the amount of people on team 2
                    {
                        return 2;
                    }
                    else
                    {
                        return 1;
                    }
                }
                else if (teams.Count == 4)
                {
                    return 0; // implement later
                }
                else if (teams.Count == 1)
                {
                    return 0;
                }
                else
                {
                    Debug.LogError("Weird team number");
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }
        else
        {
            Debug.LogError("playerPrefab not set. [GameManager][Start]");
            return 0;
        }
    }

    public Transform GetSpawn(int teamID)
    {
        if (teamID != 0)
        {
            Team teamAssigned = teams[(teamID - 1)];
            if (teamAssigned.spawnPoints.Count > 0)
            {
                GameObject spawnPoint = teamAssigned.spawnPoints[Random.Range(0, teamAssigned.spawnPoints.Count)].gameObject;
                Transform spawn = spawnPoint.gameObject.GetComponent<Transform>();
                return spawn;
            }
            else
            {
                Debug.LogError("This team has no spawnpoints!");
                return null;
            }
        }
        else
        {
            if (FFAspawnPoints.Count > 0)
            {
                GameObject spawnPoint = FFAspawnPoints[Random.Range(0, FFAspawnPoints.Count)].gameObject;
                Transform spawn = spawnPoint.gameObject.GetComponent<Transform>();
                return spawn;
            }
            else
            {
                Debug.LogError("This map has no Free-For-All spawnpoints!");
                return null;
            }
        }
    }

    public int GetTeam()
    {
        return teamID;
    }

    public void AddTeamMember(PlayerMIRROR player)
    {
        teams[player.teamID].members.Add(player);
    }

    


    /*
    public GameObject playerPrefab;
    public List<Spawnpoint> FFAspawnPoints = new List<Spawnpoint>();
    public List<Team> teams = new List<Team>();
    public GameObject timer;
    public CameraFocus mainCam;
    public LayerMask stageLayerMask;

    public GameObject weaponPickup;

    public int UNKNOWN = 0;
    public int LEVEL = 1;

    [HideInInspector]
    public PhotonView pv;

    [SerializeField]
    TMP_Text timeText;

    public AudioSource source;
    public static GameManager instance { get; private set; } // SINGLETON INSTANCE

    void Awake()
    {
        if (instance)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

        pv = gameObject.GetPhotonView();
        source = gameObject.GetComponent<AudioSource>();
        mainCam = CameraFocus.instance;

        Spawnpoint[] foundPoints = FindObjectsOfType<Spawnpoint>();
        for (int count = 0; count < foundPoints.Length; count++)
        {
            if (foundPoints[count].teamSpawn == 0)
            {
                FFAspawnPoints.Add(foundPoints[count]);
            }
            else if (foundPoints[count].teamSpawn == 1 && teams[0] != null)
            {
                teams[0].spawnPoints.Add(foundPoints[count]);
            }
            else if (foundPoints[count].teamSpawn == 2 && teams[1] != null)
            {
                teams[1].spawnPoints.Add(foundPoints[count]);
            }
            else if (foundPoints[count].teamSpawn == 3 && teams[2] != null)
            {
                teams[2].spawnPoints.Add(foundPoints[count]);
            }
            else if (foundPoints[count].teamSpawn == 4 && teams[3] != null)
            {
                teams[3].spawnPoints.Add(foundPoints[count]);
            }
        }

    }

    bool checkPlayers()
    {
        Player[] currentPlayers = FindObjectsOfType<Player>();
        
        if (currentPlayers.Length != 0)
        {
            return true;
        }
        else 
        {
            return false;
        }
    }

    GameObject getRandomPlayer()
    {
        Player[] currentPlayers = FindObjectsOfType<Player>();
        return currentPlayers[Random.Range(0, FFAspawnPoints.Count)].gameObject;
    }

    private void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            SceneManager.LoadScene("Title");
            return;
        }
        if (checkPlayers())
        {
            mainCam.setTarget(getRandomPlayer());
        }
        else
        {
            Debug.Log("No Players found");
        }
    }

    public void playerEnterGame()
    {
        if (playerPrefab)   // if there's actually a player prefab
        {
            if (teams.Count > 0)    // if the teams are higher than 0 (not FFA)
            {
                if (teams.Count == 2)   // if there are two teams
                {
                    if (teams[0].members.Count > teams[1].members.Count)    // if the amount of people on team 1 are higher than the amount of people on team 2
                    {
                        playerAssign(2);
                    }
                    else
                    {
                        playerAssign(1);
                    }
                }
                else if (teams.Count == 4)
                {
                    playerAssign(0); // implement later
                }
                else if (teams.Count == 1)
                {
                    playerAssign(0);
                }
                else
                {
                    Debug.LogError("Weird team number");
                }
            }
            else
            {
                playerAssign(0);
            }
        }
        else
        {
            Debug.LogError("playerPrefab not set. [GameManager][Start]");
        }
    }

    void spawnPlayer(Spawnpoint spawn)
    {
        GameObject newPlayer = PhotonNetwork.Instantiate(playerPrefab.name, spawn.transform.position, spawn.transform.rotation);
        Player playerSetup = newPlayer.GetComponent<Player>();
        mainCam.setTarget(newPlayer);

        if (spawn.teamSpawn != 0)
        {
            playerSetup.gameObject.GetPhotonView().RPC("joinTeam", RpcTarget.AllBuffered, (spawn.teamSpawn - 1));
            Debug.Log(PhotonManager.instance.myUsername + " joined the " + teams[spawn.teamSpawn - 1].name + " team.");
        }
        else
        {
            Debug.Log(PhotonManager.instance.myUsername + " joined the game.");
        }
    }

    void playerAssign(int teamID)
    {
        if (teamID != 0)
        {
            Team teamAssigned = teams[(teamID - 1)];
            if (teamAssigned.spawnPoints.Count > 0)
            {
                GameObject spawnPoint = teamAssigned.spawnPoints[Random.Range(0, teamAssigned.spawnPoints.Count)].gameObject;
                Spawnpoint spawn = spawnPoint.GetComponent<Spawnpoint>();
                spawnPlayer(spawn);
            }
            else
            {
                Debug.LogError("This team has no spawnpoints!");
            }
        }
        else
        {
            if (FFAspawnPoints.Count > 0)
            {
                GameObject spawnPoint = FFAspawnPoints[Random.Range(0, FFAspawnPoints.Count)].gameObject;
                Spawnpoint spawn = spawnPoint.GetComponent<Spawnpoint>();
                spawnPlayer(spawn);
            }
            else
            {
                Debug.LogError("This map has no Free-For-All spawnpoints!");
            }
        }
        
       
    }

    public Team getTeam(int teamGet)
    {
        Team team = teams[teamGet];
        return team;
    }

    

    public override void OnDisconnected(DisconnectCause cause)
    {
        //ChatroomManager.instance.consoleMessage("LeaveGameMessage");
    }

    public GameObject getSpawn(Player playerToSpawn)
    {
        if (playerToSpawn.myTeam != null)
        {
            return playerToSpawn.myTeam.spawnPoints[Random.Range(0, playerToSpawn.myTeam.spawnPoints.Count)].gameObject;
        }
        else
        {
            return FFAspawnPoints[Random.Range(0, FFAspawnPoints.Count)].gameObject;
        }
        
    }

    [PunRPC]
    public void makeShot(int bulletID, bool createdByPlayer, int creatorID, float x, float y,  float accuracy, float rotX, float rotY, float rotZ)
    {
        GameObject shot;
        if (accuracy > 0)
        {
            shot = Instantiate(PhotonManager.instance.GetBullet(bulletID).thisPrefab, new Vector2(x, y), Quaternion.Euler(new Vector3(rotX, rotY, rotZ)) * Quaternion.Euler(0, 0, Random.Range(-accuracy, accuracy)));

        }
        else
        {
            shot = Instantiate(PhotonManager.instance.GetBullet(bulletID).thisPrefab, new Vector2(x, y), Quaternion.Euler(new Vector3(rotX, rotY, rotZ)));
        }

        DefaultBullet thisShot = shot.GetComponent<DefaultBullet>();
        thisShot.ownedByPlayer = createdByPlayer;
        thisShot.ownerID = creatorID;
    }

    /*[PunRPC]
    void makePickup(float x, float y, int pickupID, int spawnerID)
    {
        Spawner[] spawners = FindObjectsOfType<Spawner>();
        Spawner spawner = null;
        for (int count = 0; count < spawners.Length; count++)
        {
            if (spawners[count].spawnerID == spawnerID)
            {
                spawner = spawners[count];
            }
        }

        if (spawner)
        {
            GameObject pick;
            pick = Instantiate(weaponPickup, new Vector2(x, y), Quaternion.identity);
            Pickup thisPick = pick.GetComponent<WeaponPickup>();
            thisPick.setType(pickupID);
            spawner.addSpawn(pick);
        }
        else
        {
            Debug.LogError("spawner not found!");
        }
    }

    [PunRPC]
    void makeWeaponPickup(float x, float y, int weaponID, int spawnerID)
    {
        Spawner[] spawners = FindObjectsOfType<WeaponSpawner>();
        Spawner spawner = null;
        for (int count = 0; count < spawners.Length; count++)
        {
            if (spawners[count].spawnerID == spawnerID)
            {
                spawner = spawners[count];
            }
        }

        if (spawner)
        {
            GameObject pick;
            pick = Instantiate(weaponPickup, new Vector2(x, y), Quaternion.identity);
            WeaponPickup wepPick = pick.GetComponent<WeaponPickup>();
            wepPick.setType(weaponID);
            spawner.addSpawn(pick);
        }
        else
        {
            Debug.LogError("spawner not found!");
        }
    }*/
}
