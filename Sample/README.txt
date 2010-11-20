This sample illustrates the use of SagePayMvc within an ASP.NET MVC 2 application.

The application uses a simplified store model to illustrate the process. 
It makes use of in-memory collections to simulate a database - this approach should not be used in a production application.

The sample makes use of the StructureMap library for dependency injection.

The core classes in the sample are:
- The BasketController. 
    This illustrates sending a request using a transaction service (ITransactionService)
- TransactionService - the default implementation of ITransactionService. 
    This illustrates how to convert the store's internal model into a collection of SagePayMvc's BasketItem objects. 
    It then uses SagePayMvc's TransactionRegistrar class (ITransactionRegistrar) to register a payment with SagePay. 
- The PaymentResponseController
    Illustrates how to receive a notification from SagePay using the SagePayResponse 
    in order to mark the transaction as either successful or failed.

SagePayMvc is configured using the <sagePay /> element of the web.config.
You will need to change the following settings in order for the sample to work:
- VendorName. This must be set to your vendor name registered with SagePay.
- NotificationHostName. The external host name of your webserver that SagePay should use when sending notifications.
The final notification URL is constructed from the NotificationHostName in conjunction with the NotificationController.