using UnityEngine;
using System.Collections;

public class ControlDireccion : MonoBehaviour 
{
	public bool Habilitado = true;
	public string playerNumber = "1";

	InputCamion input;
	float Giro = 0;

    //---------------------------------------------------------//

    private void Start()
    {
		input = InputManager.Instance.GetInput(playerNumber);
    }

    void Update ()
	{
		Giro = input.GetHorizontalAxis();

        if (!Habilitado)
        {
			return;
        }

		gameObject.SendMessage("SetGiro", Giro);
	}

	public float GetGiro()
	{		
		return Giro;
	}	
}
