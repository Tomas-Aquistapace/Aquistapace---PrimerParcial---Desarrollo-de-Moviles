using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using TMPro;

public class GameManager : MonoBehaviour
{	
	public static GameManager Instancia;

	public enum EstadoJuego{Calibrando, Jugando, Finalizado}
	public EstadoJuego EstAct = EstadoJuego.Calibrando;
	public enum NumOfPlayers { Singleplayer, Multiplayer }
	
	[Header("Timer")]
	public float TiempoDeJuego = 60;
	public TextMeshProUGUI timerText;

	[Header("Jugadores")]
	public NumOfPlayers numOfPlayers = NumOfPlayers.Multiplayer;

	public PlayerInfo PlayerInfo1 = null;
	public PlayerInfo PlayerInfo2 = null;
	
	public Player Player1;
	public Player Player2;

	[Header("Contador Inicio")]
	public TextMeshProUGUI conteoPj1Text;
	public TextMeshProUGUI conteoPj2Text;
	public float ConteoParaInicio = 3;

	bool ConteoRedresivo = true;

	float TiempEspMuestraPts = 3;
	
	[Header("Extras")]
	//posiciones de los camiones dependientes del lado que les toco en la pantalla
	//la pos 0 es para la izquierda y la 1 para la derecha
	public Vector3[]PosCamionesCarrera = new Vector3[2];

	//listas de GO que activa y desactiva por sub-escena
	//escena de calibracion
	public GameObject[] ObjsCalibracion1;
	public GameObject[] ObjsCalibracion2;
	//la pista de carreras
	public GameObject[] ObjsCarrera;
	
	//--------------------------------------------------------//
	
	void Awake()
	{
		GameManager.Instancia = this;
	}

    private void OnDestroy()
    {
		GameManager.Instancia = null;
    }

    void Start()
	{
		IniciarCalibracion();

		timerText.text = TiempoDeJuego.ToString("0");
	}
	
	void Update()
	{		
		switch (EstAct)
		{
		case EstadoJuego.Calibrando:
			if (numOfPlayers == NumOfPlayers.Multiplayer)
            {
				if (PlayerInfo1.PJ == null && Input.GetKeyDown(KeyCode.W)) 
				{
					PlayerInfo1 = new PlayerInfo(0, Player1);
					PlayerInfo1.LadoAct = Visualizacion.Lado.Izq;
					SetPosicion(PlayerInfo1);
				}

				if (PlayerInfo2.PJ == null && Input.GetKeyDown(KeyCode.UpArrow)) 
				{
					PlayerInfo2 = new PlayerInfo(1, Player2);
					PlayerInfo2.LadoAct = Visualizacion.Lado.Der;
					SetPosicion(PlayerInfo2);
				}
            }
            else
            {
				if (PlayerInfo1.PJ == null && Input.GetKeyDown(KeyCode.W))
				{
					PlayerInfo1 = new PlayerInfo(0, Player1);
					PlayerInfo1.LadoAct = Visualizacion.Lado.Izq;
					SetPosicion(PlayerInfo1);

					PlayerInfo2 = new PlayerInfo(1, Player2);
					PlayerInfo2.LadoAct = Visualizacion.Lado.Der;
					SetPosicion(PlayerInfo2);
				}
			}

			//cuando los 2 pj terminaron los tutoriales empiesa la carrera
			if (PlayerInfo1.PJ != null && PlayerInfo2.PJ != null)
			{
				if (PlayerInfo1.FinTuto2 && PlayerInfo2.FinTuto2)
				{
					EmpezarCarrera();
				}
			}
			break;			
			
		case EstadoJuego.Jugando:

			timerText.text = TiempoDeJuego.ToString("0");

			if (TiempoDeJuego <= 0)
			{
				FinalizarCarrera();
			}
			
			if(ConteoRedresivo)
			{
				StartCoroutine(StarCountdown());
				ConteoRedresivo = false;
			}
			else
			{
				//baja el tiempo del juego
				TiempoDeJuego -= Time.deltaTime;
				if (TiempoDeJuego <= 0)
				{
					//termina el juego
				}
			}
			break;
			
		case EstadoJuego.Finalizado:			
			TiempEspMuestraPts -= Time.deltaTime;
			if(TiempEspMuestraPts <= 0)
            {
				SceneManager.LoadScene("PuntosFinal");
            }
			break;
		}
	}

	public void StartPlayer1()
	{
		PlayerInfo1 = new PlayerInfo(0, Player1);
		PlayerInfo1.LadoAct = Visualizacion.Lado.Izq;
		SetPosicion(PlayerInfo1);
	}

