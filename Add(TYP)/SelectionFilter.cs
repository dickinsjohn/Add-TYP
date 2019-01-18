﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;

namespace Add_TYP_
{
    //selection filter class
    class DimensionSelectionFilter : ISelectionFilter
    {
        Document m_doc = null;
        //constructor
        public DimensionSelectionFilter(Document doc)
        {
            m_doc = doc;
        }

        //allow independent tags to be selected
        public bool AllowElement(Element elem)
        {
            return elem is Dimension;
        }

        //allow all references to be selected
        public bool AllowReference(Reference refer, XYZ point)
        {
            return true;
        }
    }
}