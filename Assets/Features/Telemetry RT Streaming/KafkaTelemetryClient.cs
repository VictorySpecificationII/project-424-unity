
using EdyCommonTools;
using Confluent.Kafka;
using System;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using VehiclePhysics;
using Newtonsoft.Json;

namespace Perrinn424{

    public class KafkaTelemetryClient : VehicleBehaviour{

        [Serializable]
        public class Settings{
        
            public string bootstrapServers = "10.144.0.2:9092";
            public string topic = "test-topic";
            //public string message = "Hello, Kafka!";
        
        }


    	public bool kafkaEnabled = true;
        public Settings settings = new Settings();            
        private KafkaTelemetryConnector connector;
        


        void Awake (){

            Debug.Log("Kafka Telemetry Experiment Started");	
            connector = new KafkaTelemetryConnector(settings.bootstrapServers);

        }

        void onEnableVehicle (){
            Debug.Log("OnEnabledVehicle Reached");
        }

        void Update (){
            //The Update method in Unity cannot be async, because it is called once per frame and should not be awaited.
            // Instead, we call an asynchronous method SendKafkaMessageAsync from within Update.        
            SendKafkaMessageAsync();

        }

        private async void SendKafkaMessageAsync(){
            try{
                await connector.ConnectAndSendAsync(settings.topic/*, settings.message*/);
                Debug.Log("Message sent to Kafka!");
            }
            catch (Exception e){
                Debug.LogException(e, this);
            }
        }

        public override void OnDisableVehicle (){
            Debug.Log("Kafka Telemetry Experiment Stopped");
        }

    }
}


