using FlightMobileApp.Models;
using FlightMobileApp.Utilities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FlightMobileApp.Managers
{
    public class MyCommandManager : ICommandManager
    {
        private readonly ITcpClient client = new MyTcpClient();
        private readonly BlockingCollection<AsyncCommand> queue = 
            new BlockingCollection<AsyncCommand>();
        private bool isClientAlreadyConnected = false;
        private readonly string ip;
        private readonly string portStr;
        private readonly Mutex mutex = new Mutex();

        //Given a configuration, get the ConnectionStrings (IP/ port).
        public MyCommandManager(IConfiguration configuration)
        {
            ip = configuration.GetConnectionString("ip");
            portStr = configuration.GetConnectionString("socketPort");
            Start();
        }
        public void Connect()
        {
            //Connect only if another connection hasn't been established yet.
            if (!isClientAlreadyConnected)
            {
                int portAsInt = int.Parse(portStr);
                client.Connect(ip, portAsInt);
                isClientAlreadyConnected = true;
            }
        }
        public void Disconnect()
        {
            //Disconnect only if a connection has already been established earlier.
            if (isClientAlreadyConnected)
            {
                client.Disconnect();
                isClientAlreadyConnected = false;
            }
        }

        //Given a path and a value, use the TelnetClient to send the value to the server.
        //Verify that the value that returns from the sever matches the given value.
        //If they do match, return Result.Ok. Otherwise, return Result.NotOk (enum).
        public Result SendContentToSimulator(string fullPath, double newValueToSend)
        {
            string setStr = "set " + fullPath + newValueToSend.ToString() + "\n";
            string getStr = "get " + fullPath + "\n";
            string returnedData;
            mutex.WaitOne();
            //Send the value to the server using the client.
            client.Write(setStr);
            client.Write(getStr);
            //Read the server's answer using the client.
            returnedData = client.Read();
            mutex.ReleaseMutex();

            //Failed to get info from server
            if (returnedData == null)
            {
                return Result.NotOk;
            }
            //Else, remove \n from the returnedData and convert it to double.
            returnedData = returnedData.Replace("\n", string.Empty);
            double valueGot = double.Parse(returnedData);
            if (newValueToSend == valueGot) 
            {
                return Result.Ok;
            }
            return Result.NotOk;
        }

        //Given a command, wrap it in an AsyncCommand object. 
        //Add it to the BlockingCollection and return its' task.
        public Task<Result> Execute(Command command)
        {
            var asyncCommand = new AsyncCommand(command);
            queue.Add(asyncCommand);
            return asyncCommand.Task;
        }
        public void Start()
        {
            Task.Factory.StartNew(ProcessCommands);
        }

        //Foreach command in the BlockingCollection, send its'
        // fields to the server and set its' result accordingly.
        public void ProcessCommands()
        {
            Result throttleRes, aileronRes, elevatorRes, rudderRes;
            Connect();
            foreach (AsyncCommand asyncCmd in queue.GetConsumingEnumerable())
            {
                try
                {
                    throttleRes = SendContentToSimulator(
                    "/controls/engines/current-engine/throttle ", asyncCmd.Command.Throttle);
                    elevatorRes = SendContentToSimulator(
                        "/controls/flight/elevator ", asyncCmd.Command.Elevator);
                    rudderRes = SendContentToSimulator(
                        "/controls/flight/rudder ", asyncCmd.Command.Rudder);
                    aileronRes = SendContentToSimulator(
                        "/controls/flight/aileron ", asyncCmd.Command.Aileron);
                    //Only if ALL of the results are OK - totalRes is OK.
                    Result totalRes = GetTotalResult(
                        throttleRes, elevatorRes, rudderRes, aileronRes);
                    asyncCmd.Completion.SetResult(totalRes);
                } catch (Exception exception)
                {
                    asyncCmd.Completion.SetException(exception);
                } 
            }
        }

        //If all fields were set properly by the connected server - set the
        // command result to be Result.Ok. Otherwise, set it to be Result.NotOk.
        public Result GetTotalResult(
            Result throttleRes, Result elevatorRes, Result rudderRes, Result aileronRes)
        {
            if (throttleRes == Result.Ok && elevatorRes == Result.Ok &&
                aileronRes == Result.Ok && rudderRes == Result.Ok)
            {
                return Result.Ok;
            }
            return Result.NotOk;
        }
        
        //This method returns true only if the given command
        //isn't null and its' fields meet the requirements.
        public bool IsCommandValid(Command command)
        {
            double maxVal = 1.0, minValMost = -1.0, minValThrottle = 0.0;
            if (command == null)
            {
                return false;
            }
            if (command.Aileron > maxVal || command.Aileron < minValMost)
            {
                return false;
            }
            if (command.Rudder > maxVal || command.Rudder < minValMost)
            {
                return false;
            }
            if (command.Elevator > maxVal || command.Elevator < minValMost)
            {
                return false;
            }
            if (command.Throttle > maxVal || command.Throttle < minValThrottle)
            {
                return false;
            }
            return true;
        }
    }
}
