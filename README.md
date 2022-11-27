# SWE_Dependencies
The goal of this exercise was to try out a consumer driven contract testing approach. In my example, I worked with C# and Pact.Net.
## How to execute
All services (1 provider, 2 different consumers) can be executed by using the following 2 commands in the root directory (where the docker-compose.yml file is).

```console
docker-compose build
docker-compose up
```

The services can be reached at the through the ports defined in the docker-compose file. 

## How to execute the tests

Both consumer solutions have test projects, whereas the provider just has a test class inside the SWE_Dependencies_Provider sub-project. The consumer pact tests can be executed without any further steps (just make sure that the ports in the test classes are not occupied). For the provider test the provider service has to be running (just use the commands from before).

## General approach

First I programmed the provider service, then the consumer services. Afterwards, I started with the consumer tests and continued with the provider test, because the provider needs the pact files from the consumers. The pact files are in the pacts directory in the root of this repo. Depending on the state of the interactions of the consumer files, a middleware ensures that the the DB has the correct data. This approach is heavily inspired by [the Pact Workshop for .NET core V3](https://github.com/DiUS/pact-workshop-dotnet-core-v3). 

## Personal opinion

Personally I think that this testing approach does make a lot of sense, however, the implementation of this testing approach is quite confusing / difficult. Without the workshop github repo I probably would have needed way more time, because the documentation of Pact.Net was confusing for me.
