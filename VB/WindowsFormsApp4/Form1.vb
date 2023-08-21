Imports DevExpress.Diagram.Core
Imports DevExpress.Diagram.Core.Native
Imports DevExpress.Utils
Imports DevExpress.XtraDiagram
Imports System
Imports System.Data
Imports System.Drawing
Imports System.Linq
Imports System.Windows
Imports System.Windows.Forms

Namespace WindowsFormsApp4

    Public Partial Class Form1
        Inherits Form

        Public Sub New()
            InitializeComponent()
            AddHandler diagramControl1.BeforeItemsResizing, AddressOf DiagramControl1_BeforeItemsResizing
            AddHandler diagramControl1.ItemsResizing, AddressOf DiagramControl1_ItemsResizing
            DiagramControl.ItemTypeRegistrator.Register(GetType(CustomDiagramContainer))
            diagramControl1.Items.Add(CreateContainerShape1())
        End Sub

        Protected Overrides Sub OnLoad(ByVal e As EventArgs)
            MyBase.OnLoad(e)
            diagramControl1.FitToItems(diagramControl1.Items)
        End Sub

        Private Sub DiagramControl1_BeforeItemsResizing(ByVal sender As Object, ByVal e As DiagramBeforeItemsResizingEventArgs)
            Dim containers = e.Items.OfType(Of CustomDiagramContainer)()
            For Each customContainer In containers
                e.Items.Remove(customContainer)
                For Each item In customContainer.Items
                    e.Items.Add(item)
                Next
            Next
        End Sub

        Private Sub DiagramControl1_ItemsResizing(ByVal sender As Object, ByVal e As DiagramItemsResizingEventArgs)
            Dim groups = e.Items.GroupBy(Function(x) x.Item.ParentItem)
            For Each group In groups
                If TypeOf group.Key Is CustomDiagramContainer Then
                    Dim customContainer = CType(group.Key, CustomDiagramContainer)
                    Dim containingRect = customContainer.Items.[Select](Function(x) x.RotatedDiagramBounds().BoundedRect()).Aggregate(Rect.Empty, New Func(Of Rect, Rect, Rect)(AddressOf Rect.Union))
                    customContainer.Position = New PointFloat(CSng(containingRect.X), CSng(containingRect.Y))
                    customContainer.Width = CSng(containingRect.Width)
                    customContainer.Height = CSng(containingRect.Height)
                End If
            Next
        End Sub

        Public Function CreateContainerShape1() As CustomDiagramContainer
            Dim container = New CustomDiagramContainer() With {.Width = 200, .Height = 200, .Position = New PointFloat(100F, 100F), .CanAddItems = False, .ItemsCanChangeParent = False, .ItemsCanCopyWithoutParent = False, .ItemsCanDeleteWithoutParent = False, .ItemsCanAttachConnectorBeginPoint = False, .ItemsCanAttachConnectorEndPoint = False}
            container.Appearance.BorderSize = 0
            container.Appearance.BackColor = Color.Transparent
            Dim innerShape1 = New DiagramShape() With {.CanSelect = True, .CanChangeParent = False, .CanEdit = True, .CanResize = False, .CanCopyWithoutParent = False, .CanDeleteWithoutParent = False, .CanMove = False, .Shape = BasicShapes.Trapezoid, .Height = 50, .Width = 200, .Content = "Custom text"}
            Dim innerShape2 = New DiagramShape() With {.CanSelect = False, .CanChangeParent = False, .CanEdit = False, .CanCopyWithoutParent = False, .CanDeleteWithoutParent = False, .CanMove = False, .Shape = BasicShapes.Rectangle, .Height = 150, .Width = 200, .Position = New PointFloat(0, 50)}
            container.Items.Add(innerShape1)
            container.Items.Add(innerShape2)
            Return container
        End Function
    End Class

    Public Class CustomDiagramContainer
        Inherits DiagramContainer

    End Class
End Namespace
