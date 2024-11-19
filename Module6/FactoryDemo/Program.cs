using System.Data.Common;
using Microsoft.Extensions.Configuration;
using System;
using Microsoft.Data.SqlClient;

/* Name : Emma Sturm
 * Date: 11/18/2024
 * CTEC 135
 * Assignment PA6-2: Create a factory model!*/ 


namespace FactoryDemo
{
    internal class Program
    {
        //Main Function
        static void Main(string[] args)
        {

            //Print out title of output
            Console.WriteLine("Factory Model Example\n");

            //Store the results from get provider config into a variable containing both 
            //the provider and connectionString
            var (provider, connectionString) = GetProviderFromConfiguation();

            //Write the provider info to output
            Console.WriteLine($"\tProvider: {provider}");

            //Write the Connection info to output
            Console.WriteLine($"\tConnectionString: {connectionString}");

            //Initialize the factory from provider given
            DbProviderFactory factory = GetDbProviderFactory(provider);

            //Create a connection to database using the factory
            using (DbConnection connection = factory.CreateConnection())
            {
                //Print out connection type
                Console.WriteLine($"\tYour connection object is a: {connection.GetType().Name}");

                connection.ConnectionString = connectionString;
                
                //Open the connection with database
                connection.Open();

                //Creation of a database command object
                DbCommand command = factory.CreateCommand();

                //Print out the type of command
                Console.WriteLine($"\tYour command object is a: {command.GetType().Name}");

                //Connect the command with the connection
                command.Connection = connection;

                //Retrieve data from the tables in the database
                command.CommandText = "\tSelect i.Id, m.Name From Inventory i inner join Makes m on m.Id = i.MakeId";
                
                //Initialize data reader
                using (DbDataReader reader = command.ExecuteReader())
                {
                    //Print data reader type
                    Console.WriteLine($"\tYour data reader object is a : {reader.GetType().Name}");
                    
                    //Print inventory title for output
                    Console.WriteLine("\n\t *** Current Inventory ***");

                    //While the reader has stuff to read from the data, print out the
                    //Id and Name of the car. 
                    while (reader.Read())
                    {
                        Console.WriteLine($"\t-> Car # {reader["Id"]} is a {reader["Name"]}");
                    }
                }
            }
        

        //This method gets the correct factory for the provider
        static DbProviderFactory GetDbProviderFactory(string provider)
        {
                //If provider is a Sql Server...
                if (provider == "SqlServer")
                {
                    //Return that factory instance
                    return SqlClientFactory.Instance;
                }

                //Otherwise if it is not a Sql Server, return a null factory
                else return null;
    
        }

        //This method gets the provider andn connectionString info
        static (string Provider, string ConnectionString) GetProviderFromConfiguation()
        {
                //Config build to fill config file
                var configuration = new ConfigurationBuilder()
                            //Set base path to current directory
                           .SetBasePath(Directory.GetCurrentDirectory())

                           //Add the json file
                           .AddJsonFile("appsettings.json")

                           //Build config
                           .Build();

            //Initialize the providername
            var providerName = configuration["ProviderName"];

            //Initialize the connection String
            var connectionString = configuration[$"{providerName}:ConnectionString"];
            
            //Return with both the providerName and the connectionString
            return (providerName, connectionString);
        }
    }

}


}
