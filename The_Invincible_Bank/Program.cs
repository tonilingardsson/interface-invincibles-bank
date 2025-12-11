namespace The_Invincible_Bank
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            var theBank = new Bank();
            theBank.Run();

        }
    }
}
