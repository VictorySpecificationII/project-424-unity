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
using EdyCommonTools;

namespace Perrinn424.TelemetryLapSystem{

    public class KafkaTelemetryClient : MonoBehaviour{

        [Serializable]
        public class Settings{
        
            public string bootstrapServers = "10.144.0.2:9092";
            public string topic = "test-topic";
            public string message = "Hello, Kafka!";
        
        }


    	public bool kafkaEnabled = true;
        public Settings settings = new Settings();            
        private KafkaTelemetryConnector connector;
        


        void OnEnableVehicle (){

            Debug.Log("Kafka Telemetry Experiment Started");	
            connector = new KafkaTelemetryConnector(settings.bootstrapServers);

        }

        void Update (){
            //The Update method in Unity cannot be async, because it is called once per frame and should not be awaited.
            // Instead, we call an asynchronous method SendKafkaMessageAsync from within Update.        
            SendKafkaMessageAsync();

        }

        private async void SendKafkaMessageAsync(){
            await connector.ConnectAndSendAsync(settings.topic, settings.message);
            Debug.Log("Message sent to Kafka!");
        }

        void OnDisableVehicle (){
            Debug.Log("Kafka Telemetry Experiment Stopped");
        }

    }
}