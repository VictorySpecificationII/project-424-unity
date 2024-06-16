// using System;
// using System.Collections.Generic;
// using UnityEngine;
// using VehiclePhysics;

// namespace Perrinn424.TelemetryLapSystem
// {
//     public class Program : MonoBehaviour
//     {
        // private KafkaTelemetryConnector kafkaConnector;
        // private VehicleBase vehicle;

        // void Start()
        // {
        //     // Initialize vehicle reference (assuming it's attached to the same GameObject)
        //     vehicle = GetComponent<VehicleBase>();

        //     // Define Kafka server and topics
        //     string bootstrapServers = "localhost:9092";
        //     string[] channels = new string[] { "Powertrain", "TyreData", "Suspension", "Aerodynamics", "Electronics" };

        //     // Initialize KafkaTelemetryConnector
        //     kafkaConnector = new KafkaTelemetryConnector(bootstrapServers, channels);

        //     // Pre-Flight: Initialize Kafka Connection
        //     kafkaConnector.InitializeConnection();

        //     // Pre-Flight: Create Topics
        //     kafkaConnector.CreateTopics();

        //     // Pre-Flight: Construct Kafka Producer
        //     kafkaConnector.ConstructProducer();

        //     // Initialize Channels and reset with vehicle data
        //     kafkaConnector.Reset(vehicle);
        // }

        // void Update()
        // {
        //     // Flight: Transmit Telemetry Data
        //     kafkaConnector.TransmitTelemetryData();
        // }

        // void OnApplicationQuit()
        // {
        //     // PostFlight: Sever Connection to Server
        //     kafkaConnector.CloseConnection();
        // }
//     }
// }

using System;
using UnityEngine;
using VehiclePhysics;

namespace Perrinn424.TelemetryLapSystem
{
    public class Program : MonoBehaviour
    {
        async void Start()
        {
            string bootstrapServers = "10.144.0.2:9092";
            string topic = "test-topic";
            string message = "Hello, Kafka!";

            KafkaTelemetryConnector connector = new KafkaTelemetryConnector(bootstrapServers);
            await connector.ConnectAndSendAsync(topic, message);

            // Optionally, you can add delay or other logic here
            // await Task.Delay(TimeSpan.FromSeconds(1));

            // Example: Unity specific logging
            Debug.Log("Message sent to Kafka!");

            // Quit the application (optional, if desired)
            Application.Quit();
        }
    }
}