using System;
using System.Net;
using IGMPSnooping.Validation;
using IGMPSnooping.IGMP;

namespace IGMPSnooping
{
    class OutputConsole
    {
        static void Main(string[] args)
        {

            Validator validator = new Validator();
            string inputIpString;
            string inputPortString;
            bool ipIsValid;
            bool isPortValid;

            System.Console.WriteLine("Welcome to IGMPSnooping!");
            do
            {
                System.Console.WriteLine("");
                System.Console.WriteLine("Insert IP Address of IGMP Group: [Insert 'e' to exit]");
                inputIpString = System.Console.ReadLine();
                ipIsValid = validator.IsIpValid(inputIpString);

                if (inputIpString.StartsWith("e"))
                {
                    System.Console.WriteLine("Closing console application");
                    return;
                }
                else {
                    if (ipIsValid)
                        System.Console.WriteLine("This is a valid ip address");
                    else
                        System.Console.WriteLine("This is not a valide ip address");
                }
            } while (!ipIsValid);

            do
            {
                System.Console.WriteLine("");
                System.Console.WriteLine("Insert Port of IGMP Group: [Insert 'e' to exit]");
                inputPortString = System.Console.ReadLine();
                isPortValid = validator.IsPortValid(inputPortString);

                if (inputPortString.StartsWith("e"))
                {
                    System.Console.WriteLine("Closing console application");
                    return;
                }
                else
                {
                    if (!isPortValid)
                        System.Console.WriteLine("This is not a valid Port");
                }
            } while (!isPortValid);

            System.Console.WriteLine("Start listening Multicast group: [" + inputIpString + "]" + " Port: [" + inputPortString + "] (CTRL+C To exit)");
            IGMPController igmp = new IGMPController(inputIpString, Int32.Parse(inputPortString));

            igmp.IGMPListener();

        }
    }
}
