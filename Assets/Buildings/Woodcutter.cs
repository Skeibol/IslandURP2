using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Woodcutter : InGameItem
{
  private CircleCollider2D _circleCollider2D;

  private SpriteRenderer _aoeSprite;

  public bool lockedOnTree;

  public List<TreeObject> treesInsideRadius;
  private ContactFilter2D filter = new ContactFilter2D().NoFilter();
  public TreeObject targetedTree;

  private int pickedTreeIndex;

  // Start is called before the first frame update
  public override void Start()
  {
    base.Start();
    _circleCollider2D = GetComponent<CircleCollider2D>();
    _aoeSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
    _circleCollider2D.radius = _aoeSprite.transform.localScale.x;
    _circleCollider2D.enabled = false;
  }

  // Update is called once per frame
  public override void Update()
  {
    base.Update();
    if (isOnCursor && !_aoeSprite.enabled) {
      _circleCollider2D.enabled = false;
      _aoeSprite.enabled = true;
    }
    else if (!isOnCursor && _aoeSprite.enabled) {
      _aoeSprite.enabled = false;
      _circleCollider2D.enabled = true;
    }
    else if (!isOnCursor && !lockedOnTree) {
      LockOnTree();
    }
    
  }

  IEnumerator cutTree()
  {
    while (targetedTree.takeDamage(15,false)) {
      yield return new WaitForSeconds(1f);
    }

    targetedTree = null;
    lockedOnTree = false;
    treesInsideRadius.RemoveAt(pickedTreeIndex);
    yield return 0;
  }

  public void LockOnTree()
  {
    if (treesInsideRadius.Any() && !lockedOnTree) {
      pickedTreeIndex = Random.Range(0, treesInsideRadius.Count);
      targetedTree = treesInsideRadius[pickedTreeIndex];
      lockedOnTree = true;
      StartCoroutine(cutTree());
    }
  }

  public void OnTriggerEnter2D(Collider2D other)
  {
    if (other.gameObject.CompareTag("tree")) {
      treesInsideRadius.Add(other.gameObject.GetComponent<TreeObject>());
    }
  }
}