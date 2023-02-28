using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public Fleet playerFleet;
    public Fleet enemyFleet;
    //public List<CapitalShip> CapitalShips;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad (transform.gameObject);
        playerFleet = gameObject.AddComponent(typeof(Fleet)) as Fleet;
        enemyFleet = gameObject.AddComponent(typeof(Fleet)) as Fleet;
        setFleet();
        setEnemyFleet();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setFleet(){
        GameObject playerCapitalShip = Resources.Load("FleetBattle/PlayerFrigate", typeof(GameObject)) as GameObject;
        //CapitalShip c = test.GetComponent(typeof(GameObject)) as GameObject;
        playerFleet.CapitalShips.Add(playerCapitalShip);
        playerFleet.CapitalShips.Add(playerCapitalShip);

        GameObject playerFighter = Resources.Load("FleetBattle/PlayerFighter", typeof(GameObject)) as GameObject;
        //StarFighter s = test2.GetComponent(typeof(GameObject)) as GameObject;
        playerFleet.starFighters.Add(playerFighter);
        playerFleet.starFighters.Add(playerFighter);
        playerFleet.starFighters.Add(playerFighter);
        //playerFleet.addCapitalShip(test.GetComponent<CapitalShip>());
        //playerFleet.starFighters.Add((StarFighter)Resources.Load("PlayerFighter", typeof(GameObject)));

    }

    public void setEnemyFleet(){
        GameObject enemyCapitalShip = Resources.Load("FleetBattle/NairanBattlecruiser", typeof(GameObject)) as GameObject;
        //CapitalShip c = test.GetComponent(typeof(GameObject)) as GameObject;
        enemyFleet.CapitalShips.Add(enemyCapitalShip);
        enemyFleet.CapitalShips.Add(enemyCapitalShip);

        GameObject enemyFigther = Resources.Load("FleetBattle/NairanFighter", typeof(GameObject)) as GameObject;
        //StarFighter s = test2.GetComponent(typeof(GameObject)) as GameObject;
        enemyFleet.starFighters.Add(enemyFigther);
        enemyFleet.starFighters.Add(enemyFigther);
        enemyFleet.starFighters.Add(enemyFigther);
        //playerFleet.addCapitalShip(test.GetComponent<CapitalShip>());
        //playerFleet.starFighters.Add((StarFighter)Resources.Load("PlayerFighter", typeof(GameObject)));

    }
}
