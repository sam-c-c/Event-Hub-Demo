var resourceGroupName = 'CloudMonthExample2'
var resourceLocation = 'uksouth'
var serviceBusName = 'cloudmonth2-eventhub-bus-${substring(subscription().subscriptionId,0,6)}'

targetScope = 'subscription'

resource resourceGroup 'Microsoft.Resources/resourceGroups@2021-04-01' = {
  name: resourceGroupName
  location: resourceLocation
}

module serviceBus './modules/create-service-bus.bicep' = {
  scope: resourceGroup
  name: serviceBusName
  params: {
    resourceLocation: resourceLocation
    serviceBusName: serviceBusName
    queueName: 'events'
  }
}
