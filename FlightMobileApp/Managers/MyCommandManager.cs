﻿using FlightMobileApp.Models;
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

        public MyCommandManager(IConfiguration configuration)
        {
            ip = configuration.GetConnectionString("ip");
            portStr = configuration.GetConnectionString("socketPort");
            //Connect(); moved to method ProcessCommands
            Start();
        }
        public void Connect()
        {
            if (!isClientAlreadyConnected)
            {
                int portAsInt = int.Parse(portStr);
                client.Connect(ip, portAsInt);
                isClientAlreadyConnected = true;
            }
        }
        public void Disconnect()
        {
            if (isClientAlreadyConnected)
            {
                client.Disconnect();
                isClientAlreadyConnected = false;
            }
        }
        public Result SendContentToSimulator(string fullPath, double newValueToSend)
        {
            string setStr = "set " + fullPath + newValueToSend.ToString() + "\n";
            string getStr = "get " + fullPath + "\n";
            string returnedData;
            try
            {
                mutex.WaitOne();
                client.Write(setStr);
                client.Write(getStr);
                returnedData = client.Read();
                mutex.ReleaseMutex();
            }
            catch (Exception) // Exception here = failed to get / set.
            {
                throw new Exception("Failed Reading or Writing to sim");
            }

            //Remove \n from the returnedData
            returnedData = returnedData.Replace("\n", string.Empty);
            double valueGot = double.Parse(returnedData);
            if (newValueToSend == valueGot) 
            {
                return Result.Ok;
            }
            // Here - just doesn't match! check if needed throw exception here, or just set the result.
            return Result.NotOk;
        }
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
        public void ProcessCommands()
        {
            Result throttleRes, aileronRes, elevatorRes, rudderRes, totalRes = Result.NotOk;
            Connect();
            foreach (AsyncCommand asyncCmd in queue.GetConsumingEnumerable())
            {
                try
                {
                    throttleRes = SendContentToSimulator(
                    "/controls/flight/throttle ", asyncCmd.Command.Throttle);
                    elevatorRes = SendContentToSimulator(
                        "/controls/flight/elevator ", asyncCmd.Command.Elevator);
                    rudderRes = SendContentToSimulator(
                        "/controls/flight/rudder ", asyncCmd.Command.Rudder);
                    aileronRes = SendContentToSimulator(
                        "/controls/flight/aileron ", asyncCmd.Command.Aileron);
                    //Only if ALL of the results are OK - totalRes is OK.
                    totalRes = GetTotalResult(throttleRes, elevatorRes, rudderRes, aileronRes);
                    asyncCmd.Completion.SetResult(totalRes);
                } catch (Exception exception)
                {
                    asyncCmd.Completion.SetException(exception);
                } 
            }
        }
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
        public bool IsCommandValid(Command command)
        {
            if (command == null)
            {
                return false;
            }
            if (command.Aileron > 1.0 || command.Aileron < -1.0)
            {
                return false;
            }
            if (command.Rudder > 1.0 || command.Rudder < -1.0)
            {
                return false;
            }
            if (command.Elevator > 1.0 || command.Elevator < -1.0)
            {
                return false;
            }
            if (command.Throttle > 1.0 || command.Throttle < 0.0)
            {
                return false;
            }
            return true;
        }
    }
}
