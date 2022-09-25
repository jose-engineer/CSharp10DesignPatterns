namespace Observer
{
    //Helper class. Hold the state change information that needs to be sent to observers. For example, a ticket change for artists with ID 1
    //and amount 2 means that 2 tickets have been sold for this artist. It's an object of this type that will be received by observers.
    public class TicketChange
    {
        public int Amount { get; private set; }
        public int ArtistId { get; private set; }

        public TicketChange(int artistId, int amount)
        {
            ArtistId = artistId;
            Amount = amount;
        }
    }

    /// <summary>
    /// Subject (Notify, Add observer and Remove Observer)
    /// It's an abstract class because we don't want it to be used on its own because it doesn't contain the state information observers 
    /// want to be notified about.
    /// </summary>
    public abstract class TicketChangeNotifier
    {
        private List<ITicketChangeListener> _observers = new();// list of observers, objects that implement ITicketChangeListener.

        public void AddObserver(ITicketChangeListener observer)//Since Concrete Observers implements ITicketChangeListener we can add it
        {
            _observers.Add(observer);
        }

        public void RemoveObserver(ITicketChangeListener observer)
        {
            _observers.Remove(observer);
        }

        public void Notify(TicketChange ticketChange)
        {
            foreach (var observer in _observers)
            {
                observer.ReceiveTicketChangeNotification(ticketChange);
            }
        }
    }

    /// <summary>
    /// ConcreteSubject (managing state)
    /// </summary>
    public class OrderService : TicketChangeNotifier //OrderService is the subclass of TicketChangeNotifier.
    {
        //CompleteTicketSale is called by clients when a ticket sale is completed. That means that at this moment, the OrderService must
        //update its state. It must update its data store so the correct amount of tickets for the correct artist is deducted (when the order
        //is fulfilled, the tickets should not be available anymore). For demo purposes, we'll assume that when we see an OrderService is
        //changing its state message that the local data store is adjusted.
        public void CompleteTicketSale(int artistId, int amount)
        {
            // change local datastore.  Datastore omitted in demo implementation.
            Console.WriteLine($"{nameof(OrderService)} is changing its state.");
            // notify observers 
            Console.WriteLine($"{nameof(OrderService)} is notifying observers...");
            Notify(new TicketChange(artistId, amount));//calls Notify method on its base class (TicketChangeNotifier) to notify observers
        }         
    }

    /// <summary>
    /// Observer
    /// </summary>
    public interface ITicketChangeListener
    {
        void ReceiveTicketChangeNotification(TicketChange ticketChange);
    }

    /// <summary>
    /// ConcreteObserver
    /// </summary>
    public class TicketResellerService : ITicketChangeListener
    {
        //This method gets a state change from the concrete subject passed in, and that is the TicketChange object. Thanks to the info it
        //gets from that object, it can adjust its own data store accordingly (for TicketResellerService or TicketStockService).
        public void ReceiveTicketChangeNotification(TicketChange ticketChange)
        {
            // update local datastore (datastore omitted in demo implementation)
            Console.WriteLine($"{nameof(TicketResellerService)} notified " +
                $"of ticket change: artist {ticketChange.ArtistId}, amount {ticketChange.Amount}");
        }
    }

    /// <summary>
    /// ConcreteObserver
    /// </summary>
    public class TicketStockService : ITicketChangeListener
    {
        public void ReceiveTicketChangeNotification(TicketChange ticketChange)
        {
            // update local datastore (datastore omitted in demo implementation)
            Console.WriteLine($"{nameof(TicketStockService)} notified " +
                $"of ticket change: artist {ticketChange.ArtistId}, amount {ticketChange.Amount}");
        }
    }
}
