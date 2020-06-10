using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Roommates.Models;
using Roommates.Repositories;

namespace Roommates
{
    class Program
    {
        /// <summary>
        ///  this is the address of the database.
        ///  we define it here as a constant since it will never change.
        /// </summary>
        private const string CONNECTION_STRING = @"server=localhost\SQLExpress;database=Roommates;integrated security=true";

        static void Main(string[] args)
        {
            RoomRepository roomRepo = new RoomRepository(CONNECTION_STRING);

            //Console.WriteLine("Getting All Roommates:");
            //Console.WriteLine();

            //List<Room> allRooms = roomRepo.GetAll();


            //foreach (Room room in allRooms)
            //{
            //    Console.WriteLine($"{room.Id} {room.Name} {room.MaxOccupancy}");
            //}

            RoommateRepository roommateRepo = new RoommateRepository(CONNECTION_STRING);
            ChoreRepository choreRepo = new ChoreRepository(CONNECTION_STRING);

            //Console.WriteLine("Getting All Rooms:");
            //Console.WriteLine();

            //List<Roommate> allRoommatesWithRoom = roommateRepo.GetAllWithRoom();



            //            foreach (var roommate in allRoommatesWithRoom)
            //            {
            //                Console.WriteLine(@$"{roommate.Id} 
            //{roommate.FirstName} {roommate.LastName}
            //{roommate.RentPortion} {roommate.MoveInDate}
            //{roommate.Room.Name}");
            //            }

            while (true)
            {
                Console.WriteLine();

                int selection = Menu();
                switch (selection)
                {
                    case 0:
                        Console.WriteLine("Goodbye");
                        return;
                    default:
                        throw new Exception("Something went wrong...invalid selection");

                    case 1:
                        List<Room> allRooms = roomRepo.GetAll();
                        foreach (Room room in allRooms)
                        {
                            Console.WriteLine($"{room.Id} {room.Name} {room.MaxOccupancy}");
                        }
                        break;
                    case 2:
                        Console.Write($"Enter the name of the new room to add: ");
                        string newName = Console.ReadLine();
                        Console.Write($"Enter {newName}'s Max Occupancy: ");
                        int newMaxOcc = Int32.Parse(Console.ReadLine());


                        Room AddedRoom = new Room
                        {
                            Name = newName,
                            MaxOccupancy = newMaxOcc
                        };

                        roomRepo.Insert(AddedRoom);

                        Console.WriteLine("-------------------------------");
                        Console.WriteLine($"Added the new Room {AddedRoom.Name} with id {AddedRoom.Id}");
                        break;

                    case 3:
                        Console.Write($"Enter Id of a room you want to delete: ");
                        int roomId = Int32.Parse(Console.ReadLine());

                        roomRepo.Delete(roomId);

                        Console.WriteLine("-------------------------------");
                        Console.WriteLine($"Deleted the room with id {roomId}");
                        break;
                    case 4:
                        Console.Write($"Enter the id of the room you'd like to edit: ");
                        int UpdatedRoomId = Int32.Parse(Console.ReadLine());
                        var selectedRoom = roomRepo.GetById(UpdatedRoomId);

                        Console.Write($"Enter a new name for {selectedRoom.Name} to update database: ");
                        string UpdatedName = Console.ReadLine();

                        Console.Write($"Enter a new Max Occupancy for {selectedRoom.Name} to update database: ");
                        int UpdatedMaxOcc = Int32.Parse(Console.ReadLine());

                        Room UpdatedRoom = new Room
                        {
                            Id = UpdatedRoomId,
                            Name = UpdatedName,
                            MaxOccupancy = UpdatedMaxOcc
                        };

                        roomRepo.Update(UpdatedRoom);

                        Console.WriteLine("-------------------------------");
                        Console.WriteLine($"Updated the {UpdatedRoom}");
                        break;

                    case 5: //List all roommates
                        List<Roommate> allRoommates = roommateRepo.GetAll();
                        foreach (Roommate roommate in allRoommates)
                        {

                            Console.WriteLine(@$"
                                Id: {roommate.Id}
                                {roommate.FirstName} {roommate.LastName}
                                Rent Portion: {roommate.RentPortion}
                                Move In Date: {roommate.MoveInDate}
                            ");

                        }
                        break;

                    case 6: //Add a roommate
                        Console.Write($"Enter the name of the new roommate's first name: ");
                        string newFirstName = Console.ReadLine();
                        Console.Write($"Enter the name of the new roommate's last name: ");
                        string newLastName = Console.ReadLine();
                        Console.Write($"Enter {newFirstName}'s Rent share percentage (Enter a number 0-100): ");
                        int RentPortion = Int32.Parse(Console.ReadLine());
                        DateTime MoveInDate = DateTime.Now;
                        List<Room> allRoomsAgain = roomRepo.GetAll();

                        Console.WriteLine($"Enter {newFirstName}'s room id to rent from the list");
                        foreach (Room room in allRoomsAgain)
                        {
                            Console.WriteLine($"Room Id: {room.Id} Room : {room.Name}:");

                        }
                        Console.Write($"> ");

                        int NewRoomId = Int32.Parse(Console.ReadLine());
                        Room newRoomforRoommate = roomRepo.GetById(NewRoomId);


                        Roommate AddedRoommate = new Roommate
                        {
                            FirstName = newFirstName,
                            LastName = newLastName,
                            RentPortion= RentPortion,
                            MoveInDate= MoveInDate,
                            Room = newRoomforRoommate
                        };

                        roommateRepo.Insert(AddedRoommate);

                        Console.WriteLine("-------------------------------");
                        Console.WriteLine($"Added the new Roommate {AddedRoommate.FirstName}{AddedRoommate.LastName}");
                        break;

                    case 7: //Edit a roommate's info 

                        List<Roommate> allRoomies = roommateRepo.GetAll();
                        foreach (Roommate roommate in allRoomies)
                        {

                            Console.WriteLine($"Id: {roommate.Id} {roommate.FirstName} {roommate.LastName}");

                        }
                       
                        Console.Write($"Enter the id (listed above) of the roommate you'd like to edit: ");
                        int UpdatedRoommateId = Int32.Parse(Console.ReadLine());
                        var selectedRoommate = roommateRepo.GetById(UpdatedRoommateId);

                        Console.Write($"Enter a first name for {selectedRoommate.FirstName} to edit the info or type in {selectedRoommate.FirstName} to keep it the same: ");
                        string UpdatedFirstName = Console.ReadLine();

                        Console.Write($"Enter a new last name instead of {selectedRoommate.LastName} or type in {selectedRoommate.LastName} to keep it the same");
                        string UpdatedLastName = Console.ReadLine();

                        Console.Write($"Enter a new rent share percentage (a number 0-100) or type in the current share ({selectedRoommate.RentPortion}) to remain the same: ");
                        int UpdateRentPortion = Int32.Parse(Console.ReadLine());

                        DateTime EditMoveInDate = DateTime.Now;

                        List<Room> allRoomsEncore = roomRepo.GetAll();

                        Console.WriteLine($"Enter a new room id to rent from the list below");
                        foreach (Room room in allRoomsEncore)
                        {
                            Console.WriteLine($"Room Id: {room.Id} Room : {room.Name}:");

                        }
                        Console.Write($"> ");

                        int EditRoomId = Int32.Parse(Console.ReadLine());
                        Room updatedRoomforRoommate = roomRepo.GetById(EditRoomId);

                        Roommate UpdatedRoommate = new Roommate
                        {
                            Id = UpdatedRoommateId,
                            FirstName = UpdatedFirstName,
                            LastName = UpdatedLastName,
                            RentPortion= UpdateRentPortion,
                            MoveInDate = EditMoveInDate,
                            Room= updatedRoomforRoommate

                        };

                        roommateRepo.Update(UpdatedRoommate);

                        Console.WriteLine("-------------------------------");
                        Console.WriteLine($"Updated {UpdatedRoommate.FirstName}{UpdatedRoommate.LastName}'s info");
                        break;

                    case 8: //delete a roommate
                        Console.Write($"Enter Id of a roommate you want to delete: ");
                        int roommateId = Int32.Parse(Console.ReadLine());

                        roomRepo.Delete(roommateId);

                        Console.WriteLine("-------------------------------");
                        Console.WriteLine($"Deleted the roommate with id {roommateId}");
                        break;

                   case 9: //List all chores
                        List<Chore> allChores = choreRepo.GetAll();
                        foreach (Chore chore in allChores)
                        {

                            Console.WriteLine(@$"Id: 
{chore.Id}
Name: {chore.Name}");
                        }
                        break;
                    case 10:
                        Console.Write($"Enter the name of the new chore to add: ");
                        string newChoreName = Console.ReadLine();

                        Chore AddedChore = new Chore
                        {
                            Name = newChoreName,
                        };
                        choreRepo.Insert(AddedChore);
                        Console.WriteLine("-------------------------------");
                        Console.WriteLine($"Added the new chore '{AddedChore.Name}' with id {AddedChore.Id}");
                        break;
                }
            }
        }

        static int Menu()
        {
            int selection = -1;

            while (selection < 0 || selection > 10)
            {
                Console.WriteLine(@"
                Welcome to Roommates!
                -------------------------
                Select an option:
                1 List all rooms
                2 Add a room
                3 Delete a room
                4 Edit a room
                5 List all roommates
                6 Add a roommate
                7 Edit a roommate
                8 Delete a roommate
                9 List all chores
                10 Add a chore
                0 Exit
                ");

                Console.Write("> ");
                string choice = Console.ReadLine();
                try
                {
                    selection = int.Parse(choice);
                }
                catch
                {
                    Console.WriteLine("Invalid Selection. Please try again.");
                }
                Console.WriteLine();
            }

            return selection;
        }

        //Room bathroom = new Room
        //{
        //    Name = "Bathroom",
        //    MaxOccupancy = 1
        //};

        //roomRepo.Insert(bathroom);

        //Console.WriteLine("-------------------------------");
        //Console.WriteLine($"Added the new Room with id {bathroom.Id}");

        //Room updatedBathroom = new Room
        //{
        //    Id = 8,
        //    Name = "Bathroom",
        //    MaxOccupancy = 1
        //};

        //roomRepo.Update(updatedBathroom);


        //Console.WriteLine("-------------------------------");
        //Console.WriteLine($"Updated the Room {updatedBathroom.Id}");

    }
}
