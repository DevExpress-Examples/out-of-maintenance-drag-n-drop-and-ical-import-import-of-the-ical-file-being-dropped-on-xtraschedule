# Drag-n-drop and iCal import - import of the iCal file being dropped on XtraScheduler


<p>This example implements the functionality to allow iCal files to be dropped on the XtraScheduler and the appointments are imported. If a file contains a single appointment, the user is prompted to change its start time to the time of the cell that the file is being dropped to. </p><p>The DragDrop event of the Scheduler control is handled to check the file being dropped, to determine the target time cell using the <a href="http://documentation.devexpress.com/#WindowsForms/clsDevExpressXtraSchedulerDrawingSchedulerHitInfotopic">SchedulerHitInfo</a> object and to call the import routine. In that routine, the <a href="http://documentation.devexpress.com/#WindowsForms/clsDevExpressXtraScheduleriCalendariCalendarImportertopic">iCalendarImporter</a> object is created, its <a href="http://documentation.devexpress.com/#WindowsForms/DevExpressXtraScheduleriCalendariCalendarImporter_CalendarStructureCreatedtopic">CalendarStructureCreated</a> and <a href="http://documentation.devexpress.com/#WindowsForms/DevExpressXtraSchedulerExchangeAppointmentImporter_AppointmentImportingtopic">AppointmentImporting</a> events are handled. The <a href="http://documentation.devexpress.com/#WindowsForms/DevExpressXtraSchedulerExchangeAppointmentImporter_Importtopic">Import</a> method accomplishes the task.</p><p>Note: the Scheduler is bound to the Access 2000 .mdb database, included in the project.</p>

<br/>


