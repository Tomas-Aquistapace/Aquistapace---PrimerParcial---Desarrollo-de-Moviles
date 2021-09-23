using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// clase encargada de TODA la visualizacion
/// de cada player, todo aquello que corresconda a 
/// cada seccion de la pantalla independientemente
/// </summary>
public class Visualizacion : MonoBehaviour 
{
	public enum Lado{Izq, Der}
	public Lado LadoAct;
	
	//ControlDireccion Direccion;
	Player Pj;
	//[SerializeField] GameObject playerCanvas;

	//las distintas camaras
	[Header("Camaras")]
	public Camera CamCalibracion;
	public Camera CamConduccion;
	public Camera CamDescarga;
	
	[Header("Canvas Jugador")]
	public GameObject playerGameHUD;
	public GameObject playerCalibracionHUD;

	//EL DINERO QUE SE TIENE
	[Header("Dinero")]
	[SerializeField] TextMeshProUGUI dineroUI;

	//PARA EL INVENTARIO
	[Header("Inventario")]	
	public float Parpadeo = 0.8f;
	float TempParp = 0f;
	bool PrimIma = true;

	public GameObject inventory;
	public List<Sprite> inventoryState = new List<Sprite>();

	//BONO DE DESCARGA
	[Header("Bono de Descarga")]
	[SerializeField] GameObject descargaPanel;
	[SerializeField] RectTransform greenFillingBonus;
	[SerializeField] TextMeshProUGUI moneyStored;
	[SerializeField] float smoothTime = 0.015f;
	float greenTime = 1f;

	//NUMERO DEL JUGADOR
	[Header("Num Juagador")]
	public Texture2D TextNum1; 
	public Texture2D TextNum2;
	public GameObject Techo;
	
	//Rect R;
	
	//------------------------------------------------------------------//
	
	void Start()
	{
		//Direccion = GetComponent<ControlDireccion>();
		Pj = GetComponent<Player>();

        if (this.gameObject.activeSelf)
        {
			playerCalibracionHUD.SetActive(true);

			playerGameHUD.SetActive(true);

			descargaPanel.SetActive(false);
		}
	}
	
	void Update() 
	{
		switch (Pj.EstAct)
		{
			case Player.Estados.EnConduccion:
				//inventario
				SetInv();
				//contador de dinero
				SetDinero();
				break;
		
			case Player.Estados.EnDescarga:
				//inventario
				SetInv();
				//el bonus
				SetBonus();
				//contador de dinero
				SetDinero();
				break;
		}
	}
	
	//--------------------------------------------------------//
	
	public void CambiarACalibracion()
	{
		CamCalibracion.enabled = true;
		CamConduccion.enabled = false;
		CamDescarga.enabled = false;

		playerGameHUD.SetActive(false);
		playerCalibracionHUD.SetActive(true);
	}
	
	public void CambiarATutorial()
	{
		CamCalibracion.enabled = false;
		CamConduccion.enabled = true;
		CamDescarga.enabled = false;
	}
	
	public void CambiarAConduccion()
	{
		CamCalibracion.enabled = false;
		CamConduccion.enabled = true;
		CamDescarga.enabled = false;

		playerCalibracionHUD.SetActive(false);
		playerGameHUD.SetActive(true);
		descargaPanel.SetActive(false);
	}
	
	public void CambiarADescarga()
	{
		CamCalibracion.enabled = false;
		CamConduccion.enabled = false;
		CamDescarga.enabled = true;

		playerGameHUD.SetActive(true);
		descargaPanel.SetActive(true);
	}
	
	//---------//
	
	public void SetLado(Lado lado)
	{
		LadoAct = lado;
		
		Rect r = new Rect();
		r.width = CamConduccion.rect.width;
		r.height = CamConduccion.rect.height;
		r.y = CamConduccion.rect.y;
		
		switch (lado)
		{
		case Lado.Der:
			r.x = 0.5f;
			break;
			
			
		case Lado.Izq:
			r.x = 0;
			break;
		}
		
		CamCalibracion.rect = r;
		CamConduccion.rect = r;
		CamDescarga.rect = r;
		
		if(LadoAct == Visualizacion.Lado.Izq)
		{
			Techo.GetComponent<Renderer>().material.mainTexture = TextNum1;
		}
		else
		{
			Techo.GetComponent<Renderer>().material.mainTexture = TextNum2;
		}
	}
	
	void SetBonus()
	{
		if(Pj.ContrDesc.PEnMov != null)
		{
			greenFillingBonus.localScale = new Vector3(1, greenTime, 1);

			if(greenTime > 0)
				greenTime -= smoothTime * Time.deltaTime;

			moneyStored.text = Pj.ContrDesc.Bonus.ToString("0");
		}
        else
        {
			greenTime = 1f;
        }
	}
	
	void SetDinero()
	{
		dineroUI.text = Pj.Dinero.ToString();
	}
	
	void SetInv()
    {
		int contador = 0;
		for (int i = 0; i < 3; i++)     // Pregunta Cuantas bolsas tiene ============= \\
		{
			if (Pj.Bolasas[i] != null)
				contador++;
		}

        switch (contador)
        {
			case 0:
				inventory.GetComponent<Image>().sprite = inventoryState[0];
				break;
			
			case 1:
				inventory.GetComponent<Image>().sprite = inventoryState[1];
				break;
			
			case 2:
				inventory.GetComponent<Image>().sprite = inventoryState[2];
				break;

			case 3:
				if(TempParp <= Parpadeo)
                {
					TempParp += Time.deltaTime;
                }
                else
                {
					TempParp = 0f;
					PrimIma = !PrimIma;
                }

                if (PrimIma)
                {
					inventory.GetComponent<Image>().sprite = inventoryState[3];
                }
                else
                {
					inventory.GetComponent<Image>().sprite = inventoryState[4];
                }
				break;
        }
	}
	
	public string PrepararNumeros(int dinero) // Se Usa para la escena final
	{
		string strDinero = dinero.ToString();
		string res = "";
		
		if(dinero < 1)//sin ditero
		{
			res = "";
		}else if(strDinero.Length == 6)//cientos de miles
		{
			for(int i = 0; i < strDinero.Length; i++)
			{
				res += strDinero[i];
				
				if(i == 2)
				{
					res += ".";
				}
			}
		}else if(strDinero.Length == 7)//millones
		{
			for(int i = 0; i < strDinero.Length; i++)
			{
				res += strDinero[i];
				
				if(i == 0 || i == 3)
				{
					res += ".";
				}
			}
		}
		
		return res;
	}	
}
