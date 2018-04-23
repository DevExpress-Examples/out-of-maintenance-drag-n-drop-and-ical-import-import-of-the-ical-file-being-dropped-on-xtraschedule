Imports Microsoft.VisualBasic
Imports System
Namespace Drag_iCalFromFile
	Partial Public Class Form1
		''' <summary>
		''' Required designer variable.
		''' </summary>
		Private components As System.ComponentModel.IContainer = Nothing

		''' <summary>
		''' Clean up any resources being used.
		''' </summary>
		''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		Protected Overrides Sub Dispose(ByVal disposing As Boolean)
			If disposing AndAlso (components IsNot Nothing) Then
				components.Dispose()
			End If
			MyBase.Dispose(disposing)
		End Sub

		#Region "Windows Form Designer generated code"

		''' <summary>
		''' Required method for Designer support - do not modify
		''' the contents of this method with the code editor.
		''' </summary>
		Private Sub InitializeComponent()
			Me.components = New System.ComponentModel.Container()
			Dim timeRuler1 As New DevExpress.XtraScheduler.TimeRuler()
			Dim timeRuler2 As New DevExpress.XtraScheduler.TimeRuler()
			Me.schedulerControl1 = New DevExpress.XtraScheduler.SchedulerControl()
			Me.schedulerStorage1 = New DevExpress.XtraScheduler.SchedulerStorage(Me.components)
			Me.appointmentsDataSet = New Drag_iCalFromFile.AppointmentsDataSet()
			Me.appointmentsBindingSource = New System.Windows.Forms.BindingSource(Me.components)
			Me.appointmentsTableAdapter = New Drag_iCalFromFile.AppointmentsDataSetTableAdapters.AppointmentsTableAdapter()
			Me.resourcesBindingSource = New System.Windows.Forms.BindingSource(Me.components)
			Me.resourcesTableAdapter = New Drag_iCalFromFile.AppointmentsDataSetTableAdapters.ResourcesTableAdapter()
			CType(Me.schedulerControl1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.schedulerStorage1, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.appointmentsDataSet, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.appointmentsBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.resourcesBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SuspendLayout()
			' 
			' schedulerControl1
			' 
			Me.schedulerControl1.Dock = System.Windows.Forms.DockStyle.Fill
			Me.schedulerControl1.Location = New System.Drawing.Point(0, 0)
			Me.schedulerControl1.Name = "schedulerControl1"
			Me.schedulerControl1.Size = New System.Drawing.Size(746, 472)
			Me.schedulerControl1.Start = New System.DateTime(2008, 11, 19, 0, 0, 0, 0)
			Me.schedulerControl1.Storage = Me.schedulerStorage1
			Me.schedulerControl1.TabIndex = 0
			Me.schedulerControl1.Text = "schedulerControl1"
			Me.schedulerControl1.Views.DayView.TimeRulers.Add(timeRuler1)
			Me.schedulerControl1.Views.WorkWeekView.TimeRulers.Add(timeRuler2)
'			Me.schedulerControl1.DragEnter += New System.Windows.Forms.DragEventHandler(Me.schedulerControl1_DragEnter);
'			Me.schedulerControl1.DragDrop += New System.Windows.Forms.DragEventHandler(Me.schedulerControl1_DragDrop);
			' 
			' schedulerStorage1
			' 
			Me.schedulerStorage1.Appointments.DataSource = Me.appointmentsBindingSource
			Me.schedulerStorage1.Appointments.Mappings.AllDay = "AllDay"
			Me.schedulerStorage1.Appointments.Mappings.Description = "Description"
			Me.schedulerStorage1.Appointments.Mappings.End = "EndTime"
			Me.schedulerStorage1.Appointments.Mappings.Label = "Label"
			Me.schedulerStorage1.Appointments.Mappings.Location = "Location"
			Me.schedulerStorage1.Appointments.Mappings.RecurrenceInfo = "RecurrenceInfo"
			Me.schedulerStorage1.Appointments.Mappings.ReminderInfo = "ReminderInfo"
			Me.schedulerStorage1.Appointments.Mappings.ResourceId = "ResId"
			Me.schedulerStorage1.Appointments.Mappings.Start = "StartTime"
			Me.schedulerStorage1.Appointments.Mappings.Status = "Status"
			Me.schedulerStorage1.Appointments.Mappings.Subject = "Subject"
			Me.schedulerStorage1.Appointments.Mappings.Type = "EventType"
			Me.schedulerStorage1.Resources.DataSource = Me.resourcesBindingSource
			Me.schedulerStorage1.Resources.Mappings.Caption = "Name"
			Me.schedulerStorage1.Resources.Mappings.Id = "ID"
'			Me.schedulerStorage1.AppointmentsChanged += New DevExpress.XtraScheduler.PersistentObjectsEventHandler(Me.OnApptChangedInsertedDeleted);
'			Me.schedulerStorage1.AppointmentsInserted += New DevExpress.XtraScheduler.PersistentObjectsEventHandler(Me.OnApptChangedInsertedDeleted);
'			Me.schedulerStorage1.AppointmentsDeleted += New DevExpress.XtraScheduler.PersistentObjectsEventHandler(Me.OnApptChangedInsertedDeleted);
			' 
			' appointmentsDataSet
			' 
			Me.appointmentsDataSet.DataSetName = "AppointmentsDataSet"
			Me.appointmentsDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
			' 
			' appointmentsBindingSource
			' 
			Me.appointmentsBindingSource.DataMember = "Appointments"
			Me.appointmentsBindingSource.DataSource = Me.appointmentsDataSet
			' 
			' appointmentsTableAdapter
			' 
			Me.appointmentsTableAdapter.ClearBeforeFill = True
			' 
			' resourcesBindingSource
			' 
			Me.resourcesBindingSource.DataMember = "Resources"
			Me.resourcesBindingSource.DataSource = Me.appointmentsDataSet
			' 
			' resourcesTableAdapter
			' 
			Me.resourcesTableAdapter.ClearBeforeFill = True
			' 
			' Form1
			' 
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6F, 13F)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.ClientSize = New System.Drawing.Size(746, 472)
			Me.Controls.Add(Me.schedulerControl1)
			Me.Name = "Form1"
			Me.Text = "Form1"
'			Me.Load += New System.EventHandler(Me.Form1_Load);
			CType(Me.schedulerControl1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.schedulerStorage1, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.appointmentsDataSet, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.appointmentsBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.resourcesBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
			Me.ResumeLayout(False)

		End Sub

		#End Region

		Private WithEvents schedulerControl1 As DevExpress.XtraScheduler.SchedulerControl
		Private WithEvents schedulerStorage1 As DevExpress.XtraScheduler.SchedulerStorage
		Private appointmentsDataSet As AppointmentsDataSet
		Private appointmentsBindingSource As System.Windows.Forms.BindingSource
		Private appointmentsTableAdapter As Drag_iCalFromFile.AppointmentsDataSetTableAdapters.AppointmentsTableAdapter
		Private resourcesBindingSource As System.Windows.Forms.BindingSource
		Private resourcesTableAdapter As Drag_iCalFromFile.AppointmentsDataSetTableAdapters.ResourcesTableAdapter
	End Class
End Namespace

