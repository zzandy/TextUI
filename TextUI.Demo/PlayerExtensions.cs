namespace TextUI.Demo
{
    public static class PlayerExtensions
    {
        public static string Name(this Player player)
        {
            return player.FirstName + " " + player.LastName;
        }
    }
}