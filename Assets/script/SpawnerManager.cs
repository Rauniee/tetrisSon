using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    [SerializeField] ShapeManager[] TumSekiller;

  
    public ShapeManager SekilOlusturFNC()
    {
        int randomSekil = Random.Range(0, TumSekiller.Length);
        ShapeManager sekil = Instantiate(TumSekiller[randomSekil], transform.position, Quaternion.identity) as ShapeManager;

        if (sekil != null)
        {
            return sekil;
        }
        else
        {
            print("dizi boş");
            return null;
        }
    }
}
