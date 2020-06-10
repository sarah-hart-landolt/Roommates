using Microsoft.Data.SqlClient;
using Roommates.Models;
using System;
using System.Collections.Generic;

namespace Roommates.Repositories
{
    /// <summary>
    ///  This class is responsible for interacting with Room data.
    ///  It inherits from the BaseRepository class so that it can use the BaseRepository's Connection property
    /// </summary>
    public class RoommateChoreRepository : BaseRepository
    {
        /// <summary>
        ///  When new RoomRespository is instantiated, pass the connection string along to the BaseRepository
        /// </summary>
        public RoommateChoreRepository(string connectionString) : base(connectionString) { }

        /// <summary>
        ///  Get a list of all Rooms in the database
        /// </summary>
        public List<RoommateChore> GetAll()
        {
            //  We must "use" the database connection.
            //  Because a database is a shared resource (other applications may be using it too) we must
            //  be careful about how we interact with it. Specifically, we Open() connections when we need to
            //  interact with the database and we Close() them when we're finished.
            //  In C#, a "using" block ensures we correctly disconnect from a resource even if there is an error.
            //  For database connections, this means the connection will be properly closed.
            using (SqlConnection conn = Connection)
            {
                // Note, we must Open() the connection, the "using" block doesn't do that for us.
                conn.Open();

                // We must "use" commands too.
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    // Here we setup the command with the SQL we want to execute before we execute it.
                    cmd.CommandText = "SELECT Id, RoommateId, ChoreId FROM RoommateChore";

                    // Execute the SQL in the database and get a "reader" that will give us access to the data.
                    SqlDataReader reader = cmd.ExecuteReader();

                    // A list to hold the roommateChores we retrieve from the database.
                    List<RoommateChore> roommateChores = new List<RoommateChore>();

                    // Read() will return true if there's more data to read
                    while (reader.Read())
                    {
                        // The "ordinal" is the numeric position of the column in the query results.
                        //  For our query, "Id" has an ordinal value of 0 and "Name" is 1.
                        int idColumnPosition = reader.GetOrdinal("Id");

                        // We user the reader's GetXXX methods to get the value for a particular ordinal.
                        int idValue = reader.GetInt32(idColumnPosition);

                        int RoommateIdPosition = reader.GetOrdinal("RoommateId");
                        int roommateIdValue = reader.GetInt32(RoommateIdPosition);

                        int ChoreIdPosition = reader.GetOrdinal("ChoreId");
                        int choreIdValue = reader.GetInt32(ChoreIdPosition);

                        // Now let's create a new roommateChore object using the data from the database.
                        RoommateChore roommateChore = new RoommateChore
                        {
                            Id = idValue,
                            RoommateId = roommateIdValue,
                            ChoreId = choreIdValue,
                        };

                        // ...and add that roommateChore object to our list.
                        roommateChores.Add(roommateChore);
                    }

                    // We should Close() the reader. Unfortunately, a "using" block won't work here.
                    reader.Close();

                    // Return the list of roommateChores who whomever called this method.
                    return roommateChores;
                }
            }
        }
        /// <summary>
        ///  Returns a single roommateChore with the given id.
        /// </summary>

        public RoommateChore GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT RoommateId, ChoreId FROM RoommateChore WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    RoommateChore roommateChore = null;

                    // If we only expect a single row back from the database, we don't need a while loop.
                    if (reader.Read())
                    {
                        roommateChore = new RoommateChore
                        {
                            Id = id,
                            RoommateId = reader.GetInt32(reader.GetOrdinal("RoommateId")),
                            ChoreId = reader.GetInt32(reader.GetOrdinal("ChoreId")),
                        };
                    }

                    reader.Close();

                    return roommateChore;
                }
            }
        }

        /// <summary>
        ///  Add a new roommateChore to the database
        ///   NOTE: This method sends data to the database,
        ///   it does not get anything from the database, so there is nothing to return.
        /// </summary>
        public void Insert(RoommateChore roommateChore)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    // These SQL parameters are annoying. Why can't we use string interpolation?
                    // ... sql injection attacks!!!
                    cmd.CommandText = @"INSERT INTO roommateChore (RoommateId, ChoreId) 
                                         OUTPUT INSERTED.Id 
                                         VALUES (@RoommateId, @ChoreId)";
                    cmd.Parameters.AddWithValue("@RoommateId", roommateChore.RoommateId);
                    cmd.Parameters.AddWithValue("@ChoreId", roommateChore.ChoreId);
                    int id = (int)cmd.ExecuteScalar();

                    roommateChore.Id = id;
                }
            }

            // when this method is finished we can look in the database and see the new roommateChore.
        }

        /// <summary>
        ///  Updates the roommateChore
        /// </summary>
        public void Update(RoommateChore roommateChore)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE RoommateChore
                                    SET RoommateId = @RoommateId,
                                        ChoreId = @ChoreId
                                    WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@RoommateId", roommateChore.RoommateId);
                    cmd.Parameters.AddWithValue("@ChoreId", roommateChore.ChoreId);
                    cmd.Parameters.AddWithValue("@id", roommateChore.Id);

                    cmd.ExecuteNonQuery();
                }
            }


        }
        /// <summary>
        ///  Delete the roommateChore with the given id
        /// </summary>
        public void Delete(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM RoommateChore WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}



