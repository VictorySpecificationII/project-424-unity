/*
Yes, that's the Telemetry system in the vehicle. 
When you have a reference to the vehicle controller, then vehicleController.telemetry.latest gives you the latest recorded datarow with all the channel values. 
vehicleController.telemetry.channelIndex is an index of the channel names and their indexes. 
With the channel index, you can get the value of a channel from the data row, as well as get channel information. 
vehicleController.telemetry.channels is the array with the information on each channel.

You may search the GitHub repo for references on when the .telemetry field is accessed.

Feel free to request more information on the PERRINN forum. That's the best place to exchange this kind of detailed information.
*/


/*Look into Channels.cs file, adapt here for Kafka use
Flow:
 - Pre-Flight: Initialize Kafka Connection (beginning of session: simulation starts)
 - Pre-Flight: Create Topics (Powertrain, Tyre Data, Suspension, Aerodynamics, Electronics)
 - Pre-Flight: Construct Kafka Producer
 - Flight: Get Channel Name
 - Flight: Get Channel Value
 - Flight: Construct Kafka Message
 - Flight: Transmit Kafka Message
 - PostFlight: Sever Connection to Server at end of session (simulation ends)
*/

using System;
using System.Collections.Generic;
using System.Linq;
using Confluent.Kafka;
using UnityEngine;
using VehiclePhysics;

namespace Perrinn424.TelemetryLapSystem
{
    //[Serializable]
    public class KafkaTelemetryConnector
    {
        // [SerializeField]
        // private string[] channels;
        // public string[] Units { get; private set; }
        // private float[] unitsMultiplier;

        // public float[] Frequencies { get; private set; }

        // private int[] channelsIndex;
        // private const int channelNotFoundIndex = -1;
        // private int lastTelemetryChannelCount;

        // private VehicleBase vehicle;
        // // private IProducer<Null, string> producer;
        // private string bootstrapServers;

        // public int Length => channels.Length;
        // private Telemetry.DataRow DataRow => vehicle.telemetry.latest;

        // public KafkaTelemetryConnector(string bootstrapServers, string[] channels)
        // {
        //     this.bootstrapServers = bootstrapServers;
        //     this.channels = channels;
        // }

        // // Pre-Flight: Initialize Kafka Connection
        // public void InitializeConnection()
        // {
        //     var config = new ProducerConfig { BootstrapServers = bootstrapServers };
        //     producer = new ProducerBuilder<Null, string>(config).Build();
        // }

        // // Pre-Flight: Create Topics
        // public void CreateTopics()
        // {
        //     using var adminClient = new AdminClientBuilder(new AdminClientConfig { BootstrapServers = bootstrapServers }).Build();
        //     try
        //     {
        //         var metadata = adminClient.GetMetadata(TimeSpan.FromSeconds(10));
        //         var existingTopics = metadata.Topics.Select(t => t.Topic).ToHashSet();
        //         var newTopics = channels.Where(t => !existingTopics.Contains(t))
        //             .Select(t => new TopicSpecification { Name = t, NumPartitions = 1, ReplicationFactor = 1 })
        //             .ToList();
        //         if (newTopics.Any())
        //         {
        //             adminClient.CreateTopicsAsync(newTopics).Wait();
        //         }
        //     }
        //     catch (Exception e)
        //     {
        //         Debug.LogError($"Error creating topics: {e.Message}");
        //     }
        // }

        // // Pre-Flight: Construct Kafka Producer
        // public void ConstructProducer()
        // {
        //     var config = new ProducerConfig { BootstrapServers = bootstrapServers };
        //     producer = new ProducerBuilder<Null, string>(config).Build();
        // }

        // public void Reset(VehicleBase vehicle)
        // {
        //     this.vehicle = vehicle;
        //     channelsIndex = Enumerable.Repeat(channelNotFoundIndex, channels.Length).ToArray();

        //     BuildInfoArrays();
        // }

        // private void BuildInfoArrays()
        // {
        //     GetChannelsIndex();
        //     GetUnits();
        //     GetFrequencies();
        // }

        // private void GetChannelsIndex()
        // {
        //     IReadOnlyList<Telemetry.ChannelInfo> telemetryChannelList = vehicle.telemetry.channels;
        //     lastTelemetryChannelCount = telemetryChannelList.Count;

