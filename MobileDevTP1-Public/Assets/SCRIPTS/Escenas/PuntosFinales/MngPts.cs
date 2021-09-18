using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MngPts : MonoBehaviour 
{

	[SerializeField] TextMeshProUGUI jugadorText;
	[SerializeField] TextMeshProUGUI dineroPj1Text;
	[SerializeField] TextMeshProUGUI dineroPj2Text;

	//---------------------------------//
	
	void Start()
	{
		SetGanador();
		SetDinero();
	}

	//---------------------------------//
	
	void SetGanador()
	{
		switch(DatosPartida.LadoGanadaor)
		{
		case DatosPartida.Lados.Der:
			jugadorText.text = "Jugador 2";
			break;
			
		case DatosPartida.Lados.Izq:
			jugadorText.text = "Jugador 1";			
			break;
		}
	}
	
	void SetDinero()
	{
		if(DatosPartida.LadoGanadaor == DatosPartida.Lados.Izq)
			dineroPj1Text.text = DatosPartida.PtsGanador.ToString();
		else
			dineroPj1Text.text = DatosPartida.PtsPerdedor.ToString();


		if(DatosPartida.LadoGanadaor == DatosPartida.Lados.Der)
			dineroPj2Text.text = DatosPartida.PtsGanador.ToString();
		else
			dineroPj2Text.text = DatosPartida.PtsPerdedor.ToString();
	}

	public void ExitScene(string scene)
    {
		SceneManager.LoadScene(scene);
    }
}
