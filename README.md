# Microservice Messaging and Contract Testing in .NET Core

![.NET](https://img.shields.io/badge/.NET-512BD4?logo=dotnet&logoColor=white)
![RabbitMQ](https://img.shields.io/badge/RabbitMQ-FF6600?logo=rabbitmq&logoColor=white)
[![Conventional Commits](https://img.shields.io/badge/Conventional%20Commits-1.0.0-FE5196?logo=conventionalcommits&logoColor=white)](https://conventionalcommits.org)

This repository focuses on **Microservice Messaging** using **MassTransit** and **RabbitMQ**, and on **Testing Microservices** using **Pact Net** for contract testing. The work is based on **Chapter 5** (Microservice Messaging) and **Chapter 7** (Testing Microservices) from the book [_Pro Microservices in .NET 6_](https://www.link.springer.com/book/10.1007/9781484278338).

## Table of Contents

1. [Introduction](#introduction)
2. [Technologies Used](#technologies-used)
3. [Project Structure](#project-structure)
   - [Microservice Messaging](#microservice-messaging-chapter-5)
   - [Testing Microservices](#testing-microservices)
4. [Getting Started](#getting-started)
   - [Prerequisites](#prerequisites)
   - [Installation](#installation)
5. [Running the Project](#running-the-project)
6. [Testing](#testing)
7. [License](#license)

## Introduction

This project demonstrates two key aspects of building microservices in .NET Core:

1. **Microservice Messaging** using **MassTransit** and **RabbitMQ** to enable asynchronous communication between microservices.
2. **Contract Testing** using **Pact Net** and **xUnit** to ensure microservices adhere to agreed-upon communication contracts over HTTP.

These implementations follow **Chapter 5** and **Chapter 7** of [_Pro Microservices in .NET 6_](https://www.link.springer.com/book/10.1007/9781484278338).

## Technologies Used

- **.NET Core**: A cross-platform framework for building modern applications.
- **MassTransit**: A message-based application framework for .NET.
- **RabbitMQ**: A messaging broker for communication between services.
- **Pact Net**: A contract testing framework that ensures services follow consumer-driven contracts.
- **xUnit**: A popular testing framework for .NET.

## Project Structure

### Microservice Messaging

Focuses on implementing **message-based communication** using **MassTransit** and **RabbitMQ**. The implementation of microservice messaging includes the following projects (contained within [MessageMicroservices](/MessageMicroservices//MessageMicroservices.sln) solution):

- [InvoiceMicroservice](/MessageMicroservices/InvoiceMicroservice/InvoiceMicroservice.csproj): a microservice which publishes a message about the newly created invoices (producer).
- [PaymentMicroservice](/MessageMicroservices/PaymentMicroservice/PaymentMicroservice.csproj): a microservice which receives the message that an invoice was created (consumer). Serves as a quick example of a downstream microservice that reacts to the creation of an invoice.
- [TestClient](/MessageMicroservices/TestClient/TestClient.csproj): a test client which takes the place of a monolith that interacts with the microservices to demonstrate message publishing and consumption.

MassTransit handles the communication, and RabbitMQ is used as the message broker.

### Testing Microservices

Focuses on **contract testing** between microservices using **Pact Net** framework. The consumer-driven contract testing approach ensures that both services can work together by adhering to predefined communication rules. Communication between microservices is achieved either over HTTP protocol, using REST APIs, or through message queues.

- ### Over HTTP Protocol

  [Contract-Testing](/Contract-Testing/Contract-Testing.sln) solution contains two microservice projects ([OrderSvc-Consumer](/Contract-Testing/OrderSvc-Consumer/OrderSvc-Consumer.csproj) and [DiscountSvc-Provider](/Contract-Testing/DiscountSvc-Provider/DiscountSvc-Provider.csproj)) and their appropriate test projects.

  - [ConsumerTests](/Contract-Testing/ConsumerTests/ConsumerTests.csproj) contains the following:
    - [DiscountSvcMock](/Contract-Testing/ConsumerTests/DiscountSvcMock.cs): a mock service which will be called from the test, instead of calling the real service
    - [DiscountSvcTests](/Contract-Testing/ConsumerTests/DiscountSvcTests.cs): a test that is reliant on the mock service. Because of the mock service, there is no need to run the service itself. Running the test will leverage **Pact Net** and the mock microservice, and will generate the contract file (see [example](/example/orders-discounts.json)).
  - [ProviderTests](/Contract-Testing/ProviderTests/ProviderTests.csproj) uses the information from the generated contract file (see [example](/example/orders-discounts.json)) to call the Discount microservice and confirm that the contract has not broken.

- ### Through Message Queues

  [MessageMicroservices](/MessageMicroservices/MessageMicroservices.sln) solution contains two microservice projects (for details see [here](#microservice-messaging)) and their appropriate test projects:

  - [ConsumerTests](/MessageMicroservices/ConsumerTests/ConsumerTests.csproj)
  - [ProducerTests](/MessageMicroservices/ProducerTests/ProducerTests.csproj)

## Getting Started

### Prerequisites

- [.NET Core SDK 6.0+](https://dotnet.microsoft.com/download/dotnet/6.0)
- [RabbitMQ](https://www.rabbitmq.com/download.html)
- [Docker](https://www.docker.com/) (optional, for running RabbitMQ in a container)
- [Pact Net](https://github.com/pact-foundation/pact-net)

### Installation

Clone the repository:

```bash
git clone https://github.com/LykourgosS/microservice-messaging-testing.git
cd microservice-messaging-testing
```

## Running the Project

1. Navigate to the appropriate folder:

   ```bash
   cd MessageMicroservices
   ```

2. Start RabbitMQ locally using Docker or install it directly. To run with Docker:

   ```bash
   docker run -d --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management
   ```

3. Launch the microservices and the test application, allowing you to observe message communication through RabbitMQ. Execute following commands in separate terminals:

   ```bash
   dotnet run --project .\InvoiceMicroservice\InvoiceMicroservice.csproj
   ```

   ```bash
   dotnet run --project .\PaymentMicroservice\PaymentMicroservice.csproj
   ```

   ```bash
   dotnet run --project .\TestClient\TestClient.csproj
   ```

> [!NOTE]
> See example video for messaging [here](/docs/messaging-demo.mp4).

## Testing

Following commands will execute the **Pact Net** contract tests to ensure both services comply with the communication contract.

- ### Over HTTP Protocol

  1.  Navigate to the `Contract-Testing` folder:

      ```bash
      cd Contract-Testing
      ```

  2.  Run test for consumer service (to generate the contract file):

      ```bash
      cd ConsumerTests
      dotnet test
      ```

  3.  Run provider service (i.e. DiscountSvc-Provider) in a separate terminal:

      ```bash
      cd DiscountSvc-Provider
      dotnet run
      ```

  4.  And now that the provider service is up and running, run test for it (to check if contact has broken based on the generated contract file):

      ```bash
      cd ..
      cd ProviderTests
      dotnet test
      ```

- ### Through Message Queues

  1.  Navigate to the `MessageMicroservices` folder:

      ```bash
      cd MessageMicroservices
      ```

  2.  Run test for consumer service (i.e. PaymentMicroservice):

      ```bash
      cd ConsumerTests
      dotnet test
      ```

  3.  Run test for producer service (i.e. InvoiceMicroservice):

      ```bash
      cd ..
      cd ProducerTests
      dotnet test
      ```

> [!NOTE]
> See example video for testing [here](/docs/testing-demo.mp4).

> [!WARNING]
> Due to a [known issue with ruby](https://github.com/ruby/ruby/pull/4505) the cloned repository should be located close to the root `C:\` (suggested solutions can be found [here](https://stackoverflow.com/a/66228021/20059477) and [here](https://stackoverflow.com/questions/66791308/running-pact-test-is-throwing-a-ruby-load-error)).

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
