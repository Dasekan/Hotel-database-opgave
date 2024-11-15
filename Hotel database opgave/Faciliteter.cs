namespace Hotel_database_opgave
{
    public class Faciliteter
    {
        public int Facilitet_No { get; set; }

        public string Name { get; set; }

        public override string ToString()
        {
            return $"ID: {Facilitet_No}, Name: {Name}";
        }
    }
}
