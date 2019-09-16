# Control de Horario
Aplicación gratuita para el registro la jornada laboral de los empleados | Real Decreto-ley 8/2019 | España

## Tecnología

Para la detección de las caras y autenticación se usa [Azure Congnite Services - Faces](https://azure.microsoft.com/es-es/services/cognitive-services/face/)

Para salvar los registros horarios se utiliza: [Azure Table Storage](https://azure.microsoft.com/es-es/services/storage/tables/)

## Funcionalidad

Nota: La applicación web es muy limitada, la entrada de empleados se hace mediante la API. (ver swagger para más info)

Login mediante reconocimiento facial.

![alt text](https://i.ibb.co/fntyX5T/login.png)

Introducción de entradas con la fecha actual o con una fecha personalizada.

![alt text](https://i.ibb.co/pynxrNT/records.png)

## Configuración

Ejemplo de appsettings.json

```
{
  "Logging": {
    "LogLevel": {
      "Default": "Warning"
    }
  },
  "AllowedHosts": "*",
  "AzureTableOptions": {
    "ConnectionString": "DefaultEndpointsProtocol=https;AccountName=....;AccountKey=...",
    "PersonTableName": "person",
    "RecordTableName": "record",
    "EmotionTableName": "emotion"
  },
  "FaceOptions": {
    "SubscriptionKey": "e13e58ba....",
    "Endpoint": "https://northeurope.api.cognitive.microsoft.com",
    "PersonGroupId": "person-group-id",
    "PersonGroupName": "person-group-name"
  }
}
```
