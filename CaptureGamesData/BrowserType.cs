namespace CaptureGamesData
{
    public enum BrowserType
    {
        //Info: This web driver will not show any explicit windows
        //when it's running.
        PhantomJSDriver = 0,

        //Warning: Default IE version is 11.0
        IE = 1,

        //Warning: Chrome driver has specific version.
        //The chrome browser which is installed in your test machine should match
        //the chrome driver version.
        Chrome = 2,

        Firefox = 3,

        Edge = 4
    }
}
