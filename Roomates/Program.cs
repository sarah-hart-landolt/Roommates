using System;
using System.Collections.Generic;
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
                        Console.Write($"Enter new Id of Room you want to delete: ");
                        int roomId = Int32.Parse(Console.ReadLine());

                        roomRepo.Delete(roomId);

                        Console.WriteLine("-------------------------------");
                        Console.WriteLine($"Deleted the room with id {roomId}");
                        break;
                    case 4:
                        Console.Write($"Enter the id of the room you'd like to update: ");
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
                            Console.WriteLine($"{room.Id} {room.Name}:");

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
                        Console.WriteLine($"Added the new Roommate with id {AddedRoommate.FirstName}{AddedRoommate.LastName}");
                        break;
                }
            }
        }

        static int Menu()
        {
            int selection = -1;

            while (selection < 0 || selection > 8)
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
