using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(MapManager))]
public class GameController : MonoBehaviour
{
    public string GameState;

    [SerializeField] Canvas canvas;
    [SerializeField] Player player;

    MapManager mapManager;

    public float score,highscore;
    
    void Start()
    {
        mapManager = GetComponent<MapManager>();
        changeUI();
        mapManager.generateMap(player.gameObject);
    }

    void Update()
    {
        if (canvas.transform.Find(GameState).Find("Score")) canvas.transform.Find(GameState).Find("Score").GetComponent<Text>().text = "Score:" + ((int)score).ToString();
        if (canvas.transform.Find(GameState).Find("HighScore")) canvas.transform.Find(GameState).Find("HighScore").GetComponent<Text>().text = "HighScore:" + ((int)highscore).ToString();

        if (GameState == "Game")
        {
            mapManager.updateMap(player.gameObject);
            Time.timeScale = 1;
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, 10, Time.deltaTime);
            
        }
        else
        {
            if ((Vector2)Camera.main.transform.position != (Vector2)player.transform.position)
            {
                Camera.main.transform.position = Vector3.Lerp(
                    Camera.main.transform.position,
                    (Vector3)((Vector2)player.transform.position) + new Vector3(0, 0, -10),
                    Time.deltaTime*100
                );
                Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, 3,Time.deltaTime);
            }
            else Time.timeScale = 0;

        }
    }
    public void restartGame()
    {
        changeState("Loading");
        //SceneManager.LoadScene("SampleScene");
        mapManager.clearMap();
        mapManager.generateMap(player.gameObject);
        mapManager.updateMap(player.gameObject);
        player.transform.position = player.startPos;
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        changeState("Menu");
    }
    public void changeState(string state)
    {
        GameState = state;
        if (GameState == "GameOver" && (int)score > (int)highscore) highscore = score;
        if (state == "Menu" && score != 0) score = 0;
        changeUI();
    }
    public void changeUI()
    {
        foreach (Transform obj in canvas.GetComponentInChildren<Transform>())
        {
            if (obj.name == GameState) obj.gameObject.SetActive(true);
            else obj.gameObject.SetActive(false);
        }
    }
}
