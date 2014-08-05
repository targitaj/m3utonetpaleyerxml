CREATE TABLE [dbo].[VetumaPayment]
(
	[Id] INT NOT NULL IDENTITY, 
	[ApplicationFormId] INT NOT NULL,
	
	[TransactionId] NVARCHAR(20) NOT NULL, 
	[PaidSum] FLOAT NOT NULL, 
    [IsPaid] BIT NOT NULL, 

    [OrderNumber] INT NOT NULL, 
    [ReferenceNumber] NVARCHAR(50) NOT NULL, 
    [ArchivingCode] NVARCHAR(50) NULL, 
   
    [PaymentId] NVARCHAR(50) NULL, 
    [CreationDate] DATETIME NULL, 
    [PaymentDate] DATETIME NULL, 
    CONSTRAINT [FK_VetumaPaymentModel_ToEsrvApplication] FOREIGN KEY ([ApplicationFormId]) REFERENCES [dbo].[ApplicationForm]([ApplicationFormId])
)