using System;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Shapes;

namespace Jay8TestSuite.IE
{
    public class ElementHighlighter : Popup, IElementHighlighter
    {
        public ElementHighlighter(WebBrowser browser)
            :base()
        {
            _browser = browser;
            _rectangle = new Rectangle();
            _canvas = new Canvas();
            AllowsTransparency = true;
            PlacementTarget = browser;
        }
        private Rectangle _rectangle;
        private Canvas _canvas;
        private WebBrowser _browser;

        private bool _enabled = false;

        public bool Enabled
        {
            get
            {
                return _enabled;
            }
            set
            {
                _enabled = value;
            }
        }
    }
}
