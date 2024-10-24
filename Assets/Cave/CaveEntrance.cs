using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CaveEntrance : MonoBehaviour
{
  public bool isPlayerInCave;
  public GameObject FadePoint;
  public GameObject CaveInterior;
  public GameObject CaveExterior;


  private GameObject Player;
  private GameObject OutWorld;
  private SpriteRenderer Fade;
  private SpriteRenderer CaveBackground;
  private ChunkManager ChunkManager;
  private Transform _parent;
  private SpriteRenderer _parentSprite;
  private SpriteRenderer _playerRenderer;
  private Light2D _light;

  // Start is called before the first frame update
  void Start()
  {
    Player = GameObject.Find("Player");
    Fade = GameObject.Find("Fade").GetComponent<SpriteRenderer>();
    OutWorld = GameObject.Find("OutWorld");
    CaveBackground = GameObject.Find("CaveBackground").GetComponent<SpriteRenderer>();
    ChunkManager = GameObject.Find("Chunks").GetComponent<ChunkManager>();
    _parent = transform.parent.parent;
    _playerRenderer = Player.GetComponent<SpriteRenderer>();
    _light = GameObject.Find("Global Light 2D").GetComponent<Light2D>();
  }

  // Update is called once per frame
  void Update()
  {
    var dist = Vector3.Distance(Player.transform.position, FadePoint.transform.position);
    if (dist < 2) {
      Fade.color = new Color(0, 0, 0, 1.5f - dist);
      if (!isPlayerInCave) {
        if (Fade.color.a > 0.99f) {
          CaveBackground.color = new Color(0, 0, 0, 1);
          _playerRenderer.sortingLayerName = "Caves";
          _light.intensity = 0.3f;
          _parent.SetParent(GameObject.Find("Caves").transform);
          OutWorld.SetActive(false);
        }
        else if (Fade.color.a < 0.96f) {
          CaveBackground.color = new Color(0, 0, 0, 0);
          OutWorld.SetActive(true);
          _light.intensity = 1f;
          _playerRenderer.sortingLayerName = "Default";
          _parent.SetParent(ChunkManager.currentChunk.transform);
        }
      }
    }
  }

  private void OnTriggerEnter2D(Collider2D other)
  {
    if (other.name == "Player") {
      CaveExterior.SetActive(true);
      CaveInterior.SetActive(false);
      isPlayerInCave = false;
    }
  }
}