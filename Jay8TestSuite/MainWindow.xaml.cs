using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;
using mshtml;
using Jay8TestSuite.IE;

namespace Jay8TestSuite
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Popup _popup;
        private Rectangle _rect;
        private Canvas _canvas;
        private bool _selectorToggle = false;
        private HTMLDocument _doc;
        private IElementHighlighter _elementHighlighter;

        public MainWindow()
        {
            InitializeComponent();

            browser.Source = new Uri("http://google.com");
            /*
            _canvas = new Canvas();
            _popup = new Popup();
            _popup.AllowsTransparency = true;
            _popup.PlacementTarget = browser;
            _rect = new Rectangle();
            _rect.Stroke = System.Windows.Media.Brushes.SkyBlue;
            _rect.Fill = System.Windows.Media.Brushes.Black;
            _rect.HorizontalAlignment = HorizontalAlignment.Left;
            _rect.VerticalAlignment = VerticalAlignment.Top;
            _rect.Height = 50;
            _rect.Width = 50;
            _rect.Opacity = 0.25;
            _canvas.Width = 50;
            _canvas.Height = 50;
            _canvas.Children.Add(_rect);
            _popup.Placement = PlacementMode.Bottom;
            _popup.PlacementRectangle = new Rect(200, 100, 0, 20);
            _popup.Child = _canvas;
            _popup.IsOpen = _selectorToggle;
             */
            selectorButton.Content = "Selector Off";

            Window w = Window.GetWindow(browser);
            if (null != w)
            {
                w.LocationChanged += delegate(object sender, EventArgs args)
                {
                    var offset = _popup.HorizontalOffset;
                    _popup.HorizontalOffset = offset + 1;
                    _popup.HorizontalOffset = offset;
                };
            }
        }

        private void SetupSelector()
        {
            _doc = browser.Document as HTMLDocument;
            _popup.MouseMove += new MouseEventHandler(window_MouseMove);
            
            HTMLDocumentEvents2_Event iEvent;
            iEvent = (HTMLDocumentEvents2_Event)_doc;
            _popup.IsOpen = _selectorToggle;
            //iEvent.onclick += new mshtml.HTMLDocumentEvents2_onclickEventHandler(ClickEventHandler);
            //iEvent.onmouseout += new mshtml.HTMLDocumentEvents2_onmouseoutEventHandler(MouseOutEventHandler);   
            //iEvent.onmouseover += new mshtml.HTMLDocumentEvents2_onmouseoverEventHandler(MouseOverEventHandler); 
            iEvent.onmousemove += new mshtml.HTMLDocumentEvents2_onmousemoveEventHandler(MouseMoveEventHandler);
        }

        private Rect GetElementRect(IHTMLElement elem)
        {
            var l = 0;
            var t = 0;

            while (elem != null)
            {
                l += elem.offsetLeft;
                t += elem.offsetTop;
                elem = elem.offsetParent;
            }
            var doc = _doc;
            var html = _doc.getElementsByTagName("HTML").item(null, 0);
            l -= html.scrollLeft;
            t -= html.scrollTop;
            
            return new Rect(l,t,0,0);
            
        }

        private void window_MouseMove(object sender, MouseEventArgs e)
        {
            // Update the mouse path that is drawn onto the Panel. 
            int mouseX = Convert.ToInt32(e.GetPosition(this).X);
            int mouseY = Convert.ToInt32(e.GetPosition(this).Y);
            IHTMLElement element = _doc.elementFromPoint(mouseX, mouseY);

            if (element != null)
            {
                HighlightElement(element);
              
            }
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

        private int _currentClassNameCount = 0;

        private bool IsClassNameUnique(string className)
        {
            if (className != null)
            {
                _currentClassNameCount = 0;
                var result = ClassNameFind(_doc.body, className);
                Console.WriteLine("ClassCount " + result);
                if (result == 1)
                {
                    return true;
                }
            }
            return false;
        }

        private void GetIdentifier(IHTMLElement element)
        {
            var selectorFound = false;
            ElementTextBlock.Text = element.tagName + "\n";
            if (element.id != null)
            {
                
                ElementTextBlock.Text += "var element = browser.FindId(\""+ element.id + "\");\n";
                selectorFound = true;
            }
            if (IsClassNameUnique(element.className))
            {
                ElementTextBlock.Text += "var element = browser.FindCss(\""+ element.className + "\");\n";
                selectorFound = true;
            }
            if(!selectorFound)
            {
                ElementTextBlock.Text += "Ah crap, can't find this selector. What a bitch!";
            }
        }

        private void HighlightElement(IHTMLElement element)
        {
            var bounds = GetElementRect(element);
            _canvas.Width = element.offsetWidth;
            _canvas.Height = element.offsetHeight;
            _rect.Width = element.offsetWidth;
            _rect.Height = element.offsetHeight;
            _popup.PlacementRectangle = bounds;
            GetIdentifier(element);
        }
        
        private void MouseMoveEventHandler(mshtml.IHTMLEventObj e)
        {
            Console.WriteLine("x: " + e.clientX + " y: " + e.clientY);
            IHTMLElement element = _doc.elementFromPoint(e.clientX, e.clientY);
            
            if (element != null)
            {
                HighlightElement(element);
            }            
        }
        
        private bool ClickEventHandler(mshtml.IHTMLEventObj eventObject)
        {
            var element = eventObject.srcElement;
            
            _canvas.Height = element.offsetHeight;
            if (browser.ActualWidth < element.offsetWidth + element.offsetLeft)
            {
                _canvas.Width = browser.ActualWidth - element.offsetLeft - SystemParameters.VerticalScrollBarWidth;
                _rect.Width = browser.ActualWidth - element.offsetLeft - SystemParameters.VerticalScrollBarWidth;
            }
            else
            {
                _canvas.Width = element.offsetWidth;
                _rect.Width = element.offsetWidth;
            }
            _rect.Height = element.offsetHeight;
            _popup.PlacementRectangle = new Rect(element.offsetLeft, element.offsetTop, 0, 0);
            return true;
        }

        private void MouseOverEventHandler(mshtml.IHTMLEventObj eventObject)
        {
            var element = eventObject.srcElement;
            _canvas.Width = element.offsetWidth;
            _canvas.Height = element.offsetHeight;
            _rect.Width = element.offsetWidth;
            _rect.Height = element.offsetHeight;
            _popup.PlacementRectangle = new Rect(element.offsetLeft, element.offsetTop, 0, 0);
            
        }

        private void MouseOutEventHandler(mshtml.IHTMLEventObj eventObject)
        {
            var element = eventObject.srcElement;
            _canvas.Width = 0;
            _canvas.Height = 0;
            _rect.Width = 0;
            _rect.Height = 0;
            _popup.PlacementRectangle = new Rect(0, 0, 0, 0);

        }

        private void selectorButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectorToggle)
            {
                _selectorToggle = false;
                selectorButton.Content = "Selector Off";
            }
            else
            {
                _selectorToggle = true;
                selectorButton.Content = "Selector On";
                SetupSelector();
            }
        }
    }
}
