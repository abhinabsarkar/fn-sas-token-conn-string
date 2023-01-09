# Azure Function - Generate SAS token for Storage account
Azure Function to return a SAS token for a file within a given Storage Account

### Pre-requisites to run this sample
* A Storage account 
* Create a container named `images` in the storage account

### Running it locally with Storage Account in Azure
Add the connection string for the storage account in the `local.settings.json` file for `saabhiimages` & `AzureWebJobsStorage`. Run the function app from VS code.

Test it by running a curl request & replace the `blobname` with the file stored in `images` container

`http://localhost:7071/api/{blobname}`

```bash
# request
curl http://localhost:7071/api/betta-fish.png
# response
sv=2021-10-04&spr=https&st=2023-01-09T19%3A42%3A32Z&se=2023-01-09T19%3A52%3A32Z&sr=b&sp=r&sig=xil%2BD6aQzPl0Wx5SnvMPvzVKzlJgh9X3KM5jWCN4GaU%3D
```

### Running it on Azure
Add the application setting `AzureWebJobssaabhiimages` in function configuration.

![alt txt](/images/appsettings.jpg)

```bash
# run the curl request 
curl https://fn-valet-key.azurewebsites.net/api/betta-fish.png?code=xxxxxxxxxxxxx
```

