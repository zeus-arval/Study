using System;
using System.IO.Ports;
using System.Threading;

namespace COMPort
{
    internal class Program
    {
        static bool _continue;
        static SerialPort _serialPort;

        static void Main(string[] args)
        {
            string name;
            string message;
            StringComparer stringComparer = StringComparer.OrdinalIgnoreCase;
            Thread readThread = new Thread(Read);

            // Create a new SerialPort object with default settings
            _serialPort = new SerialPort();

            // Allow the user to set the appropriate properties.
            _serialPort.PortName = SetPortName(_serialPort.PortName);
            _serialPort.BaudRate = SetPortBaudRate(_serialPort.BaudRate);
            _serialPort.Parity = SetPortParity(_serialPort.Parity);
            _serialPort.DataBits = SetPortDataBits(_serialPort.DataBits);
            _serialPort.StopBits = SetPortStopBits(_serialPort.StopBits);
            _serialPort.Handshake = SetPortHandShake(_serialPort.Handshake);

            //Set the read/write timeout
            _serialPort.ReadTimeout = 500;
            _serialPort.WriteTimeout = 500;

            _serialPort.Open();
            _continue = true;
            readThread.Start();

            Console.Write("Name: ");
            name = Console.ReadLine();

            Console.WriteLine("Type QUIT to exit");

            while (_continue)
            {
                message = Console.ReadLine();

                if (stringComparer.Equals("quit", message))
                {
                    _continue = false;
                }
                else
                {
                    _serialPort.WriteLine(
                        String.Format($"<{name}>: {message}"));
                }
            }

            readThread.Join();
            _serialPort.Close();
        }

        public static void Read()
        {
            while (_continue)
            {
                try
                {
                    string message = _serialPort.ReadLine();
                    Console.WriteLine(message);
                }
                catch (TimeoutException e)
                {

                }
            }
        }

        public static string SetPortName(string defaultPortName)
        {
            string portName;

            Console.WriteLine("Available Ports:");
            foreach (string s in SerialPort.GetPortNames())
            {
                Console.WriteLine($"    {s}");
            }
            
            Console.Write($"Enter a COM port value (Default: {defaultPortName}): ");
            portName = Console.ReadLine();

            if (portName == "" || !(portName.ToLower()).StartsWith("com"))
            {
                portName = defaultPortName;
            }
            return portName;
        }

        public static int SetPortBaudRate(int defaultPortBaudRate)
        {
            string baudRate;

            Console.WriteLine($"Baud Rate(Default {defaultPortBaudRate}): ");
            baudRate = Console.ReadLine();

            if(baudRate == "")
            {
                baudRate = defaultPortBaudRate.ToString();
            }

            return int.Parse(baudRate);
        }

        public static Parity SetPortParity(Parity defaultPortParity)
        {
            string parity;

            Console.WriteLine($"Available Parity options: ");
            foreach(string s in Enum.GetNames(typeof(Parity)))
            {
                Console.WriteLine($"    {s}");
            }
            
            Console.Write($"Enter Parity value (Default: {defaultPortParity.ToString()}): ");
            parity = Console.ReadLine();

            if(parity == "")
            {
                parity = defaultPortParity.ToString();
            }
            return (Parity)Enum.Parse(typeof(Parity), parity, true);
        }

        public static int SetPortDataBits(int defaultPortDataBits)
        {
            string dataBits;

            Console.Write($"Enter DataBits value (Default: {defaultPortDataBits}): ");
            dataBits = Console.ReadLine();

            if(dataBits == "")
            {
                dataBits = defaultPortDataBits.ToString();
            }

            return int.Parse(dataBits.ToUpperInvariant());
        }

        public static StopBits SetPortStopBits(StopBits defaultPortStopBits)
        {
            string stopBits;

            Console.WriteLine("Available StopBits options: ");
            foreach(string s in Enum.GetNames(typeof(StopBits)))
            {
                Console.WriteLine($"    {s}");
            }

            Console.Write($"Enter StopBits value (None is not supported and \n" +
                $"raises an ArgumentOutOfRangeException.\n(Default: {defaultPortStopBits.ToString()}): ");
            stopBits = Console.ReadLine();

            if(stopBits == "")
            {
                stopBits = defaultPortStopBits.ToString();
            }

            return (StopBits)Enum.Parse(typeof(StopBits), stopBits, true);
        }

        public static Handshake SetPortHandShake(Handshake defaultPortHandshake)
        {
            string handShake;

            Console.WriteLine("Available Handshake options: ");
            foreach(string s in Enum.GetNames(typeof(Handshake)))
            {
                Console.WriteLine($"    {s}");
            }

            Console.Write($"Enter Handshake value (Default: {defaultPortHandshake}): ");
            handShake = Console.ReadLine();

            if (handShake == "")
            {
                handShake = defaultPortHandshake.ToString();
            }

            return (Handshake)Enum.Parse(typeof(Handshake), handShake, true);
        }
    }
}
