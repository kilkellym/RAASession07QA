#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#endregion

namespace RAASession07QA
{
    [Transaction(TransactionMode.Manual)]
    public class Command : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;

            // get all view templates

            List<string> tmpList = GetViewTemplateNames(doc);

            // open form
            Form1 curForm = new Form1(tmpList, "Select view templates to delete");
            curForm.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            curForm.ShowDialog();

            // if user clicks cancel, return cancelled
            if (curForm.DialogResult == System.Windows.Forms.DialogResult.Cancel)
                return Result.Cancelled;

            // get selected view template names
            List<string> selectedItem = curForm.GetSelectedItems();

            int counter = 0;
            using (Transaction t = new Transaction(doc))
            {
                t.Start("Delete view templates");
                foreach (string item in selectedItem)
                {
                    View curVT = GetViewTemplateByName(doc, item);

                    doc.Delete(curVT.Id);
                    counter++;
                }
                t.Commit();
            }

            TaskDialog.Show("Complete", "Deleted " + counter.ToString() + " view templates.");
                
            return Result.Succeeded;
        }

        private View GetViewTemplateByName(Document doc, string vtName)
        {
            List<View> vtList = GetViewTemplates(doc);

            foreach (View view in vtList)
            {
                if(view.Name == vtName)
                {
                    return view;
                }
            }

            return null;
        }

        private List<string> GetViewTemplateNames(Document doc)
        {
            List<string> returnList = new List<string>();

            List<View> vtList = GetViewTemplates(doc);

            foreach(View view in vtList)
            {
                returnList.Add(view.Name);
            }

            return returnList;
        }

        private List<View> GetViewTemplates(Document doc)
        {
            List<View> returnList = new List<View>();

            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(View));

            foreach (View view in collector)
            {
                if (view.IsTemplate == true)
                {
                    returnList.Add(view);
                }
            }

            return returnList;
        }
    }
}
