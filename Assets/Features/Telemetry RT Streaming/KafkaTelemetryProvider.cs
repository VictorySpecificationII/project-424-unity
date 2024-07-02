
using UnityEngine;
using VehiclePhysics;
using VehiclePhysics.InputManagement;
using EdyCommonTools;
using Newtonsoft.Json;

namespace Perrinn424
{

public class KafkaTelemetryProvider : VehicleBehaviour{
	
	public bool emitTelemetry = true;

	public override void OnEnableVehicle ()
		{
		Debug.Log("Provider for Kafka Enabled");
		}

	public override void OnDisableVehicle ()
		{
		Debug.Log("Provider for Kafka Disabled");
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

		public override void PollValues (float[] values, int index, Object instance)
			{
			VehicleBase vehicle = instance as VehicleBase;

			Telemetry.DataRow latest = vehicle.telemetry.latest;
            string json = JsonConvert.SerializeObject(latest);
            Debug.Log(json);
			}
		}
	}

}