using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

using Security_Check;

namespace Add_TYP_
{
    //Transaction assigned as automatic
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Automatic)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]

    //Creating an external command to add TYP to dimensions
    public class AddTYP :IExternalCommand
    {
        //instances to store application and the document
        UIDocument m_document = null;
        IList<Reference> selectedObjects = new List<Reference>();

        Dimension dim = null;

        bool security = false;

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                //call to the security check method to check for authentication
                security=SecurityLNT.Security_Check();
               
                if (security == false)
                {
                    return Result.Succeeded;
                }

                //open  the active document in revit
                m_document = commandData.Application.ActiveUIDocument;

                selectedObjects.Clear();

                try
                {
                    //pick the objects of type dimensions using the pick objects method
                    selectedObjects = m_document.Selection.PickObjects(ObjectType.Element, 
                        new DimensionSelectionFilter(m_document.Document), "Select Elements by a Selection Rectangle.");
                }
                catch(Exception e)
                {
                    //return succeed even if it fails to avoid the'user aborted operation' message
                    if(e is OperationCanceledException)
                        return Result.Succeeded;
                }

                foreach (Reference refer in selectedObjects)
                {        
                    //convert the refernces picked into elements and type cast it to dimensions
                    dim = (Dimension)m_document.Document.GetElement(refer);

                    //if the dimensions are independent dimensions aadd the text to below property
                    if (dim.NumberOfSegments == 0)
                    {
                        dim.Below = "(TYP)";
                    }
                    else
                    {
                        //if dimensions are of continuous type add text to below property 
                        //of each segment by iterating through the blelow property
                        foreach(DimensionSegment segment in dim.Segments)
                        {
                            segment.Below = "(TYP)";
                        }
                    }
                }

                return Result.Succeeded;

            }
            catch (Exception e)
            {
                message = e.Message;
                return Autodesk.Revit.UI.Result.Failed;
            }
            throw new NotImplementedException();
        }
    }
}
