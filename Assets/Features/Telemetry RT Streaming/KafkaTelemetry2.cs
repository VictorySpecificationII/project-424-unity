using UnityEngine;
using VehiclePhysics;
using VehiclePhysics.InputManagement;
using EdyCommonTools;
using Newtonsoft.Json;
using Confluent.Kafka;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perrinn424
{
    public class KafkaTelemetry2 : VehicleBehaviour
    {
        public bool emitTelemetry = true;

        // Time control for one-second interval sending
        private float lastSendTime = 0f;
        private readonly float sendInterval = 1f;

        public override void OnEnableVehicle()
        {
            Debug.Log("[KafkaTelemetry2] Vehicle enabled: Kafka telemetry component active.");
        }

        public override bool EmitTelemetry()
        {
            return emitTelemetry;
        }

        public override void RegisterTelemetry()
        {
            vehicle.telemetry.Register<Perrinn424Distance>(this);
            Debug.Log("[KafkaTelemetry2] Registered Perrinn424Distance telemetry group.");
        }

        public override void UnregisterTelemetry()
        {
            vehicle.telemetry.Unregister<Perrinn424Distance>(this);
            Debug.Log("[KafkaTelemetry2] Unregistered Perrinn424Distance telemetry group.");
        }

        public class Perrinn424Distance : Telemetry.ChannelGroup
        {
            public override int GetChannelCount()
            {
                // Adjust to your actual channel count if needed
                return 2;
            }

            public override Telemetry.PollFrequency GetPollFrequency()
            {
                // Use Normal since Slow is unavailable
                return Telemetry.PollFrequency.Normal;
            }

            public override void GetChannelInfo(Telemetry.ChannelInfo[] channelInfo, Object instance)
            {
                // Optional channel info
            }

            public override void PollValues(float[] values, int index, Object instance)
            {
                KafkaTelemetry2 kafkaTelemetry = instance as KafkaTelemetry2;
                if (kafkaTelemetry == null) return;

                // Only send once every 1 second
                if (Time.time - kafkaTelemetry.lastSendTime < kafkaTelemetry.sendInterval)
                {
                    // Skip sending this poll
                    return;
                }
                kafkaTelemetry.lastSendTime = Time.time;

                VehicleBase vehicle = kafkaTelemetry.vehicle;
                int numChannels = vehicle.telemetry.latest.values.Length;

                var batch = new List<object>(numChannels);

                for (int i = 0; i < numChannels; i++)
                {
                    Telemetry.ChannelInfo channelInfo = vehicle.telemetry.channels[i];
                    Telemetry.ChannelGroupInfo groupInfo = channelInfo.group;

                    var telemetryMessage = new
                    {
                        Timestamp = Time.time,
                        ChannelId = i,
                        ChannelName = channelInfo.name,
                        ChannelValue = vehicle.telemetry.latest.values[i],
                        ChannelUnit = vehicle.telemetry.GetChannelSemmantic(i).displayUnitsSuffix,
                        ChannelMinValue = vehicle.telemetry.GetChannelSemmantic(i).displayRangeMin,
                        ChannelMaxValue = vehicle.telemetry.GetChannelSemmantic(i).displayRangeMax,
                        ChannelMultiplier = vehicle.telemetry.GetChannelSemmantic(i).displayMultiplier,
                        ChannelGroup = groupInfo.channels.ToString(),
                        ChannelCount = groupInfo.channelCount,
                        ExpectedFrequency = groupInfo.expectedFrequency.ToString(),
                        ActualFrequency = groupInfo.actualFrequency,
                        UpdateInterval = groupInfo.updateInterval,
                        FrequencyLabel = groupInfo.updateFrequencyLabel,
                        Semantic = channelInfo.semantic.ToString()
                    };

                    batch.Add(telemetryMessage);
                }

                if (kafkaTelemetry.emitTelemetry)
                {
                    string jsonBatch = JsonConvert.SerializeObject(batch, Formatting.Indented);
                    _ = SendKafkaMessageAsync(jsonBatch);
                }

                // Optional dummy telemetry output
                // values[index + 0] = 0.0f;
                // values[index + 1] = 0.0f;
            }
        }

        private static async Task SendKafkaMessageAsync(string message)
        {
            var config = new ProducerConfig { BootstrapServers = "192.168.1.243:9092" };

            using (var producer = new ProducerBuilder<Null, string>(config).Build())
            {
                try
                {
                    var kafkaMessage = new Message<Null, string> { Value = message };
                    Debug.Log($"[KafkaTelemetry2] Sending batch to Kafka:\n{message}");
                    var deliveryResult = await producer.ProduceAsync("p424-telemetry-batch", kafkaMessage);
                    Debug.Log($"[KafkaTelemetry2] Batch delivered to {deliveryResult.TopicPartitionOffset}");
                }
                catch (ProduceException<Null, string> e)
                {
                    Debug.LogError($"[KafkaTelemetry2] Delivery failed: {e.Error.Reason}");
                }
            }
        }
    }
}
