using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public SpawnerManager spawner;
    public BoardManager board;
    private ShapeManager aktifSekil;


    [Header("Sayaçlar")]
    //[Range(02f, 1f)] 
    [SerializeField]
    private float asagiInmeSuresi = .1f;

    private float asagiInmeSayac;

    //[Range(02f, 1f)] 
    [SerializeField] private float sagSolTusaBasmaSuresi = 0.25f;

    private float sagSolTusaBasmaSayac;

    //[Range(02f, 1f)]
    [SerializeField] private float sagSolDonmeSuresi = 0.25f;

    private float sagSolDonmeSayac;
    //[Range(02f, 1f)]

    private float asagiTusaBasmaSayac;
    [SerializeField] private float asagiTusaBasmaSuresi = 0.25f;

    bool gameOver = false;

    private void Start()
    {
        
        // Unity 6 için güncel ve doğru kodlar:
        board = GameObject.FindFirstObjectByType<BoardManager>();
        spawner = GameObject.FindFirstObjectByType<SpawnerManager>();

        if (spawner)
        {
            if (aktifSekil == null)
            {
                aktifSekil = spawner.SekilOlusturFNC();
                //aktifSekil.transform.position = new Vector3(4f, 20f, 0f);
                aktifSekil.transform.position = VectoruIntYapFNC(aktifSekil.transform.position);
            }
        }
    }

    private void Update()
    {
        if (!board || !spawner || !aktifSekil || gameOver)
        {
            return;
        }

        GirisKontrolFNC();

    }

    void GirisKontrolFNC()
    {
        if ((Input.GetKey("right") && Time.time > sagSolTusaBasmaSayac || Input.GetKeyDown("right")))
        {
            aktifSekil.SagaHareketFNC();
            sagSolTusaBasmaSayac = Time.time + sagSolTusaBasmaSuresi;

            if (!board.GecerliPozisyondami(aktifSekil))
            {
                aktifSekil.SolaHareketFNC();
            }
        }
        else if ((Input.GetKey("left") && Time.time > sagSolTusaBasmaSayac || Input.GetKeyDown("left")))
        {
            aktifSekil.SolaHareketFNC();
            sagSolTusaBasmaSayac = Time.time + sagSolTusaBasmaSuresi;

            if (!board.GecerliPozisyondami(aktifSekil))
            {
                aktifSekil.SagaHareketFNC();
            }
        }
        else if ((Input.GetKeyDown("up") && Time.time > sagSolDonmeSayac))
        {
            aktifSekil.SagaDonFNC(); // 1. Şekli döndür
            sagSolDonmeSayac = Time.time + sagSolDonmeSuresi;

            // 2. Döndükten sonra kollar dışarı taştı mı?
            if (!board.GecerliPozisyondami(aktifSekil))
            {
                // 3. WALL KICK MANTIĞI: Şekli sola iterek içeri sığdırmayı dene
                aktifSekil.SolaHareketFNC();
        
                if (!board.GecerliPozisyondami(aktifSekil))
                {
                    // Sola itmek kurtarmadıysa, sağa iterek sığdırmayı dene
                    aktifSekil.SagaHareketFNC(); // Sola gitmeyi geri al
                    aktifSekil.SagaHareketFNC(); // Sağa git
            
                    if (!board.GecerliPozisyondami(aktifSekil))
                    {
                        // 4. Hiçbir yere sığmıyorsa, her şeyi iptal et (Geri Al)
                        aktifSekil.SolaHareketFNC(); // Tekrar merkeze dön
                        aktifSekil.SolaDonFNC();     // Dönüşü tersine çevirip iptal et!
                    }
                }
            }
        }
        else if (Input.GetKey("down") && Time.time > asagiTusaBasmaSayac || Time.time > asagiInmeSayac)
        {
            asagiInmeSayac = Time.time + asagiInmeSuresi;
            asagiTusaBasmaSayac = Time.time + asagiTusaBasmaSuresi;
            if (aktifSekil)
            {
                aktifSekil.AsagiHareketFNC();

                if (!board.GecerliPozisyondami(aktifSekil))
                {
                    if (board.DisariTastimiFNC(aktifSekil))
                    {
                        aktifSekil.YukariHareketFNC();
                        gameOver = true;
                    }
                    else
                    {
                        YerleştiFNC();
                    }

                }

            }
        }
    }




    private void YerleştiFNC()
            {
                sagSolTusaBasmaSayac = Time.time;
                asagiTusaBasmaSayac = Time.time;
                sagSolDonmeSayac = Time.time;

                aktifSekil.YukariHareketFNC();

                board.SekliIzgaraIcerisineAlFNC(aktifSekil);


                if (spawner)
                {
                    aktifSekil = spawner.SekilOlusturFNC();
                }

                board.TumSatirlariTemizleFNC();

            }

            Vector2 VectoruIntYapFNC(Vector2 vector)
            {
                return new Vector2(Mathf.Round(vector.x), Mathf.Round(vector.y));
            }

        }
    


    


