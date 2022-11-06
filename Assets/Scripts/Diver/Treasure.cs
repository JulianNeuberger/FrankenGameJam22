public class Treasure : Collectible
{
    public override void Collect()
    {
        Destroy(gameObject);
    }
}
