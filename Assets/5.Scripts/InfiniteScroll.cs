using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InfiniteScroll : MonoBehaviour
{
    public List<GameObject> backgrounds;
    public float scrollSpeed = 2f;
    private float spriteWidth;
    private GameObject rightmostObject;
    private float cameraLeftX;
    void Start()
    {
        spriteWidth = backgrounds[0].GetComponent<SpriteRenderer>().bounds.size.x;
        cameraLeftX = Camera.main.transform.position.x - Camera.main.orthographicSize * Camera.main.aspect - spriteWidth;
    }

    void Update()
    {
        for (int i = 0; i < backgrounds.Count; i++)
        {
            backgrounds[i].transform.position += Vector3.left * scrollSpeed * Time.deltaTime;
        }

        for (int i = 0; i < backgrounds.Count; i++)
        {
            if (backgrounds[i].transform.position.x <= cameraLeftX)
            {
                rightmostObject = backgrounds.OrderByDescending(obj => obj.transform.position.x).First();
                var index = backgrounds.IndexOf(rightmostObject);
                float newX = backgrounds[index].transform.position.x + spriteWidth;

                backgrounds[i].transform.position = new Vector3(newX, backgrounds[i].transform.position.y, backgrounds[i].transform.position.z);
            }
        }
    }
}
