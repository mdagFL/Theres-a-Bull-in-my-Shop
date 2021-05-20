using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class China : PhysicsObject
{
    public GameObject destroyedObject;
    public Player player;
    public float breakingSpeed;
    public int damage;
    public GameObject myShelf;
    private bool isBroken = false;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        player = GameObject.FindObjectOfType<Player>();
        myShelf = this.transform.parent.gameObject;
    }

    private void OnCollisionEnter(Collision col)
    {
        if(col.relativeVelocity.magnitude > breakingSpeed && isBroken == false)
        {
            isBroken = true;
            player.Hurt(damage);
            
            if (myShelf != null)
            {
                Shelf shelf = myShelf.GetComponent<Shelf>();
                shelf.china.RemoveAt(shelf.china.IndexOf(this.gameObject));
            }

            
            GameObject broken = Instantiate(destroyedObject, this.transform.position, this.transform.rotation);
            broken.transform.localScale = this.transform.lossyScale;
            Destroy(gameObject);
        }
    }
}
