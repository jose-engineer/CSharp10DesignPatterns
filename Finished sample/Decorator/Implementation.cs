using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decorator
{
    /// <summary>
    /// ConcreteComponent1
    /// </summary>
    public sealed class CloudMailService : IMailService//This class couold be a sealed class and even then you can wrap it
    {
        public bool SendMail(string message)
        {
            Console.WriteLine($"Message \"{message}\" sent via {nameof(CloudMailService)}.");
            return true;
        }
    }

    /// <summary>
    /// ConcreteComponent2
    /// </summary>
    public sealed class OnPremiseMailService : IMailService
    {
        public bool SendMail(string message)
        {
            Console.WriteLine($"Message \"{message}\" sent via {nameof(OnPremiseMailService)}.");
            return true;
        }
    }

    /// <summary>
    /// Component (as interface)
    /// </summary>
    public interface IMailService
    {
        bool SendMail(string message);
    }


    /// <summary>
    /// Decorator (as abstract base class)
    /// 
    /// This class wraps the IMailService, but it doesn't add functionality to the mailService. This is an abstract class to store 
    /// the IMailservice instance. You could name this a base wrapper. We don't want this to be used on its own, so we mark it as
    /// abstract.
    /// </summary>
    public abstract class MailServiceDecoratorBase : IMailService
    {
        private readonly IMailService _mailService;
        public MailServiceDecoratorBase(IMailService mailService)
        {
            _mailService = mailService;
        }
        
        public virtual bool SendMail(string message)//mark as virtual so implementing classes can override this and add functionality.
        {
            return _mailService.SendMail(message);
        }
    }

    /// <summary>
    /// ConcreteDecorator1
    /// </summary>
    public class StatisticsDecorator : MailServiceDecoratorBase //Implements the MailServiceDecoratorBase abstract clas
    {
        public StatisticsDecorator(IMailService mailService) 
            : base(mailService)//Call into base constructor, so mailService is stored in the private field '_mailService' from the base wrapper.
        {
        }

        public override bool SendMail(string message)//Override the SendMail method, collect some statistics, and send the mail.
        {
            // Fake collecting statistics 
            Console.WriteLine($"Collecting statistics in {nameof(StatisticsDecorator)}."); //ADDED FUNCTIONALITY
            return base.SendMail(message);//call base.SendMail, as the actual implementation of sending mail is at that level.
        }
    }

    /// <summary>
    /// ConcreteDecorator2
    /// </summary>
    public class MessageDatabaseDecorator : MailServiceDecoratorBase
    {
        /// <summary>
        /// A list of sent messages - a "fake" database
        /// </summary>
        public List<string> SentMessages { get; private set; } = new List<string>();

        public MessageDatabaseDecorator(IMailService mailService)
            : base(mailService)
        {
        }

        public override bool SendMail(string message)
        {
            if (base.SendMail(message))
            {
                // store sent message
                SentMessages.Add(message);
                return true;
            };

            return false;
        }
    }
}
