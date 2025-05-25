
using UnityEngine;
using VehiclePhysics;
using VehiclePhysics.InputManagement;
using EdyCommonTools;
using Newtonsoft.Json;
using Confluent.Kafka;
using System.Threading.Tasks;

namespace Perrinn424
{

public class KafkaTelemetry2: VehicleBehaviour
	{
	public bool emitTelemetry = true;


	public override void OnEnableVehicle ()
		{

		}


	public override bool EmitTelemetry ()
		{
		return emitTelemetry;
		}


	public override void RegisterTelemetry ()
		{
		vehicle.telemetry.Register<Perrinn424Distance>(vehicle);
		}


	public override void UnregisterTelemetry ()
		{
		vehicle.telemetry.Unregister<Perrinn424Distance>(vehicle);
		}

	public class Perrinn424Distance : Telemetry.ChannelGroup
		{
		public override int GetChannelCount ()
			{
			return 2;
			}


		public override Telemetry.PollFrequency GetPollFrequency ()
			{
			return Telemetry.PollFrequency.Normal;
			}


		public override void GetChannelInfo (Telemetry.ChannelInfo[] channelInfo, Object instance)
			{
			VehicleBase vehicle = instance as VehicleBase;

			// // Custom distance semantic

			// var distanceSemantic = new Telemetry.SemanticInfo();
			// distanceSemantic.SetRangeAndFormat(0, 21000, "0.000", " km", multiplier:0.001f, quantization:1000);

			// // Fill-in channel information

			// channelInfo[0].SetNameAndSemantic("LapDistance", Telemetry.Semantic.Custom, distanceSemantic);
			// channelInfo[1].SetNameAndSemantic("TotalDistance", Telemetry.Semantic.Custom, distanceSemantic);
			}


