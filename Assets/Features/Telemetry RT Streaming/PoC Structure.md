# Research Notes


## Topics

 - Driver Inputs
 - Accelerations
 - Wheel Speeds
 - Driveshaft Torque
 - Differential
 - Chassis
 - Suspension Travel
 - Dampers
 - Slip Ratio
 - Slip Angle
 - Camber Angle
 - Powertrain
 - Aerodynamics
 - Distance
 - Force Feedback
 - Autopilot
 - Performance Benchmark
 - Reference Lap
 - Battery

## Questions for Edy

 - Certain pages in Telemetry do not have titles, do they form part of the topic?
 - All that data in the telemetry, I need them - will the standard telemetry class provide them? or do we have to instrument VPP?
 - Pull Request for NuGet - what do?
 - The metrics - do they have min and max values embedded in the code? thinking about the dashboards and panels lower and upper limits so we don't have to set everything and instead have them be set automatically

# ChangeLog since fork

- added nugetforunity
- added kafka redistributable and client
- created docker compose file with kafka stack, still working on the backing, ML side, S3 storage, and dashboards


# Todo, near future

 - ARCHITECTURE DECISIONS
 - Create a producer on the car side
 - Craft messages to go to appropriate topics
 - Decide on backing store for data
 - Decide on stack for adhoc analysis
 - Decide on stack for real time analysis
 - Decide on stack for real time ML
 - Decide on stack for Reporting
 - Decide on stack for visualization


# Todo, way down the road

 - API-first approach to generating dashboards (min/max thresholds, units)



# Possible Infrastructure Combinations


## AWS - Fully Managed Services ($$$)

Data Ingestion:

    Race car telemetry data → Amazon MSK (Kafka) ← Producers
                                 └──→ Amazon Timestream

Data Storage:
   └── Amazon Timestream ← Real-time storage for time-series data
   └── Amazon S3 ← Storage for batch processing and ML training data

Real-time Processing and Online Learning:

    Apache Kafka Consumers (Kafka Streams / Apache Flink) ← Real-time data processing
        └──→ Online Learning Algorithms ← Continuous model updates based on real-time data and real-time inference for live recommendations

Batch Processing for ML:

    Amazon SageMaker ← Training machine learning models using historical data from S3 and Timestream, and real-time inference for live recommendations

Visualization and Monitoring:

    Grafana ← Visualization tool for real-time and historical data monitoring for Performance Engineers
    Amazon QuickSight ← Real-time dashboards for MLE's to monitor model performance and insights, including live recommendations.

## HomeLab - Self Managed

 - Proxmox VE
 - Golden Images
 - Terraform
 - Kubernetes & Helm
 - SRE on infra side
 - Services
 - SRE on service side