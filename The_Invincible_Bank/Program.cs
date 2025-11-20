namespace The_Invincible_Bank
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var theBank = new Bank();
            //theBank.Run();

            UI.DisplayLoggo();
            UI.DisplayMessage("Hello fellow creature!\nMy name is The Bank, and im here to...\nEAT YOUR SOUL");
        }
    }
}