	public void StartPlayer2()
	{
		PlayerInfo2 = new PlayerInfo(1, Player2);
		PlayerInfo2.LadoAct = Visualizacion.Lado.Der;
		SetPosicion(PlayerInfo2);
	}

	IEnumerator StarCountdown()
    {
        while (ConteoParaInicio >= 0)
        {
			ConteoParaInicio -= Time.deltaTime;

			if (ConteoParaInicio > 1)
			{
				conteoPj1Text.text = ConteoParaInicio.ToString("0");
				conteoPj2Text.text = ConteoParaInicio.ToString("0");
			}
			else
			{
				conteoPj1Text.text = "GO";
				conteoPj2Text.text = "GO";
			}

			yield return null;
        }

		conteoPj1Text.enabled = false;
		conteoPj2Text.enabled = false;

		EmpezarCarrera();
    }
	
	//----------------------------------------------------------//
	
	public void IniciarCalibracion()
	{
		for(int i = 0; i < ObjsCalibracion1.Length; i++)
		{
			if (numOfPlayers == NumOfPlayers.Multiplayer)
			{
				ObjsCalibracion1[i].SetActive(true);
				ObjsCalibracion2[i].SetActive(true);
			}
			else
            {
				ObjsCalibracion1[i].SetActive(true);
			}
		}
		
		for(int i = 0; i < ObjsCarrera.Length; i++)
		{
			ObjsCarrera[i].SetActive(false);
		}

		if(numOfPlayers == NumOfPlayers.Multiplayer)
        {
			Player1.CambiarACalibracion();
			Player2.CambiarACalibracion();
        }
        else
        {
			Player1.CambiarACalibracion();
		}
	}
	
	void EmpezarCarrera()
	{
		Player1.GetComponent<Frenado>().RestaurarVel();
		Player1.GetComponent<ControlDireccion>().Habilitado = true;
			
		Player2.GetComponent<Frenado>().RestaurarVel();
		Player2.GetComponent<ControlDireccion>().Habilitado = true;
	}
	
	void FinalizarCarrera()
	{		
		EstAct = GameManager.EstadoJuego.Finalizado;
		
		TiempoDeJuego = 0;
		
		if(Player1.Dinero > Player2.Dinero)
		{
			//lado que gano
			if(PlayerInfo1.LadoAct == Visualizacion.Lado.Der)
				DatosPartida.LadoGanadaor = DatosPartida.Lados.Der;
			else
				DatosPartida.LadoGanadaor = DatosPartida.Lados.Izq;
			
			//puntajes
			DatosPartida.PtsGanador = Player1.Dinero;
			DatosPartida.PtsPerdedor = Player2.Dinero;
		}
		else
		{
			//lado que gano
			if(PlayerInfo2.LadoAct == Visualizacion.Lado.Der)
				DatosPartida.LadoGanadaor = DatosPartida.Lados.Der;
			else
				DatosPartida.LadoGanadaor = DatosPartida.Lados.Izq;
			
			//puntajes
			DatosPartida.PtsGanador = Player2.Dinero;
			DatosPartida.PtsPerdedor = Player1.Dinero;
		}

		if (numOfPlayers == NumOfPlayers.Multiplayer)
		{ 
			Player1.GetComponent<Frenado>().Frenar();
			Player2.GetComponent<Frenado>().Frenar();
			
			Player1.ContrDesc.FinDelJuego();
			Player2.ContrDesc.FinDelJuego();
		}
        else
        {
			Player1.GetComponent<Frenado>().Frenar();
			Player1.ContrDesc.FinDelJuego();
        }
	}
	
	//se encarga de posicionar la camara derecha para el jugador que esta a la derecha y viseversa
	void SetPosicion(PlayerInfo pjInf)
	{
		pjInf.PJ.GetComponent<Visualizacion>().SetLado(pjInf.LadoAct);
		//en este momento, solo la primera vez, deberia setear la otra camara asi no se superponen
		pjInf.PJ.ContrCalib.IniciarTesteo();
		
		if(pjInf.PJ == Player1)
		{
			if(pjInf.LadoAct == Visualizacion.Lado.Izq)
				Player2.GetComponent<Visualizacion>().SetLado(Visualizacion.Lado.Der);
			else
				Player2.GetComponent<Visualizacion>().SetLado(Visualizacion.Lado.Izq);
		}
		else
		{
			if(pjInf.LadoAct == Visualizacion.Lado.Izq)
				Player1.GetComponent<Visualizacion>().SetLado(Visualizacion.Lado.Der);
			else
				Player1.GetComponent<Visualizacion>().SetLado(Visualizacion.Lado.Izq);
		}
	}
	
