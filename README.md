# The commits after 10:30 are only for the readme.md!!!
# Go to - running the project
[Running the project](https://github.com/ImSk1/SoftUni-Fest-2023/tree/main#running-the-project)
# Payment Service Application for Softuni Fest 2023
- <img src="https://github.com/ImSk1/SoftUni-Fest-2023/assets/68961310/2cf27d7a-b4d1-430a-a3ea-14ee5d18e723" alt="Screenshot 1 Title" width="300" />
- <img src="https://github.com/ImSk1/SoftUni-Fest-2023/assets/68961310/07101beb-db98-493d-9b6a-37466daeaa1c" alt="Screenshot 1 Title" width="300" />
- <img src="https://github.com/ImSk1/SoftUni-Fest-2023/assets/68961310/afe5791b-0851-405f-8a12-b85f0f81a365" alt="Screenshot 1 Title" width="300" />
 
## Description

The Payment Service Application is a platform designed to facilitate online transactions between businesses and their clients. It enables businesses to register and configure their products or services for seamless payment processing, while allowing clients to discover merchants and make purchases using the Stripe platform and Metamask for crypto purchases. The application offers distinct interfaces for both business and client users within a unified environment, ensuring a smooth and efficient payment experience.

## Team

- [David Petkov](https://github.com/dpS1lence)
- [David Hristov](https://github.com/ImSk1)
- [Emil Dolchinkov](https://github.com/EmilDol)

## Technologies

- **ASP.NET Core 7.0**: Framework for building web applications and services with .NET.
- **AutoMapper**: A convention-based object-object mapper.
- **MailKit**: A cross-platform mail client library for .NET.
- **Entity Framework Core**: A modern object-database mapper for .NET.
- **Polly**: A resilience and transient-fault-handling library.
- **Serilog**: A diagnostic logging library.
- **Stripe.net**: .NET client library for the Stripe API.

## Running the Project

!Set The Environment in - Properties/launchsettings.json from `"ASPNETCORE_ENVIRONMENT": "Development"` to `"ASPNETCORE_ENVIRONMENT": "Production"`!

You will need to download the [MetaMask](https://metamask.io/download/) extention for your browser.
You will need a MetaMask wallet.
For testing you can fund your wallet from [sepoliafauscet](https://sepoliafaucet.com/).
Uppon Business registration you have to fill your wallet id that can be found in the extention!
- <img src="https://github.com/ImSk1/SoftUni-Fest-2023/assets/68961310/f36236d2-48b5-4150-bf42-e973e23c9349" alt="Screenshot 1 Title" width="300" />
<br>
- <img src="https://github.com/ImSk1/SoftUni-Fest-2023/assets/68961310/0b28ca79-d024-4a8b-8754-6bef8b55781d" alt="Screenshot 1 Title" width="300" /><br>
For the Stripe connection you can just click the skip button for testing.<br>
In order to run the project, you will need to add an `appsettings.json` file in the project's folder. It should look something like this:<br>
If something doesent work as expected try commenting out the `await DatabaseMiddleware.MigrateDatabase(scope, app.Configuration, app.Logger);` row in the `Program.cs` file and run the `update-database` command in the pm console.<br>

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    },
    "Urls": {
      "SeqServerUrl": "http://localhost:5341"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "<insert your connection string>."
  },
  "Stripe": {
    "PublicKey": "pk_test_51O5kkhC2AQuDpkcCKNeCZpFgnecsVYSi5wKMkE6r4278nggXUkCcSknQ1PetNGftCUMuK4i8V3ioMGAdBcS8nABy00YXkmzWp6",
    "SecretKey": "sk_test_51O5kkhC2AQuDpkcC1Uf5g5GO5B9iQtnliBzu4pnnJpNJHShPckiFg82U31Vg5zItVjq1ld6JM2UPUgthjXjJrWJA00vek8CL7e",
    "ClientId": "ca_OtZObyUT5NlcNdDQNhF5chQlJcxGzQTX"
  },
  "EmailSending": {
    "SmtpServer": "smtp.gmail.com",
    "SmtpPort": 587,
    "SmtpUsername": "notiflexnoreply@gmail.com",
    "SmtpPassword": "eueuusntmpvxfahb",
    "FromName": "Insightify",
    "FromAddress": "notiflexnoreply@gmail.com"
  },
  "EtherScan": {
    "ApiKey": "NG1XRHGEXWADZBY2TJJGP2KFU7P3CEAZY1"
  }
}
```
## Screenshots
### Home Page
 ![alt text](https://github.com/ImSk1/SoftUni-Fest-2023/assets/68961310/a23699b8-6fbb-4a33-a37c-b60aa00b2e09)
  ### Business Register
 ![alt text](https://github.com/ImSk1/SoftUni-Fest-2023/assets/68961310/149e4b50-0411-4f05-a53f-0b77e42854db)
  ### Client Register
 ![alt text](https://github.com/ImSk1/SoftUni-Fest-2023/assets/68961310/32b71b56-4169-4203-ace2-407acc5eede4)
  ### Log in
 ![alt text](https://github.com/ImSk1/SoftUni-Fest-2023/assets/68961310/db82d22b-836f-4ba2-a3fb-3eb7726517cf)
  ### Stripe Payment
 ![alt text](https://github.com/ImSk1/SoftUni-Fest-2023/assets/68961310/b95d02fb-5324-4321-b465-ca35ba9fc63f)
  ### Etherium Payment
 ![alt text](https://github.com/ImSk1/SoftUni-Fest-2023/assets/68961310/0a75851f-9154-4174-9799-dea8d9ad1d16)
  ### Business Dashboard
 ![alt text](https://github.com/ImSk1/SoftUni-Fest-2023/assets/68961310/4b9e9c6d-5970-4eeb-8fff-eb1526a7060f)
 ![alt text](https://github.com/ImSk1/SoftUni-Fest-2023/assets/68961310/33b6a3f8-c251-446e-814f-d6be71f60a1a)
  ### Create Listing
 ![alt text](https://github.com/ImSk1/SoftUni-Fest-2023/assets/68961310/b9208b7b-9e1e-4356-ac44-87e9abd77c8e)
  ### Your Listings
 ![alt text](https://github.com/ImSk1/SoftUni-Fest-2023/assets/68961310/a34eea40-f40e-43c4-8a6c-6012a01097e9)
  ### Product Details (When the product is owned by your business)
 ![alt text](https://github.com/ImSk1/SoftUni-Fest-2023/assets/68961310/8dcb0d70-6776-4b83-8db3-863c35afa4b0)
  ### Product Details (When you are client)
 ![alt text](https://github.com/ImSk1/SoftUni-Fest-2023/assets/68961310/bec2b198-be01-4d70-83af-c62dee2e7081)
  ### Payments
 ![alt text](https://github.com/ImSk1/SoftUni-Fest-2023/assets/68961310/83ed772f-f798-4c64-8635-49f89304ab95)
  ### Retailers
![alt text](https://github.com/ImSk1/SoftUni-Fest-2023/assets/68961310/54943a31-840f-45be-8fe0-acba62c5a1bd)
