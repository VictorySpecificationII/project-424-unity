# Usage

I'm running Linux, depending on your OS YMMV but for VSCode on Ubuntu do the following, IN ORDER:

## How-To

Right, so it turns out - there's a NuGet built for Unity. Nice to have known several hours ago.

You shouldn't need to install it, but in case you do:

 - Follow link number 1 in the Sources section.
 - Look for the packages Confluent.Kafka and librdkafka, and install them.
 - Et Voila! Profit!


# Sources
 - https://github.com/GlitchEnzo/NuGetForUnity
 - https://learn.microsoft.com/en-gb/dotnet/core/install/linux-snap-sdk#1-install-the-sdk
 - https://www.nuget.org/packages/Confluent.Kafka/
 - https://www.nuget.org/packages/librdkafka.redist
 - https://github.com/confluentinc/confluent-kafka-dotnet
 - https://github.com/confluentinc/librdkafka
 - https://stackoverflow.com/questions/54898608/unity-confluents-apache-kafka-net-client
 - https://stackoverflow.com/questions/64434605/getting-error-cannot-find-any-csproj-or-fsproj-file-for-your-project-please-f
 - https://developer.confluent.io/get-started/dotnet/

# TODO

 - [ ] check how to get the groups for telemetry
 - [ ] construct json message structure (current version has null key)
 - [ ] send messages to kafka, one at a time