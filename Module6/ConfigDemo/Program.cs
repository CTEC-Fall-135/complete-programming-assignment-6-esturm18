using Microsoft.Extensions.Configuration;
using System;

/* Name: Emma Sturm
 * Date: 11/18/24
 * CTEC 135
 * Assignment PA-61: Print out provider information*/ 
class Program
{
    public static void Main(string[] args)
    {
        //Get values from appsettings file by calling getInfo method
        string[] values = GetInfo();

        //Print message
        Console.WriteLine("Provider Info: ");
        Console.WriteLine($"\tProvider: {values[0]}");
        Console.WriteLine($"\tConnectionString: {values[1]}");
    }
    
    //Method to get appsettings information
    public static string[] GetInfo()

    {
        //Get config object
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        //Get provider and config info from appsettings
        string provider = config["ProviderName"];
        string connectionString = config["SqlServer:ConnectionString"];

        //Put them into an array of strings to then return to main function
        return new string[] { provider, connectionString };
    }
}