	void CambiarACarrera()
	{
		for(int i = 0; i < ObjsCarrera.Length; i++)
		{
			ObjsCarrera[i].SetActive(true);
		}

		for(int i = 0; i < ObjsCalibracion1.Length; i++)
		{
			ObjsCalibracion1[i].SetActive(false);
		}
		
		for(int i = 0; i < ObjsCalibracion2.Length; i++)
		{
			ObjsCalibracion2[i].SetActive(false);
		}

		if (numOfPlayers == NumOfPlayers.Multiplayer)
		{
			PlayerInfo1.FinCalibrado = true;
			PlayerInfo2.FinCalibrado = true;

			//posiciona los camiones dependiendo de que lado de la pantalla esten
			if (PlayerInfo1.LadoAct == Visualizacion.Lado.Izq)
			{
				Player1.gameObject.transform.position = PosCamionesCarrera[0];
				Player2.gameObject.transform.position = PosCamionesCarrera[1];
			}
			else
			{
				Player1.gameObject.transform.position = PosCamionesCarrera[1];
				Player2.gameObject.transform.position = PosCamionesCarrera[0];
			}

			Player1.transform.forward = Vector3.forward;
			Player1.GetComponent<Frenado>().Frenar();
			Player1.CambiarAConduccion();

			Player2.transform.forward = Vector3.forward;
			Player2.GetComponent<Frenado>().Frenar();
			Player2.CambiarAConduccion();

			//los deja andando
			Player1.GetComponent<Frenado>().RestaurarVel();
			Player2.GetComponent<Frenado>().RestaurarVel();
			//cancela la direccion
			Player1.GetComponent<ControlDireccion>().Habilitado = false;
			Player2.GetComponent<ControlDireccion>().Habilitado = false;
			//les de direccion
			Player1.transform.forward = Vector3.forward;
			Player2.transform.forward = Vector3.forward;
			
			EstAct = EstadoJuego.Jugando;
		}
        else
        {
			PlayerInfo1.FinCalibrado = true;

			//posiciona los camiones dependiendo de que lado de la pantalla esten
			Player1.gameObject.transform.position = PosCamionesCarrera[0];

			Player1.transform.forward = Vector3.forward;
			Player1.GetComponent<Frenado>().Frenar();
			Player1.CambiarAConduccion();

			//los deja andando
			Player1.GetComponent<Frenado>().RestaurarVel();
			//cancela la direccion
			Player1.GetComponent<ControlDireccion>().Habilitado = false;
			//les de direccion
			Player1.transform.forward = Vector3.forward;
			
			EstAct = GameManager.EstadoJuego.Jugando;
		}

	}
	
	public void FinTutorial(int playerID)
	{
        if (numOfPlayers == NumOfPlayers.Multiplayer)
        {
			if(playerID == 0)
			{
				PlayerInfo1.FinTuto2 = true;
				
			}else if(playerID == 1)
			{
				PlayerInfo2.FinTuto2 = true;
			}
			
			if(PlayerInfo1.FinTuto2 && PlayerInfo2.FinTuto2)
			{
				CambiarACarrera();
			}
        }
        else
        {
			if (playerID == 0)
			{
				PlayerInfo1.FinTuto2 = true;
			}

			if (PlayerInfo1.FinTuto2)
			{
				CambiarACarrera();
			}
		}
	}
	
	public void FinCalibracion(int playerID)
	{
		if (numOfPlayers == NumOfPlayers.Multiplayer)
		{
			if (playerID == 0)
			{
				PlayerInfo1.FinTuto1 = true;

			}
			else if (playerID == 1)
			{
				PlayerInfo2.FinTuto1 = true;
			}

			if (PlayerInfo1.PJ != null && PlayerInfo2.PJ != null)
				if (PlayerInfo1.FinTuto1 && PlayerInfo2.FinTuto1)
					CambiarACarrera();
		}
        else
        {
			if (playerID == 0)
			{
				PlayerInfo1.FinTuto1 = true;
			}

			if (PlayerInfo1.PJ != null)
				if (PlayerInfo1.FinTuto1)
					CambiarACarrera();
		}			
	}	
	
	[System.Serializable]
	public class PlayerInfo
	{
		public PlayerInfo(int tipoDeInput, Player pj)
		{
            TipoDeInput = tipoDeInput;
			PJ = pj;
		}
		
		public bool FinCalibrado = false;
		public bool FinTuto1 = false;
		public bool FinTuto2 = false;
		
		public Visualizacion.Lado LadoAct;

        public int TipoDeInput = -1;
		
		public Player PJ;
	}	
}
