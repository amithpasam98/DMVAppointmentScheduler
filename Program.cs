using System;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace DmvAppointmentScheduler
{
    class Program
    {
        public static Random random = new Random();
        public static List<Appointment> appointmentList = new List<Appointment>();
        static void Main(string[] args)
        {
            CustomerList customers = ReadCustomerData();
            TellerList tellers = ReadTellerData();
            Calculation(customers, tellers);
            OutputTotalLengthToConsole();

        }
        private static CustomerList ReadCustomerData()
        {
            string fileName = "CustomerData.json";
            string path = Path.Combine(Environment.CurrentDirectory, @"InputData", fileName);
            string jsonString = File.ReadAllText(path);
            CustomerList customerData = JsonConvert.DeserializeObject<CustomerList>(jsonString);
            return customerData;

        }
        private static TellerList ReadTellerData()
        {
            string fileName = "TellerData.json";
            string path = Path.Combine(Environment.CurrentDirectory, @"InputData", fileName);
            string jsonString = File.ReadAllText(path);
            TellerList tellerData = JsonConvert.DeserializeObject<TellerList>(jsonString);
            return tellerData;

        }
        static void Calculation(CustomerList customers, TellerList tellers)
        {
            List<Teller> tellerList = tellers.Teller.ToList();

            foreach(Customer customer in customers.Customer) {
                List<Teller> unmatchedTellers = tellerList.Where(t => t.specialtyType != customer.type).OrderBy(t => t.multiplier).ToList(); //List of all tellers whose speciality type does not match with customer type
                List<Teller> matchedTellers = tellerList.Where(t => t.specialtyType == customer.type).OrderBy(t => t.multiplier).ToList(); //List of all tellers whose speciality type match with customer type
           
                var teller = tellerList.Where(t => t.specialtyType == customer.type).Count() !=0 ?
                                matchedTellers.FirstOrDefault() : unmatchedTellers.FirstOrDefault();
                
                var appointment = new Appointment(customer, teller);
                appointmentList.Add(appointment);
                tellerList = tellerList.Where(t => t.id != teller.id).ToList(); // Removes the teller from list once it is assigned to customer

                if(tellerList.Count()==0) {        //When all the tellers are assigned in first loop the list is refreshed for new assignment
                    tellerList = tellers.Teller.ToList();
                }
            }
        }
        static void OutputTotalLengthToConsole()
        {
            var tellerAppointments =
                from appointment in appointmentList
                group appointment by appointment.teller into tellerGroup
                select new
                {
                    teller = tellerGroup.Key,
                    totalDuration = tellerGroup.Sum(x => x.duration),
                };
            var max = tellerAppointments.OrderBy(i => i.totalDuration).LastOrDefault();
            Console.WriteLine("Teller " + max.teller.id + " will work for " + max.totalDuration + " minutes!");
        }

    }
}
