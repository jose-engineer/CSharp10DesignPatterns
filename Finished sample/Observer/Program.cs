using Observer;

TicketStockService ticketStockService = new();
TicketResellerService ticketResellerService = new();
OrderService orderService = new();

// add two observers
orderService.AddObserver(ticketStockService);//As a parameter to AddObserver we need to pass something that implements ITicketChangeListener
orderService.AddObserver(ticketResellerService);//and both our TicketStockService and TicketResellerService do.

// notify, artist 1, amount 2
orderService.CompleteTicketSale(1, 2);//This call should result in both observers being notified of this.
                                      //The nice thing here is that we can manage all of this at runtime. We added the observers at runtime.
Console.WriteLine();

// remove one observer
orderService.RemoveObserver(ticketResellerService);

// notify, artist 2, amount 4
orderService.CompleteTicketSale(2, 4);
