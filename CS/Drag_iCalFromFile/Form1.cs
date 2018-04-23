using System;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using DevExpress.XtraScheduler;
using DevExpress.XtraScheduler.Drawing;
using DevExpress.XtraScheduler.iCalendar;
using DevExpress.XtraScheduler.iCalendar.Components;

namespace Drag_iCalFromFile
{
    public partial class Form1 : Form
    {
        // Drop destination time.
        DateTime targetTime;
        // The number of events in a calendar file being dropped.
        int eventsCount;
        // Time intervals of events in a calendar file.
        TimeIntervalCollectionEx timeCollectionEx = new TimeIntervalCollectionEx();


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'appointmentsDataSet.Resources' table. You can move, or remove it, as needed.
            this.resourcesTableAdapter.Fill(this.appointmentsDataSet.Resources);
            // TODO: This line of code loads data into the 'appointmentsDataSet.Appointments' table. You can move, or remove it, as needed.
            this.appointmentsTableAdapter.Fill(this.appointmentsDataSet.Appointments);
            appointmentsTableAdapter.Adapter.RowUpdated += new OleDbRowUpdatedEventHandler(appointmentsTableAdapter_RowUpdated);

        }

        #region DataBinding
        private void OnApptChangedInsertedDeleted(object sender, PersistentObjectsEventArgs e)
        {
            this.appointmentsTableAdapter.Update(this.appointmentsDataSet);
            this.appointmentsDataSet.AcceptChanges();

        }

        private void appointmentsTableAdapter_RowUpdated(object sender, OleDbRowUpdatedEventArgs e)
        {
            if (e.Status == UpdateStatus.Continue && e.StatementType == StatementType.Insert)
            {
                int id = 0;
                using (OleDbCommand cmd = new OleDbCommand("SELECT @@IDENTITY", appointmentsTableAdapter.Connection))
                {
                    id = (int)cmd.ExecuteScalar();
                }
                e.Row["ID"] = id;
            }
        }
        #endregion

        #region Drag-n-Drop
        private void schedulerControl1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void schedulerControl1_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                Point pt = schedulerControl1.PointToClient(new Point(e.X, e.Y));
                GetTargetTime(pt);
                DoFileDrop(pt, (string[])e.Data.GetData(DataFormats.FileDrop));
            }
        }
        private void GetTargetTime(Point pt)
        {
            SchedulerHitInfo hitInfo = schedulerControl1.ActiveView.ViewInfo.CalcHitInfo(pt, true);
            if (hitInfo.HitTest == SchedulerHitTest.Cell)
            {
                SelectableIntervalViewInfo cell = hitInfo.ViewInfo;
                targetTime = cell.Interval.Start;
            }
            else targetTime = DateTime.MinValue;
        }
        #endregion

        private void DoFileDrop(Point pt, string[] files)
        {
            if (files != null && files.Length > 0)
            {
                for (int k = 0; k < files.Length; k++)
                {
                    try
                    {
                        ImportAppointments(files[k], pt);
                    }
                    catch (iCalendarInvalidFileFormatException)
                    {
                        string message = String.Format(@"The file ""{0}"" is not a valid Internet Calendar file", Path.GetFileName(files[0]));
                        MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (iCalendarEventImportException ex)
                    {
                        VEventCollection events = ex.Events;
                        int count = events.Count;
                        string message = String.Empty;
                        for (int i = 0; i < count; i++)
                        {
                            VEvent ev = ex.Events[i];
                            message += String.Format("Unable to import event '{0}' started on {1:D} at {2}\n", ev.Summary.Value, ev.Start.Value.Date, ev.Start.Value.TimeOfDay);
                        }
                        MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        AfterImportActions();
                    }
                }
            }
        }

        void ImportAppointments(string filePath, Point pt)
        {
            if (filePath == null)
                return;
            // Create an Importer object, subscribe to its events and perform the import.
            iCalendarImporter importer = new iCalendarImporter(schedulerStorage1);
            importer.CalendarStructureCreated += new iCalendarStructureCreatedEventHandler(importer_CalendarStructureCreated);
            importer.AppointmentImporting += new AppointmentImportingEventHandler(importer_AppointmentImporting);
            importer.Import(filePath);
        }

        #region iCalImport
        void importer_AppointmentImporting(object sender, AppointmentImportingEventArgs e)
        {
            // TODO: check whether a particular appointment should be imported.

            // If a file contains a single appointment, prompt to place it at the drop destination time.
            if ((eventsCount == 1) && (targetTime != DateTime.MinValue)) {
               DialogResult doIt = MessageBox.Show("You can set the appointment start time to a drop destination time\nat "
                   + targetTime.ToString() + ".\nProceed?",
                    "Info", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (doIt == DialogResult.Yes) e.Appointment.Start = targetTime;
            }

            timeCollectionEx.Add(new TimeInterval(e.Appointment.Start, e.Appointment.End));

        }
        void importer_CalendarStructureCreated(object sender, iCalendarStructureCreatedEventArgs e)
        {
            iCalendarImporter importer = (iCalendarImporter)sender;
            int maxNoOfAppointments = importer.SourceObjectCount;
            // TODO: use the events count to adjust the scheduler view or to initialize indicators.
            eventsCount = maxNoOfAppointments;
        }

        private void AfterImportActions()
        {
            // Display recently added events.
            schedulerControl1.GoToDate(timeCollectionEx.Start);
            timeCollectionEx.Clear();
            // TODO: do whatever you have to.
        }
        #endregion

    }
}