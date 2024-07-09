
using EdyCommonTools;
using Confluent.Kafka;
using System;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using VehiclePhysics;
using Newtonsoft.Json;

namespace Perrinn424
{
    public class KafkaTelemetryClient : MonoBehaviour
    {
        [Serializable]
        public class Settings
        {
            public string bootstrapServers = "10.144.0.2:9092";
            public string topic = "test-topic";
        }

        public bool kafkaEnabled = true;
        public Settings settings = new Settings();
        private KafkaTelemetryConnector connector;

        void Awake()
        {
            Debug.Log("Kafka Telemetry Experiment Started");
            connector = gameObject.AddComponent<KafkaTelemetryConnector>();
            connector.bootstrapServers = settings.bootstrapServers;
        }

        void OnEnable()
        {
            Debug.Log("OnEnabledVehicle Reached");
        }

        void Update()
        {
            if (kafkaEnabled)
            {
                SendKafkaMessageAsync();
            }
        }

        private async void SendKafkaMessageAsync()
        {
            try
            {
                await connector.ConnectAndSendAsync(settings.topic);
                Debug.Log("Message sent to Kafka!");
            }
            catch (Exception e)
            {
                Debug.LogException(e, this);
            }
        }

        void OnDisable()
        {
            Debug.Log("Kafka Telemetry Experiment Stopped");
        }
    }
}

/*
Yes, that's the Telemetry system in the vehicle. 
When you have a reference to the vehicle controller, then vehicleController.telemetry.latest gives you the latest recorded datarow with all the channel values. 
vehicleController.telemetry.channelIndex is an index of the channel names and their indexes. 
With the channel index, you can get the value of a channel from the data row, as well as get channel information. 
vehicleController.telemetry.channels is the array with the information on each channel.

You may search the GitHub repo for references on when the .telemetry field is accessed.

Feel free to request more information on the PERRINN forum. That's the best place to exchange this kind of detailed information.
*/