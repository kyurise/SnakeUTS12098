using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Snake : MonoBehaviour
{
    public int xSize, ySize;
    public GameObject Blok;

    GameObject head;

    public Material headMaterial, tailMaterial;

    List<GameObject> tail;

    Vector2 dir;

    public Text points;
    // Start is called before the first frame update
    void Start()
    {
        waktuAntarGerakan = 0.5f;
        dir = Vector2.right;
        buatGrid();
        buatPlayer();
        tempatMakanan();
        Blok.SetActive(false);
        isAlive = true;
    }

    private Vector2 getRandomPos(){
        return new Vector2(Random.Range(-xSize/2+1,xSize/2), Random.Range(-ySize/2+1, ySize/2));
    }

    private bool containedInSnake(Vector2 tempatPos){
        bool isInHead = tempatPos.x == head.transform.position.x && tempatPos.y == head.transform.position.y;
        bool isInTail = false;  
        foreach (var item in tail){
            if (item.transform.position.x == tempatPos.x && item.transform.position.y == tempatPos.y){
                isInTail = true;
            }
        }
        return isInHead || isInTail;
    }

    GameObject makanan;

    bool isAlive;
    private void tempatMakanan(){
        Vector2 tempatPos = getRandomPos();
        while(containedInSnake(tempatPos)){
            tempatPos = getRandomPos();
        }
        makanan = Instantiate(Blok);
        makanan.transform.position = new Vector3(tempatPos.x, tempatPos.y, 0);
        makanan.SetActive(true);
    }

    private void buatPlayer(){
        head = Instantiate(Blok) as GameObject;
        head.GetComponent<MeshRenderer>().material = headMaterial;
        tail = new List<GameObject>();
    }

    public void buatGrid(){
        for (int x = 0; x<=xSize;x++){
            GameObject borderBottom = Instantiate(Blok) as GameObject;
            borderBottom.GetComponent<Transform>().position = new Vector3(x-xSize/2, -ySize/2, 0);

            GameObject borderTop = Instantiate(Blok) as GameObject;
            borderTop.GetComponent<Transform>().position = new Vector3(x-xSize/2, ySize-ySize/2, 0);
        }

        for (int y = 0; y <= ySize; y++){
            GameObject borderLeft = Instantiate(Blok) as GameObject;
            borderLeft.GetComponent<Transform>().position = new Vector3(xSize-(xSize/2), y-(ySize/2), 0);

            GameObject borderRight = Instantiate(Blok) as GameObject;
            borderRight.GetComponent<Transform>().position = new Vector3(-xSize/2, y-(ySize/2), 0);
        }
        
    }


    float waktuBerlalu, waktuAntarGerakan;

    public GameObject gameOverUI;

    private void gameOver(){
        isAlive = false;
        gameOverUI.SetActive(true);
    }

    public void restart(){
        SceneManager.LoadScene(0);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.DownArrow)){
            dir = Vector2.down;
        } else if (Input.GetKey(KeyCode.UpArrow)){
            dir = Vector2.up;
        } else if (Input.GetKey(KeyCode.RightArrow)){
            dir = Vector2.right;
        } else if (Input.GetKey(KeyCode.LeftArrow)){
            dir = Vector2.left;
        }

        waktuBerlalu += Time.deltaTime;
        if (waktuAntarGerakan < waktuBerlalu){
            waktuBerlalu = 0;

            Vector3  newPosition = head.GetComponent<Transform>().position + new Vector3(dir.x, dir.y, 0);
            if(newPosition.x >= xSize/2
            || newPosition.x <= -xSize/2
            || newPosition.y >= ySize/2
            || newPosition.y <= -ySize/2){

            }
        

        

            foreach (var item in tail){
                if (item.transform.position == newPosition){
                    //Game Over
                }
            }
            if (newPosition.x == makanan.transform.position.x && newPosition.y == makanan.transform.position.y){
                GameObject newTitle = Instantiate(Blok);
                newTitle.SetActive(true);
                newTitle.transform.position = makanan.transform.position;
                DestroyImmediate(makanan);
                head.GetComponent<MeshRenderer>().material = tailMaterial;
                tail.Add(head);
                head = newTitle;
                head.GetComponent<MeshRenderer>().material = headMaterial;
                tempatMakanan();
                points.text = "Poin : "+ tail.Count;
            }

            if (tail.Count == 0){
                head.transform.position = newPosition;
                
            } else {
                head.GetComponent<MeshRenderer>().material = tailMaterial;
                tail.Add(head);
                head = tail[0];
                head.GetComponent<MeshRenderer>().material = headMaterial;
                tail.RemoveAt(0);
                head.transform.position = newPosition;
            }
        }
    }
}
