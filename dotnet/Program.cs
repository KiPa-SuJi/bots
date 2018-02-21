

using System;
using System.Configuration;
using System.Numerics;
using System.Reflection.Metadata;
using Bot;

namespace EhterDelta.Bots.Dontnet
{
    class Program
    {
        static void Main(string[] args)
        {
            // Tells us which bot program to run
            if (args.Length < 1 || args[0] != "taker" && args[0] != "maker" && args[0] != "custom")
            {
                Console.WriteLine("Please run with 'taker' or 'maker' or 'custom' argument!");
                return;
            }

            // maps the configuration file information to a model
            var config = new EtherDeltaConfiguration
            {
                SocketUrl = ConfigurationManager.AppSettings["SocketUrl"],
                Provider = ConfigurationManager.AppSettings["Provider"],
                AddressEtherDelta = ConfigurationManager.AppSettings["AddressEtherDelta"],
                AbiFile = ConfigurationManager.AppSettings["AbiFile"],
                TokenFile = ConfigurationManager.AppSettings["TokenFile"],
                Token = ConfigurationManager.AppSettings["Token"],
                User = ConfigurationManager.AppSettings["User"],
                PrivateKey = ConfigurationManager.AppSettings["PrivateKey"],
                UnitDecimals = int.Parse(ConfigurationManager.AppSettings["UnitDecimals"]),
                GasPrice = new BigInteger(UInt64.Parse(ConfigurationManager.AppSettings["GasPrice"])),
                GasLimit = new BigInteger(UInt64.Parse(ConfigurationManager.AppSettings["GasLimit"]))
            };

            // create a console logger if in verbose mode
            ILogger logger = null;
            if (args.Length == 2 && args[1] == "-v")
            {
                logger = new ConsoleLogger();
            }

            if (args[0] == "taker")
            {
                new Taker(config, logger);
            }
            else if(args[0] == "maker")
            {
                new Maker(config, logger);
            }
            else if (args[0] == "custom")
            {
                new Custom(config, logger);
            }

            Console.ReadLine();
        }

        private class ConsoleLogger : ILogger
        {
            public void Log(string message)
            {
                Console.WriteLine($"{DateTimeOffset.Now.DateTime.ToUniversalTime()} :  {message}");
            }
        }
    }
}