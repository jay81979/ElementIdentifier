using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mshtml;

namespace Jay8TestSuite.IE
{
    public class ElementIdentifier
    {
        private HTMLDocument _document;

        private string Identify(HTMLDocument document, IHTMLElement element)
        {
            _document = document;
            string identity = "";
            var selectorFound = false;
            identity = element.tagName + "\n";
            if (element.id != null)
            {

                identity += "var element = browser.FindId(\"" + element.id + "\");\n";
                selectorFound = true;
            }
            if (IsClassNameUnique(element.className))
            {
                identity += "var element = browser.FindCss(\"" + element.className + "\");\n";
                selectorFound = true;
            }
            if (!selectorFound)
            {
                identity += "Ah crap, can't find this selector. What a bitch!";
            }
            return identity;
        }

        private int _currentClassNameCount = 0;

        private bool IsClassNameUnique(string className)
        {
            if (className != null)
            {
                _currentClassNameCount = 0;
                var result = ClassNameFind(_document.body, className);
                Console.WriteLine("ClassCount " + result);
                if (result == 1)
                {
                    return true;
                }
            }
            return false;
        }

        private int ClassNameFind(IHTMLElement element, string className)
        {
            foreach (IHTMLElement childElement in element.children)
            {
                if (childElement.className != null && childElement.className.Equals(className))
                {
                    _currentClassNameCount++;
                }
                ClassNameFind(childElement, className);
            }
            return _currentClassNameCount;
        }
    }
}
