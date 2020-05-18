using Microsoft.Azure.WebJobs;

namespace Riva.Announcements.Functions
{
    public static class CreateAnnouncementFunction
    {
        [FunctionName(ConstantVariables.CreateAnnouncementFunctionName)]
        public static void Run(
            [ServiceBusTrigger(
                ConstantVariables.ServiceBusTopicName, 
                ConstantVariables.CreateAnnouncementSubscriptionName, 
                Connection = ConstantVariables.ServiceBusConnectionStringName)]
            string inputMessage,
            [CosmosDB(
                ConstantVariables.CosmosDbDatabaseName, 
                ConstantVariables.CosmosDbCollectionName, 
                ConnectionStringSetting = ConstantVariables.CosmosDbConnectionStringName)] 
            out object document)
        {
            document = inputMessage;
        }
    }
}
