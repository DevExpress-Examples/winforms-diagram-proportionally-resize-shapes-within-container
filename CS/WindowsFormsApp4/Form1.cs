using DevExpress.Diagram.Core;
using DevExpress.Diagram.Core.Native;
using DevExpress.Utils;
using DevExpress.Utils.Serializing;
using DevExpress.XtraDiagram;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace WindowsFormsApp4
{
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();

            diagramControl1.BeforeItemsResizing += DiagramControl1_BeforeItemsResizing;
            diagramControl1.ItemsResizing += DiagramControl1_ItemsResizing;

            diagramControl1.Items.Add(CreateContainerShape1());
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            diagramControl1.FitToItems(diagramControl1.Items);
        }

        private void DiagramControl1_BeforeItemsResizing(object sender, DiagramBeforeItemsResizingEventArgs e) {
            var containers = e.Items.OfType<DiagramContainer>();
            foreach (var container in containers) {
                e.Items.Remove(container);
                foreach (var item in container.Items)
                    e.Items.Add(item);
            }
        }
        private void DiagramControl1_ItemsResizing(object sender, DiagramItemsResizingEventArgs e) {
            var groups = e.Items.GroupBy(x => x.Item.ParentItem);
            foreach (var group in groups) {
                if (group.Key is DiagramContainer container) {
                    var containingRect = container.Items.Select(x => x.RotatedDiagramBounds().BoundedRect()).Aggregate(Rect.Empty, Rect.Union);
                    container.Position = new PointFloat((float)containingRect.X, (float)containingRect.Y);
                    container.Width = (float)containingRect.Width;
                    container.Height = (float)containingRect.Height;
                }
            }
        }

        public DiagramContainer CreateContainerShape1() {
            var container = new DiagramContainer() {
                Width = 200,
                Height = 200,
                Position = new PointFloat(100f, 100f),
                CanAddItems = false,
                ItemsCanChangeParent = false,
                ItemsCanCopyWithoutParent = false,
                ItemsCanDeleteWithoutParent = false,
                ItemsCanAttachConnectorBeginPoint = false,
                ItemsCanAttachConnectorEndPoint = false
            };

            container.Appearance.BorderSize = 0;
            container.Appearance.BackColor = Color.Transparent;

            var innerShape1 = new DiagramShape() {
                CanSelect = true,
                CanChangeParent = false,
                CanEdit = true,
                CanResize = false,
                CanCopyWithoutParent = false,
                CanDeleteWithoutParent = false,
                CanMove = false,
                Shape = BasicShapes.Trapezoid,
                Height = 50,
                Width = 200,

                Content = "Custom text"
            };

            var innerShape2 = new DiagramShape()
            {
                CanSelect = false,
                CanChangeParent = false,
                CanEdit = false,
                CanCopyWithoutParent = false,
                CanDeleteWithoutParent = false,
                CanMove = false,
                Shape = BasicShapes.Rectangle,
                Height = 150,
                Width = 200,
                Position = new PointFloat(0, 50),
            };

            container.Items.Add(innerShape1);
            container.Items.Add(innerShape2);

            return container;
        }
    }
}
