// instantiate mail services
using Decorator;

var cloudMailService = new CloudMailService();
cloudMailService.SendMail("Hi there."); //Default behavior of this service

var onPremiseMailService = new OnPremiseMailService();
onPremiseMailService.SendMail("Hi there."); //Default behavior of this service

// add behavior, to collect statistics
var statisticsDecorator = new StatisticsDecorator(cloudMailService); //Decorate the cloudMailService with StatisticsDecorator
statisticsDecorator.SendMail($"Hi there via {nameof(StatisticsDecorator)} wrapper.");//send mail through that StatisticsDecorator, 
//this should show us some added behavior, in other words, some added functionality

// add behavior, the decorators work on the MailService interfaces and not on the concrete implementations. So, we send a few messages
// through the MessageDatabaseDecorator, and then we run through the stored messages to see whether or not our decorator successfully
// executed.
var messageDatabaseDecorator = new MessageDatabaseDecorator(onPremiseMailService);
messageDatabaseDecorator.SendMail($"Hi there via {nameof(MessageDatabaseDecorator)} wrapper, message 1.");
messageDatabaseDecorator.SendMail($"Hi there via {nameof(MessageDatabaseDecorator)} wrapper, message 2.");

foreach (var message in messageDatabaseDecorator.SentMessages)

{
    Console.WriteLine($"Stored message: \"{message}\"");
}

Console.ReadKey();