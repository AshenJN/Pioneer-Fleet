using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerData : MonoBehaviour
{
    public Fleet playerFleet;
    public Fleet enemyFleet;
    public int miniGame;
    public int miniGameScore;

    public int currency;

    public bool LoadBridgeFirstTime = true;

    public int battleTurn = 1;
    public string FleetLog;

   // public int starFighterDif=0;

    void Start()
    {
        currency = 150;
        DontDestroyOnLoad (transform.gameObject);
        playerFleet = gameObject.AddComponent(typeof(Fleet)) as Fleet;
        enemyFleet = gameObject.AddComponent(typeof(Fleet)) as Fleet;
        setFleet();
        FleetLog = "An enemy fleet approaches";
        //setEnemyFleet("NairanBattlecruiser",2,"NairanFighter",3);
    }

    // Update is called once per frame
    void Update()
    {
        //test
    }

    public void addToFleetLog(string s){
        FleetLog = FleetLog + s;
    }

    public void clearFleetLog(){
        FleetLog = "";
    }

    public void setFleet(){
        addPlayerCaptialShip("FederationFrigate");
        addPlayerCaptialShip("FederationFrigate");
        
        addPlayerStarFighter("FederationFighter");
        addPlayerStarFighter("FederationFighter");
        addPlayerStarFighter("FederationFighter");
    }

    public void offScreen(){
        for(int i = 0; i < playerFleet.CapitalShips.Count;i++){
            playerFleet.CapitalShips[i].gameObject.SetActive(false);
            //playerFleet.CapitalShips[i].transform.position = transform.position;
            //playerFleet.CapitalShips[i].transform.rotation = transform.rotation;
        }
        for(int i = 0; i < playerFleet.StarFighters.Count;i++){
            playerFleet.StarFighters[i].gameObject.SetActive(false);
            playerFleet.StarFighters[i].endCombat();
            Debug.Log("Bombing run is " + playerFleet.StarFighters[i].bombingRun);
            //playerFleet.StarFighters[i].transform.position = transform.position;
            //playerFleet.StarFighters[i].transform.rotation = transform.rotation;

        }

        for(int i = 0; i < enemyFleet.CapitalShips.Count;i++){
            enemyFleet.CapitalShips[i].gameObject.SetActive(false);
            //enemyFleet.CapitalShips[i].transform.position = transform.position;
            //enemyFleet.CapitalShips[i].transform.rotation = transform.rotation;

        }
        for(int i = 0; i < enemyFleet.StarFighters.Count;i++){
            enemyFleet.StarFighters[i].gameObject.SetActive(false);
            enemyFleet.StarFighters[i].endCombat();
            //enemyFleet.StarFighters[i].transform.position = transform.position;
            //enemyFleet.StarFighters[i].transform.rotation = transform.rotation;

        }
    }

    public void HandleDamageandDeadFighters(){
        for(int i = 0; i < playerFleet.StarFighters.Count;i++){
            if(playerFleet.StarFighters[i].damadge == true)
                playerFleet.StarFighters[i].damadge = false;
            if(playerFleet.StarFighters[i].deadge == true)
                playerFleet.StarFighters.RemoveAt(i);
        }
    }

    public void HandleDeadCapitalShips(){
        for(int i = 0; i < playerFleet.CapitalShips.Count;i++){
            if(playerFleet.StarFighters[i].damadge == true)
                playerFleet.StarFighters[i].damadge = false;
            if(playerFleet.StarFighters[i].deadge == true)
                playerFleet.StarFighters.RemoveAt(i);
        }
    }

      public List<StarFighter> PlayerActiveFighters(){
        List<StarFighter> S = new List<StarFighter>();
        for(int i = 0; i < playerFleet.StarFighters.Count; i++){
            if(playerFleet.StarFighters[i].getActive() == true){
                S.Add(playerFleet.StarFighters[i]);
            }
        }
        return S;
    }

    
      public List<StarFighter> EnemyActiveFighters(){
        List<StarFighter> S = new List<StarFighter>();
        for(int i = 0; i < enemyFleet.StarFighters.Count; i++){
            if(enemyFleet.StarFighters[i].getActive() == true){
                S.Add(enemyFleet.StarFighters[i]);
            }
        }
        return S;
    }

    public List<CapitalShip> PlayerActiveCapitalShips(){
        List<CapitalShip> S = new List<CapitalShip>();
        for(int i = 0; i < playerFleet.CapitalShips.Count; i++){
            if(playerFleet.CapitalShips[i].getActive() == true){
                S.Add(playerFleet.CapitalShips[i]);
            }
        }
        return S;
    }

    public List<CapitalShip> EnemyActiveCapitalShips(){
        List<CapitalShip> S = new List<CapitalShip>();
        for(int i = 0; i < enemyFleet.CapitalShips.Count; i++){
            if(enemyFleet.CapitalShips[i].getActive() == true){
                S.Add(enemyFleet.CapitalShips[i]);
            }
        }
        return S;
    }

    public float PlayerFleetHealth(){
        List<CapitalShip> CS = playerFleet.CapitalShips;
        List<StarFighter> SF = playerFleet.StarFighters;
        float currentValue = 0f;
        float maxValue = 0f;
        for(int i = 0; i < CS.Count; i++){
            maxValue += CS[i].maxHull;
            maxValue += CS[i].maxShield;
            currentValue += CS[i].currentHull;
            currentValue += CS[i].currentShield; 
        }
        for(int i = 0; i < SF.Count; i++){
            maxValue += SF[i].Accuracy;
            maxValue += SF[i].BombingPower;
            if(SF[i].getActive()){
                maxValue += SF[i].Accuracy;
                maxValue += SF[i].BombingPower;
            }
        }
        return currentValue/maxValue;
    }

    public float EnemyFleetHealth(){
        List<CapitalShip> CS = enemyFleet.CapitalShips;
        List<StarFighter> SF = enemyFleet.StarFighters;
        float currentValue = 0;
        float maxValue = 0;
        for(int i = 0; i < CS.Count; i++){
            maxValue += CS[i].maxHull;
            maxValue += CS[i].maxShield;
            currentValue += CS[i].currentHull;
            currentValue += CS[i].currentShield; 
        }
        for(int i = 0; i < SF.Count; i++){
            maxValue += SF[i].Accuracy;
            maxValue += SF[i].BombingPower;
            if(SF[i].getActive()){
                maxValue += SF[i].Accuracy;
                maxValue += SF[i].BombingPower;
            }
        }
        return currentValue/maxValue;
    }


    public void addPlayerCaptialShip(string name){
        CapitalShip playerCapitalShip = Instantiate(Resources.Load("FleetBattle/" + name, typeof(CapitalShip)) as CapitalShip, transform);
        playerFleet.CapitalShips.Add(playerCapitalShip);
    }

    public void addPlayerStarFighter(string name){
        StarFighter playerStarFighter = Instantiate(Resources.Load("FleetBattle/" + name, typeof(StarFighter)) as StarFighter, transform);
        playerFleet.StarFighters.Add(playerStarFighter);
    }

    public void setEnemyFleet(){
        EventData data = EventData.GetData();
        for(int i = 0; i < data.EnemyCapitalShips.Count; i++) {
            for(int j = 0; j < data.EnemyCapitalShipsNums[i]; j++) {
                CapitalShip enemyCapitalShip = Instantiate(Resources.Load("FleetBattle/" + data.EnemyCapitalShips[i], typeof(CapitalShip)) as CapitalShip, transform);
                enemyFleet.CapitalShips.Add(enemyCapitalShip);
            }
        }

        for(int i = 0; i < data.EnemyStarFighers.Count; i++) {
            for(int j = 0; j < data.EnemyStarFigherNums[i]; j++) {
                StarFighter enemyFigther = Instantiate(Resources.Load("FleetBattle/" + data.EnemyStarFighers[i], typeof(StarFighter)) as StarFighter, transform);
                enemyFleet.StarFighters.Add(enemyFigther);
            }
            
        }

     //   for(int i = 0; i < captitalShipNum; i++){
     //       CapitalShip enemyCapitalShip = Instantiate(Resources.Load("FleetBattle/" + captialShipType, typeof(CapitalShip)) as CapitalShip, transform);
     //       enemyFleet.CapitalShips.Add(enemyCapitalShip);
     //   }
     //   for(int i = 0; i < starFighterNum; i++){
     //       StarFighter enemyFigther = Instantiate(Resources.Load("FleetBattle/" + starFighterType, typeof(StarFighter)) as StarFighter, transform);
     //       enemyFleet.StarFighters.Add(enemyFigther);
     //   }
    }

    public void clearEnemyFleet(){
        for(int i = 0; i < enemyFleet.CapitalShips.Count;i++){
            Destroy(enemyFleet.CapitalShips[i].gameObject);
            //enemyFleet.CapitalShips[i].transform.position = transform.position;
            //enemyFleet.CapitalShips[i].transform.rotation = transform.rotation;

        }
        for(int i = 0; i < enemyFleet.StarFighters.Count;i++){
            Destroy(enemyFleet.StarFighters[i].gameObject);

        }
        enemyFleet.CapitalShips.Clear();
        enemyFleet.StarFighters.Clear();
    }

    public void updateHP(int value)
    {
        for(int i = 0; i < playerFleet.CapitalShips.Count;i++){
            playerFleet.CapitalShips[i].AddHP(value);
        }
    }

    public void updateCurrency(int value)
    {
        currency += value;
    }

    public void setMinigameInfo(int x, int x2){
        miniGame = x;
        miniGameScore = x2;
    }

    public void setMiniGameScore(int x){
        miniGameScore = x;
    }

    public static string getRandomCapitalShip(){
        int x = Random.Range(1,4);
        switch(x){
            case 1:
            return "FederationFrigate";

            case 2:
            return "NairanBattlecruiser";

            case 3:
            return "NairanFrigate";
        }
        return "FederationFrigate";
    }

    public static string getRandomStarFighter(){
        int x = Random.Range(1,3);
        switch(x){
            case 1:
            return "FederationFighter";

            case 2:
            return "NairanFighter";
        }
        return "FederationFighter";
    }

}
