alter table lt_system_configuration
  add useaviadocumentvatinorder tinyint(1) NOT NULL after allowagentsetinvoicevat,
  add aviadocumentvatoptions int(11) NOT NULL;

update lt_system_configuration
  set useaviadocumentvatinorder = if(invoicevatoptions = 0, 0, 1),
      aviadocumentvatoptions = if(invoicevatoptions = 0 or invoicevatoptions = 1, 0, 1);

alter table lt_system_configuration
  drop column invoicevatoptions;

  
alter table lt_system_configuration
  add accountantdisplaystring varchar(255) after aviadocumentvatoptions;