using Confluent.Kafka;
using System;
using System.Threading.Tasks;
using UnityEngine;
using VehiclePhysics;
using Newtonsoft.Json;

namespace Perrinn424
{
    public class KafkaTelemetryConnector : MonoBehaviour
    {
        public string bootstrapServers;
        private IProducer<string, string> producer;
        private VehicleBase vehicle;

        void Awake()
        {
            vehicle = GetComponent<VehicleBase>();
        }

        private string DataRowToJson()
        {
            // Assuming vehicle.telemetry.latest is a DataRow object
            // and you have a method to convert it to a JSON string

            string json = JsonConvert.SerializeObject(vehicle.telemetry.latest);
            return json;
        }

        public async Task ConnectAndSendAsync(string topic)
        {
            var config = new ProducerConfig { BootstrapServers = bootstrapServers };

            // Create a new producer instance
            using (producer = new ProducerBuilder<string, string>(config).Build())
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