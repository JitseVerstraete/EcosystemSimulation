using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        m_SpriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        Vector2 worldSize = SimulationScript.GetWorldSize();

        m_Sprite = Sprite.Create(m_Texture, new Rect(0f, 0f, m_Texture.width, m_Texture.height), new Vector2(0f, 0f), 64, 0, SpriteMeshType.FullRect);
        

        gameObject.transform.position = new Vector3(-worldSize.x, -worldSize.y, 1f);




        m_SpriteRenderer.sprite = m_Sprite;
        m_SpriteRenderer.drawMode = SpriteDrawMode.Tiled;
        m_SpriteRenderer.tileMode = SpriteTileMode.Continuous;
        m_SpriteRenderer.size = new Vector3(worldSize.x * 2 , worldSize.y * 2 );
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.localScale = new Vector3(1f, 1f, 1);
    }





    public Texture2D m_Texture;
    private Sprite m_Sprite;
    



    private SpriteRenderer m_SpriteRenderer;
}
