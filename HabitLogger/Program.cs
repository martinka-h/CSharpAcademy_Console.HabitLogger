using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Numerics;
using System;
using System.Runtime.CompilerServices;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Globalization;
using System.Data;

class Program
{
    static string connectionString = @"Data Source = habitlogger.db";

    static void Main(string[] args)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText =
                @"CREATE TABLE IF NOT EXISTS drinking_water (
Id INTEGER PRIMARY KEY AUTOINCREMENT,
DATE TEXT,
Quantity INTEGER)";

            tableCmd.ExecuteNonQuery();
            connection.Close();
        }

        GetUserInput();
    }
    static void GetUserInput()
    {
        Console.Clear();
        bool closeApp = false;
        while (!closeApp)
        {
            Console.WriteLine("\n\nMAIN MENU");
            Console.WriteLine("\nWhat would you like to do?");
            Console.WriteLine(
    @"
Type 0 to exit the app
Type 1 to view all records.
Type 2 to insert record.
Type 3 to delete record.
Type 4 to update record.
---------------------------------------------");

            string? command = Console.ReadLine();

            switch (command)
            {
                case "0":
                    Console.WriteLine("\nGoodbye!\n");
                    closeApp = true;
                    break;
                case "1":
                    GetAllRecords();
                    break;
                case "2":
                    Insert();
                    break;
                case "3":
                    DeleteRecord();
                    break;
               /* case "4":
                    Update();
                    break;*/
                default:
                    Console.WriteLine("\nInvalid Command. Please type a number from 0 to 4.\n");
                    break;
            }
        }
    }
    static void Insert()
    {
        string date = GetDateInput();
        int quantity = GetNumberInput("\n\nPlease insert the number of glasses or other measure of your choice(no decimals allowed). Type 0 to return to main menu.\n\n");

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText =
                $"INSERT INTO drinking_water(date, quantity) VALUES('{date}', {quantity})";
            tableCmd.ExecuteNonQuery();
            connection.Close();
        }
    }

    static string GetDateInput()
    {
        Console.WriteLine("\n\nPlease insert the date: (Format: dd-mm-yy). Type 0 to return to main menu.");
        string? dateInput = Console.ReadLine();

        if (dateInput == "0") GetUserInput();

        return dateInput;
    }

    static int GetNumberInput(string message)
    {
        Console.WriteLine(message);

        string? numberInput = Console.ReadLine();

        if (numberInput == "0") GetUserInput();

        int finalInput = Convert.ToInt32(numberInput);

        return finalInput;
    }

    static void GetAllRecords()
    {
        Console.Clear();
        using(var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText =
                $"SELECT * FROM drinking_water ";

            List<DrinkingWater> tableData = new();

            SqliteDataReader reader = tableCmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    tableData.Add(
                        new DrinkingWater
                        {
                            Id = reader.GetInt32(0),
                            Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yy", new CultureInfo("en-US")),
                            Quantity = reader.GetInt32(2),
                        });
                }
            } else
            {
                Console.WriteLine("No rows found");
            }

            connection.Close();

            Console.WriteLine("-------------------------------------------------\n");
            foreach (var dw in tableData)
            {
                Console.WriteLine($"{dw.Id} - {dw.Date.ToString("dd-MM-yyyy")} - Quantity: {dw.Quantity}");
            }
            Console.WriteLine("-------------------------------------------------\n");

        }
    }

    private static void DeleteRecord()
    {
        Console.Clear();
        GetAllRecords();

        var recordId = GetNumberInput("\nPlease type the Id of the record you want to delete or type 0 to return to main menu.");

        if (recordId == 0)
        {
            GetUserInput();
        }

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"DELETE from drinking_water WHERE Id = '{recordId}'";

            int rowCount = tableCmd.ExecuteNonQuery();

            if (rowCount == 0)
            {
                Console.WriteLine($"\nRecord with Id {recordId} doesn't exist;\n");
                DeleteRecord();
            }

            Console.WriteLine($"\nRecord with Id {recordId} was deleted \n");
            GetUserInput();
        }
    }

}

public class DrinkingWater
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int Quantity { get; set; }
}