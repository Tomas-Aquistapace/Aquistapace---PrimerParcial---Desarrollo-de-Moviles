public class CamionesInputManager : InputManager
{
    protected override InputCamion CreateInput(string player)
    {
        InputCamion input = null;
#if UNITY_EDITOR
        input = new CamionInputKeyTouch(player);
#elif UNITY_ANDROID || UNITY_IOS
        input = new CamionInputTouch(player);
#else
        input = new CamionInputKeys(player);
#endif
        return input;
    }
}