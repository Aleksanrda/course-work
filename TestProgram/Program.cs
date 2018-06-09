using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary1;

namespace TestProgram
{
    class Program
    {
        static void Main(string[] args)
        {

            CommanderDirectory commanderDirectory = new CommanderDirectory();
            //TestAdding(commanderDirectory);
            //TestUpdating(commanderDirectory);

            //TestGrouping(commanderDirectory);

            // TestRemoving(commanderDirectory);
            //string resultString = ""
            //List<Soldier> result = commanderDirectory.RemoveSoldier(resultString);
            Console.ReadKey();
        }



        private static void TestAdding(CommanderDirectory directory)
        {
            var soldier = new Soldier
            {
                Name = "Test forth"
            };

            directory.AddSoldier(soldier);

            var allSoldiers = directory.GetAllSoldiers();
        }

        private static void TestUpdating(CommanderDirectory directory)
        {
            var soldier = new Soldier
            {
                Id = 4,
                Name = "Test forth updated name",
                Surname = "Updated surname"
            };

            directory.UpdateSoldier(4, soldier);

            var allSoldiers = directory.GetAllSoldiers();
        }

        private static void TestRemoving(CommanderDirectory directory)
        {
            directory.RemoveSoldier(2);

            var allSoldiers = directory.GetAllSoldiers();
        }

        //private static void TestGrouping(CommanderDirectory directory)
        //{
        //    var result = directory.GroupSoldiersByServiceAttitude(); //searchByService
        //}

    }



}
