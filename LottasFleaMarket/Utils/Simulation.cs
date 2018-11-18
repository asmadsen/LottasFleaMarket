using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using LottasFleaMarket.Models;
using LottasFleaMarket.Models.Factories;

namespace LottasFleaMarket.Utils {
    public class Simulation {
        public static object RunningCheckLock = new object();
        public static bool RunningSimulation = false;
        private bool _isCurrentlyRunning = false;
        
        private int _numberOfBuyers;
        private int _numberOfSellers;
        private int _numberOfSellerBelongings;
        private decimal _buyerStartBalance;

        public Simulation(int numberOfBuyers = 50, int numberOfSellers = 50, int numberOfSellerBelongings = 20, decimal buyerStartBalance = 10000) {
            _numberOfBuyers = numberOfBuyers;
            _numberOfSellers = numberOfSellers;
            _numberOfSellerBelongings = numberOfSellerBelongings;
            _buyerStartBalance = buyerStartBalance;
        }

        public Simulation WithNumberOfBuyers(int numberOfBuyers) {
            IsRunningCheck();
            _numberOfBuyers = numberOfBuyers;
            return this;
        }

        public Simulation WithNumberOfSellers(int numberOfSellers) {
            IsRunningCheck();
            if (numberOfSellers > 64) {
                throw new Exception("You cannot have more than 64 sellers because of limitations of WaitHandle.all");
            }
            _numberOfSellers = numberOfSellers;
            return this;
        }

        public Simulation WithNumberOfSellerBelongings(int numberOfSellerBelongings) {
            IsRunningCheck();
            _numberOfSellerBelongings = numberOfSellerBelongings;
            return this;
        }

        public Simulation WithBuyerStartBalance(int buyerStartBalance) {
            IsRunningCheck();
            _buyerStartBalance = buyerStartBalance;
            return this;
        }

        public void StartSimulation() {
            IsRunningCheck();
            List<Seller> sellers;
            List<Buyer> buyers;
            lock (RunningCheckLock) {
                RunningSimulation = true;
                _isCurrentlyRunning = true;

                var buyerBuilder = PersonFactory.BuyerBuilder()
                    .WithStartBalance(_buyerStartBalance);

                buyers = new string[_numberOfBuyers]
                    .Select(_null => buyerBuilder.Build()).ToList();
                
                
                var sellerBuilder = PersonFactory.SellerBuilder(true)
                    .WithNumberOfBelongings(_numberOfSellerBelongings);

                sellers = new string[_numberOfSellers]
                    .Select(_null => sellerBuilder.Build()).ToList();

                var loopCount = 1;
                while (sellers.Any(seller => seller.HasMoreNotListedItems)) {
                    WaitHandle[] waitHandles = sellers.FindAll(seller => seller.HasMoreNotListedItems)
                        .OrderBy(seller => Guid.NewGuid())
                        .Select(seller => {
                            var waitHandle = new ManualResetEvent(false);
                            ThreadPool.QueueUserWorkItem(delegate(object state) {
                                var mre = (ManualResetEvent) state;
                                var threadSafeRandom = new ThreadSafeRandom();
                                seller.SellItems(threadSafeRandom.Next(1, 50));
                                mre.Set();
                            }, waitHandle);
                            return waitHandle;
                        }).ToArray();

                    WaitHandle.WaitAll(waitHandles);
                    Console.WriteLine($"All sellers have now sold it's #{loopCount++} batch");
                }

                _isCurrentlyRunning = false;
                RunningSimulation = false;
                Console.WriteLine("Everything is sold");
            }
            PrintStatistics(sellers, buyers);
        }

        private void PrintStatistics(List<Seller> sellers, List<Buyer> buyers) {
            Console.Write("\r\n\r\n");
            Console.WriteLine("Statistics:");
            
            Console.WriteLine("Number of sellers: {0, -3}", sellers.Count);
            Console.WriteLine("Number of buyers: {0, -3}", buyers.Count);
            
            var sellerReports = new List<SellerReport>();
            var buyerReports = new List<BuyerReport>();

            buyers.ForEach(b => buyerReports.Add(b.GenerateReport() as BuyerReport));
            sellers.ForEach(s => sellerReports.Add(s.GenerateReport() as SellerReport));
            
            Console.Write("\r\n\r\n");
            
            Console.WriteLine("Number of seller reports: {0, -3}", sellerReports.Count);
            Console.WriteLine("Number of buyer reports: {0, -3}", buyerReports.Count);
            
            Console.Write("\r\n\r\n");
            
            Console.WriteLine("Sellers:");
            Console.Write("\r\n");
            
            var table = new ConsoleTable("{0, -14} {1, -10} {2, -13} ${3, -10} ${4 ,-13}", "Name", "Items left", "Items Sold", "Initial Value", "Total income")
                .WithHeaderFormat("{0, -10} {1, -8} {2, -13} {3, -13} {4, -13}");
            foreach (var report in sellerReports) {
                table.PushRow(report.Owner.Name, report.Owner.NumberOfBelongings, report.ItemSold,
                    report.InitialItemValue, report.MoneyMade);
            }
            table.PrintTable();
            
            Console.Write("\r\n");
            Console.WriteLine("Buyers:");
            Console.Write("\r\n");

            table = new ConsoleTable("{0, -15} {1,-12} ${2, -10} ${3, -10} ${4, -13}", "Name", "Items Bought",
                    "Initial balance", "Money spent", "Bank balance")
                .WithHeaderFormat("{0, -10} {1, -13} {2, -13} {3, -13} {4, -13}");
            foreach (var report in buyerReports) {
                table.PushRow(report.Owner.Name, report.ItemsBought, report.StartBalance, report.MoneySpent,
                    report.EndBalance);
            }
            table.PrintTable();
        }

        private static void IsRunningCheck() {
            lock (RunningCheckLock) {
                if (RunningSimulation) {
                    throw new Exception("Cannot finish the current action because of an running simulation");
                }
            }
        }
    }
}