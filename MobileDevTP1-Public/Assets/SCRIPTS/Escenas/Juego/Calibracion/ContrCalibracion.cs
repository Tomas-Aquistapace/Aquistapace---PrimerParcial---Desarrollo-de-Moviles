using UnityEngine;
using System.Collections;

public class ContrCalibracion : MonoBehaviour
{
	public Player Pj;

	public float TiempEspCalib = 3;
	float Tempo2 = 0;
	
	public enum Estados{Calibrando, Tutorial, Finalizado}
	public Estados EstAct = Estados.Calibrando;
	
	public ManejoPallets Partida;
	public ManejoPallets Llegada;
	public Pallet P;
    public ManejoPallets palletsMover;
	
	GameManager GM;

	Renderer partidaR;
	Collider partidaC;

	Renderer llegadaR;
	Collider llegadaC;
    
	//----------------------------------------------------//

    private void Awake()
    {
		partidaR = Partida.GetComponent<Renderer>();
		partidaC = Partida.GetComponent<Collider>();

		llegadaR = Llegada.GetComponent<Renderer>();
		llegadaC = Llegada.GetComponent<Collider>();
	}

    void Start () 
	{
        palletsMover.enabled = false;
        Pj.ContrCalib = this;
		
		GM = GameObject.Find("GameMgr").GetComponent<GameManager>();
		
		P.CintaReceptora = Llegada.gameObject;
		Partida.Recibir(P);
		
		SetActivComp(false);
	}
	
	void Update ()
	{
		if(EstAct == ContrCalibracion.Estados.Tutorial)
		{
			if(Tempo2 < TiempEspCalib)
			{
				Tempo2 += Time.deltaTime;
				if (Tempo2 > TiempEspCalib)
				{
					Debug.LogWarning("Se mete acá");
					SetActivComp(true);
				}
			}
		}
	}
	
	void FinCalibracion()
	{
		/*
		Reiniciar();
		GM.CambiarATutorial(Pj.IdPlayer);
		*/
	}
	
	public void IniciarTesteo()
	{
		EstAct = ContrCalibracion.Estados.Tutorial;
        palletsMover.enabled = true;
        //Reiniciar();
    }
	
	public void FinTutorial()
	{
		EstAct = ContrCalibracion.Estados.Finalizado;
        palletsMover.enabled = false;
        GM.FinCalibracion(Pj.IdPlayer);
	}
	
	void SetActivComp(bool estado)
	{
		if(partidaR != null)
			partidaR.enabled = estado;

		partidaC.enabled = estado;
		
		if(llegadaR != null)
			llegadaR.enabled = estado;

		llegadaC.enabled = estado;
		
		P.GetComponent<Renderer>().enabled = estado;
	}
}
