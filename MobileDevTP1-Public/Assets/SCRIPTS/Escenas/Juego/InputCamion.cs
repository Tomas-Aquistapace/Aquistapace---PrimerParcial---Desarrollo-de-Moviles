using UnityEngine;

/// <summary>
/// Patron "Factory"
/// </summary>

public abstract class InputCamion
{    
    public enum Buttons
    {
        Start,
        Left,
        Right,
        Down
    }

    public string Player = "";

    public void SetPlayer(string player)
    {
        Player = player;
    }

    public abstract bool GetButton(Buttons btn);
    public abstract float GetHorizontalAxis();
    public abstract void SetHorizontal(float val);
}



public class CamionInputKeys : InputCamion
{
    string ButtonToStr(Buttons btn)
    {
        switch (btn)
        {
            case Buttons.Start:
                return $"Start{Player}";
            case Buttons.Left:
                return $"Left{Player}";
            case Buttons.Right:
                return $"Right{Player}";
            case Buttons.Down:
                return $"Down{Player}";
            default:
                return "";
        }
    } 

    public CamionInputKeys(string player)
    {
        SetPlayer(player);
    }

    public override bool GetButton(Buttons btn)
    {
        string btnStr = ButtonToStr(btn);
        return Input.GetButton(btnStr);
    }

    public override float GetHorizontalAxis()
    {
        return Input.GetAxis($"Horizontal{Player}");
    }

    public override void SetHorizontal(float val)
    {
        // ....
    }
}



public class CamionInputTouch : InputCamion
{
    public CamionInputTouch(string player)
    {
        SetPlayer(player);
    }

    float horizontal = 0f;

    public override bool GetButton(Buttons btn)
    {
        return false;
    }

    public override float GetHorizontalAxis()
    {
        return horizontal;
    }

    public override void SetHorizontal(float val)
    {
        horizontal = val;
    }
}



public class CamionInputKeyTouch : InputCamion
{
    CamionInputKeys camionInputKeys;
    CamionInputTouch camionInputTouch;

    public CamionInputKeyTouch(string player)
    {
        camionInputKeys = new CamionInputKeys(player);
        camionInputTouch = new CamionInputTouch(player);

        SetPlayer(player);
    }

    public override bool GetButton(Buttons btn)
    {
        return camionInputKeys.GetButton(btn);
    }

    public override float GetHorizontalAxis()
    {
        return camionInputTouch.GetHorizontalAxis() + camionInputKeys.GetHorizontalAxis();
    }

    public override void SetHorizontal(float val)
    {
        camionInputTouch.SetHorizontal(val);
    }
}