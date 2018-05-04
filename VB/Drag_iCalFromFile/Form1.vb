Imports System
Imports System.Data
Imports System.Data.OleDb
Imports System.Drawing
Imports System.IO
Imports System.Windows.Forms
Imports DevExpress.XtraScheduler
Imports DevExpress.XtraScheduler.Drawing
Imports DevExpress.XtraScheduler.iCalendar
Imports DevExpress.XtraScheduler.iCalendar.Components

Namespace Drag_iCalFromFile
    Partial Public Class Form1
        Inherits Form

        ' Drop destination time.
        Private targetTime As Date
        ' The number of events in a calendar file being dropped.
        Private eventsCount As Integer
        ' Time intervals of events in a calendar file.
        Private timeCollectionEx As New TimeIntervalCollectionEx()


        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
            ' TODO: This line of code loads data into the 'appointmentsDataSet.Resources' table. You can move, or remove it, as needed.
            Me.resourcesTableAdapter.Fill(Me.appointmentsDataSet_Renamed.Resources)
            ' TODO: This line of code loads data into the 'appointmentsDataSet.Appointments' table. You can move, or remove it, as needed.
            Me.appointmentsTableAdapter.Fill(Me.appointmentsDataSet_Renamed.Appointments)
            AddHandler appointmentsTableAdapter.Adapter.RowUpdated, AddressOf appointmentsTableAdapter_RowUpdated

        End Sub

        #Region "DataBinding"
        Private Sub OnApptChangedInsertedDeleted(ByVal sender As Object, ByVal e As PersistentObjectsEventArgs) Handles schedulerStorage1.AppointmentsChanged, schedulerStorage1.AppointmentsInserted, schedulerStorage1.AppointmentsDeleted
            Me.appointmentsTableAdapter.Update(Me.appointmentsDataSet_Renamed)
            Me.appointmentsDataSet_Renamed.AcceptChanges()

        End Sub

        Private Sub appointmentsTableAdapter_RowUpdated(ByVal sender As Object, ByVal e As OleDbRowUpdatedEventArgs)
            If e.Status = UpdateStatus.Continue AndAlso e.StatementType = StatementType.Insert Then
                Dim id As Integer = 0
                Using cmd As New OleDbCommand("SELECT @@IDENTITY", appointmentsTableAdapter.Connection)
                    id = DirectCast(cmd.ExecuteScalar(), Integer)
                End Using
                e.Row("ID") = id
            End If
        End Sub
        #End Region

        #Region "Drag-n-Drop"
        Private Sub schedulerControl1_DragEnter(ByVal sender As Object, ByVal e As DragEventArgs) Handles schedulerControl1.DragEnter
            e.Effect = DragDropEffects.Move
        End Sub

        Private Sub schedulerControl1_DragDrop(ByVal sender As Object, ByVal e As DragEventArgs) Handles schedulerControl1.DragDrop
            If e.Data.GetDataPresent(DataFormats.FileDrop) Then
                Dim pt As Point = schedulerControl1.PointToClient(New Point(e.X, e.Y))
                GetTargetTime(pt)
                DoFileDrop(pt, DirectCast(e.Data.GetData(DataFormats.FileDrop), String()))
            End If
        End Sub
        Private Sub GetTargetTime(ByVal pt As Point)
            Dim hitInfo As SchedulerHitInfo = schedulerControl1.ActiveView.ViewInfo.CalcHitInfo(pt, True)
            If hitInfo.HitTest = SchedulerHitTest.Cell Then
                Dim cell As SelectableIntervalViewInfo = hitInfo.ViewInfo
                targetTime = cell.Interval.Start
            Else
                targetTime = Date.MinValue
            End If
        End Sub
        #End Region

        Private Sub DoFileDrop(ByVal pt As Point, ByVal files() As String)
            If files IsNot Nothing AndAlso files.Length > 0 Then
                For k As Integer = 0 To files.Length - 1
                    Try
                        ImportAppointments(files(k), pt)
                    Catch e1 As iCalendarInvalidFileFormatException
                        Dim message As String = String.Format("The file ""{0}"" is not a valid Internet Calendar file", Path.GetFileName(files(0)))
                        MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Catch ex As iCalendarEventImportException

                        Dim events_Renamed As VEventCollection = ex.Events
                        Dim count As Integer = events_Renamed.Count
                        Dim message As String = String.Empty
                        For i As Integer = 0 To count - 1
                            Dim ev As VEvent = ex.Events(i)
                            message &= String.Format("Unable to import event '{0}' started on {1:D} at {2}" & vbLf, ev.Summary.Value, ev.Start.Value.Date, ev.Start.Value.TimeOfDay)
                        Next i
                        MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Finally
                        AfterImportActions()
                    End Try
                Next k
            End If
        End Sub

        Private Sub ImportAppointments(ByVal filePath As String, ByVal pt As Point)
            If filePath Is Nothing Then
                Return
            End If
            ' Create an Importer object, subscribe to its events and perform the import.
            Dim importer As New iCalendarImporter(schedulerStorage1)
            AddHandler importer.CalendarStructureCreated, AddressOf importer_CalendarStructureCreated
            AddHandler importer.AppointmentImporting, AddressOf importer_AppointmentImporting
            importer.Import(filePath)
        End Sub

        #Region "iCalImport"
        Private Sub importer_AppointmentImporting(ByVal sender As Object, ByVal e As AppointmentImportingEventArgs)
            ' TODO: check whether a particular appointment should be imported.

            ' If a file contains a single appointment, prompt to place it at the drop destination time.
            If (eventsCount = 1) AndAlso (targetTime <> Date.MinValue) Then
               Dim doIt As DialogResult = MessageBox.Show("You can set the appointment start time to a drop destination time" & vbLf & "at " & targetTime.ToString() & "." & vbLf & "Proceed?", "Info", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                If doIt = System.Windows.Forms.DialogResult.Yes Then
                    e.Appointment.Start = targetTime
                End If
            End If

            timeCollectionEx.Add(New TimeInterval(e.Appointment.Start, e.Appointment.End))

        End Sub
        Private Sub importer_CalendarStructureCreated(ByVal sender As Object, ByVal e As iCalendarStructureCreatedEventArgs)
            Dim importer As iCalendarImporter = DirectCast(sender, iCalendarImporter)
            Dim maxNoOfAppointments As Integer = importer.SourceObjectCount
            ' TODO: use the events count to adjust the scheduler view or to initialize indicators.
            eventsCount = maxNoOfAppointments
        End Sub

        Private Sub AfterImportActions()
            ' Display recently added events.
            schedulerControl1.GoToDate(timeCollectionEx.Start)
            timeCollectionEx.Clear()
            ' TODO: do whatever you have to.
        End Sub
        #End Region

    End Class
End Namespace