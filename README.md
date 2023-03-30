# Event Hub Demo

This demo application consists of 2 Azure Functions. One is triggered by HTTP requests and the other is triggered by a message going into a queue in the Azure Service Bus.

In order to run the solution first you need to create the Service Bus. In the .infrastructure folder there is an Azure Resource Manager (ARM) template that creates a resource group, the service bus and the required queue needed for the demo.

To run this you need to use the Azure CLI. The instructions to installing that can be found [here](https://learn.microsoft.com/en-us/cli/azure/install-azure-cli).

Once you have Azure CLI installed you can run the following commands:

```
az login

az deployment sub create --location uksouth --template-file ./.infrastructure/EventHubARMTemplate.json

az logout
```

This will ask you to log into your Azure account, run the template file (make sure you run this at the root of your project) and then log you back out of your account.

Once run you should see something like this in your resources:

![image](https://user-images.githubusercontent.com/73018467/228929250-e37eb781-da4f-43b7-8c83-31e498d0730c.png)

Select the service bus, under Settings on the left hand side select Shared access policies and then click RootManageSharedAccessKey:

![image](https://user-images.githubusercontent.com/73018467/228929790-358176cd-e240-435f-bbe8-be438329e302.png)

On the next page, copy the value for the Primary Connection String. This is needed by the Azure Functions.

In Visual Studio, expand EventHub.Dispatch project and select the local.settings.json file. Replace the CHANGE THIS text for the ServiceBusConfiguration:ConnectionString property with the Primary Connection String.

Repeat the step above for the EventHub.Listener.

Finally, right click the EventHub.Dispatch & Start Without Debugging. Do the same for the EventHub.Listener.

You can then send a request using a tool like Postman or Insomnia to the Dispatch endpoint:

```
{
	"applicationId": "a5580c68-e4f6-4686-bd21-cb37465ba7e6",
	"applicationName": "my-application",
	"eventId": 123,
	"eventName": "This is a test event"
}
```
You should see something like the below, where the one on the left is the dispatch function and the one on the right is the listener:
![image](https://user-images.githubusercontent.com/73018467/228932666-0ec422c8-c16e-40f7-92ec-ecbac5d5a92f.png)
