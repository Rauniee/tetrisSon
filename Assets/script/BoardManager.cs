using System;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField] private Transform tilePrefab;

    public int yukseklik = 22;
    public int genislik = 20;

    private Transform[,] izgara;

    private void Awake()
    {
        izgara = new Transform[genislik, yukseklik];

    }

    private void Start()
    {
        BosKareleriOlusturFNC();
    }

    
    bool BoardIcindemi(int x, int y)
    {
        return (x>=0 && x<genislik && y>=0 );
    }

    bool KareDolumu(int x, int y, ShapeManager shape)
    {
        // 1. Önce x ve y değerlerinin ızgaramızın içinde olduğundan emin olmalıyız.
        // Eğer x veya y sınır dışındaysa (örneğin y=22 gibi), ızgaraya hiç bakmadan false döndür.
        if (x < 0 || x >= genislik || y < 0 || y >= yukseklik)
        {
            return false; 
        }
        return (izgara[x, y] != null && izgara[x, y].parent != shape.transform);
    }
    public bool GecerliPozisyondami(ShapeManager shape)
    {
        // AJAN 1: Metot çalışmaya başladı mı ve şeklin içinde kaç kare var?
        Debug.Log("1. Metoda girildi. Şekil: " + shape.name + " | İçindeki kare sayısı: " + shape.transform.childCount);
        foreach (Transform child in shape.transform)
        {
            Vector2 pos = VectoruIntYapFNC(child.position);
        
            if (!BoardIcindemi((int)pos.x, (int)pos.y))
            {
                Debug.LogWarning("3. BoardIcindemi kontrolüne takıldı! Koordinat: X:" + pos.x + " Y:" + pos.y);
                return false; 
            }

            if (pos.y <= yukseklik)
            {
                if (KareDolumu((int)pos.x, (int)pos.y, shape))
                {
                    Debug.LogWarning("4. KareDolumu kontrolüne takıldı! Koordinat: X:" + pos.x + " Y:" + pos.y);
                    return false;
                }
            }
           
        }

        return true; 
    }


    void BosKareleriOlusturFNC()
    {
        if (tilePrefab != null)
        {
            for (int y = 0; y < yukseklik; y++)
            {
                for (int x = 0; x < genislik; x++)
                {
                    Transform tile = Instantiate(tilePrefab, new Vector3(x, y, 0), Quaternion.identity);
                    tile.name = "x " + x.ToString() + " " + ", " + "y " + y.ToString();
                    tile.parent = this.transform;
                }
            }
        }
    }

    public void SekliIzgaraIcerisineAlFNC(ShapeManager shape)
    {
        if (shape == null)
            return;

        foreach (Transform child in shape.transform)
        {
            Vector2 pos = VectoruIntYapFNC(child.position);
            int x = (int)pos.x;
            int y = (int)pos.y;

            // KESİN GÜVENLİK FİLTRESİ: 
            // Eğer koordinat 0-9 veya 0-21 aralığının dışındaysa (örn: X=10), 
            // ızgaraya kaydetmeye çalışıp oyunu çökertme, direkt atla!
            if (x >= 0 && x < genislik && y >= 0 && y < yukseklik)
            {
                izgara[x, y] = child;
            }
            else
            {
                Debug.LogWarning($"Sınır dışı kare ızgaraya alınamadı: X:{x} Y:{y}");
            }
            
        }
    }

    bool SatirTamamlandımiFNC(int y)
        {
            for (int x = 0; x < genislik; ++x)
            {
                if (izgara[x, y] == null)
                {
                    return false;
                }
            }

            return true;
        }

        void SatiriTemizleFNC(int y)
        {
            for (int x = 0; x < genislik; ++x)
            {
                if (izgara[x, y] == null)
                {
                    Destroy(izgara[x, y].gameObject);
                }

                izgara[x, y] = null;
            }

        }

        void BirSatirAsagiIndirFNC(int y)
        {
            for (int x = 0; x < genislik; ++x)
            {
                if (izgara[x, y] != null)
                {
                    izgara[x, y - 1] = izgara[x, y];
                    izgara[x, y] = null;
                    izgara[x, y - 1].position += Vector3.down;
                }
            }
        }

        void TumSatirlariAsagiIndirFNC(int baslangicY)
        {
            for (int i = baslangicY; i < yukseklik; ++i)
            {
                BirSatirAsagiIndirFNC(i);
            }
        }

        public void TumSatirlariTemizleFNC()
        {
            for (int y = 0; y < yukseklik; y++)
            {
                if (SatirTamamlandımiFNC(y))
                {
                    SatiriTemizleFNC(y);
                    TumSatirlariAsagiIndirFNC(y + 1);
                    y--;
                }
            }
        }

        public bool DisariTastimiFNC(ShapeManager shape)
        {
            foreach (Transform child in shape.transform)
            {
                if (child.transform.position.y >= yukseklik - 1)
                {
                    return true;
                }
            }

            return false;
        }
        
        
        //if (shape == null)
        //return;

        //foreach (Transform child in shape.transform)
        //{
        //  Vector2 pos=VectoruIntYapFNC(child.position);
        //izgara[(int)pos.x, (int)pos.y] = child;

        Vector2 VectoruIntYapFNC(Vector2 vector)
        {
            return new Vector2(Mathf.Round(vector.x), Mathf.Round(vector.y));
        } 
    }

