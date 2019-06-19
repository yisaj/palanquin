using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public LevelController level;
    public GoodDotController goodDot;
    //public BadDotController badDot;
    public float dotHeight = 0.1f;
    private GoodDotController currentGoodDot;
    [System.NonSerialized]
    public bool gameIsOver = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currentGoodDot == null)
        {
            currentGoodDot = spawnGoodDot();
        }
    }

    GoodDotController spawnGoodDot()
    {
        Vector3 position = new Vector3();
        Quaternion rotation = new Quaternion();
        float halfRange;
        switch (Random.Range(0, 4))
        {
            case 0:
                halfRange = level.width / 2f - goodDot.transform.localScale.x;
                position = new Vector3(Random.Range(-halfRange, halfRange), dotHeight, level.height / 2f + goodDot.carryThrough);
                rotation = Quaternion.AngleAxis(90, Vector3.up);
                break;
            case 1:
                halfRange = level.width / 2f - goodDot.transform.localScale.x;
                position = new Vector3(Random.Range(-halfRange, halfRange), dotHeight, level.height / -2f - goodDot.carryThrough);
                rotation = Quaternion.AngleAxis(-90, Vector3.up);
                break;
            case 2:
                halfRange = level.height / 2f - goodDot.transform.localScale.y;
                position = new Vector3(level.width / 2f + goodDot.carryThrough, dotHeight, Random.Range(-halfRange, halfRange));
                rotation = Quaternion.AngleAxis(180, Vector3.up);
                break;
            case 3:
                halfRange = level.height / 2f - goodDot.transform.localScale.y;
                position = new Vector3(level.width / -2f - goodDot.carryThrough, dotHeight, Random.Range(-halfRange, halfRange));
                break;
        }
        Debug.Log(rotation);
        GoodDotController dot = Instantiate(goodDot, position, rotation);
        dot.game = this;
        return dot;
    }

    public void GameOver()
    {
        gameIsOver = true;
    }
}
