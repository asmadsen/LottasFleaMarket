using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading;
using Bogus;
using LottasFleaMarket.Models;
using LottasFleaMarket.Models.Enums;
using LottasFleaMarket.Models.Factories;
using LottasFleaMarket.Utils;

namespace LottasFleaMarket {
    public class Program {
   
        static void Main(string[] args) {
            var simulation = new Simulation(20, 20, 5);
            
            simulation.StartSimulation();

            Console.WriteLine("Press enter key to exit");
            Console.ReadLine();
        }
    }
}