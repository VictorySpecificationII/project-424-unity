
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

			// Custom distance semantic

			var distanceSemantic = new Telemetry.SemanticInfo();
			distanceSemantic.SetRangeAndFormat(0, 21000, "0.000", " km", multiplier:0.001f, quantization:1000);

			// Fill-in channel information

			channelInfo[0].SetNameAndSemantic("LapDistance", Telemetry.Semantic.Custom, distanceSemantic);
			channelInfo[1].SetNameAndSemantic("TotalDistance", Telemetry.Semantic.Custom, distanceSemantic);
			}


		public override void PollValues (float[] values, int index, Object instance)
			{
			VehicleBase vehicle = instance as VehicleBase;

			Telemetry.DataRow latest = vehicle.telemetry.latest;
            string json = JsonConvert.SerializeObject(latest);
            Debug.Log(json);
			SendKafkaMessageAsync(json);

			values[index+0] = (float)latest.distance;
			values[index+1] = (float)latest.totalDistance;
			}
		}

		private static async Task SendKafkaMessageAsync(string message)
		{
			var config = new ProducerConfig { BootstrapServers = "10.144.0.2:9092" };

			// Create a new producer instance
			using (var producer = new ProducerBuilder<Null, string>(config).Build())
			{
				try
				{
					// Construct the message to send
					var kafkaMessage = new Message<Null, string> { Value = message };

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