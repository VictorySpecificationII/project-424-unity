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

# WARNING: DEPRECATED :WARNING

The steps below are deprecated, i'm only including them in case something doesn't work.

## Install the .NET SDK

 - Run ```sudo snap install dotnet-sdk --classic```. Version used for development is 8.0.300 as of the time of writing.
 - Add ```export DOTNET_ROOT=/snap/dotnet-sdk/current``` to your .bashrc file, reboot your box.

## Install NuGet in VSCode

 - Open your project workspace in VSCode
 - Open the Command Palette by pressing ```Ctrl+Shift+P```
 - Select > Nuget Package Manager GUI
 - Click Install Package


## Install librdkafka through NuGet

This depends on you having installed dotnet. Step above will help you.

 - In the main project dir, run ```dotnet new console```
 - In VSCode, press ```CTRL+SHIFT+P```
 - Run ```NuGet Package Manager: Install Package```
 - look for ```redist``` and press Enter
 - Find librdkafka.redist and press Enter
 - Pick version 2.4.0 and press Enter

## Install Confluent.Kafka Client through dotnet

 - In the root directory of your project, run ```dotnet add package -v 2.4.0 Confluent.Kafka```

```NOTE: I don't know why it won't play nice through NuGet, nobody has been able to figure it out.```