		public override void PollValues (float[] values, int index, Object instance)
			{
			VehicleBase vehicle = instance as VehicleBase;

			int numChannels = vehicle.telemetry.latest.values.Length;
			for (int i = 0; i <= numChannels -1; i++){

			
			/*
			in game telemetry screen
			channel number
			semantic
			group
			update frequency
			channel minimum value
			channel maximum value
			latest value

			Usage Notes
		    // Get the value for the fourth channel.

		    //float channelValue = vehicle.telemetry.latest.values[3];

		    // Get the string with the units that may be appended to the value. It may include leading spaces.

		    //string channelUnits = vehicle.telemetry.GetChannelSemmantic(12).displayUnitsSuffix;

		    // Alternatively, we can get a formatted string with both value and units.
		    //string channelValueWithUnits = vehicle.telemetry.FormatChannelValue(3);

			*/
			//UNDER DEVELOPMENT

			// Retrieve ChannelInfo object
			Telemetry.ChannelInfo channelInfo = vehicle.telemetry.channels[i];

			// Retrieve ChannelGroupInfo object
			Telemetry.ChannelGroupInfo groupInfo = channelInfo.group;

			string channel_group = groupInfo.channels.ToString();
			int channel_count = groupInfo.channelCount;
			string expectedFreq = groupInfo.expectedFrequency.ToString();
			float actualFreq = groupInfo.actualFrequency;
			int interval = groupInfo.updateInterval;
			string frequencyLabel = groupInfo.updateFrequencyLabel;


			// //Output or use the values as needed
			// Debug.Log($"Channel ID: {i}");
			// Debug.Log($"Channel Group: {channel_group}");
			// Debug.Log($"Channel Count: {channel_count}");
			// Debug.Log($"Expected Frequency: {expectedFreq}");
			// Debug.Log($"Actual Frequency: {actualFreq}");
			// Debug.Log($"Update Interval: {interval}");
			// Debug.Log($"Frequency Label: {frequencyLabel}");

			// Access the semantic information
			Telemetry.Semantic semantic = channelInfo.semantic;
			// Telemetry.SemanticInfo customSemantic = channelInfo.customSemantic;//dont use for now, until you figure it out

			// Log or use the semantic information
			//Debug.Log($"Semantic: {semantic}");
			// Debug.Log($"Custom Semantic: {customSemantic}");

			var channel_id = i;
			var channel_name = vehicle.telemetry.channels[i].name;
			var channel_value = vehicle.telemetry.latest.values[i];
			var channel_unit = vehicle.telemetry.GetChannelSemmantic(i).displayUnitsSuffix;
			var channel_min_value = vehicle.telemetry.GetChannelSemmantic(i).displayRangeMin;
			var channel_max_value = vehicle.telemetry.GetChannelSemmantic(i).displayRangeMax;
			var channel_multiplier = vehicle.telemetry.GetChannelSemmantic(i).displayMultiplier;

			// Debug.Log($"Channel Name: {channel_name}");
			// Debug.Log($"Channel Value: {channel_value}");
			// Debug.Log($"Channel_Unit: {channel_unit}");
			// Debug.Log($"Channel Min: {channel_min_value}");
			// Debug.Log($"Channel Max: {channel_max_value}");
			// Debug.Log($"Channel Multiplier: {channel_multiplier}");

			//LEGACY
			// Debug.Log($"Channel Name: {vehicle.telemetry.channels[i].name}"); //Get name of channel
		    // Debug.Log($"Channel Value: {vehicle.telemetry.latest.values[i]}"); // Get value of channel
			// Debug.Log($"Channel Unit: {vehicle.telemetry.GetChannelSemmantic(i).displayUnitsSuffix}"); //Get unit of channel
			// Debug.Log($"Channel Minimum: {vehicle.telemetry.GetChannelSemmantic(i).displayRangeMin}"); // Get minimum value
			// Debug.Log($"Channel Maximum: {vehicle.telemetry.GetChannelSemmantic(i).displayRangeMax}"); // Get maximum value
			// Debug.Log($"Channel Multiplier: {vehicle.telemetry.GetChannelSemmantic(i).displayMultiplier}"); // Get multiplier
			// Construct the telemetry message using the variables
			var telemetryMessage = new
			{
				ChannelId = channel_id,
				ChannelName = channel_name,
				ChannelValue = channel_value,
				ChannelUnit = channel_unit,
				ChannelMinValue = channel_min_value,
				ChannelMaxValue = channel_max_value,
				ChannelMultiplier = channel_multiplier,
				ChannelGroup = channel_group,
				ChannelCount = channel_count,
				ExpectedFrequency = expectedFreq,
				ActualFrequency = actualFreq,
				UpdateInterval = interval,
				FrequencyLabel = frequencyLabel,
				Semantic = semantic.ToString()
			};

			string jsonMessage = JsonConvert.SerializeObject(telemetryMessage, Formatting.Indented);
			//Debug.Log(jsonMessage);
			SendKafkaMessageAsync(jsonMessage);

			}

			//Telemetry.DataRow latest = vehicle.telemetry.latest;
            //string json = JsonConvert.SerializeObject(latest);
            //Debug.Log(json);
			//SendKafkaMessageAsync(json);

			// values[index+0] = (float)latest.distance;
			// values[index+1] = (float)latest.totalDistance;
			}
		}

		private static async Task SendKafkaMessageAsync(string message)
		{
			var config = new ProducerConfig { BootstrapServers = "192.168.1.243:9092" };

			// Create a new producer instance
			using (var producer = new ProducerBuilder<Null, string>(config).Build())
			{
				try
				{
					// Construct the message to send
					var kafkaMessage = new Message<Null, string> { Value = message };

					Debug.Log($"Message content: {kafkaMessage.Value}");

					// Produce the message to the specified topic
					var deliveryResult = await producer.ProduceAsync("my-topic", kafkaMessage);

					// Log the delivery result
					Debug.Log($"Message delivered to {deliveryResult.TopicPartitionOffset}");
				}
				catch (ProduceException<Null, string> e)
				{
					Debug.Log($"Delivery failed: {e.Error.Reason}");
				}
			}
		}
	}

}
