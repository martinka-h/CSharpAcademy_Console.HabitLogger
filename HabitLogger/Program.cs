using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Numerics;
using System;
using System.Runtime.CompilerServices;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
                    break;
                case "2":
                    Insert();
                    break;
                /*case "3":
                    Delete();
                    break;
                case "4":
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

}