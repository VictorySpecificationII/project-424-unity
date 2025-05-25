# Usage

I'm running Linux, depending on your OS YMMV but for VSCode on Ubuntu do the following, IN ORDER:

## How-To

Right, so it turns out - there's a NuGet built for Unity. Nice to have known several hours ago.

You shouldn't need to install it, but in case you do:

 - Follow link number 1 in the Sources section.
 - Look for the packages Confluent.Kafka and librdkafka, and install them.
 - Et Voila! Profit!

## First time setup

 ```
wget https://security.ubuntu.com/ubuntu/pool/main/o/openssl1.0/libssl1.0.0_1.0.2n-1ubuntu5.13_amd64.deb
sudo dpkg -i libssl1.0.0_1.0.2n-1ubuntu5.13_amd64.deb
wget https://www.nuget.org/api/v2/package/librdkafka.redist/2.4.0 -O librdkafka.redist.2.4.0.nupkg
unzip librdkafka.redist.2.4.0.nupkg
mkdir -p ~/Desktop/project-424-unity/Assets/Plugins/x86_64
cp runtimes/linux-x64/native/librdkafka.so ~/Desktop/project-424-unity/Assets/Plugins/x86_64/

 ```


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