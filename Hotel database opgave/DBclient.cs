using Microsoft.Data.SqlClient;

namespace Hotel_database_opgave
{
    public class DBclient
    {
        string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=HotelDB5;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

        private int GetMaxFacilityId(SqlConnection connection)
        {
            Console.WriteLine("Calling -> GetMaxFacilityNo");

            
            string queryStringMaxFacilityNo = "SELECT MAX (Facilitet_No)  FROM Faciliteter";
            Console.WriteLine($"SQL applied: {queryStringMaxFacilityNo}");

            //Apply SQL command
            SqlCommand command = new SqlCommand(queryStringMaxFacilityNo, connection);
            SqlDataReader reader = command.ExecuteReader();

            
            int MaxFacility_No = 0;

            //Is there any rows in the query
            if (reader.Read())
            {
               
                MaxFacility_No = reader.GetInt32(0); //Reading int fro 1st column
            }

            //Close reader
            reader.Close();

            Console.WriteLine($"Max Facility No#: {MaxFacility_No}");
            Console.WriteLine();

           
            return MaxFacility_No;
        }

        private int DeleteFacility(SqlConnection connection, int Facilitet_No)
        {
            Console.WriteLine("Calling -> DeleteFacility");

            
            string deleteCommandString = $"DELETE FROM Faciliteter  WHERE Facilitet_No = {Facilitet_No}";
            Console.WriteLine($"SQL applied: {deleteCommandString}");

            
            SqlCommand command = new SqlCommand(deleteCommandString, connection);
            Console.WriteLine($"Deleting Facility #{Facilitet_No}");
            int numberOfRowsAffected = command.ExecuteNonQuery();

            Console.WriteLine($"Number of rows affected: {numberOfRowsAffected}");
            Console.WriteLine();

            //Return number of rows affected
            return numberOfRowsAffected;
        }

        private int UpdateFacility(SqlConnection connection, Faciliteter facility)
        {
            Console.WriteLine("Calling -> UpdateFacility");

            
            string updateCommandString = $"UPDATE Faciliteter SET Name='{facility.Name}' WHERE Facilitet_No = {facility.Facilitet_No}";
            Console.WriteLine($"SQL applied: {updateCommandString}");

            //Apply SQL command
            SqlCommand command = new SqlCommand(updateCommandString, connection);
            Console.WriteLine($"Updating facility #{facility.Facilitet_No}");
            int numberOfRowsAffected = command.ExecuteNonQuery();

            Console.WriteLine($"Number of rows affected: {numberOfRowsAffected}");
            Console.WriteLine();

            //Return number of rows affected
            return numberOfRowsAffected;
        }

        private int InsertFacility(SqlConnection connection, Faciliteter facility)
        {
            Console.WriteLine("Calling -> InsertFacility");

            
            string insertCommandString = $"INSERT INTO Faciliteter VALUES({facility.Facilitet_No}, '{facility.Name}')";
            Console.WriteLine($"SQL applied: {insertCommandString}");

            //Apply SQL command
            SqlCommand command = new SqlCommand(insertCommandString, connection);

            Console.WriteLine($"Creating facility #{facility.Facilitet_No}");
            int numberOfRowsAffected = command.ExecuteNonQuery();

            Console.WriteLine($"Number of rows affected: {numberOfRowsAffected}");
            Console.WriteLine();

            //Return number of rows affected 
            return numberOfRowsAffected;
        }

        private List<Faciliteter> ListAllFacilities(SqlConnection connection)
        {
            Console.WriteLine("Calling -> ListAllFacilities");

            
            string queryStringAllFacilities = "SELECT * FROM Faciliteter";
            Console.WriteLine($"SQL applied: {queryStringAllFacilities}");

            //Apply SQL command
            SqlCommand command = new SqlCommand(queryStringAllFacilities, connection);
            SqlDataReader reader = command.ExecuteReader();

            Console.WriteLine("Listing all facilities:");

            //NO rows in the query 
            if (!reader.HasRows)
            {
                //End here
                Console.WriteLine("No facilities in database");
                reader.Close();

               
                return null;
            }

           
            List<Faciliteter> facilities = new List<Faciliteter>();
            while (reader.Read())
            {
                
                Faciliteter nextfacility = new Faciliteter()
                {
                    Facilitet_No = reader.GetInt32(0), //Reading int from 1st column
                    Name = reader.GetString(1),    //Reading string from 2nd column
                   
                };

                
                facilities.Add(nextfacility);

                Console.WriteLine(nextfacility);
            }

            //Close reader
            reader.Close();
            Console.WriteLine();

            
            return facilities;
        }

        private Faciliteter Getfacilities(SqlConnection connection, int Facilitet_no)
        {
            Console.WriteLine("Calling -> GetFacility");

            
            string queryStringOneFacility = $"SELECT * FROM Faciliteter WHERE Facilitet_no = {Facilitet_no}";
            Console.WriteLine($"SQL applied: {queryStringOneFacility}");

            //Prepare SQK command
            SqlCommand command = new SqlCommand(queryStringOneFacility, connection);
            SqlDataReader reader = command.ExecuteReader();

            Console.WriteLine($"Finding facility#: {Facilitet_no}");

            //NO rows in the query? 
            if (!reader.HasRows)
            {
                //End here
                Console.WriteLine("No facilities in database");
                reader.Close();

                
                return null;
            }

           
            Faciliteter facilities = null;
            if (reader.Read())
            {
                facilities = new Faciliteter()
                {
                    Facilitet_No = reader.GetInt32(0), //Reading int fro 1st column
                    Name = reader.GetString(1),    //Reading string from 2nd column
                    
                };

                Console.WriteLine(facilities);
            }

            //Close reader
            reader.Close();
            Console.WriteLine();

            
            return facilities;
        }
        public void Start()
        {
            
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //Open connection
                connection.Open();

              
                ListAllFacilities(connection);

              
                Faciliteter newFacility = new Faciliteter()
                {
                    Facilitet_No = GetMaxFacilityId(connection) + 1,
                    Name = "New Facility",
                    
                };

               
                InsertFacility(connection, newFacility);

                ListAllFacilities(connection);

                //Get the newly inserted facility from the database in order to update it 
                Faciliteter facilityToBeUpdated = Getfacilities(connection, newFacility.Facilitet_No);

                //Alter Name properties
                facilityToBeUpdated.Name += "(updated)";
              

                //Update the facility in the database 
                UpdateFacility(connection, facilityToBeUpdated);

                //List all facility including the updated one
                ListAllFacilities(connection);

                //Get the updated facility in order to delete it
                Faciliteter facilityToBeDeleted = Getfacilities(connection, facilityToBeUpdated.Facilitet_No);

                //Delete the facility
                DeleteFacility(connection, facilityToBeDeleted.Facilitet_No);

                //List all facility - now without the deleted one
                ListAllFacilities(connection);
            }
        }
    }
}
    

