using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWarManager : MonoBehaviour
{

	public GameObject m_fogOfWarPlane;
	public Transform m_player;
	public LayerMask m_fogLayer;
	public float m_radius = 5f;
	private float m_radiusSqr { get { return m_radius * m_radius; } }

	private Mesh m_mesh;
	private Vector3[] m_vertices;
	private Color[] m_colors;

	private InGameManager inGameMgr = null;

	private void Awake()
	{
		inGameMgr = InGameManager.instance;
		m_player = inGameMgr.hero.transform;

		if(m_fogOfWarPlane == null)
		{
			m_fogOfWarPlane = inGameMgr.transform.Find("Fog/Plane").gameObject;
		}
	}

	void Start()
	{
		Initialize();
	}

	// Update is called once per frame
	void Update()
	{
		Ray r = new Ray(transform.position, m_player.position - transform.position);
		RaycastHit hit;
		if (Physics.Raycast(r, out hit, 1000, m_fogLayer, QueryTriggerInteraction.Collide))
		{
			for (int i = 0; i < m_vertices.Length; i++)
			{
				Vector3 v = m_fogOfWarPlane.transform.TransformPoint(m_vertices[i]);
				float dist = Vector3.SqrMagnitude(v - hit.point);
				if (dist < m_radiusSqr)
				{
					float alpha = Mathf.Min(m_colors[i].a, dist / m_radiusSqr);
					m_colors[i].a = alpha;
				}
			}
			UpdateColor();
		}
	}

	void Initialize()
	{
		m_mesh = m_fogOfWarPlane.GetComponent<MeshFilter>().mesh;
		m_vertices = m_mesh.vertices;
		m_colors = new Color[m_vertices.Length];
		for (int i = 0; i < m_colors.Length; i++)
		{
			m_colors[i] = Color.black;
		}
		UpdateColor();
	}

	void UpdateColor()
	{
		m_mesh.colors = m_colors;
	}
}
/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWarManager : MonoBehaviour
{
    public Texture2D fogTexture;
    private static Color[] _colors;

    private void Awake()
    {
        _colors = fogTexture.GetPixels();

        for (int i = 0; i < 512 * 512; i++)
        {
            _colors[i] = Color.black;
        }

        for (int i = 0; i < 512 * 512; i++)
        {
            if (i > 150000 && i < 220000)
            {
                _colors[i] = Color.white;
            }
        }

        fogTexture.SetPixels(_colors);
        fogTexture.Apply();

    }

    public static void UpdatePosition(Vector3 position)
    {
        Vector2Int characterPosition = new Vector2Int((int)position.x + 32, (int)position.z + 32);
        characterPosition.x = (int)Mathf.Clamp(characterPosition.x, 0.0f, 512f);
        characterPosition.y = (int)Mathf.Clamp(characterPosition.y, 0.0f, 512f);
        _colors[characterPosition.x + characterPosition.y * 512] = Color.white;

    }

    void Update()
    {
        //for (int i = 0; i < 512 * 512; i++)
        //{
        //    Color c = _colors[i];
        //    c.r = c.r * (1.0f - 0.2f * Time.deltaTime);
        //    _colors[i] = c;
        //}

        //fogTexture.SetPixels(_colors);
        //fogTexture.Apply();
    }
}
*/
