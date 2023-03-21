using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceOutcropScript : MonoBehaviour
{
    public float VariationCoefficient;

    public EconomyManager economyManager;
    public ResourceManager resourceManager;

    public float worth;
    public float InfluenceRadius;

    public bool activated;

    public GameObject Player;

    private void Start()
    {
        VariationCoefficient = Random.Range(0.5f, 1.5f);
        this.gameObject.transform.localScale = Vector3.Scale(new Vector3(VariationCoefficient, VariationCoefficient, VariationCoefficient), 
            this.gameObject.transform.localScale); //check if this works
        worth = economyManager.baseResourceValue * VariationCoefficient;
        InfluenceRadius = resourceManager.baseResourceInfluenceRadius * VariationCoefficient;

    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == 8)
        {
            Debug.Log("Picked up resource");
            ExtractResource();
            Destroy(this.gameObject);
        }
    }

    public void ExtractResource()
    {
        Player.GetComponent<PlayerInventory>().ListOfRawResources.Add( new ResourceScript(worth));
        Destroy(this.gameObject); //reset state of outcrop to emptuy
    }
}
