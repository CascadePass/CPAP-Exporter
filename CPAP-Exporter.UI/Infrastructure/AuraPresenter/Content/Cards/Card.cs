using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CascadePass.CPAPExporter
{
    public class Card : ContentStylingCue
    {
        public Card()
        {
        }
        public Card(object displayContent) : base(displayContent)
        {
        }
        public Card(object displayContent, CuedContentType messageType) : base(displayContent, messageType)
        {
        }


    }
}
