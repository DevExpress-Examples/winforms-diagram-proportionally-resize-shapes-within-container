# WinForms - How to proportionally resize shapes whithin the parent container

This example demonstrates how to support proportional resizing for shapes when the parent container is resized. You can implement this additional logic if you create custom shapes based on containers, or use containers to support the grouping functionality.

To support this feature, handle `DiagramControl`'s [BeforeItemsResizing](https://docs.devexpress.com/WindowsForms/DevExpress.XtraDiagram.DiagramControl.BeforeItemsResizing) event and pass a container's child items to the `e.Items` collection:

```cs
private void DiagramControl1_BeforeItemsResizing(object sender, DiagramBeforeItemsResizingEventArgs e) {
	var containers = e.Items.OfType<DiagramContainer>();
	foreach (var container in containers) {
		e.Items.Remove(container);
		foreach (var item in container.Items)
			e.Items.Add(item);
	}
}

```

In this case, `DiagramControl` will resize the inner items instead of the parent container.
After that, handle `DiagramControl`'s [ItemsResizing](https://docs.devexpress.com/WindowsForms/DevExpress.XtraDiagram.DiagramControl.ItemsResizing) event and correct the container's position and size:

```cs
private void DiagramControl1_ItemsResizing(object sender, DiagramItemsResizingEventArgs e) {
	var groups = e.Items.GroupBy(x => x.Item.ParentItem);
	foreach (var group in groups) {
		var container = (DiagramContainer)group.Key;
		var containingRect = container.Items.Select(x => x.RotatedDiagramBounds().BoundedRect()).Aggregate(Rect.Empty, Rect.Union);
		container.Position = new PointFloat((float)containingRect.X, (float)containingRect.Y);
		container.Width = (float)containingRect.Width;
		container.Height = (float)containingRect.Height;
	}
}

```

## Files to Review

- link.cs (VB: link.vb)
- link.js
- ...

## Documentation

- link
- link
- ...

## More Examples

- link
- link
- ...