        //     for (int channelIndex = 0; channelIndex < channels.Length; channelIndex++)
        //     {
        //         if (IsChannelValidAndActive(channelIndex))
        //         {
        //             continue;
        //         }

        //         int telemetryChannelIndex = vehicle.telemetry.GetChannelIndex(channels[channelIndex]);
        //         SetTelemetryChannelIndex(channelIndex, telemetryChannelIndex);
        //     }
        // }

        // private void GetUnits()
        // {
        //     var semantics = channelsIndex.Select(index => vehicle.telemetry.GetChannelSemmantic(index));

        //     Units = semantics.Select(semantic => semantic.displayUnits).ToArray();
        //     unitsMultiplier = semantics.Select(semantic => semantic.displayMultiplier).ToArray();
        // }

        // private void GetFrequencies()
        // {
        //     Frequencies = channelsIndex.Select(GetFrequency).ToArray();
        // }

        // private float GetFrequency(int channelIndex)
        // {
        //     if (channelIndex == channelNotFoundIndex)
        //     {
        //         return -1.0f;
        //     }

        //     return vehicle.telemetry.channels[channelIndex].group.actualFrequency;
        // }

        // public void RefreshIfNeeded()
        // {
        //     if (vehicle.telemetry.channels.Count == lastTelemetryChannelCount)
        //     {
        //         return;
        //     }

        //     foreach (int channelIndex in channelsIndex)
        //     {
        //         if (channelIndex == channelNotFoundIndex)
        //         {
        //             BuildInfoArrays();
        //             return;
        //         }
        //     }
        // }

        // public float GetValue(int index)
        // {
        //     if (IsChannelValidAndActive(index))
        //     {
        //         int telemetryChannelIndex = GetTelemetryChannelIndex(index);
        //         return DataRow.values[telemetryChannelIndex] * unitsMultiplier[index];
        //     }

        //     return float.NaN;
        // }

        // public IEnumerable<string> GetHeaders()
        // {
        //     return channels;
        // }

        // private bool IsChannelValidAndActive(int index)
        // {
        //     int telemetryChannelIndex = GetTelemetryChannelIndex(index);

        //     if (telemetryChannelIndex == channelNotFoundIndex)
        //     {
        //         return false;
        //     }

        //     if (vehicle.telemetry.channels[telemetryChannelIndex].group.instance == null) // registered but not active
        //     {
        //         return false;
        //     }

        //     return true;
        // }

        // private int GetTelemetryChannelIndex(int channelIndex)
        // {
        //     return channelsIndex[channelIndex];
        // }

        // private void SetTelemetryChannelIndex(int channelIndex, int telemetryChannelIndex)
        // {
        //     channelsIndex[channelIndex] = telemetryChannelIndex;
        // }

        // // Flight: Transmit Telemetry Data
        // public void TransmitTelemetryData()
        // {
        //     for (int i = 0; i < channels.Length; i++)
        //     {
        //         string channelName = channels[i];
        //         float channelValue = GetValue(i);

        //         if (!float.IsNaN(channelValue))
        //         {
        //             var message = ConstructMessage(channelName, channelValue.ToString());
        //             TransmitMessage(channelName, message);
        //         }
        //     }
        // }

        // // Flight: Construct Kafka Message
        // private Message<Null, string> ConstructMessage(string channelName, string channelValue)
        // {
        //     return new Message<Null, string> { Key = channelName, Value = channelValue };
        // }

        // // Flight: Transmit Kafka Message
        // private void TransmitMessage(string topic, Message<Null, string> message)
        // {
        //     producer.Produce(topic, message, (deliveryReport) =>
        //     {
        //         if (deliveryReport.Error.Code != ErrorCode.NoError)
        //         {
        //             Debug.LogError($"Failed to deliver message: {deliveryReport.Error.Reason}");
        //         }
        //         else
        //         {
        //             Debug.Log($"Message delivered to {deliveryReport.TopicPartitionOffset}");
        //         }
        //     });
        // }

        // // PostFlight: Sever Connection to Server
        // public void CloseConnection()
        // {
        //     producer.Flush(TimeSpan.FromSeconds(10));
        //     producer.Dispose();
        // }
    }
}
