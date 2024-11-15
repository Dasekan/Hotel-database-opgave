namespace Hotel_database_opgave
{
    class Program
    {
        static void Main(string[] args)
        {
            DBclient dbc = new DBclient();
            dbc.Start();
        }
    }
}
