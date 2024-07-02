
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
    public class KafkaTelemetryConnector : VehicleBehaviour
    {
        private string bootstrapServers;
        private IProducer<string, string> producer;
        VehicleBase vehicle;
        
        public KafkaTelemetryConnector(string bootstrapServers)
        {
            this.bootstrapServers = bootstrapServers;
        }

        private string DataRowToJson()
        {
            string json = "test!!";
            return json;
        }


        public async Task ConnectAndSendAsync(string topic/*, string message*/)
        {
            var config = new ProducerConfig { BootstrapServers = bootstrapServers };
            
            // Create a new producer instance
            using (var producer = new ProducerBuilder<string, string>(config).Build())
            {
                try
                {
                    string jsonMessage = DataRowToJson();
                    // Construct the message to send
                    var kafkaMessage = new Message<string, string>
                    {
                        Value = jsonMessage
                    };

                    // Produce the message to the specified topic
                    var deliveryResult = await producer.ProduceAsync(topic, kafkaMessage);

                    // Log the delivery result
                    Debug.Log($"Message delivered to {deliveryResult.TopicPartitionOffset}");
                }
                catch (ProduceException<string, string> e)
                {
                    Debug.Log($"Delivery failed: {e.Error.Reason}");
                }
            }
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