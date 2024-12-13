﻿using System.Diagnostics;
using Pallas.NET.Models;
using Pallas.NET.Models.Enums;
using Microsoft.Extensions.Configuration;
using Pallas.NET;

static double GetCurrentMemoryUsageInMB()
{
    Process currentProcess = Process.GetCurrentProcess();

    // Getting the physical memory usage of the current process in bytes
    long memoryUsed = currentProcess.WorkingSet64;

    // Convert to megabytes for easier reading
    double memoryUsedMb = memoryUsed / 1024.0 / 1024.0;

    return memoryUsedMb;
}

var configuration = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .Build();

// Set client/node connection config
string clientConnection = configuration["CardanoClientConnection"] ?? "/Users/gantuangcoc98/.dmtr/tmp/nebulous-audience-903991/mainnet-mr1dcc.socket";
string nodeConnection = configuration["CardanoNodeConnection"] ?? "backbone.cardano.iog.io:3001";

// N2C Protocol Implementation
async void ExecuteN2cProtocol()
{
    Client? client = new();
    Point? tip = await client.ConnectAsync(clientConnection, NetworkMagic.MAINNET, ClientType.N2C);

    List<Point> points = 
    [
        new(140474748, "72028be5129ea06bf47c7939efdd93ee4d7364f61b2512c426ef68780ee80d81")
    ];

    if (tip is not null)
    {
        Console.WriteLine($"Tip: {tip.Hash}");
        Console.WriteLine("--------------------------------------------------------------------------------");
    }
    
    Console.WriteLine("Current Intersection:");
    Console.WriteLine($"Slot: {points.First().Slot} Hash: {points.First().Hash}");
    Console.WriteLine("--------------------------------------------------------------------------------");

    await foreach (NextResponse? nextResponse in client.StartChainSyncAsync(points))
    {
        if (nextResponse.Action == NextResponseAction.Await)
        {
            Console.WriteLine("Awaiting...");
        }
        else if (nextResponse.Action == NextResponseAction.RollForward || nextResponse.Action == NextResponseAction.RollBack)
        {
            string action = nextResponse.Action == NextResponseAction.RollBack ? "Rolling back..." : "Rolling forward...";
            Console.WriteLine(action);

            if (nextResponse.Action == NextResponseAction.RollBack)
            {
                Console.WriteLine("RollBack Intersection");
                Console.WriteLine($"Slot: {nextResponse.Point.Slot} Hash: {nextResponse.Point.Hash}");
            }

            if (nextResponse.Action == NextResponseAction.RollForward)
            {
                Console.WriteLine("RollForward Intersection");
                Console.WriteLine($"Slot: {nextResponse.Point.Slot} Hash: {nextResponse.Point.Hash}");

                Environment.Exit(0);
            }

            Console.WriteLine("--------------------------------------------------------------------------------");
        }
    }
}

// N2N Protocol Implementation
async void ExecuteN2nProtocol()
{
    Client? n2nClient = new();
    Point? tip = await n2nClient.ConnectAsync(nodeConnection, NetworkMagic.MAINNET, ClientType.N2N);

    List<Point> points = [new(140474748, "72028be5129ea06bf47c7939efdd93ee4d7364f61b2512c426ef68780ee80d81")];

    if (tip is not null)
    {
        Console.WriteLine($"Tip: {tip.Hash}");
        Console.WriteLine("--------------------------------------------------------------------------------");
    }
    
    Console.WriteLine("Current Intersection:");
    Console.WriteLine($"Slot: {points.First().Slot} Hash: {points.First().Hash}");
    Console.WriteLine("--------------------------------------------------------------------------------");

    await foreach (NextResponse? nextResponse in n2nClient.StartChainSyncAsync(points))
    {
        if (nextResponse.Action == NextResponseAction.Await)
        {
            Console.WriteLine("Awaiting...");
        }
        else if (nextResponse.Action == NextResponseAction.RollForward || nextResponse.Action == NextResponseAction.RollBack)
        {
            string action = nextResponse.Action == NextResponseAction.RollBack ? "Rolling back..." : "Rolling forward...";
            Console.WriteLine(action);

            if (nextResponse.Action == NextResponseAction.RollBack)
            {
                Console.WriteLine("RollBack Intersection");
                Console.WriteLine($"Slot: {nextResponse.Point.Slot} Hash: {nextResponse.Point.Hash}");
            }

            if (nextResponse.Action == NextResponseAction.RollForward)
            {
                Console.WriteLine("RollForward Intersection");
                Console.WriteLine($"Slot: {nextResponse.Point.Slot} Hash: {nextResponse.Point.Hash}");

                Environment.Exit(0);
            }

            Console.WriteLine("--------------------------------------------------------------------------------");
        }
    }
}

// Test either Client or Node protocol
// await Task.Run(ExecuteN2cProtocol);
await Task.Run(ExecuteN2nProtocol);

while (true)
{
    await Task.Delay(1000);
}